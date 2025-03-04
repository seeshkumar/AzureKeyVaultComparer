
namespace AzureKeyVaultComparerLibrary.Models
{
    public class KeyVault
    {
        public Dictionary<string, string> Secrets { get; set; } //<secret name, secret value>
        public string Name { get; set; }
    }
}
