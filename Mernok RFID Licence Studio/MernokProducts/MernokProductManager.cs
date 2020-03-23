using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MernokProducts
{
    public class MernokProductManager
    {
        public static string CreateMernokProductFile(MernokProductFile f)
        {
            string result = "File created succesfully";
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MernokProductFile));
                using (TextWriter writer = new StreamWriter(@"C:\MernokAssets\MernokProductList.xml"))
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

        public static MernokProductFile ReadMernokProductFile()
        {
            //todo: add exception handling
            //Try Read the XML file
            XmlSerializer deserializer = new XmlSerializer(typeof(MernokProductFile));
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //TextReader reader = new StreamReader(Environment.CurrentDirectory + @"\C2xxParameters.xml");
            TextReader reader = new StreamReader(@"C:\MernokAssets\MernokProductList.xml");
            //           TextReader reader = new StreamReader(filename);//(Environment.CurrentDirectory + @"\C2xxParameters.xml");
            MernokAssetContent = reader.ReadToEnd();
            reader = new StringReader((string)MernokAssetContent.Clone());
            object obj = deserializer.Deserialize(reader);
            MernokProductFile f = (MernokProductFile)obj;
            reader.Close();
            return f;
        }



    }
}
