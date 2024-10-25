namespace OmiLAXR.Composers
{
    public interface IStatement
    {
        public bool IsDiscarded();
        public void Discard();

        public string ToDataStandardString();
    }
}