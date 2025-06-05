namespace OmiLAXR.Composers
{
    public interface IActorComposer
    {
        public Actor Actor { get; }
        public Author Authority { get; }
        public Actor[] GroupMembers { get; }
        public Actor[] TeamMembers { get; }
        public Team Team { get; }
        public Instructor Instructor { get; }
        public Pipeline Pipeline { get; }
    }
}