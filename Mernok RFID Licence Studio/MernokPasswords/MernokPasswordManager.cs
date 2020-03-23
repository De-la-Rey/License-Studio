using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MernokPasswords
{
    public static class MernokPasswordManager
    {
        public static string CreateMernokAssetFile(MernokPasswordFile f)
        {
            string result = "File created succesfully";
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(MernokPasswordFile));
                System.IO.Directory.CreateDirectory(@"C:\Passwords");
                using (TextWriter writer = new StreamWriter(@"C:\Passwords\MernokPasswordMasterList.xml"))
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

        public static string MernokPasswordContent { get; set; }

        public static MernokPasswordFile ReadMernokPasswordFile(string path)
        {
            //todo: add exception handling
            //Try Read the XML file
            XmlSerializer deserializer = new XmlSerializer(typeof(MernokPasswordFile));
            string appPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            //TextReader reader = new StreamReader(Environment.CurrentDirectory + @"\C2xxParameters.xml");
            TextReader reader = new StreamReader(path);
            //           TextReader reader = new StreamReader(filename);//(Environment.CurrentDirectory + @"\C2xxParameters.xml");
            MernokPasswordContent = reader.ReadToEnd();
            reader = new StringReader((string)MernokPasswordContent.Clone());
            object obj = deserializer.Deserialize(reader);
            MernokPasswordFile f = (MernokPasswordFile)obj;
            reader.Close();
            return f;
        }

    }
}
