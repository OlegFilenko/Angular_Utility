﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angular_Utility.models {
    public class GenerateComponentModel : IDataModel {
        public string[] dialogs = new string[]{};
        public bool 
            routing = false,
            flat = false,
            abstr = false,
            settingStore = false;

        public string path { get; set; }
        public string name { get; set; }
    }
}
