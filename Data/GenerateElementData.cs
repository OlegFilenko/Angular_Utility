using Angular_Utility.Dictionaries;
using Angular_Utility.Enums;
using System;

namespace Angular_Utility.Data {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| GenerateElementData |=============================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class GenerateElementData

    public sealed class GenerateElementData {
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        public readonly string
            name,
            path;

        public readonly NgElement _type;

        public string content { get; private set; } = string.Empty;

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        public GenerateElementData(QueryData queryData_) {
            if(queryData_.dataDictionary.TryGetValue("type", out string lType)) {
                _type = (NgElement)Enum.Parse(typeof(NgElement), lType);
            } else {
                _type = NgElement.component;
            }
            name = queryData_.getValue("name") + ExtensionDict.value(_type);
            path = Utility.projectPath + Utility.normalisePath(queryData_.getValue("path", true),true);
            _generateContent();
        }

        public GenerateElementData(NgElement type_, string name_, string path_, object data_ = null) {
            _type = type_;
            name = name_ + ExtensionDict.value(_type);
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
