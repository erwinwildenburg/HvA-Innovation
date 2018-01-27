namespace API.Configuration
{
    public abstract class ExtSettings
    {
        public bool ReplaceExtAssemblies { get; set; }

        public string[] SearchAssemblies { get; set; }

        public bool ReplaceExtPaths { get; set; }

        public string[] SearchPaths { get; set; }
    }
}
