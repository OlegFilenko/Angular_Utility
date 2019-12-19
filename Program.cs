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
            string[] lQueryPartsArr = lQuery.Split(new string[] { " -" }, StringSplitOptions.None);

            if(Regex.IsMatch(lQueryPartsArr[0], _generateComponentPattern)) {
                _getPropertiesForGenerateComponent(lQueryPartsArr.Skip(1).ToArray());
                return;
            } else if(Regex.IsMatch(lQueryPartsArr[0], _removeComponentPattern)) {
                return;
            } else if(Regex.IsMatch(lQueryPartsArr[0], _renameComponentPattern)) {
                return;
            }
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
                lAbstract = _checkParamExists(params_, "abstract"),
                lSettingStore = _checkParamExists(params_, "settings_store");

            GenerateComponentModel lDataModel = new GenerateComponentModel {
                path = _checkPath(lPath),
                name = _checkName(lName, lPath),
                dialogs = lDialogs.Replace(" ", "").Split(','),
                routing = lRouting,
                flat = lFlat,
                abstr = lAbstract,
                settingStore = lSettingStore
            };

            if (lDataModel.dialogs.Length == 1 && lDataModel.dialogs[0] == "") {
                lDataModel.dialogs = new string[] { };
            }

            _generateComponent(lDataModel);

            _okMessage("==Done==");
            _processingRequest();
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

                string[] BP = dataModel_.dialogs;

                lElements = new List<_ng_element>{ 
                    _ng_element.component,
                    _ng_element.module,
                    _ng_element.html,
                    _ng_element.style
                };

                if (dataModel_.routing) { lElements.Add(_ng_element.routinModule); }
                if (dataModel_.settingStore) { lElements.Add(_ng_element.settingsStoreService); }
            }

            _createFiles(lElements.ToArray(), dataModel_);

            if (dataModel_.dialogs.Length > 0) {
                foreach (string dialog in dataModel_.dialogs) { 
                    Directory.CreateDirectory(_projectPath + dataModel_.path + "dialogs/"+ dialog + "/");
                    _createFiles(new _ng_element[] { 
                        _ng_element.component, 
                        _ng_element.module,
                        _ng_element.html,
                        _ng_element.style
                    }, new GenerateComponentModel(){ 
                        name=dialog,
                        path=dataModel_.path + "dialogs/" + dialog + "/"}
                    );
                }
            }
        }

        //------------| CREATE_FILES |-------------------------------------------------------------------------------------
        private static void _createFiles(_ng_element[] elementsArr_, GenerateComponentModel dataModel_) {
            foreach (_ng_element element in elementsArr_) {
                _createFile(element, dataModel_);
            }
        }

        //------------| CREATE_FILE |-------------------------------------------------------------------------------------
        private static void _createFile(_ng_element element_, GenerateComponentModel dataModel_) {
            using (StreamWriter lFileStream = File.CreateText(_projectPath + dataModel_.path + dataModel_.name + _extensionsArr[(int)element_])) {
                lFileStream.Write(_selectElement(element_, dataModel_));
            }
        }

        //------------| SELECT_ELEMENT |-------------------------------------------------------------------------------------
        private static string _selectElement (_ng_element element_, GenerateComponentModel dataModel_) {
            switch (element_) {
                case _ng_element.component:
                    return _componentTemplate(dataModel_);
                case _ng_element.module:
                    return _moduleTemplate(dataModel_);
                case _ng_element.routinModule:
                    return _routingModuleTemplate(dataModel_);
                case _ng_element.style:
                    return _styleTemplate(dataModel_);
                case _ng_element.html:
                    return _htmlTemplate(dataModel_);
                case _ng_element.settingsStoreService:
                    return _settingsStoreTemplate(dataModel_);
                default:
                    break;
            }

            return "";
        }

        //------------| COMPONENT_TEMPLATE |-------------------------------------------------------------------------------------
        private static string _componentTemplate(GenerateComponentModel dataModel_) {
            string 
                lContent = string.Empty,
                lComponent = string.Empty,
                lImportComponent = string.Empty;

            if(!dataModel_.abstr) {
                lImportComponent = "Component, ";
                lComponent = string.Format(@"
@Component({{
  selector: '{0}',
  templateUrl: './{0}.component.html',
  styleUrls: ['./{0}.component.scss']
}})", dataModel_.name);
            }

            lContent = string.Format(@"import {{ {0}OnInit, Injector }} from '@angular/core';
{1}
export class {2}Component implements OnInit {{

  constructor(
    protected readonly injector: Injector
  ) {{
  }}

  ngOnInit() {{
  }}

}}", lImportComponent, lComponent, _getExportName(dataModel_.name));
            return lContent;
        }

        //------------| MODULE_TEMPLATE |-------------------------------------------------------------------------------------
        private static string _moduleTemplate (GenerateComponentModel dataModel_) {
            string
                lContent = string.Empty,
                lExportName = _getExportName(dataModel_.name),
                lImportRoutingModule = (dataModel_.routing) ? string.Format(@"import {{ {0}RoutingModule }} from './{1}-routing.module';
", lExportName, dataModel_.name) : "",
                lImports = (dataModel_.routing) ? string.Format(@",
    {0}RoutingModule", lExportName) : "",
                lNgModelImports = (dataModel_.routing) ? "" : $@",
  entryComponents: [{lExportName}Component],
  exports: [{lExportName}Component]",
                lDialogsImports = string.Empty,
                lDialogsNgModuleImports = string.Empty;

            if (dataModel_.dialogs.Length != 0) {
                foreach (string dialog in dataModel_.dialogs) {
                    lDialogsImports += string.Format(@"import {{ {0}Module }} from './dialogs/{1}/{1}.module';
", _getExportName(dialog), dialog);
                    lDialogsNgModuleImports += string.Format(@",
    {0}Module", _getExportName(dialog));
                }
            }

            lContent = string.Format(@"import {{ NgModule }} from '@angular/core';
import {{ CommonModule }} from '@angular/common';
{4}
{2}import {{ {0}Component }} from './{1}.component';

@NgModule({{
  imports: [
    CommonModule{3}{5}
  ],
  declarations: [{0}Component]{6}
}})
export class {0}Module {{ }}", lExportName, dataModel_.name, lImportRoutingModule, lImports, lDialogsImports, lDialogsNgModuleImports, lNgModelImports);

            return lContent;
        }

        //------------| ROUTING_MODULE_TEMPLATE |-------------------------------------------------------------------------------------
        private static string _routingModuleTemplate(GenerateComponentModel dataModel_) {
            string lContent = string.Empty;

            lContent = string.Format(@"import {{ NgModule }} from '@angular/core';
import {{ Routes, RouterModule }} from '@angular/router';
import {{ {0}Component }} from './{1}.component';

const routes: Routes = [
  {{
    path: '',
    component: {0}Component
  }}
];

@NgModule({{
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
}})
export class {0}RoutingModule {{ }}", _getExportName(dataModel_.name), dataModel_.name);


            string appRoutingModule = File.ReadAllText(_projectPath + "app-routing.module.ts");
            string startSerching = "children: [";
            int startIndex = appRoutingModule.IndexOf(startSerching) + startSerching.Length;
            int endIndex = appRoutingModule.IndexOf(@"}
    ]", startIndex);
            appRoutingModule = appRoutingModule.Insert(endIndex + 1, string.Format(@",
      {{
        path: '{0}',
        loadChildren: '{1}'
      }}", dataModel_.name, "app/" + dataModel_.path + dataModel_.name + ".module" + "#" + _getExportName(dataModel_.name) + "Module"));
            File.WriteAllText(_projectPath + "app-routing.module.ts", appRoutingModule);

            return lContent;
        }

        //------------| STYLE_TEMPLATE |-------------------------------------------------------------------------------------
        private static string _styleTemplate(GenerateComponentModel dataModel_) {
            string lContent = string.Empty;
            return lContent;
        }

        //------------| HTML_TEMPLATE |-------------------------------------------------------------------------------------
        private static string _htmlTemplate(GenerateComponentModel dataModel_) {
            string lContent = string.Empty;

            lContent = string.Format(@"<p>
  {0} works!
</p>", dataModel_.name);

            return lContent;
        }


        //------------| SETTINGS_STORE_TEMPLATE |-------------------------------------------------------------------------------------
        private static string _settingsStoreTemplate(GenerateComponentModel dataModel_)
        {
            string lContent = string.Empty;

            lContent = string.Format(@"import {{ Injectable }} from '@angular/core';

@Injectable()
export class {0}SettingsStoreService {{

  private default: any = <any>{{ }}

  private stored: any;

  constructor() {{
    this.stored = {{ ...this.default }};
  }}

  get(): any {{
    return this.stored;
  }}

  reset(): any {{
    return this.default;
  }}
}}", _getExportName(dataModel_.name));

            return lContent;
        }

        //------------| GET_EXPORT_NAME |-------------------------------------------------------------------------------------
        private static string _getExportName(string name_) {
            string[] lNamesParts = name_.Split('-');
            for(int i = 0; i < lNamesParts.Length; i++) {
                string lPart = lNamesParts[i];
                lPart = lPart[0].ToString().ToUpper() + lPart.Substring(1);
                lNamesParts[i] = lPart;
            }
            return string.Join("", lNamesParts);
        }

        //------------| CHECK_NAME |-------------------------------------------------------------------------------------
        private static string _checkName(string name_, string path_) {
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
            if (path_ == string.Empty){ return "";  }
            if (path_[0] == '/'){ path_ = path_.Substring(1); }
            if (path_[path_.Length - 1] != '/') { path_ += "/"; }
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
            string[] lArr = params_.Where(param => Regex.IsMatch(param, $"^{paramName_}=\".*\"")).ToArray();
            string lParam = (lArr.Length > 0) ? lArr[0] : "";
            return lParam.Replace($"{paramName_}=\"", "").Replace("\"", "");
        }

        //------------| CHECK_PARAM_EXISTS |-------------------------------------------------------------------------------------
        private static bool _checkParamExists(string[] params_, string paramName_) {
            return Array.Exists(params_, param => Regex.IsMatch(param, $"^-{paramName_}$"));
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
