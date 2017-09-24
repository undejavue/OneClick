using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ClassLibrary.Excel
{
    public class XmlProcessor
    {

        public static void ProcessXml(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (var file in di.GetFiles())
            {

                XDocument xmlFile = XDocument.Load(file.FullName);


                var nameElement = xmlFile.Descendants("AttributeList").ElementAt(0).Descendants().FirstOrDefault(el => el.Name == "Name");
                var address = xmlFile.Descendants("AttributeList").ElementAt(0).Descendants().FirstOrDefault(el => el.Name == "Number");
                var value = address.Value.ToString();
                var udt = value[2];
                var symbol = "X";
                switch (udt)
                {
                    case '1':
                        symbol = "A";
                        break;
                    case '2':
                        symbol = "B";
                        break;
                    case '3':
                        symbol = "D";
                        break;
                    case '4':
                        symbol = "Y";
                        break;
                    case '5':
                        symbol = "M";
                        break;
                    case '7':
                        symbol = "MXR";
                        break;
                    case '9':
                        symbol = "SNC";
                        break;
                }

                var newName = value.Substring(0, 2) + symbol;

                if (nameElement != null)
                {
                    nameElement.Value = newName;
                }

                xmlFile.Save(file.FullName);
            }
        }
    }
}
