/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using OmiLAXR.TrackingBehaviours;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OmiLAXR.Composers
{
    /// <summary>
    /// Abstract base class for statement composers that process tracking behaviors.
    /// Manages statement caching, composition, and delivery to endpoints.
    /// </summary>
    /// <typeparam name="T">Type of tracking behavior this composer handles</typeparam>
    /// <typeparam name="TStatement">Type of statements.</typeparam>
    [DefaultExecutionOrder(-100)]
    public abstract class Composer<T, TStatement> : DataProviderPipelineComponent, IComposer
        where T : PipelineComponent, ITrackingBehaviour
        where TStatement : class, IStatement
    {
        
        /// <summary>
        /// Array of tracking behaviors this composer is processing
        /// </summary>
        [HideInInspector] public T[] trackingBehaviours;
        
        /// <summary>
        /// Cache for storing statements with string keys
        /// </summary>
        private readonly Dictionary<string, TStatement> _statementCache = new Dictionary<string, TStatement>();
        
        /// <summary>
        /// Cache for storing statements with integer keys
        /// </summary>
        private readonly Dictionary<int, TStatement> _statementCacheInt = new Dictionary<int, TStatement>();
        
        /// <summary>
        /// Stores a statement in cache with string key
        /// </summary>
        public void StoreStatement(string key, TStatement statement)
            => _statementCache[key] = statement;
            
        /// <summary>
        /// Stores a statement in cache with integer key
        /// </summary>
        public void StoreStatement(int key, TStatement statement)
            => _statementCacheInt[key] = statement;
        
        public void StoreStatement(ITrackingBehaviour tb, GameObject target, TStatement statement)
            => StoreStatement(CombineHash(tb.GetHashCode(), target.GetHashCode()), statement);
        
        /// <summary>
        /// Retrieves a cached statement by string key. Optionally removes it from the cache.
        /// </summary>
        public TStatement RestoreStatement(string key, bool erase = false)
        {
            if (_statementCache.TryGetValue(key, out var statement))
            {
                if (erase)
                    _statementCache.Remove(key);

                return statement;
            }

            return null;
        }
        
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static int CombineHash(int h1, int h2)
        {
#if UNITY_2021_2_OR_NEWER
    return System.HashCode.Combine(h1, h2);
#else
            unchecked { return ((h1 << 5) + h1) ^ h2; }
#endif
        }


        /// <summary>
        /// Retrieves a cached statement by integer key. Optionally removes it from the cache.
        /// </summary>
        public TStatement RestoreStatement(int key, bool erase = false)
        {
            if (_statementCacheInt.TryGetValue(key, out var statement))
            {
                if (erase)
                    _statementCacheInt.Remove(key);

                return statement;
            }

            return null;
        }

        /// <summary>
        /// Retrieves a cached statement by composite key. Optionally removes it from the cache.
        /// </summary>
        public TStatement RestoreStatement(ITrackingBehaviour tb, GameObject target, bool erase = false)
        {
            var key = CombineHash(tb.GetHashCode(), target.GetHashCode());
            return RestoreStatement(key, erase);
        }
        
        /// <summary>
        /// Initializes the composer, finds tracking behaviors, and starts composition
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            // Generate composer name from class name
            _name = GetType().Name.Replace("Composer", "");

            // Find all tracking behaviors of the specified type
            trackingBehaviours = GetTrackingBehaviours<T>();

            // Start composing statements for each tracking behavior
            foreach (var trackingBehaviour in trackingBehaviours)
                Compose(trackingBehaviour);

            // Process any queued statements
            HandleWaitList();
        }

        /// <summary>
        /// Gets author information for statements created by this composer
        /// </summary>
        public abstract Author GetAuthor();

        public virtual string GetDataStandardVersion() => "1.0.0";

        /// <summary>
        /// Cached composer name derived from class name
        /// </summary>
        private string _name;
        
        /// <summary>
        /// Returns the display name of this composer
        /// </summary>
        public virtual string GetName() => _name;
        
        /// <summary>
        /// Returns the logical grouping for this composer
        /// </summary>
        public virtual ComposerGroup GetGroup() => ComposerGroup.Other;

        /// <summary>
        /// Indicates if this is a higher-level composer that processes other composers' output
        /// </summary>
        public virtual bool IsHigherComposer => false;
        
        /// <summary>
        /// Event fired after a statement has been composed and is ready for delivery
        /// </summary>
        public event ComposerAction<IStatement> AfterComposed;

        /// <summary>
        /// Queue for statements waiting for event handlers to be registered
        /// </summary>
        private readonly List<TStatement> _waitList = new List<TStatement>();

        /// <summary>
        /// Finds all tracking behaviors of specified type in the scene
        /// </summary>
        /// <param name="includeInactive">Whether to include inactive GameObjects</param>
        protected static TB[] GetTrackingBehaviours<TB>(bool includeInactive = false)
            where TB : Object, ITrackingBehaviour => FindObjects<TB>(includeInactive);

        /// <summary>
        /// Obsolete: Use SendStatement(ITrackingBehaviour, IStatement) instead
        /// </summary>
        [Obsolete(
            "Use SendStatement(ITrackingBehaviour, IStatement) instead. Immediate is not needed anymore due efficient thread queue handling.",
            true)]
        protected void SendStatement(ITrackingBehaviour statementOwner, TStatement statement, bool immediate)
            => SendStatement(statementOwner, statement);

        /// <summary>
        /// Sends a composed statement for delivery to endpoints
        /// </summary>
        /// <param name="statementOwner">The tracking behavior that generated this statement</param>
        /// <param name="statement">The composed statement to send</param>
        protected void SendStatement(ITrackingBehaviour statementOwner, TStatement statement)
        {
            if (!enabled)
                return;
            // Set ownership and composer information
            statement.SetOwner(statementOwner);

            // If no handlers registered, queue statement for later
            if (AfterComposed?.GetInvocationList().Length < 1)
            {
                print("Enqueued statement: " + statement.ToShortString() + " in waitlist.");
                _waitList.Add(statement);
                return;
            }
            
            // Send statement to registered handlers
            AfterComposed?.Invoke(this, statement);
        }

        /// <summary>
        /// Obsolete: Use SendStatement(ITrackingBehaviour, IStatement) instead
        /// </summary>
        [Obsolete(
            "Use SendStatement(ITrackingBehaviour, IStatement) instead. Immediate is not needed anymore due efficient thread queue handling.",
            true)]
        protected void SendStatementImmediate(ITrackingBehaviour statementOwner, TStatement statement)
            => SendStatement(statementOwner, statement, immediate: true);

        /// <summary>
        /// Obsolete: Use SendStatement(ITrackingBehaviour, IStatement, bool) instead
        /// </summary>
        [Obsolete("Use SendStatement(ITrackingBehaviour, IStatement, bool) instead.")]
        protected void SendStatement(TStatement statement, bool immediate = false)
        {
            SendStatement(trackingBehaviours.First(), statement, immediate);
        }

        /// <summary>
        /// Obsolete: Use SendStatementImmediate(ITrackingBehaviour, IStatement) instead
        /// </summary>
        [Obsolete("Use SendStatementImmediate(ITrackingBehaviour, IStatement) instead.")]
        protected void SendStatementImmediate(TStatement statement)
            => SendStatement(statement, immediate: true);

        /// <summary>
        /// Implements composition logic for the specific tracking behavior type.
        /// Override this method to define how statements are created from tracking events.
        /// </summary>
        /// <param name="tb">The tracking behavior to compose statements for</param>
        protected abstract void Compose(T tb);

        /// <summary>
        /// Processes queued statements when event handlers become available
        /// </summary>
        private void HandleWaitList()
        {
            // Send all queued statements if handlers are now available
            if (_waitList.Count > 0 && AfterComposed?.GetInvocationList().Length > 0)
            {
                foreach (var statement in _waitList)
                {
                    AfterComposed?.Invoke(this, statement);
                }

                _waitList.Clear();
            }
        }

        /// <summary>
        /// Checks for queued statements each frame and processes them when handlers become available
        /// </summary>
        private void Update()
        {
            HandleWaitList();
        }
    }
}