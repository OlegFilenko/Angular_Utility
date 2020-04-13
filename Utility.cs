using Angular_Utility.Dictionaries;
using System;
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

        private static string 
            _projectPath = string.Empty,
            _solutionPath = string.Empty;

        #endregion
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        public static string projectPath => _projectPath;
        public static string solutionPath => _solutionPath;

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Cunstructor

        static Utility() {
            _solutionPath = File.ReadAllLines("Projects")[0];
            ConsoleWriter.accentMessageLine(_projectPath);
            _projectPath = _solutionPath + "/ClientApp/src/app/";
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
        public static string findSubstring(string source_, string start_, string end_, bool include_ = false) {
            int lIndex = 0;
            return findSubstring(source_, start_, end_, ref lIndex, include_);
        }

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

        //------------| SET_TO_MODULE |-------------------------------------------------------------------------------------
        public static void setToModule(string modulePath_, string setName_, string filePath_, string name_) {
            if(!File.Exists(modulePath_)) {
                ConsoleWriter.warnMessageLine($"ERROR. Module {modulePath_} not exists");
                return;
            }

            string
                lModuleContent = addToImports(modulePath_, name_, filePath_, out bool alreadyExists),
                lBlock = Regex.Match(lModuleContent, setName_ + "(\\s?)*:(\\s?)*[\\[][ \n\r\t:_{}a-z0-9,/'!.)(@-]*[\\]]", RegexOptions.IgnoreCase).Value,
                lBlockEnd = Regex.Match(lModuleContent, "[\n\t\r ]*\\]").Value;

            if(lBlock != "") {
                if(lBlock.IndexOf(name_) == -1) {
                    string
                        lBlockStart = Regex.Match(lBlock, setName_ + "(\\s?)*:(\\s?)*[\\[]").Value,
                        lBlockBody = findSubstring(lBlock, lBlockStart, lBlockEnd).TrimEnd();
                    lModuleContent = lModuleContent.Replace(lBlockBody, lBlockBody + (lBlockBody.EndsWith(",")? "": ",") + "\n    " + name_);
                } else {
                    if(alreadyExists) {
                        return;
                    }
                }
            } else {
                int lLastSquareBracketIndex  = lModuleContent.LastIndexOf("]") + 1;
                lModuleContent.Insert(lLastSquareBracketIndex, $@",
  {setName_}: [
    name_
  ]
");
            }

            File.WriteAllText(modulePath_, lModuleContent);
        }

        public static void setToAppModule(string setName_, string filePath_, string name_) {
            setToModule(projectPath + "app.module.ts", setName_, filePath_, name_);
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

        //------------| ADD_TO_IMPORTS |-------------------------------------------------------------------------------------
        public static string addToImports(string filePath_, string importName_, string importPath_, out bool alreadyExists) {
            alreadyExists = false;
            if(File.Exists(filePath_)) {
                Uri
                    lFilePathUri = new Uri(filePath_),
                    lImportPathUri = new Uri(importPath_);

                string lImportRelativePath = lFilePathUri.MakeRelativeUri(lImportPathUri).ToString();
                FileInfo lImportPathFileInfo = new FileInfo(lImportRelativePath);
                lImportRelativePath = lImportRelativePath.Replace(lImportPathFileInfo.Name, Path.GetFileNameWithoutExtension(lImportRelativePath));

                if(lImportRelativePath[0] != '.') { lImportRelativePath = "./" + lImportRelativePath; }

                string lContent = File.ReadAllText(filePath_);
                string lImportInsert = Regex.Match(lContent, $"import(\\s?)*{{[a-z0-9_, -] +}}(\\s?)*from(\\s?)*(\'|\")(\\s?)*({lImportRelativePath})(\\s?)*(\'|\")(\\s?)*;", RegexOptions.IgnoreCase).Value;

                if(lImportInsert == "") {
                    lImportInsert = $"import {{ {importName_} }} from '{lImportRelativePath}';";

                    List<string> fileStringLines = new List<string>(lContent.Split('\n'));

                    int lLastImportIndex = 0;
                    for(int i = 0; i < fileStringLines.Count; i++) {
                        string lLine = fileStringLines[i].Trim();
                        if(Regex.IsMatch(lLine, "import[ }{*a-z0-9,_-]+from(\\s?)*(\'|\")[ @.,/a-z0-9_-]+(\'|\")(\\s?)*;", RegexOptions.IgnoreCase)) {
                            lLastImportIndex = i;
                        }
                    }

                    if(lLastImportIndex != 0) {
                        lLastImportIndex += 1;
                    }
                    fileStringLines.Insert(lLastImportIndex, lImportInsert);
                    lContent = string.Join("\n", fileStringLines.ToArray());
                } else {
                    if(lImportInsert == $"import {{ {importName_} }} from '{lImportRelativePath}';") { alreadyExists = true; return lContent; };
                    string 
                        lImportPart = findSubstring(lImportInsert, "import", "}").Trim(),
                        lNewImportInsert = lImportInsert.Replace(lImportPart, lImportPart + $", {importName_}");
                    lContent.Replace(lNewImportInsert, lNewImportInsert);
                }
                return lContent;
            }
            return "";
        }

        public static void addToImports(FileInfo filePath_, string importName_, string importPath_) {
            if(File.Exists(filePath_.FullName)) {
                string lContent = addToImports(filePath_.FullName, importName_, importPath_, out bool alreadyExists );
                if(!alreadyExists) {
                    File.WriteAllText(filePath_.FullName, lContent);
                }
            } else {
                ConsoleWriter.warnMessageLine("ERROR! File not exists - " + filePath_.FullName);
            }
        }

        #endregion
    }
    #endregion
}
