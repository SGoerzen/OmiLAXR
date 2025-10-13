/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
using System;

namespace OmiLAXR.Types
{
    /// <summary>
    /// Interface that provide calibration functionality
    /// for VR/AR applications within the OmiLAXR framework.
    /// </summary>
    public interface ICalibratable
    {
        /// <summary>
        /// Initiates the eye tracking calibration process.
        /// This method should begin the calibration sequence where the user
        /// looks at specific points to establish accurate eye tracking.
        /// </summary>
        void StartCalibration();

        /// <summary>
        /// Stops the currently running eye tracking calibration process.
        /// This method should cleanly terminate any ongoing calibration
        /// and return the system to its previous state.
        /// </summary>
        void StopCalibration();

        /// <summary>
        /// Checks whether the eye tracking system has been successfully calibrated.
        /// </summary>
        /// <returns>
        /// True if the eye tracking system is calibrated and ready for use;
        /// false if calibration is required or has failed.
        /// </returns>
        bool IsCalibrated { get; }

        bool NeedsCalibration { get; }

        /// <summary>
        /// Event triggered when the calibration process begins.
        /// Subscribe to this event to receive notifications when
        /// eye tracking calibration starts.
        /// </summary>
        event Action OnCalibrationStarted;

        /// <summary>
        /// Event triggered when the calibration process ends.
        /// Subscribe to this event to receive notifications when
        /// eye tracking calibration stops, whether completed or cancelled.
        /// </summary>
        event Action<bool> OnCalibrationEnded;
    }
}