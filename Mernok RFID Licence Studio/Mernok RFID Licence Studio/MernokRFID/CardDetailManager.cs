using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Mernok_RFID_Licence_Studio
{
    public static class CardDetailManager
    {
        public static string CreateCardDetailFile(CardDetailsFile f)
        {
            string result = "File created succesfully";
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CardDetailsFile));
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\IssuerLicense";
                if (Directory.Exists(path))
                {
                    Console.WriteLine("That path exists already.");
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(path);
                }

                using (TextWriter writer = new StreamWriter(path + @"\IssuerCard.merlic"))
                {
                    serializer.Serialize(writer, f);
                }
            }
            catch (Exception e)
            {
                result = e.ToString();
            }

            return result;
        }

        public static string CardDetailContent { get; set; }
        //todo: Change this to accept a path for the file
        //       public static MernokAssetFile ReadMernokAssetFile(string filename)
        public static CardDetailsFile ReadCardDetailFile(string FileName)
        {
            //todo: add exception handling
            //Try Read the XML file
            XmlSerializer deserializer = new XmlSerializer(typeof(CardDetailsFile));
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //TextReader reader = new StreamReader(Environment.CurrentDirectory + @"\C2xxParameters.xml");
            //TextReader reader = new StreamReader(@"C:\MernokAssets\MernokAssetList.xml");
            TextReader reader = new StreamReader(FileName);//(Environment.CurrentDirectory + @"\C2xxParameters.xml");
            CardDetailContent = reader.ReadToEnd();
            reader = new StringReader((string)CardDetailContent.Clone());
            object obj = deserializer.Deserialize(reader);
            CardDetailsFile f = (CardDetailsFile)obj;
            reader.Close();
            return f;
        }
    }
}
