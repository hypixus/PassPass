namespace PassPassLib
{
    public class Database
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Version { get; private set; }
        public string Path { get; private set; }
        

        public Database(string filepath, string password)
        {
            //TODO
            importFromFile(filepath, password);
        }

        public Database(string name)
        {
            this.Name = name;
            this.Description = string.Empty;
            this.Path = string.Empty;
            this.Version = 1;
        }

        public void setName(string newName)
        {
            this.Name = newName;
        }

        public void setDescription(string newDesc)
        {
            this.Description = newDesc;
        }

        public void updateVersion()
        {
            //TODO
            //Create algorithm updating between new formats of database. For now unnecessary, due to only one standard.
        }

        private void importFromFile(string filePath, string password)
        {
            //TODO
            string name = "";
            string description = "";
            int version = -1;
            this.Name = name;
            this.Description = description;
            this.Version = version;
        }

    }
}