using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerConnection.TEJFormatImport
{
    class TEJImport
    {
        string _fileName;
        public TEJImport(string fileName)
        {
            _fileName = fileName;
        }

        public List<string> Import()
        {
            List<string> ImportData = new List<string>();
            using (StreamReader TEJReader = new StreamReader(_fileName , Encoding.GetEncoding("Big5")))
            {
                TEJReader.ReadLine();
                while (TEJReader.Peek() != -1)
                {
                    ImportData.Add(TEJReader.ReadLine());
                }
            }

            return ImportData;
        }
    }
}
