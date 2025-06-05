namespace OmiLAXR.Composers
{
    public interface IStatement
    {
        public void SetActor(Actor actor);
        public void SetInstructor(Instructor instructor);
        IComposer GetComposer();
        bool IsDiscarded();
        void Discard();

        string ToDataStandardString();
        PipelineInfo GetSenderPipelineInfo();
    }
}