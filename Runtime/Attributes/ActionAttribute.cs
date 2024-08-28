namespace OmiLAXR
{
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ActionAttribute : System.Attribute
    {
        public readonly string Name;

        public ActionAttribute(string name)
        {
            Name = name;
        }
    }
}