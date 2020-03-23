using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MernokPasswords
{

    public class PasswordDecriptor
    {

 //       private string _PassWordOut;

        public static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        public static string[] PasswordToDetails(string Password)
        {
            string requestName;
            uint _requesterUID;
            string _CreatorName;
            uint _creatorUID;

            byte[] PassBytes = StringToByteArray(Password);
            
            string[] details = new string[4];

            var index = PassBytes.Where(n => n == (byte)'$').First();

            var NewArray = System.Text.Encoding.Default.GetString(PassBytes.Take(index).ToArray());

           _requesterUID = uint.Parse(NewArray.Substring(0,NewArray.IndexOf('$')), System.Globalization.NumberStyles.HexNumber);
            requestName = NewArray.Substring(NewArray.IndexOf('$')+1, NewArray.IndexOf('%')-2);
            _creatorUID = uint.Parse(NewArray.Substring(NewArray.IndexOf('#')+1, NewArray.LastIndexOf(NewArray.Last())- NewArray.IndexOf('#')), System.Globalization.NumberStyles.HexNumber);
            _CreatorName = NewArray.Substring(NewArray.IndexOf('%')+1, NewArray.IndexOf('#')- NewArray.IndexOf('%')-1);


            details[0] = _requesterUID.ToString();
            details[1] = requestName;
            details[2] = _creatorUID.ToString();
            details[3] = _CreatorName;


            return details;
        }

 
    }
}
