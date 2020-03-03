using Angular_Utility.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Angular_Utility.Data {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| FileContentData |================================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class FileContentData

    public static class FileContentData {
        public static readonly string
            GENERATE_COMPONENT_DATA_NAME = "GenerateComponentData",
            REFLECT_MODELS_DATA_NAME = "ReflectModelsData",
            REFLECT_CONTROLLER_DATA_NAME = "ReflectControllerData";
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        //------------| GET_CONTENT |-------------------------------------------------------------------------------------
        public static string getContent(GenerateElementData elementData_, object data_ = null) {

            switch(elementData_.lType) {
                case NgElement.component:
                    return _componentContent(elementData_, data_);
                case NgElement.module:
                    return _moduleContent(elementData_, data_);
                case NgElement.html:
                    return _htmlContent(elementData_, data_);
                case NgElement.style:
                    return _styleContent(elementData_, data_);
                case NgElement.service:
                    break;
                case NgElement.model:
                    return _modelContent(elementData_, data_);
                case NgElement.directive:
                    break;
                case NgElement.moduleRouting:
                    break;
                case NgElement.validator:
                    break;
                case NgElement.guard:
                    break;
                case NgElement.interceptor:
                    break;
                case NgElement.animation:
                    break;
                case NgElement.ngEnum:
                    break;
                case NgElement.ngClass:
                    break;
                case NgElement.ngInterface:
                    break;
            }

            return "";
        }

        #endregion
        //===================================================== PRIVATE_METHODS ================================================>
        #region Private_Methods

        //------------| COMPONENT_CONTENT |-------------------------------------------------------------------------------------
        private static string _componentContent(GenerateElementData elementData_, object data_ = null) {

            string
                lAngularCore = "",
                lImports = "",
                lInjections = "",
                lExtends = "",
                lImplements = "implements OnInit",
                lConstructorBody = "",
                lOnInit = "",
                lVariables = "",
                lComponentDecorator = "";

            bool isAbstract = false;

            if(data_ != null) {
                if(data_.GetType().Name == GENERATE_COMPONENT_DATA_NAME) {
                    GenerateComponentData lComponentData = data_ as GenerateComponentData;
                    isAbstract = (lComponentData.type == NgComponent.abstr);

                    if(lComponentData.type == NgComponent.dialog) {
                        lAngularCore = ", Injector, Inject";
                        lExtends = "extends DialogAbstractDefaultsComponent";
                        lInjections = "\n    @Inject(MAT_DIALOG_DATA) protected defaults: any\n    protected readonly injector: Injector";
                        lConstructorBody = "\n    super(defaults, injector);";
                        lOnInit = "\n    super.ngOnInit()";
                        lVariables = "  icon: string = 'info',\n  title: string = 'Инфо';";
                        lImports = "\nimport { MAT_DIALOG_DATA } from '@angular/material';";
                        lImplements = "";
                    }

                    if(lComponentData.type == NgComponent.page) {
                        lAngularCore = ", OnInit";
                        lImplements = " implements OnInit";
                        lVariables = "  readonly modelStringKey: string;";
                    }

                    lComponentDecorator = (!isAbstract) ? $@"@Component({{
        selector:'{elementData_.name}',
        templateUrl:'./{elementData_.name}.component.html',
        styleUrls: ['./{elementData_.name}.component.scss']
    }})" : "";
                }
            }

            return $@"import {{ Component{lAngularCore} }} from '@angular/core';{lImports}

{lComponentDecorator}
export class {Utility.getExportName(elementData_.name)}Component {lExtends}{lImplements} {{
{lVariables}
  constructor({lInjections}
  ) {{{lConstructorBody}
  }}

