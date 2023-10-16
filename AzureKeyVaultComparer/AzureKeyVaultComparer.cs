using AzureKeyVaultComparerLibrary.Models;
using System.Data;
using System.Text;

namespace AzureKeyVaultComparerLibrary
{
    public enum OutputFormat
    {
        Console,
        Csv
    }
    public class AzureKeyVaultComparer
    {
        public static DataTable Compare(List<string> keyVaultNames, bool showSecrets = false, OutputFormat outputFormat = OutputFormat.Console, string outputPath = "report.csv")
        {
            try
            {

                var keyValults = GetAllKeyVaults(keyVaultNames);
                var result = GenerateResult(keyValults, showSecrets);
                DataTable resultTable = GenerateResultTable(result, keyVaultNames);

                if (outputFormat == OutputFormat.Csv)
                {
                    ReportOutputHandler.SaveCSV(resultTable, outputPath);
                }
                else if (outputFormat == OutputFormat.Console)
                {
                    ReportOutputHandler.PrintResult(resultTable);
                }

                return resultTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        //<secretKey, <keyvaultName, secretValue>>
        private static Dictionary<string, Dictionary<string, string>> GenerateResult(List<KeyVault> keyVaults, bool showSecrets)
        {
            var result = new Dictionary<string, Dictionary<string, string>>();
            var track = new Dictionary<string, string>(); //<secretValue, uniqueCode(Identifier)>
            foreach (var keyVault in keyVaults)
            {
                foreach (var secret in keyVault.Secrets)
                {
                    if (!track.ContainsKey(secret.Value))
                    {
                        string trackValue = new string('*', track.Count + 1);
                        track.Add(secret.Value, trackValue);
                    }

                    if (!result.ContainsKey(secret.Key))
                    {
                        var tempDict = new Dictionary<string, string>
                        {
                            { keyVault.Name, (showSecrets ? secret.Value : track[secret.Value]) }
                        };
                        result.Add(secret.Key, tempDict);
                    }
                    else
                    {
                        if (showSecrets)
                        {
                            result[secret.Key].Add(keyVault.Name, secret.Value);
                        }
                        else
                        {
                            result[secret.Key].Add(keyVault.Name, track[secret.Value]);
                        }
                    }
                }
            }
            return result;
        }

        private static DataTable GenerateResultTable(Dictionary<string, Dictionary<string, string>> result, List<string> keyVaultNames)
        {

            var resultTable = new DataTable();
            resultTable.Columns.Add("Secret Name", typeof(string));

            keyVaultNames.Sort();

            foreach (var keyVaultName in keyVaultNames)
            {
                resultTable.Columns.Add(keyVaultName, typeof(string));
            }


            foreach (var secretName in result.Keys.OrderBy(k => k).Select(k => k))
            {
                var row = resultTable.NewRow();
                row["Secret Name"] = secretName;

                foreach (var pair in result[secretName])
                {
                    row[pair.Key] = pair.Value;
                }
                resultTable.Rows.Add(row);
            }
            return resultTable;
        }
        private static List<KeyVault> GetAllKeyVaults(List<string> keyVaultNames)
        {
            var keyValults = new List<KeyVault>();
            foreach (var keyVaultName in keyVaultNames)
            {
                var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net");
                KeyVault keyVault = new KeyVault() { Name = keyVaultName, Secrets = AzureKeyVaultFetcher.FetchKeyVaultSecretsAsync(keyVaultUri).Result };
                keyValults.Add(keyVault);
            }
            return keyValults;
        }

    }

}