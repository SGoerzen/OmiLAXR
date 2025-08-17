/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.TrackingBehaviours
{
    /// <summary>
    /// Delegate for tracking behavior events with no additional parameters.
    /// </summary>
    public delegate void TrackingBehaviourAction(ITrackingBehaviour sender);
    
    /// <summary>
    /// Delegate for tracking behavior events with one typed parameter.
    /// </summary>
    public delegate void TrackingBehaviourAction<in T>(ITrackingBehaviour sender, T obj);
    
    /// <summary>
    /// Delegate for tracking behavior events with two typed parameters.
    /// </summary>
    public delegate void TrackingBehaviourAction<in T1, in T2>(ITrackingBehaviour sender, T1 obj1, T2 obj2);
    
    /// <summary>
    /// Delegate for tracking behavior events with three typed parameters.
    /// </summary>
    public delegate void TrackingBehaviourAction<in T1, in T2, in T3>(ITrackingBehaviour sender, T1 obj1, T2 obj2, T3 obj3);
    
    /// <summary>
    /// Delegate for tracking behavior events with four typed parameters.
    /// </summary>
    public delegate void TrackingBehaviourAction<in T1, in T2, in T3, in T4>(ITrackingBehaviour sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4);
    
    /// <summary>
    /// Delegate for tracking behavior events with five typed parameters.
    /// </summary>
    public delegate void TrackingBehaviourAction<in T1, in T2, in T3, in T4, in T5>(ITrackingBehaviour sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5);
    
    /// <summary>
    /// Delegate for tracking behavior events with six typed parameters.
    /// </summary>
    public delegate void TrackingBehaviourAction<in T1, in T2, in T3, in T4, in T5, in T6>(ITrackingBehaviour sender, T1 obj1, T2 obj2, T3 obj3, T4 obj4, T5 obj5, T6 obj6);
}