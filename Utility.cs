using Angular_Utility.Dictionaries;
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

        //public static IReadOnlyDictionary<string, string> typeСonversionDict;
        public static string projectPath => _projectPath;

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Cunstructor

        static Utility() {
            _projectPath = File.ReadAllLines("Projects")[0];
            ConsoleWriter.accentMessageLine(_projectPath);
            _projectPath += "/ClientApp/src/app/";
        }

        #endregion
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        //------------| INIT |-------------------------------------------------------------------------------------
        public static void init() {
        }

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
            if(!TypeСonversionDict.tryGetValue(value_, out lDataType)) {
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
        public static string normalisePath(string path_, bool checkPathEnd_ = false) {
            path_ = path_.Replace(@"\", "/");
            if(checkPathEnd_) {
                path_ += (path_.Last() != '/') ? "/" : "";
            }
            return path_;
        }

        //------------| FIND_SUBSTRING |-------------------------------------------------------------------------------------
        public static string findSubstring(string source_, string start_, string end_, ref int index_, bool include_ = false) {
            string lResult = "";
            index_ = source_.IndexOf(start_, index_);
            if (index_ != -1) {
                if(!include_) { index_ += start_.Length; }
                int lEndIndex = source_.IndexOf(end_, index_);
                if(include_) { lEndIndex += end_.Length; }
                lResult = source_.Substring(index_, lEndIndex - index_);
                index_ = lEndIndex;
            }

            return lResult;
        }

        //------------| SET_TO_APP_MODULE |-------------------------------------------------------------------------------------
        public static void setToModule(string modulePath_, string setName_, string filePath_, string name_) {
            if(!File.Exists(_projectPath + modulePath_)) {
                ConsoleWriter.warnMessageLine($"ERROR. Module {modulePath_} not exists");
                return;
            }

            string
                lModuleContent = File.ReadAllText(projectPath + modulePath_),
                lBlock = Regex.Match(lModuleContent, setName_ + "?[ ]*:?[ ]*[\\[][ \n\r\t:_{}a-z,]*[\\]]", RegexOptions.IgnoreCase).Value,
                lBlockEnd = Regex.Match(lModuleContent, "[\n\t\r ]*\\]").Value;

            if (lBlock != "") {
                string
                    lInsert = string.Empty,
                    lImportInserts = string.Empty;

                string lImportPath = "./" + filePath_.Replace(projectPath, "");

                if(lBlock.IndexOf(name_) == -1) {
                    lInsert = ",\n    " + name_;
                    lImportInserts = $"import {{ {name_} }} from '{lImportPath}';";
                } else {
                    ConsoleWriter.warnMessageLine("Импорт с таким именем сервиса уже существует - " + name_);
                }
                if(lInsert != "") {
                    string 
                        lNewBlock = lBlock.Replace(lBlockEnd, lInsert + lBlockEnd),
                        lImports = Regex.Match(lModuleContent, "import[ ]*{[a-z0-9}{'@/;\n\r\t,_ -.]*(export|@NgModule)", RegexOptions.IgnoreCase).Value,
                        lNewImports;
                    List<string> lAppModuleLines = new List<string>(lImports.Split('\n'));
                    int lLastImportIndex = 0; 
                    for (int i = 0; i < lAppModuleLines.Count; i++){
                        string lLine = lAppModuleLines[i].Trim();
                        if (Regex.IsMatch(lLine, "import(\\s?){[ a-z0-9,_-]+}(\\s?)from(\\s?)[./A-Za-z0-9\'-]*.service(\\s?)*(\'|\")(\\s?)*;", RegexOptions.IgnoreCase)) {
                            lLastImportIndex = i;
                        }
                    }
                    lAppModuleLines.Insert(lLastImportIndex + 1, lImportInserts);
                    lNewImports = string.Join("\n", lAppModuleLines.ToArray());
                    lModuleContent = lModuleContent.Replace(lImports, lNewImports);
                    lModuleContent = lModuleContent.Replace(lBlock, lNewBlock);
                    File.WriteAllText(projectPath + modulePath_, lModuleContent);
                }
            }
        }

        public static void setToAppModule(string setName_, string filePath_, string name_) {
            setToModule("app.module.ts", setName_, filePath_, name_);
        }

        //------------| SET_ROUTE |-------------------------------------------------------------------------------------
        public static void setRoute(string routingModulePath_, string modulePath_, string parentNodeComponent_,  string routePrefix_ = "" , string routeVar = "") {
            string 
                lRoutingModuleContent = File.ReadAllText(_projectPath + routingModulePath_, System.Text.Encoding.UTF8),
                lLoadChildren = Path.ChangeExtension(modulePath_, ""),
                lName = Path.GetFileName(modulePath_);
            lLoadChildren = lLoadChildren.Substring(0, lLoadChildren.Length - 1) + "#" + getExportName(Path.GetFileNameWithoutExtension(modulePath_));
            lLoadChildren = "app/" + lLoadChildren.Replace(projectPath, "");
            lName = lName.Replace(ExtensionDict.value(Enums.NgElement.module), "");

            string 
                lStart = Regex.Match(lRoutingModuleContent, $@"component: {parentNodeComponent_}['\]\[,.\w_\t\r\n :/-]*children\s?[:]\s?[[]").Value,
                lEnd = "]";
            int lIndex = 0;

            string 
                lRoutes = findSubstring(lRoutingModuleContent, lStart, lEnd, ref  lIndex).Trim(),
                lNewRoutes = lRoutes + $@",
      {{
        path: '{lName}',
        loadChildren: '{lLoadChildren}'
      }}";
            lRoutingModuleContent = lRoutingModuleContent.Replace(lRoutes, lNewRoutes);
            File.WriteAllText(_projectPath + routingModulePath_, lRoutingModuleContent);
        }

        //------------| SET_TO_APP_ROUTING |-------------------------------------------------------------------------------------
        public static void setToAppRouting(string modulePath_, string routePrefix_ = "", string routeVar_ = "") {
            setRoute("app-routing.module.ts", modulePath_, "LayoutComponent", routePrefix_, routeVar_);
        }

        #endregion
    }
    #endregion
}
