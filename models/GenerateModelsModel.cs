using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angular_Utility.models {
    class GenerateModelsModel : IDataModel {
        public string fromPath;

        public string[]
            suffixes = new string[] { },
            prefixes = new string[] { };

        public bool table = false;

        public string path { get; set; }
        public string name { get; set; }
    }
}
