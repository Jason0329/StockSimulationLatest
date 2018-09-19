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
            

            TEJImport importTEJData = new TEJImport("技術面.txt");
            List<string> importedDataList = importTEJData.Import();

            int count = 0;
            int addCount = 0;
            string insertListCommnad = "";

            List<string> insertCommandList = new List<string>();
            //string insertCommand;
            Parallel.ForEach(importedDataList, (importedData, loopState) => 
            //foreach ( var importedData in importedDataList)
            {
                
                string data = importedData.Replace("\t", ",");
                string datetime = data.Split(',')[2].Trim();

                //string monthRevenuePublishDatetime = data.Split(',')[2].Trim();
                //try
                //{
                //    monthRevenuePublishDatetime = monthRevenuePublishDatetime.Insert(6, "-");
                //    monthRevenuePublishDatetime = monthRevenuePublishDatetime.Insert(4, "-");
                //}
                //catch(Exception e)
                //{

                //}

                datetime = datetime.Insert(6, "-");
                datetime = datetime.Insert(4, "-");// + "10";
                string ID = data.Split(',')[0].Trim() + data.Split(',')[2].Trim();

                string insertCommand = "INSERT INTO [StockDatabase].[dbo].[TechnologicalDataModels] VALUES " +
                    "('" + ID + "','" + data.Split(',')[0].Trim().TrimStart(new char[] { 'T', 'W', 'N' }) + "','" + data.Split(',')[1].Trim() +
                    "','" + datetime +    "',"+//   "'," + "'" + monthRevenuePublishDatetime + "'," +
                    data.Split(new char[] { ',' }, 4)[3].Replace("\t", "").Replace(" ", "").Replace(",,", ",null,")
                    .Replace(",-,", ",null,") + ");";
                insertCommand = insertCommand.Replace(",-,", ",null,").Replace("-)", "null)").Replace(",NTD,", ",'NTD',").Replace(",H,", ",'H',").Replace(",Q,", ",'Q',").Replace(",Y,", ",'Y',").Replace(",N,", ",'N',").Replace("'s", "s").Replace(",,", ",null,");
                //insertListCommnad += insertCommand;
                insertCommandList.Add(insertCommand);

                count++;


                if (count % 10000 == 0)
                    Console.WriteLine(count);
            });

            int AllData = (int)((insertCommandList.Count / 5000) );

            Parallel.For(0, AllData , i => { InsertDatabase(ref insertCommandList, i * 5000, (i + 1) * 5000); });

        }

        static void InsertDatabase(ref List<string> InsertCommandList, int StartIndex, int EndIndex)
        {

            string insertCommnad = "";
            SQLServerConnectionService sqlServerConnectionService = new SQLServerConnectionService();
            for (int i = StartIndex; i < EndIndex; i++)
            {
                insertCommnad += InsertCommandList[i];

                if (i % 2000 == 0)
                {
                    try
                    {
                        sqlServerConnectionService.CreateInstance(insertCommnad);
                        insertCommnad = "";
                        Console.WriteLine(StartIndex + "~" + i + " Succeed");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }

            try
            {
                sqlServerConnectionService.CreateInstance(insertCommnad);
                Console.WriteLine(StartIndex + "~" + EndIndex + " Succeed");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }
    }
}
