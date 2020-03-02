using Angular_Utility.Enums;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Angular_Utility.Data {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| QueryData |======================================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class QueryData

    public sealed class QueryData {
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        public readonly string
            name,
            path;
        public readonly NgAction actionType;
        public IReadOnlyDictionary<string, string> dataDictionary;

        #endregion
        //=================================================== PRIVATE_VARIABLES ================================================>
        #region Private_Variables

        private readonly string

            _generateComponentPattern = "(generate[-|_]component|gen[-|_]component|g[-|_]component|gen[-|_]comp|g[-|_]comp)",
            _generateElementPattern = "(generate[-|_]element|gen[-|_]element|g[-|_]element|gen[-|_]el|g[-|_]el)",
            _deleteComponentPattern = "(remove[-|_]component|rem[-|_]component|rm[-|_]component|rem[-|_]comp|rm[-|_]comp)",
            _renameComponentPattern = "(rename[-|_]component|ren[-|_]component|rn[-|_]component|ren[-|_]comp|rn[-|_]comp)",
            _generateModelPattern = "(generate[-|_]models|gen[-|_]models|g[-|_]models|gen[-|_]mds|g[-|_]mds)",
            _reflectModelsPattern = "(reflect[-|_]models|ref[-|_]models|ref[-|_]mod)";

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        public QueryData(string query_) {
            string[] lQueryPartsArr = query_.Split(new string[] { " -" }, StringSplitOptions.None);
            Dictionary<string, string> lDict = new Dictionary<string, string>();

            for(int i = 1; i < lQueryPartsArr.Length; i++) {
                string lPart = lQueryPartsArr[i];

                string[] lPropParts = lPart.Split('=');
                if(lPropParts.Length == 1) {
                    lDict.Add(lPropParts[0], "true");
                } else if(lPropParts.Length == 2) {
                    lDict.Add(lPropParts[0], lPropParts[1].Trim());
                }
            }

            dataDictionary = lDict;

            string lAction = lQueryPartsArr[0];

            if(Regex.IsMatch(lAction, _generateComponentPattern)) {
                actionType = NgAction.generateComponent;
                return;
            } else if(Regex.IsMatch(lAction, _generateElementPattern)) {
                actionType = NgAction.generateElement;
                return;
            } else if(Regex.IsMatch(lAction, _deleteComponentPattern)) {
                actionType = NgAction.deleteComponent;
                return;
            } else if(Regex.IsMatch(lAction, _renameComponentPattern)) {
                actionType = NgAction.renameComponent;
                return;
            } else if(Regex.IsMatch(lAction, _generateModelPattern)) {
                actionType = NgAction.generateModel;
                return;
            } else if(Regex.IsMatch(lAction, _reflectModelsPattern)) {
                actionType = NgAction.reflectModels;
                return;
            } else {
                actionType = NgAction.errorQuery;
                return;
            }
        }

        #endregion
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        //------------| GET_VALUE |-------------------------------------------------------------------------------------
        public string getValue(string keyString_, bool errorHightlight_ = true) {
            string lResult;
            if(!dataDictionary.TryGetValue(keyString_, out lResult)) {
                ConsoleWriter.warnMessageSpan("Query error. Key ");
                ConsoleWriter.messageSpan(keyString_, ConsoleColor.Blue);
                ConsoleWriter.warnMessageSpan(" not set", true);
            };
            return lResult;
        }

        //------------| GET_BOOLEAN_VALUE |-------------------------------------------------------------------------------------
        public bool getBooleanValue(string keyString_) {
            if(!dataDictionary.TryGetValue(keyString_, out string outString)) {
                return false;
            }
            return true;
        }

        #endregion

    }
    #endregion
}
