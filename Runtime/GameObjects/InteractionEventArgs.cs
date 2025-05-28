namespace OmiLAXR
{
    /// <summary>
    /// Contains data about the pointer interaction events.
    /// </summary>
    public struct InteractionEventArgs
    {
        /// <summary>
        /// Total number of presses recorded since this component was initialized.
        /// </summary>
        public int TotalPresses { get; set; }
            
        /// <summary>
        /// Number of presses recorded during the current hover.
        /// </summary>
        public int PressesInHover { get; set; }
            
        /// <summary>
        /// Duration in seconds that the pointer has been hovering over this element.
        /// </summary>
        public float HoverDuration { get; set; }
            
        /// <summary>
        /// Duration in seconds that the pointer has been pressing this element.
        /// </summary>
        public float PressDuration { get; set; }
    }
}