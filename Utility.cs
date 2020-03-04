using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Angular_Utility {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| Utility |========================================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class Utility
    public static class Utility {

        public static IReadOnlyDictionary<string, string> typeСonversionDict;

        //====================================================| CONSTRUCTOR |===================================================>
        #region Cunstructor

        static Utility() {
            string
                STRING = "string",
                NUMBER = "number";

            Dictionary<string, string> lTypeDict = new Dictionary<string, string>();
            lTypeDict["string"] = STRING;
            lTypeDict["Guid"] = STRING;
            lTypeDict["short"] = NUMBER;
            lTypeDict["byte"] = NUMBER;
            lTypeDict["int"] = NUMBER;
            lTypeDict["long"] = NUMBER;
            lTypeDict["Boolean"] = "boolean";
            lTypeDict["DateTime"] = "Date";

            lTypeDict["string?"] = STRING;
            lTypeDict["Guid?"] = STRING;
            lTypeDict["short?"] = NUMBER;
            lTypeDict["byte?"] = NUMBER;
            lTypeDict["int?"] = NUMBER;
            lTypeDict["long?"] = NUMBER;
            lTypeDict["Boolean?"] = "boolean";
            lTypeDict["DateTime?"] = "Date";

            typeСonversionDict = lTypeDict;
        }

        #endregion
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        //------------| GET_EXPORT_NAME |-------------------------------------------------------------------------------------
        public static string getExportName(string name_) {
            string[] lNamesParts = name_.Split(new char[] { '-', '.' });
            for(int i = 0; i < lNamesParts.Length; i++) {
                string lPart = lNamesParts[i];
                lPart = lPart[0].ToString().ToUpper() + lPart.Substring(1);
                lNamesParts[i] = lPart;
            }
            return string.Join("", lNamesParts);
        }

        //------------| GET_CLIENT_DATA_NAME |-------------------------------------------------------------------------------------
        public static string getClientName(string value_) {
            string firstLeter = value_[0].ToString().ToLower();
            return (firstLeter + value_.Substring(1));
        }

        //------------| GET_CLIENT_FILENAME|-------------------------------------------------------------------------------------
        public static string getClientFileName(string value_) {
            string lResult = "";
            value_ = getClientName(value_);

            for(int i = 0; i < value_.Length; i++) {
                if(value_[i].ToString() == value_[i].ToString().ToUpper()) {
                    lResult += "-" + value_[i].ToString().ToLower();
                } else {
                    lResult += value_[i].ToString();
                }
            }
            return lResult;
        }

        //------------| GET_CLIENT_DATA_TYPE|-------------------------------------------------------------------------------------
        public static string getClientDataType(string value_, out bool isOptional_) {
            value_ = value_.Trim();
            Regex lListCheck = new Regex("^List<");
            string lDataType = "";
            if(!typeСonversionDict.TryGetValue(value_, out lDataType)) {
                if(lListCheck.IsMatch(value_)) {
                    lDataType = value_.Replace("List<", "").Replace(">", "") + "[]";
                } else {
                    lDataType = value_;
                }
            }
            isOptional_ = (value_.Last() == '?');

            return lDataType;
        }

        //------------| TO_VALID_PATH |-------------------------------------------------------------------------------------
        public static string toValidPath(string path_) {
            path_ = path_.Replace(@"\", "/");
            if (Path.GetFileName(path_) == "") { 
                path_ += (path_.Last() != '/') ? "/" : "";
            }
            return path_;
        }

        //------------| FIND_SUBSTRING |-------------------------------------------------------------------------------------
        public static string findSubstring(string source_, string start_, string end_, ref int index_) {
            string lResult = "";
            index_ = source_.IndexOf(start_, index_);
            if (index_ != -1) {
                index_ += start_.Length;
                int lEndIndex = source_.IndexOf(end_, index_);
                lResult = source_.Substring(index_, lEndIndex - index_);
                index_ = lEndIndex;
            }

            return lResult;
        }

        #endregion

    }
    #endregion
}
