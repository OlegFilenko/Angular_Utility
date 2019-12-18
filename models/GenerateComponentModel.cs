using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angular_Utility.models {
    public class GenerateComponentModel {
        public string
            name,
            path;
        public string[] dialogs = new string[]{};
        public bool 
            routing = false,
            flat = false,
            abstr = false,
            settingStore = false;
    }
}
