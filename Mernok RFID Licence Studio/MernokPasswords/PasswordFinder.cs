using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MernokPasswords
{
    public class PasswordFinder
    {
        public static bool FindPasswordinFile(string Password, MernokPasswordFile f)
        {
            foreach (var item in f.mernokPasswordList)
            {
                if (item.Password == Password)
                    return true;
            }
            return false;
        }

    }
}
