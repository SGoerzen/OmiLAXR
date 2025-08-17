/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Composers
{
    /// <summary>
    /// Delegate for composer events with no additional parameters.
    /// Used for simple notifications from composers.
    /// </summary>
    /// <param name="sender">The composer that triggered the event</param>
    public delegate void ComposerAction(IComposer sender);
    
    /// <summary>
    /// Delegate for composer events with one typed parameter.
    /// Commonly used for statement composition events.
    /// </summary>
    /// <typeparam name="T">Type of the event parameter</typeparam>
    /// <param name="sender">The composer that triggered the event</param>
    /// <param name="obj">The event data object</param>
    public delegate void ComposerAction<in T>(IComposer sender, T obj);
    
    /// <summary>
    /// Delegate for composer events with two typed parameters.
    /// Used for events that need to pass multiple related data objects.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <param name="sender">The composer that triggered the event</param>
    /// <param name="obj1">The first event data object</param>
    /// <param name="obj2">The second event data object</param>
    public delegate void ComposerAction<in T1, in T2>(IComposer sender, T1 obj1, T2 obj2);
    
    /// <summary>
    /// Delegate for composer events with three typed parameters.
    /// Used for complex events requiring multiple data objects.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <typeparam name="T3">Type of the third parameter</typeparam>
    /// <param name="sender">The composer that triggered the event</param>
    /// <param name="obj1">The first event data object</param>
    /// <param name="obj2">The second event data object</param>
    /// <param name="obj3">The third event data object</param>
    public delegate void ComposerAction<in T1, in T2, in T3>(IComposer sender, T1 obj1, T2 obj2, T3 obj3);
    
    /// <summary>
    /// Delegate for composer events with four typed parameters.
    /// Used for very complex events requiring multiple related data objects.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <typeparam name="T3">Type of the third parameter</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter</typeparam>
    /// <param name="sender">The composer that triggered the event</param>
    /// <param name="obj1">The first event data object</param>
    /// <param name="obj2">The second event data object</param>
    /// <param name="obj3">The third event data object</param>
    /// <param name="obj4">The fourth event data object</param>
    public delegate void ComposerAction<in T1, in T2, in T3, in T4>(IComposer sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4);
    
    /// <summary>
    /// Delegate for composer events with five typed parameters.
    /// Used for highly complex events with extensive data requirements.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <typeparam name="T3">Type of the third parameter</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter</typeparam>
    /// <param name="sender">The composer that triggered the event</param>
    /// <param name="obj1">The first event data object</param>
    /// <param name="obj2">The second event data object</param>
    /// <param name="obj3">The third event data object</param>
    /// <param name="obj4">The fourth event data object</param>
    /// <param name="obj5">The fifth event data object</param>
    public delegate void ComposerAction<in T1, in T2, in T3, in T4, in T5>(IComposer sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5);
    
    /// <summary>
    /// Delegate for composer events with six typed parameters.
    /// Used for extremely complex events with maximum data payload.
    /// </summary>
    /// <typeparam name="T1">Type of the first parameter</typeparam>
    /// <typeparam name="T2">Type of the second parameter</typeparam>
    /// <typeparam name="T3">Type of the third parameter</typeparam>
    /// <typeparam name="T4">Type of the fourth parameter</typeparam>
    /// <typeparam name="T5">Type of the fifth parameter</typeparam>
    /// <typeparam name="T6">Type of the sixth parameter</typeparam>
    /// <param name="sender">The composer that triggered the event</param>
    /// <param name="obj1">The first event data object</param>
    /// <param name="obj2">The second event data object</param>
    /// <param name="obj3">The third event data object</param>
    /// <param name="obj4">The fourth event data object</param>
    /// <param name="obj5">The fifth event data object</param>
    /// <param name="obj6">The sixth event data object</param>
    public delegate void ComposerAction<in T1, in T2, in T3, in T4, in T5, in T6>(IComposer sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6);
}