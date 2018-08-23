using SQLServerConnection.SQLServerConnection;
using SQLServerConnection.TEJFormatImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerConnection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            SQLServerConnectionService sqlServerConnectionService = new SQLServerConnectionService();

            TEJImport importTEJData = new TEJImport("Finance.txt");
            List<string> importedDataList = importTEJData.Import();

            int count = 0;
            Parallel.ForEach(importedDataList, (importedData, loopState) => 
            {
                string data = importedData.Replace("\t", ",");
                string datetime = data.Split(',')[2].Trim();
                datetime = datetime.Insert(6, "-");
                datetime = datetime.Insert(4, "-");
                string ID = data.Split(',')[0].Trim() + data.Split(',')[2].Trim();
                try
                {
                    string insertCommand = "INSERT INTO [StockDatabase].[dbo].[BasicFinancialDataModels] VALUES " +
                        "('" + ID + "','" + data.Split(',')[0].Trim().TrimStart(new char[] { 'T', 'W', 'N' }) + "','" + data.Split(',')[1].Trim() +
                        "','" + datetime + "'," +
                        data.Split(new char[] { ',' }, 4)[3].Replace("\t", "").Replace(" ", "").Replace(",,", ",null,")
                        .Replace(",-,", ",null,") + ");";
                    insertCommand = insertCommand.Replace(",-,", ",null,").Replace("-)", "null)");
                    sqlServerConnectionService.CreateInstance(insertCommand);
                    count++;
                }
                catch(Exception e)
                { }

                if (count % 5000 == 0)
                    Console.WriteLine(count);
            });
        }
    }
}
