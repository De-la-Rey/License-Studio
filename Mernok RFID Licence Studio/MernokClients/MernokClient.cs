using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MernokClients
{
    public class MernokClient   
    {
        public byte Client;
        public byte Group;
        public string ClientSiteName;
        public string ClientGroupName;
    }

    public class MernokClientFile
    {
        public List<MernokClient> mernokClientList;
        public UInt16 version;
        public DateTime dateCreated;
        public string createdBy;
    }
}
