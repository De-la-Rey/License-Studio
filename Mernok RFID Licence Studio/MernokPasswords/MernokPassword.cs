using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MernokPasswords
{
    public class MernokPassword
    {
        public string requestName;
        public uint requesterUID;
        public string CreatorName;
        public uint creatorUID;
        public string Password;
        public byte AccessLevel;
    }

    public class MernokPasswordFile
    {
        public List<MernokPassword> mernokPasswordList;
        public UInt16 version;
        public DateTime dateCreated;
        public string createdBy;            //Name of file creator
    }
}
