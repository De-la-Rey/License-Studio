using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MernokClients
{
    public class MernokClientManager
    {
        public static string CreateMernokClientFile(MernokClientFile f)
        {
            string result = "File created succesfully";
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MernokClientFile));
                using (TextWriter writer = new StreamWriter(@"C:\MernokAssets\MernokClientList.xml"))
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

        public static string MernokAssetContent { get; set; }

        public static MernokClientFile ReadMernokClientFile()
        {
            //todo: add exception handling
            //Try Read the XML file
            XmlSerializer deserializer = new XmlSerializer(typeof(MernokClientFile));
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //TextReader reader = new StreamReader(Environment.CurrentDirectory + @"\C2xxParameters.xml");
            TextReader reader = new StreamReader(@"C:\MernokAssets\MernokClientList.xml");
            //           TextReader reader = new StreamReader(filename);//(Environment.CurrentDirectory + @"\C2xxParameters.xml");
            MernokAssetContent = reader.ReadToEnd();
            reader = new StringReader((string)MernokAssetContent.Clone());
            object obj = deserializer.Deserialize(reader);
            MernokClientFile f = (MernokClientFile)obj;
            reader.Close();
            return f;
        }

    }
}
