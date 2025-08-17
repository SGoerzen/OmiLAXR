/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;
using System.Collections.Generic;
using System.Linq;

namespace OmiLAXR.Composers.HigherComposers
{
    /// <summary>
    /// Abstract base class for higher-level composers that aggregate and process multiple related statements.
    /// Higher composers can analyze patterns of statements to detect complex events or behaviors.
    /// </summary>
    /// <typeparam name="T">The type of statement this composer processes.</typeparam>
    public abstract class HigherComposer<T> : PipelineComponent, IComposer
        where T : IStatement
    {
        private string _name;
        
        /// <summary>
        /// Gets the name of this composer.
        /// </summary>
        /// <returns>The name of the composer.</returns>
        public virtual string GetName() => _name;

        public ComposerGroup GetGroup() => ComposerGroup.Other;
        /// <summary>
        /// Defines the conditions that this composer looks for in statements.
        /// Must be implemented by derived classes to specify matching criteria.
        /// </summary>
        /// <returns>Dictionary mapping condition names to their match conditions.</returns>
        protected abstract Dictionary<string, MatchCondition<T>> Conditions();
        
        /// <summary>
        /// Helper method to find a pipeline of the specified type.
        /// </summary>
        /// <typeparam name="TP">The type of pipeline to find.</typeparam>
        /// <returns>The first pipeline of the specified type found in the scene.</returns>
        protected static TP GetPipeline<TP>()
            where TP : Pipeline => FindObject<TP>(true);
        
        /// <summary>
        /// Structure that defines a condition for matching statements.
        /// Contains the condition logic, expected number of matches, and collection of matching statements.
        /// </summary>
        /// <typeparam name="T0">The type of statement this condition applies to.</typeparam>
        protected readonly struct MatchCondition<T0>
            where T0 : IStatement
        {
            /// <summary>
            /// Delegate type for the condition function that evaluates statements.
            /// </summary>
            /// <param name="statement">The statement to evaluate.</param>
            /// <returns>True if the statement matches the condition, false otherwise.</returns>
            public delegate bool MatchConditionHandler(T0 statement);
            
            /// <summary>
            /// The condition function that determines if a statement matches.
            /// </summary>
            public readonly MatchConditionHandler Condition;
            
            /// <summary>
            /// The minimum number of matching statements required to satisfy this condition.
            /// </summary>
            public readonly int ExpectedMatches;
            
            /// <summary>
            /// Collection of statements that have matched this condition.
            /// </summary>
            public readonly List<T0> MatchingStatements;
            
            /// <summary>
            /// Gets the current number of statements that match this condition.
            /// </summary>
            public int ActualMatches => MatchingStatements.Count;
            
            /// <summary>
            /// Determines if the condition has collected enough matching statements.
            /// </summary>
            public bool HasEnoughMatches => ActualMatches >= ExpectedMatches;
            
            /// <summary>
            /// Initializes a new instance of the MatchCondition struct.
            /// </summary>
            /// <param name="expectedMatches">The minimum number of matching statements required.</param>
            /// <param name="condition">The function that evaluates if statements match.</param>
            public MatchCondition(int expectedMatches, MatchConditionHandler condition)
            {
                Condition = condition;
                ExpectedMatches = expectedMatches;
                MatchingStatements = new List<T0>();
            }

            /// <summary>
            /// Evaluates a statement against the condition and adds it to matching statements if it matches.
            /// </summary>
            /// <param name="statement">The statement to evaluate.</param>
            public void CheckCondition(T0 statement)
            {
                if (Condition(statement))
                    MatchingStatements.Add(statement);
            }
        }

        /// <summary>
        /// Event that fires when this composer creates a new statement.
        /// </summary>
        public event ComposerAction<IStatement> AfterComposed;
        
        /// <summary>
        /// Indicates that this is a higher-level composer.
        /// </summary>
        public bool IsHigherComposer => true;
        
        /// <summary>
        /// Indicates whether this composer is currently enabled.
        /// </summary>
        public bool IsEnabled => enabled;
        
        /// <summary>
        /// Gets information about the author of this composer.
        /// </summary>
        /// <returns>Author information.</returns>
        public abstract Author GetAuthor();

        /// <summary>
        /// Examines a statement to see if it matches any of the conditions defined by this composer.
        /// If enough statements match either any or all conditions, triggers the appropriate handler.
        /// </summary>
        /// <param name="statement">The statement to examine.</param>
        public void LookFor(IStatement statement)
        {
            // Skip if statement is not of the expected type
            if (statement.GetType() != typeof(T))
                return;
            
            var conditions = Conditions();
            var stmt = (T)statement;
            
            // Check statement against all conditions
            foreach (var condition in conditions.Values)
            {
                condition.CheckCondition(stmt);
            }

            // Check if any conditions have enough matches
            var matchesAny = 
                conditions.Where(c => c.Value.HasEnoughMatches)
                    .ToDictionary<KeyValuePair<string, MatchCondition<T>>, string, IEnumerable<T>>(
                        c => c.Key, 
                        c => c.Value.MatchingStatements
                        );
            
            // If any conditions have enough matches, call the any-match handler
            if (matchesAny.Count > 0)
            {
                OnMatchAnyConditions(matchesAny);
            }
            
            // Check if all conditions have enough matches
            var matchesAll = conditions.Values.All(c => c.HasEnoughMatches);
            if (!matchesAll) 
                return;
            
            // If all conditions have enough matches, call the all-match handler
            var matchingStatements = conditions.ToDictionary(
                i => i.Key,
                i => (IEnumerable<T>)i.Value.MatchingStatements
            );
            OnMatchAllConditions(matchingStatements);
        }

        /// <summary>
        /// Called when all conditions have collected enough matching statements.
        /// Must be implemented by derived classes to handle the matched statements.
        /// </summary>
        /// <param name="matchingStatements">Dictionary mapping condition names to their matching statements.</param>
        protected abstract void OnMatchAllConditions(Dictionary<string, IEnumerable<T>> matchingStatements);

        /// <summary>
        /// Called when any conditions have collected enough matching statements.
        /// Can be overridden by derived classes to handle partial matches.
        /// </summary>
        /// <param name="matchingStatements">Dictionary mapping condition names to their matching statements.</param>
        protected virtual void OnMatchAnyConditions(Dictionary<string, IEnumerable<T>> matchingStatements)
        {
            // Default implementation does nothing - derived classes can override
        }
        
        /// <summary>
        /// Sends a new statement through the pipeline.
        /// </summary>
        /// <param name="statement">The statement to send.</param>
        /// <param name="immediate">Whether to process the statement immediately or queue it.</param>
        [Obsolete("Use SendStatement(IStatement) instead. Immediate is not needed anymore due efficient thread queue handling.", true)]
        protected void SendStatement(IStatement statement, bool immediate)
            => SendStatement(statement);
        protected void SendStatement(IStatement statement)
        {
            if (!IsEnabled)
                return;
            AfterComposed?.Invoke(this, statement);
        }
        
        /// <summary>
        /// Convenience method to send a statement for immediate processing.
        /// </summary>
        /// <param name="statement">The statement to send immediately.</param>
        [Obsolete("Use SendStatement(ITrackingBehaviour, IStatement) instead. Immediate is not needed anymore due efficient thread queue handling.", true)]
        protected void SendStatementImmediate(IStatement statement)
            => SendStatement(statement, immediate: true);
    }
}