  ngOnInit() {{{lOnInit}
  }}

}}";
        }

        //------------| MODULE_CONTENT |-------------------------------------------------------------------------------------
        private static string _moduleContent(GenerateElementData elementData_, object data_ = null) {
            string
                lImports = "",
                lModuleImports = "",
                lModuleExports = "",
                lParams = "";

            if(data_ != null) {
                if(data_.GetType().Name == GENERATE_COMPONENT_DATA_NAME) {
                    GenerateComponentData lComponentData = data_ as GenerateComponentData;
                    string
                        lParamsBlock = $@"
    declarations: [{Utility.getExportName(lComponentData.name)}Component],
    entryComponents: [{Utility.getExportName(lComponentData.name)}Component],
    exports: [{Utility.getExportName(lComponentData.name)}Component]";

                    if(lComponentData.type == NgComponent.dialog) {
                        lImports = $@"import {{ CommonModule }} from '@angular/common';
import {{ {Utility.getExportName(lComponentData.name)} }} from './{lComponentData.name}.component';
import {{ FormsModule }} from '@angular/forms';";

                        lModuleImports = $@",
        MaterialModule,
        FormsModule, 
        DialogWindowModule";
                        lParams = lParamsBlock;
                    }

                    if(lComponentData.type == NgComponent.page) {
                        lParams = lParamsBlock;
                    }
                }
            }

            return $@"import {{ NgModule }} from '@angular/core';
{lImports}

@NgModule({{
    imports: [
        CommonModule{lModuleImports}
    ],
    {lModuleExports}
    {lParams}
}})
export class {Utility.getExportName(elementData_.name)}Module {{ }}";
        }

        //------------| HTML_CONTENT |-------------------------------------------------------------------------------------
        private static string _htmlContent(GenerateElementData elementData_, object data_ = null) {
            if(data_ != null) {
                if(data_.GetType().Name == GENERATE_COMPONENT_DATA_NAME) {
                    GenerateComponentData lComponentData = data_ as GenerateComponentData;
                    if(lComponentData.type == NgComponent.dialog) {
                        return @"<dialog-window icon='{{icon}}' title='{{title}}' (closeButtonClick)='close()'>
</dialog-window>";
                    }
                }
            }
            return $@"<p>
  {elementData_.name} html work!
</p>";

        }

        //------------| STYLE_CONTENT |-------------------------------------------------------------------------------------
        private static string _styleContent(GenerateElementData elementData_, object data_ = null) {
            return "";
        }

        //------------| MODEL_CONTENT |-------------------------------------------------------------------------------------
        private static string _modelContent(GenerateElementData elementData_, object data_ = null) {
            string lClientFileContent = string.Empty;
            if(data_ != null) {
                if(data_.GetType().FullName == "System.IO.FileInfo") {
                    FileInfo lFileInfo = data_ as FileInfo;
                    string lApiFileContent = File.ReadAllText(lFileInfo.FullName);
                    Regex _fieldCheck = new Regex("^public");
                    string
    lStart = @"Model
    {",
    lEnd = @"}
}";

                    int
                        lStartIndex = lApiFileContent.IndexOf(lStart) + lStart.Length,
                        lEndIndex = lApiFileContent.IndexOf(lEnd);
                    lApiFileContent = lApiFileContent.Substring(lStartIndex, (lEndIndex - lStartIndex));

                    string[] lFileContentLines = lApiFileContent.Split('\n');
                    List<string> lLineList = new List<string>();
                    foreach(string line in lFileContentLines) {
                        string clearLine = line.Trim();
                        if(_fieldCheck.IsMatch(clearLine)) {
                            string[] lineParts = clearLine.Split(' ');
                            bool lOptional = false;
                            string
                                lType = Utility.getClientDataType(lineParts[1], out lOptional),
                                lName = Utility.getClientName(lineParts[2]);
                            lLineList.Add("  " + lName + ((lOptional) ? "?" : "") + ": " + lType + ";");
                        }
                    }
                    string
                        lFileName = lFileInfo.Name.Replace(lFileInfo.Extension, "");

                    lClientFileContent = $@"export interface {lFileName} {{
{String.Join("\n", lLineList)}
}}";

                }
            }
            return lClientFileContent;
        }

        //------------| SERVICE_CONTENT |-------------------------------------------------------------------------------------
        private static string _serviceContent(GenerateComponentData elementData_, object data_ = null) {
            string lServiceContent = string.Empty;
            if (data_ != null) {
                if (data_.GetType().Name == REFLECT_CONTROLLER_DATA_NAME) {
                    Console.WriteLine("REFLECT_CONTROLLER_DATA_NAME OK!");
                }
            }
            return lServiceContent;
        }

        #endregion

    }
    #endregion
}
