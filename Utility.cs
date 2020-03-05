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
        //=================================================== PRIVATE_VARIABLES ================================================>
        #region Private_Variables

        private static string _projectPath = string.Empty;

        #endregion
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        public static IReadOnlyDictionary<string, string> typeСonversionDict;
        public static string projectPath => _projectPath;

        #endregion
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

            _projectPath = File.ReadAllLines("Projects")[0];
            ConsoleWriter.accentMessageLine(_projectPath);
            _projectPath += "/ClientApp/src/app/";
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

        //------------| SET_TO_APP_MODULE |-------------------------------------------------------------------------------------
        public static void setToAppModule(string setName_, string name_) {
            setToModule("app.module.ts", setName_, name_);
        }

        //------------| SET_TO_MODULE |-------------------------------------------------------------------------------------
        public static void setToModule(string modulePath_, string setName_, string name_) {
            setToModule(modulePath_, setName_, new string[] { name_ });
        }

        public static void setToModule(string modulePath_, string setName_, params string[] names_) {
            if(!File.Exists(_projectPath + modulePath_)) {
                ConsoleWriter.warnMessageLine($"ERROR. Module {modulePath_} not exists");
                return;
            }

            string
                lModuleContent = File.ReadAllText(projectPath + modulePath_),
                lBlock = Regex.Match(lModuleContent, setName_ + "?[ ]*:?[ ]*[\\[][ \n\r\t:_{}a-z,]*[\\]]", RegexOptions.IgnoreCase).Value,
                lBlockEnd = Regex.Match(lModuleContent, "[\n\t\r ]*\\]").Value,
                lInsert = string.Empty;

            if (lBlock != "") { 
                foreach(string name_ in names_) {
                    if(lBlock.IndexOf(name_) != -1) {
                        lInsert += ",\n    " + name_;
                    }
                }

                string lNewBlock = lBlock.Replace(lBlockEnd, lInsert + lBlockEnd);

                lModuleContent.Replace(lBlock, lNewBlock);

                //File.WriteAllText(projectPath + modulePath_, lModuleContent);
            }

        }
        #endregion
    }
    #endregion
}
