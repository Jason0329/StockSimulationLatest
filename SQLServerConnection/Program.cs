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

            TEJImport importTEJData = new TEJImport("Tech20180824-2.txt");
            List<string> importedDataList = importTEJData.Import();

            int count = 0;
            object cc = new object();
            //string insertCommand;
            Parallel.ForEach(importedDataList, (importedData, loopState) => 
            //foreach ( var inputdata in importedDataList)
            {
                try
                {
                    string data = importedData.Replace("\t", ",");
                    string datetime = data.Split(',')[2].Trim();
                    datetime = datetime.Insert(6, "-");
                    datetime = datetime.Insert(4, "-") ;
                    string ID = data.Split(',')[0].Trim() + data.Split(',')[2].Trim();

                    string insertCommand = "INSERT INTO [StockDatabase].[dbo].[TechnologicalDataModels] VALUES " +
                        "('" + ID + "','" + data.Split(',')[0].Trim().TrimStart(new char[] { 'T', 'W', 'N' }) + "','" + data.Split(',')[1].Trim() +
                        "','" + datetime + "'," +
                        data.Split(new char[] { ',' }, 4)[3].Replace("\t", "").Replace(" ", "").Replace(",,", ",null,")
                        .Replace(",-,", ",null,") + ");";
                    insertCommand = insertCommand.Replace(",-,", ",null,").Replace("-)", "null)").Replace(",NTD,", ",'NTD',").Replace(",H,", ",'H',").Replace(",Q,", ",'Q',").Replace(",Y,", ",'Y',").Replace(",N,", ",'N',").Replace("'s", "s");

                    //lock (cc)
                    //{
                        sqlServerConnectionService.CreateInstance(insertCommand);
                    //}
                    count++;
                }
                catch (Exception e)
                { }

                if (count % 100 == 0)
                    Console.WriteLine(count);
            });
        }
    }
}
