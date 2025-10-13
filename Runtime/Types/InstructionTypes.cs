namespace OmiLAXR.Types
{
    public enum InstructionTypes
    {
        Text,           // Plain or formatted text instructions
        Audio,          // Voice recordings, podcasts, TTS
        Image,          // Static visual instructions
        Video,          // Pre-recorded or streaming video
        Interactive,    // Simulations, AR/VR, guided wizards
        Live,           // Live sessions (video, audio, or in-person)
        Data            // Machine-readable formats (JSON, XML, scripts, etc.)
    }
}