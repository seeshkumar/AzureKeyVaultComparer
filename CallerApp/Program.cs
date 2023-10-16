List<string> keyVaultNames = new List<string>();
keyVaultNames.Add("keyvault1");
keyVaultNames.Add("keyvault2");
keyVaultNames.Add("keyvault3");
keyVaultNames.Add("keyvault4");
AzureKeyVaultComparerLibrary.AzureKeyVaultComparer.Compare(keyVaultNames, showSecrets : false,outputFormat:AzureKeyVaultComparerLibrary.OutputFormat.Console);
