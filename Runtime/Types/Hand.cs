/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Types
{
    /// <summary>
    /// Enumeration representing different hand states for XR controller tracking.
    /// Used to identify which hand performed an interaction or gesture in VR/AR environments.
    /// </summary>
    public enum Hand
    {
        /// <summary>
        /// Hand information is not available or could not be determined.
        /// Used as default value when hand tracking fails or is not applicable.
        /// </summary>
        Unknown, 
        
        /// <summary>
        /// Left hand controller or tracked hand.
        /// Represents interactions performed with the user's left hand.
        /// </summary>
        Left,
        
        /// <summary>
        /// Right hand controller or tracked hand.
        /// Represents interactions performed with the user's right hand.
        /// </summary>
        Right, 
        
        /// <summary>
        /// Both hands are involved in the interaction.
        /// Used for two-handed gestures or simultaneous bilateral actions.
        /// </summary>
        Both
    }
}