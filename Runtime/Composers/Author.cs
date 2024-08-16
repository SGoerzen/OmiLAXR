namespace OmiLAXR.Composers
{
    public struct Author
    {
        public string Name { get; private set; }
        public string Email { get; private set; }

        public Author(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}