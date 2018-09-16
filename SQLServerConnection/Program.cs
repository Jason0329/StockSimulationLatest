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

            TEJImport importTEJData = new TEJImport("StockMarket20000914.txt");
            List<string> importedDataList = importTEJData.Import();

            int count = 0;
            int addCount = 0;
            string insertListCommnad = "";
            List<string> cc = new List<string>();
            //string insertCommand;
            Parallel.ForEach(importedDataList, (importedData, loopState) => 
            //foreach ( var importedData in importedDataList)
            {
                
                string data = importedData.Replace("\t", ",");
                string datetime = data.Split(',')[2].Trim();
                datetime = datetime.Insert(6, "-");
                datetime = datetime.Insert(4, "-");
                string ID = data.Split(',')[0].Trim() + data.Split(',')[2].Trim();

                string insertCommand = "INSERT INTO [StockDatabase].[dbo].[TechnologicalDataModels] VALUES " +
                    "('" + ID + "','" + data.Split(',')[0].Trim().TrimStart(new char[] { 'T', 'W', 'N' }) + "','" + data.Split(',')[1].Trim() +
                    "','" + datetime + "'," +
                    data.Split(new char[] { ',' }, 4)[3].Replace("\t", "").Replace(" ", "").Replace(",,", ",null,")
                    .Replace(",-,", ",null,") + ");";
                insertCommand = insertCommand.Replace(",-,", ",null,").Replace("-)", "null)").Replace(",NTD,", ",'NTD',").Replace(",H,", ",'H',").Replace(",Q,", ",'Q',").Replace(",Y,", ",'Y',").Replace(",N,", ",'N',").Replace("'s", "s");
                //insertListCommnad += insertCommand;
                cc.Add(insertCommand);

                count++;

                if (cc.Count > 10000)
                {
                    lock (cc)
                    {
                        if (cc.Count > 10000)
                        {
                            addCount++;
                            //if (addCount % 10000 == 0)
                            for (int i = 0; i < cc.Count; i++)
                            {
                                insertListCommnad += cc[i];
                            }

                            try
                            {

                                sqlServerConnectionService.CreateInstance(insertListCommnad);

                                addCount++;

                                cc.Clear();
                                insertListCommnad = "";
                            }
                            catch (Exception e)
                            { }
                            //}
                        }
                    }
                }

                if (count % 10000 == 0)
                    Console.WriteLine(count);
            });
        }
    }
}
