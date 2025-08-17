/*
* SPDX-License-Identifier: AGPL-3.0-or-later
* Copyright (C) 2025 Sergej Görzen <sergej.goerzen@gmail.com>
* This file is part of OmiLAXR.
*/
namespace OmiLAXR.Composers
{
    /// <summary>
    /// Categorizes data statements by their functional and semantic role in XR-based Learning Analytics.
    /// These groups support filtering, interpretation, and visualization of learning-related interactions and system events.
    /// </summary>
    public enum ComposerGroup
    {
        /// <summary>
        /// System-level events, such as logging, session control, or framework operations.
        /// </summary>
        System,

        /// <summary>
        /// Visual attention indicators, especially derived from eye tracking or head direction (e.g. gaze on object).
        /// </summary>
        Attention,

        /// <summary>
        /// Gestures and body movements (e.g. hand tracking, symbolic gestures, interaction via controllers).
        /// </summary>
        Gesture,

        /// <summary>
        /// Affective states such as frustration, joy, confusion—typically inferred through analysis or self-report.
        /// </summary>
        Emotion,

        /// <summary>
        /// Environmental conditions and system-relevant spatial/visual context (e.g. lighting, spatial anchors, object states).
        /// </summary>
        Environment,

        /// <summary>
        /// Physiological and psychometric measurements (e.g. heart rate, GSR, EEG), possibly from biosensors.
        /// </summary>
        Physiology,

        /// <summary>
        /// General user input, including button presses, trigger pulls, UI selection, and controller events.
        /// </summary>
        Input,

        /// <summary>
        /// Locomotion and spatial navigation data (e.g. walking, teleporting, rotation).
        /// </summary>
        Locomotion,

        /// <summary>
        /// Cognitive state indicators or inferred mental processes, such as load, memory use, or problem-solving behavior.
        /// </summary>
        Cognition,

        /// <summary>
        /// Collaboration and interaction between users or agents, such as communication or co-manipulation of objects.
        /// </summary>
        Collaboration,

        /// <summary>
        /// Task-related activities such as starting, completing, or retrying a learning task or challenge.
        /// </summary>
        Task,

        /// <summary>
        /// Feedback delivered to the user, e.g. hints, corrections, reinforcement, or scaffolding.
        /// </summary>
        Feedback,

        /// <summary>
        /// Movement- or object-related actions using virtual tools (e.g. grabbing, rotating, activating devices).
        /// </summary>
        ToolUse,

        /// <summary>
        /// Speech-related interaction, including spoken commands, verbal feedback, or analysis of speech content.
        /// </summary>
        Speech,

        /// <summary>
        /// Errors, failed actions, or invalid interactions captured during task execution or interaction.
        /// </summary>
        Error,

        /// <summary>
        /// Performance metrics or evaluation outcomes, such as scores, completion time, or success rates.
        /// </summary>
        Performance,

        /// <summary>
        /// Contextual metadata, such as scenario settings, level info, environment configuration, or session parameters.
        /// </summary>
        Context,

        /// <summary>
        /// Data from external assistance systems (e.g. coaching tools, researcher annotations, automated tutors).
        /// </summary>
        Assistance,

        /// <summary>
        /// Unclassified or miscellaneous events that do not fit into predefined categories.
        /// </summary>
        Other
    }
}