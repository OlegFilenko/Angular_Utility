using Angular_Utility.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Angular_Utility.Data {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| GenerateDirectiveData |==========================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class GenerateDirectiveData

    public sealed class GenerateDirectiveData {
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        public readonly string 
            name, 
            path;

        public readonly bool flat;
        public readonly NgElement[] elements = { NgElement.directive, NgElement.module };
        public bool isValid => _isValid;
        #endregion
        //=================================================== PRIVATE_VARIABLES ================================================>
        #region Private_Variables

        private bool _isValid = true;

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        public GenerateDirectiveData(QueryData queryData_) {
            name = queryData_.getValue("name");
            path = queryData_.getValue("path");


            if(name == null || path == null) {
                _isValid = false;
                return;
            }

            path = Utility.projectPath + Utility.normalisePath(path, true);
            flat = queryData_.getBooleanValue("flat");
            if(!flat) { path += (name + "/"); }

        }

        #endregion
    }
    #endregion
}
