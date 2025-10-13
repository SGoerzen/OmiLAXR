/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using UnityEngine;

namespace OmiLAXR
{
    /// <summary>
    /// Interface for components that need access to debug logging functionality.
    /// Provides a standardized way to expose debug logging capabilities.
    /// </summary>
    public interface IDebugSender
    {
        /// <summary>
        /// Gets the DebugLog instance used by this component for logging operations.
        /// </summary>
        DebugLog DebugLog { get; }
    }
    
    /// <summary>
    /// Delegate for building custom debug messages with optional prefixes.
    /// Used for advanced message formatting scenarios.
    /// </summary>
    /// <param name="prefix">Optional prefix for the message (defaults to "OmiLAXR")</param>
    /// <returns>Formatted debug message string</returns>
    public delegate string BuildMessageResponse(string prefix = "OmiLAXR");
    
    /// <summary>
    /// Centralized debug logging system with consistent message formatting and prefixing.
    /// Provides categorized logging methods (Print, Error, Warning) with automatic prefix handling.
    /// Ensures consistent log formatting across the entire OmiLAXR system.
    /// </summary>
    public sealed class DebugLog
    {
        /// <summary>
        /// Prefix string used to identify log messages from this logger instance.
        /// Appears at the beginning of all messages for easy filtering and identification.
        /// </summary>
        private readonly string _prefix;
        
        /// <summary>
        /// Initializes a new DebugLog instance with the specified prefix.
        /// </summary>
        /// <param name="prefix">Prefix string to use for all log messages</param>
        public DebugLog(string prefix)
        {
            _prefix = prefix;
        }
        
        /// <summary>
        /// Default OmiLAXR system logger instance used throughout the framework.
        /// Provides consistent logging for core system components and behaviors.
        /// </summary>
        public static readonly DebugLog OmiLAXR = new DebugLog("OmiLAXR");
        
        /// <summary>
        /// Builds a formatted log message with prefix and optional string formatting.
        /// Internal helper method used by all public logging methods.
        /// </summary>
        /// <param name="prefix">Prefix to use for this message</param>
        /// <param name="message">Message format string</param>
        /// <param name="ps">Optional parameters for string formatting</param>
        /// <returns>Fully formatted log message with prefix</returns>
        private static string BuildMessage(string prefix, string message, params object[] ps)
            => $"[{prefix}]: {((ps != null && ps.Length > 0) ? string.Format(message, ps) : message)}";
        
        /// <summary>
        /// Logs an informational message to the Unity console.
        /// Supports string formatting with optional parameters.
        /// </summary>
        /// <param name="message">Message format string</param>
        /// <param name="ps">Optional parameters for string formatting</param>
        public void Print(string message, params object[] ps)
            => Debug.Log(BuildMessage(_prefix, message, ps));
        
        /// <summary>
        /// Logs an error message to the Unity console.
        /// Supports string formatting with optional parameters.
        /// </summary>
        /// <param name="message">Error message format string</param>
        /// <param name="ps">Optional parameters for string formatting</param>
        public void Error(string message, params object[] ps)
            => Debug.LogError(BuildMessage(_prefix, message, ps));
        
        /// <summary>
        /// Logs a warning message to the Unity console.
        /// Supports string formatting with optional parameters.
        /// </summary>
        /// <param name="message">Warning message format string</param>
        /// <param name="ps">Optional parameters for string formatting</param>
        public void Warning(string message, params object[] ps)
            => Debug.LogWarning(BuildMessage(_prefix, message, ps));
        
        /// <summary>
        /// Logs an informational message with an associated Unity Object context.
        /// The context object will be highlighted in the Unity console when the message is clicked.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="context">Unity Object to associate with this log message</param>
        public void Print(string message, Object context)
            => Debug.Log(BuildMessage(_prefix, message), context);
        
        /// <summary>
        /// Logs an error message with an associated Unity Object context.
        /// The context object will be highlighted in the Unity console when the message is clicked.
        /// </summary>
        /// <param name="message">Error message to log</param>
        /// <param name="context">Unity Object to associate with this error message</param>
        public void Error(string message, Object context)
            => Debug.LogError(BuildMessage(_prefix, message), context);
        
        /// <summary>
        /// Logs a warning message with an associated Unity Object context.
        /// The context object will be highlighted in the Unity console when the message is clicked.
        /// </summary>
        /// <param name="message">Warning message to log</param>
        /// <param name="context">Unity Object to associate with this warning message</param>
        public void Warning(string message, Object context)
            => Debug.LogWarning(BuildMessage(_prefix, message), context);
    }
}