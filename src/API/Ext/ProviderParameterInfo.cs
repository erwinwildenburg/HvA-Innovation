namespace API.Ext
{
    public class ProviderParameterInfo
    {
        public ProviderParameterInfo(string name, bool isRequired = false)
        {
            Name = name;
            IsRequired = isRequired;
        }

        public string Name { get; }

        public bool IsRequired { get; }
    }
}
