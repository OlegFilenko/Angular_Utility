using Angular_Utility.models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Angular_Utility
{

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| Program |========================================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class Program

    public class Program {
        //=================================================== PRIVATE_VARIABLES ================================================>
        #region Private_Variables

        private enum _ng_element {
            component,
            module,
            routinModule,
            style,
            html,
            settingsStoreService
        }

        private static string[] _extensionsArr = {
            ".component.ts",
            ".module.ts",
            "-routing.module.ts",
            ".component.scss",
            ".component.html",
            "-settings-store.service.ts"
        };

        private static string 
            _projectPath = string.Empty,
            _generateComponentPattern = "(generate_component || gen_component || g_component || gen_comp || g_comp)",
            _removeComponentPattern = "(remove_component || rem_component || rm_component || rem_comp || rm_comp)",
            _renameComponentPattern = "(rename_component || ren_component || rn_component || ren_comp || rn_comp)";
        #endregion
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        //------------| MAIN |-------------------------------------------------------------------------------------
        public static void Main(string[] args) {
            //Console.WriteLine("Путь к Angular проекту: ");
            //_projectPath = Console.ReadLine();
            _projectPath = File.ReadAllLines("Projects")[0];
            _accentMessage(_projectPath);
            _projectPath += "/ClientApp/src/app/";
            _processingRequest();
        }

        #endregion
        //===================================================== PRIVATE_METHODS ================================================>
        #region Private_Methods

        //------------| QUERY_HANDLER |-------------------------------------------------------------------------------------
        private static void _processingRequest() {
            _accentMessage("Введите запрос");
            string lQuery = Console.ReadLine();
            string[] lQueryPartsArr = lQuery.Split(' ');

            if(Regex.IsMatch(lQueryPartsArr[0], _generateComponentPattern)) {
                _getPropertiesForGenerateComponent(lQueryPartsArr.Skip(1).ToArray());
                return;
            } else if(Regex.IsMatch(lQueryPartsArr[0], _removeComponentPattern)){
                return;
            } else if(Regex.IsMatch(lQueryPartsArr[0], _renameComponentPattern)) {
                return;
            }

            lQuery = null;
            lQueryPartsArr = null;
            _processingRequest();
        }

        //------------| GENERATE_COMPONENT |-------------------------------------------------------------------------------------
        private static void _getPropertiesForGenerateComponent(string[] params_) {
            string 
                lName = _getParamValue(params_, "name"),
                lPath = _getParamValue(params_, "path"),
                lDialogs = _getParamValue(params_, "dialogs");
            bool 
                lRouting = _checkParamExists(params_, "routing"),
                lFlat = _checkParamExists(params_, "flat"),
                lAbstract = _checkParamExists(params_, "absract"),
                lSettingStore = _checkParamExists(params_, "settings_store");

            GenerateComponentModel lDataModel = new GenerateComponentModel {
                path = _checkPath(lPath),
                name = _checkName(lName, lPath),
                dilogs = lDialogs?.Replace(" ", "").Split(','),
                routing = lRouting,
                flat = lFlat,
                abstr = lAbstract,
                settingStore = lSettingStore
            };

            _generateComponent(lDataModel);
        }

        //------------| GENERATE_COMPONENT |-------------------------------------------------------------------------------------
        private static void _generateComponent(GenerateComponentModel dataModel_){
            List<_ng_element> lElements;
            if (dataModel_.flat){
                lElements = new List<_ng_element>{
                    _ng_element.component,
                    _ng_element.html,
                    _ng_element.style
                };
            }
            else if (dataModel_.abstr){
                lElements = new List<_ng_element>{
                    _ng_element.component,
                };
            } else {
                dataModel_.path += dataModel_.name + "/";
                Directory.CreateDirectory(_projectPath + dataModel_.path);

                lElements = new List<_ng_element>{ 
                    _ng_element.module,
                    _ng_element.component,
                    _ng_element.html,
                    _ng_element.style
                };

                if (dataModel_.routing) { lElements.Add(_ng_element.routinModule); }
                if (dataModel_.settingStore) { lElements.Add(_ng_element.settingsStoreService); }
            }

            foreach (_ng_element element in lElements){
                _createFile(element, dataModel_);
            }

            Console.ReadKey();
        }

        //------------| CREATE_FILE |-------------------------------------------------------------------------------------
        private static void _createFile(_ng_element element_, GenerateComponentModel dataModel_) {
            using (StreamWriter lFileStream = File.CreateText(_projectPath + dataModel_.path + dataModel_.name + _extensionsArr[(int)element_]))
            {
                lFileStream.Write(_extensionsArr[(int)element_]);
            }
        }

        //------------| CHECK_NAME |-------------------------------------------------------------------------------------
        private static string _checkName(string name_, string path_){
            bool lIsValid = true;
            if (name_ != ""){
                if (Directory.Exists(path_ + name_)) {
                    _warnMessage("Директория компонента по указанному пути уже существует. Укажите другое имя компонента");
                    lIsValid = false;
                }
            } else {
                _warnMessage("Необходимо указать имя компонента");
                lIsValid = false;
            }

            if (!lIsValid){
                _accentMessage("Имя: ");
                name_ = _checkName(Console.ReadLine(), path_);
            }

            return name_;
        }

        //------------| CHECK_PATH |-------------------------------------------------------------------------------------
        private static string _checkPath(string path_) {
            if (path_[0] == '/'){ path_ = path_.Substring(1); }
            if (path_[path_.Length - 1] != '/') { path_ += "/"; }
            //if (!Directory.Exists(_projectPath + path_)){
            //    _warnMessage("Такой директории не существует");
            //    _accentMessage("Создать? (Y/N)");
            //    string answer = string.Empty;
            //    do {
            //        answer = Console.ReadLine();
            //    } while (answer != "y" && answer != "n");

            //    if (answer == "y"){
            //        Directory.CreateDirectory(_projectPath + path_);
            //        _okMessage("Создана новая директория :" + path_);
            //        return path_;
            //    } else if (answer == "n") { 
            //        _accentMessage("Путь: ");
            //        path_ = _checkPath(Console.ReadLine());
            //    }
            //}
            return path_;
        }

        //------------| RENAME_COMPONENT |-------------------------------------------------------------------------------------
        private static void _getPropertiesForRenameComponent(string[] params_) {
            Console.WriteLine("rename compoent");
            Console.WriteLine(string.Join("\n", params_));
            Console.ReadKey();
        }

        //------------| REMOVE_COMPONENT |-------------------------------------------------------------------------------------
        private static void _getPropertiesForRemoveComponent(string[] params_) {
            Console.WriteLine("remove compoent");
            Console.WriteLine(string.Join("\n", params_));
            Console.ReadKey();
        }

        //------------| GET_PARAM_VALUE |-------------------------------------------------------------------------------------
        private static string _getParamValue(string[] params_, string paramName_) {
            if(params_.Length == 0) { return ""; }
            string[] lArr = params_.Where(param => Regex.IsMatch(param, $"^-{paramName_}=\".*\"")).ToArray();
            string lParam = (lArr.Length > 0) ? lArr[0] : "";
            return lParam.Replace($"-{paramName_}=\"", "").Replace("\"", "");
        }

        //------------| CHECK_PARAM_EXISTS |-------------------------------------------------------------------------------------
        private static bool _checkParamExists(string[] params_, string paramName_) {
            return Array.Exists(params_, param => Regex.IsMatch(param, $"^--{paramName_}$"));
        }

        //------------| DISTINGUISH_MESSAGE |-------------------------------------------------------------------------------------
        private static void _distinguishMessage(string message_, ConsoleColor messageColor_){
            ConsoleColor defColor = Console.ForegroundColor;
            Console.ForegroundColor = messageColor_;
            Console.WriteLine(message_);
            Console.ForegroundColor = defColor;
        }

        //------------| WARN_MESSAGE |-------------------------------------------------------------------------------------
        private static void _warnMessage(string warnString_){
            _distinguishMessage(warnString_, ConsoleColor.Red);
        }

        //------------| OK_MESSAGE |-------------------------------------------------------------------------------------
        private static void _okMessage(string okString_) {
            _distinguishMessage(okString_, ConsoleColor.Green);
        }

        //------------| ACCENT_MESSAGE |-------------------------------------------------------------------------------------
        private static void _accentMessage(string accentMessage_){
            _distinguishMessage(accentMessage_, ConsoleColor.Yellow);
        }

        #endregion
    }
    #endregion
}
