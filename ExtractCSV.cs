using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace ParseText
{
	public class ExtractCSV
    {
        public static void ExtractCsvData3()
        {
            string cs = @"server=serverName;userid=user;password=password;database=lookup";
            MySqlConnection conn = null;
            conn = new MySqlConnection(cs);
            conn.Open();

            var fileName = @"C:\Users\dayto\Downloads\2023 Code Descriptions in Tabular Order\icd10cm_order_addenda_2023.txt";
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = Encoding.UTF8,
                Delimiter = "||",
                BadDataFound = null,
                MissingFieldFound = null,
                HasHeaderRecord = false
            };

            try
            {
                using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (var textReader = new StreamReader(fs, Encoding.UTF8))
                    using (var csv = new CsvReader(textReader, configuration))
                    {
                        var data = csv.GetRecords<ColumnName>().ToList();
                        data.ForEach(x =>
                        {
                            x.Action = x.Action.Trim();
                            x.Status = x.Status.Trim();
                            x.Code = x.Code.Trim();
                            x.ShortCode = x.ShortCode.Trim();
                            x.LongCode = x.LongCode.Trim();

                        });
                        int count = 0;

                        StringBuilder sb = new StringBuilder();
                        StringBuilder update = new StringBuilder();
                        sb.AppendLine(
                            "INSERT INTO icd10code10012023 (Code, Billable, Levels, ShortDescription, LongDescription, Chapter, SubCategory) VALUES");
                        foreach (var d in data)
                        {
                            if (d.Action.Equals("Add:", StringComparison.InvariantCultureIgnoreCase))
                            {
                                int levels = d.Code.Length - 3;
                                string stm = $@"select i.Code, i.Chapter, i.SubCategory from dbname.icd10code10012021 i where i.Code like '{d.Code.Substring(0, 3)}%' limit 1;";

                                using (var cmd = new MySqlCommand(stm, conn))
                                {
                                    using (var reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            var chapter = reader["Chapter"];
                                            var subCategory = reader["SubCategory"];
                                            sb.AppendLine($@"('{d.Code}',{d.Status}, {levels}, '{d.ShortCode}', '{d.LongCode}', {chapter}, {subCategory}),");
                                        }
                                    }
                                }

                                count += 1;
                            }
                        }

                        foreach (var d in data)
                        {
                            if (d.Action.Equals("Revise to:", StringComparison.InvariantCultureIgnoreCase))
                            {
                                update.AppendLine($@"UPDATE `icd10code10012023` SET `ShortDescription`='{d.ShortCode}', `LongDescription`='{d.LongCode}' 
                                    WHERE `Code`= '{d.Code}'; ");
                                count += 1;
                            }
                        }

                        var queryw = sb.ToString();
                        var updatequery = update.ToString();
                        var serializedData = JsonConvert.SerializeObject(data);
                        File.WriteAllText(@"C:\Users\dayto\Downloads\2023 Code Descriptions in Tabular Order\ParseText.txt", serializedData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex.ToString());
            }
            finally
            {

                if (conn != null)
                {
                    conn.Close();
                }

            }
        }
    }
}