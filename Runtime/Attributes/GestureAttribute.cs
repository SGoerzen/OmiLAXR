namespace OmiLAXR
{
    [System.AttributeUsage(System.AttributeTargets.Event | System.AttributeTargets.Field, AllowMultiple = true)]
    public class GestureAttribute : System.Attribute
    {
        public readonly string Name;

        public GestureAttribute(string name)
        {
            Name = name;
        }
    }
}