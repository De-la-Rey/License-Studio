using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MernokProducts
{
    public class MernokProduct
    {
        public byte Product;
        public string ProductName;

    }

    public class MernokProductFile
    {
        public List<MernokProduct> mernokProductList;
        public UInt16 version;
        public DateTime dateCreated;
        public string createdBy;
    }
}
