using System.Data;

namespace AzureKeyVaultComparerLibrary
{
    internal class ReportOutputHandler
    {
        internal static void PrintResult(DataTable data)
        {
            Console.WriteLine();
            Dictionary<string, int> colWidths = new Dictionary<string, int>();

            foreach (DataColumn col in data.Columns)
            {

                Console.Write(col.ColumnName ?? "-");
                var maxLabelSize = data.Rows.OfType<DataRow>()
                        .Select(m => (m.Field<object>(col.ColumnName)?.ToString() ?? "").Length)
                        .OrderByDescending(m => m).FirstOrDefault();

                colWidths.Add(col.ColumnName, maxLabelSize);
                for (int i = 0; i < maxLabelSize - col.ColumnName.Length + 10; i++) Console.Write(" ");
            }

            Console.WriteLine();

            foreach (DataRow dataRow in data.Rows)
            {
                for (int j = 0; j < dataRow.ItemArray.Length; j++)
                {
                    if (dataRow.ItemArray[j].GetType() == typeof(DBNull))
                    {
                        Console.Write("-");
                        for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - 1 + 10; i++) Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(dataRow.ItemArray[j]);
                        for (int i = 0; i < colWidths[data.Columns[j].ColumnName] - dataRow.ItemArray[j].ToString().Length + 10; i++) Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        internal static void SaveCSV(DataTable dtDataTable, string strFilePath)
        {
            StreamWriter sw = new StreamWriter(strFilePath);
            //headers    
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sw.Write(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }
            sw.Write(sw.NewLine);
            foreach (DataRow dr in dtDataTable.Rows)
            {
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        }
                        else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
            var fullPath = Path.GetFullPath(strFilePath);
            Console.WriteLine("Exported data to :" + fullPath);
        }
    }
}
