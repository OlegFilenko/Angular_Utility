using Angular_Utility.Enums;
using System;
using System.Collections.Generic;

namespace Angular_Utility.Data {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| GenerateElementData |=============================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class GenerateElementData

    public sealed class GenerateElementData {
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        private static IReadOnlyDictionary<NgElement, string> _extensionDictionary;

        public readonly string
            name,
            path;

        public readonly NgElement lType;

        public string content { get; private set; } = string.Empty;

        #endregion
        #region Private_Variables

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        static GenerateElementData() {
            Dictionary<NgElement, string> lDict = new Dictionary<NgElement, string>();
            lDict[NgElement.component] = ".component.ts";
            lDict[NgElement.html] = ".component.html";
            lDict[NgElement.style] = ".component.scss";
            lDict[NgElement.module] = ".module.ts";
            lDict[NgElement.moduleRouting] = "-routing.module.ts";
            lDict[NgElement.model] = ".model.ts";
            lDict[NgElement.service] = ".service.ts";
            lDict[NgElement.directive] = ".directive.ts";
            lDict[NgElement.validator] = ".validator.ts";
            lDict[NgElement.guard] = ".guard.ts";
            lDict[NgElement.interceptor] = ".interceptor.ts";
            lDict[NgElement.animation] = ".animation.ts";
            lDict[NgElement.ngEnum] = ".enum.ts";
            lDict[NgElement.ngClass] = ".ts";
            lDict[NgElement.ngInterface] = ".ts";

            _extensionDictionary = lDict;
        }

        public GenerateElementData(QueryData queryData_) {
            if(queryData_.dataDictionary.TryGetValue("type", out string lType)) {
                this.lType = (NgElement)Enum.Parse(typeof(NgElement), lType);
            } else {
                this.lType = NgElement.component;
            }
            name = queryData_.getValue("name") + _extensionDictionary[this.lType];
            path = Utility.toValidPath(queryData_.getValue("path"));
            _generateContent();
        }

        public GenerateElementData(NgElement type_, string name_, string path_, object data_ = null) {
            lType = type_;
            name = name_ + _extensionDictionary[lType];
            path = path_;
            _generateContent(data_);
        }

        #endregion
        //===================================================== PRIVATE_METHODS ================================================>
        #region Private_Methods

        //------------| GENERATE_CONTENT |-------------------------------------------------------------------------------------
        private void _generateContent(object data_ = null) {
            content = FileContentData.getContent(this, data_);
        }

        #endregion
    }
    #endregion
}
