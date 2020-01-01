using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angular_Utility.models {
    interface IDataModel {
        string path { get; set; }
        string name { get; set; }
    }
}
