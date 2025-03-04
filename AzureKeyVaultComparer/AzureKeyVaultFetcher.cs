
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace AzureKeyVaultComparerLibrary
{
    internal class AzureKeyVaultFetcher
    {
        static int instace = 0;
        internal static async Task<Dictionary<string, string>> FetchKeyVaultSecretsAsync(Uri keyVaultUri)
        {
            /*try
            {

                var client = new SecretClient(keyVaultUri, new DefaultAzureCredential());
                var secrets = client.GetPropertiesOfSecrets();
                var secretValues = new Dictionary<string, string>();

                foreach (var secretProperties in secrets)
                {
                    var secret = await client.GetSecretAsync(secretProperties.Name);
                    secretValues.Add(secretProperties.Name, secret.Value.Value);
                }
                return secretValues;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching secrets: {ex.Message}");
                return null;
            }*/



            /*JSON DATA FOR TESTING*/
            instace++;
            var secretValues = new Dictionary<string, string>();
            if (instace == 1)
            {
                secretValues.Add("secret1", "value1");
                secretValues.Add("secret2", "value2");
            }
            if (instace == 2)
            {
                secretValues.Add("secret1", "value1");
                secretValues.Add("secret2", "value4");
                secretValues.Add("secret3", "value3");
            }
            if (instace == 3)
            {
                secretValues.Add("secret2", "value1");
                secretValues.Add("secret5", "value6");
                secretValues.Add("secret1", "value2");
            }
            if (instace != 4)
            {

                secretValues.Add("secret6", "value6");
            }

            return secretValues;
        }

    }
}
