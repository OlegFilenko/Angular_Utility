using Angular_Utility.Enums;
using System;
using System.Collections.Generic;
using System.IO;

namespace Angular_Utility.Data {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| GenerateComponentData |==========================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class GenerateComponentData

    public sealed class GenerateComponentData {
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        public readonly string
            name,
            path,
            parentModule;

        public readonly bool
            flat,
            constructorInject,
            settingStore;

        public readonly NgComponent type;

        public NgElement[] elementsSet => _set.ToArray();
        public bool isValid => _isValid;

        #endregion
        //=================================================== PRIVATE_VARIABLES ================================================>
        #region Private_Variables

        private NgElement[][] _setsArray = new NgElement[][]{
            new NgElement[]{ NgElement.component, NgElement.module, NgElement.style },
            new NgElement[]{ NgElement.component, NgElement.module, NgElement.style, NgElement.html },
            new NgElement[]{ NgElement.component, NgElement.module, NgElement.style, NgElement.html, NgElement.moduleRouting },
        };

        private List<NgElement> _set = new List<NgElement>();
        private bool _isValid = true;

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        public GenerateComponentData(QueryData queryData_) {
            name = queryData_.getValue("name");
            path = queryData_.getValue("path");

            if(name == null || path == null) {
                _isValid = false;
                return;
            }

            path = Utility.projectPath + Utility.normalisePath(path, true);

            if(queryData_.dataDictionary.TryGetValue("parentModule", out string lParent)) {
                parentModule = Utility.normalisePath(lParent);
            } else {
                parentModule = "";
            }

            if(queryData_.dataDictionary.TryGetValue("type", out string lType)) {
                type = (NgComponent)Enum.Parse(typeof(NgComponent), lType);
                if(type == NgComponent.dialog && parentModule != "") {
                    Utility.setToModule(parentModule, "imports", path, name);
                }
            } else {
                type = NgComponent.component;
            }


            flat = queryData_.getBooleanValue("flat");
            settingStore = queryData_.getBooleanValue("settingStore");
            constructorInject = queryData_.getBooleanValue("constructorInject");

            if(!flat) { path += (name + "/"); }

            _selectElementsSet(type);
        }

        #endregion
        //===================================================== PRIVATE_METHODS ================================================>
        #region Private_Methods

        //------------| SELECT_ELEMENT_SET |-------------------------------------------------------------------------------------
        private void _selectElementsSet(NgComponent componentType_) {

            switch(componentType_) {
                case NgComponent.abstr:
                    _set = new List<NgElement>(_setsArray[0]);
                    break;
                case NgComponent.component:
                    _set = new List<NgElement>(_setsArray[1]);
                    break;
                case NgComponent.page:
                    _set = new List<NgElement>(_setsArray[2]);
                    break;
                case NgComponent.table:
                    _set = new List<NgElement>(_setsArray[1]);
                    break;
                case NgComponent.tableAPI:
                    _set = new List<NgElement>(_setsArray[1]);
                    break;
                case NgComponent.tableClient:
                    _set = new List<NgElement>(_setsArray[1]);
                    break;
                case NgComponent.dialog:
                    _set = new List<NgElement>(_setsArray[1]);
                    break;
                case NgComponent.dialogBase:
                    _set = new List<NgElement>(_setsArray[1]);
                    break;
            }

            if(settingStore) {
                _set.Add(NgElement.service);
            }
        }

        #endregion

    }
    #endregion
}
