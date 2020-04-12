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
            GENERATE_DIRECTIVE_DATA_NAME = "GenerateDirectiveData",
            REFLECT_MODELS_DATA_NAME = "ReflectModelsData",
            REFLECT_CONTROLLER_DATA_NAME = "ReflectControllerData";
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        //------------| GET_CONTENT |-------------------------------------------------------------------------------------
        public static string getContent(GenerateElementData elementData_, object data_ = null) {

            switch(elementData_._type) {
                case NgElement.component:
                    return _componentContent(elementData_, data_);
                case NgElement.module:
                    return _moduleContent(elementData_, data_);
                case NgElement.html:
                    return _htmlContent(elementData_, data_);
                case NgElement.style:
                    return _styleContent(elementData_, data_);
                case NgElement.service:
                    return _serviceContent(elementData_, data_);
                case NgElement.model:
                    return _modelContent(elementData_, data_);
                case NgElement.moduleRouting:
                    return _moduleRoutingContent(elementData_, data_);
                case NgElement.directive:
                    return _directiveContent(elementData_, data_);
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
                lComponentDecorator = "",
                lName = elementData_.name.Replace(Dictionaries.ExtensionDict.value(NgElement.component), ""),
                lExportName = Utility.getExportName(lName);

            bool isAbstract = false;

            if(data_ != null) {
                if(data_.GetType().Name == GENERATE_COMPONENT_DATA_NAME) {
                    GenerateComponentData lComponentData = data_ as GenerateComponentData;
                    isAbstract = (lComponentData.type == NgComponent.abstr);

                    if(lComponentData.type == NgComponent.dialog) {
                        lAngularCore = ", Injector, Inject";
                        lExtends = "extends DialogAbstractDefaultsComponent";
                        lInjections = "\n    @Inject(MAT_DIALOG_DATA) protected defaults: any,\n    protected readonly injector: Injector";
                        lConstructorBody = "\n    super(defaults, injector);";
                        lOnInit = "\n    super.ngOnInit()";
                        lVariables = "  icon: string = 'info';\n  title: string = 'Инфо';";
                        lImports = "\nimport { MAT_DIALOG_DATA } from '@angular/material';\nimport { DialogAbstractDefaultsComponent } from 'app/shared/dialog-window/dialog-abstract-defaults.component';";
                        lImplements = "";
                    }

                    if(lComponentData.type == NgComponent.page) {
                        lAngularCore = ", OnInit";
                        lImplements = " implements OnInit";
                        lVariables = "  readonly modelStringKey: string;";
                    }

                    lComponentDecorator = (!isAbstract) ? $@"@Component({{
  selector:'{lName}',
  templateUrl:'./{lName}.component.html',
  styleUrls: ['./{lName}.component.scss']
}})" : "";
                }
            }

            return $@"import {{ Component{lAngularCore} }} from '@angular/core';{lImports}

{lComponentDecorator}
export class {lExportName}Component {lExtends}{lImplements} {{
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
                lModuleImports = "",
                lModuleExports = "",
                lParams = "",
                lName = elementData_.name.Replace(Dictionaries.ExtensionDict.value(NgElement.module), ""),
                lExportName = Utility.getExportName(lName),
                lImports = $@"
import {{ {lExportName}Component }} from './{lName}.component';";

            if(data_ != null) {
                if(data_.GetType().Name == GENERATE_COMPONENT_DATA_NAME) {
                    GenerateComponentData lComponentData = data_ as GenerateComponentData;
                    lParams = $@",
  declarations: [{lExportName}Component],
  entryComponents: [{lExportName}Component],
  exports: [{lExportName}Component]";

                    if(lComponentData.type == NgComponent.dialog) {
                        lImports += $@"
import {{ FormsModule }} from '@angular/forms';
import {{ MaterialModule }} from 'app/shared/material-components.module';
import {{ DialogWindowModule }} from 'app/shared/dialog-window/dialog-window.module';";

                        lModuleImports = $@",
    MaterialModule,
    FormsModule, 
    DialogWindowModule";
                    }

                    if(lComponentData.type == NgComponent.page) {
                    }
                }
                if(data_.GetType().Name == GENERATE_DIRECTIVE_DATA_NAME) {
                    GenerateDirectiveData lDirectiveData = data_ as GenerateDirectiveData;
                    lImports = $@"
import {{ {lExportName}Directive }} from './{lName}.directive';";

                    lParams = $@",
  declarations: [{lExportName}Directive],
  exports: [{lExportName}Directive]";
                }
            }

            return $@"import {{ NgModule }} from '@angular/core';
import {{ CommonModule }} from '@angular/common';
{lImports}

@NgModule({{
  imports: [
    CommonModule{lModuleImports}
  ]{lModuleExports}{lParams}
}})
export class {lExportName}Module {{ }}";
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
  {elementData_.name.Replace(Dictionaries.ExtensionDict.value(NgElement.html), "")} work!
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
        private static string _serviceContent(GenerateElementData elementData_, object data_ = null) {
            bool lLocalImport = false;
            string
                lRoute = string.Empty,
                lServiceMethods = string.Empty;
            if (data_ != null) {
                if (data_.GetType().Name == REFLECT_CONTROLLER_DATA_NAME) {
                    ReflectControllerData lControllerData = data_ as ReflectControllerData;
                    string lControllerContent = File.ReadAllText(lControllerData.controllerPath);
                    int lIndex = 0;

                    lRoute = Utility.findSubstring(lControllerContent, "[Route(\"", "\")]", ref lIndex).Replace("api/", "");
                    string
                        lControllerName = Utility.findSubstring(lControllerContent, "public class ", "Controller", ref lIndex);
                    lRoute = (lRoute.IndexOf("[controller]") != -1) ? lRoute.Replace("[controller]", lControllerName) : lRoute;
                    do {
                        string lMethodHead = Utility.findSubstring(lControllerContent, "[Ht", "{", ref lIndex).Replace("  ", " ");
                        if (lIndex != -1) {
                            int lMHeadIndex = 0;
                            string[] lHttpRequestParts = Utility.findSubstring(lMethodHead, "tp", "\")]", ref lMHeadIndex).Split('"');
                            string 
                                lHttpRequestType  = lHttpRequestParts[0].Replace("(", "").ToLower(),
                                lHttpRequestValue = lHttpRequestParts[1];
                            string[] lMethodHeadNameArr = Utility.findSubstring(lMethodHead, "public ", "(", ref lMHeadIndex).Split(' ');
                            string 
                                lResponseType = Utility.getClientDataType(lMethodHeadNameArr[0], out bool optional1),
                                lMethodName = Utility.getClientName(lMethodHeadNameArr[1]);

                            string
                                lData = "",
                                lDataWithType = "";

                            if (lHttpRequestType == "post" || lHttpRequestType == "put") {
                                string[] lMethodRequestTypeArr = Utility.findSubstring(lMethodHead, "[FromBody]", ")", ref lMHeadIndex).Replace("[Required]", "").Trim().Split(' ');
                                string lRequestType = Utility.getClientDataType(lMethodRequestTypeArr[0], out bool optional2);
                                lData = ", data";
                                lDataWithType = $", data: {lRequestType}";
                            }

                            lHttpRequestValue = (lHttpRequestValue == "[action]") ? lMethodHeadNameArr[1] : lHttpRequestValue;

                            lServiceMethods += $@"  {lMethodName}(stringKey: string{lDataWithType}): Observable<{lResponseType}> {{
    return this.http.{lHttpRequestType}<{lResponseType}>(`${{this.controller}}/{lHttpRequestValue}`{lData}, this.getHttpOptions(stringKey));
  }}"+"\n\n";
                        }
                    } while (lIndex != -1);
                }
            }

            string lName = Utility.getExportName(elementData_.name.Replace(".ts", ""));

            string lServicePath = elementData_.path + elementData_.name;

            if(lLocalImport) {
            } else {
                Utility.setToAppModule("providers", lServicePath, lName);
            }

            string lEnvRelPath = new Uri(elementData_.path).MakeRelativeUri(new Uri(Utility.solutionPath + "/ClientApp/src/environments/environment")).ToString();
            return $@"import {{ HttpClient,HttpHeaders }} from '@angular/common/http';
import {{ Injectable }} from '@angular/core';
import {{ Observable }} from 'rxjs';
import {{ environment }} from '{lEnvRelPath}';

@Injectable()
export class {lName} {{
  private readonly controller = `${{environment.api}}{lRoute}`;

  constructor(private readonly http: HttpClient) {{ }}

  private getHttpOptions(modelStringKey: string) {{
    return {{ headers: new HttpHeaders({{ 'ModelStringKey': modelStringKey }}) }};
  }}
    
{lServiceMethods}
}}";
        }

        //------------| MODULE_ROUTING_CONTENT |-------------------------------------------------------------------------------------
        private static string _moduleRoutingContent(GenerateElementData elementData_, object data_ = null) {
            string 
                lRoutes = string.Empty,
                lImports = string.Empty,
                lName = elementData_.name.Replace(Dictionaries.ExtensionDict.value(NgElement.moduleRouting), ""),
                lExportName = Utility.getExportName(lName);
            ;
            if(data_ != null) {
                if(data_.GetType().Name == GENERATE_COMPONENT_DATA_NAME) {
                    lRoutes = $@"
  {{
    path: '',
    component: {lExportName}Component
  }}
";
                    lImports = $@"import {{ {lExportName}Component }} from './{lName}.component';";
                }
            }
            return $@"import {{ NgModule }} from '@angular/core';
import {{ Routes,RouterModule }} from '@angular/router';
{lImports}

const routes: Routes = [{lRoutes}];

@NgModule({{
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
}})
export class {lExportName}RoutingModule {{ }}
";
        }

        //------------| DIRECTIVE_CONTENT |-------------------------------------------------------------------------------------
        private static string _directiveContent(GenerateElementData elementData_, object data_ = null) {
            string
                lName = elementData_.name.Replace(Dictionaries.ExtensionDict.value(NgElement.directive), ""),
                lExportName = Utility.getExportName(lName);
            if(data_ != null) {
                if(data_.GetType().Name == GENERATE_DIRECTIVE_DATA_NAME) {

                }
            }

            return $@"import {{ Directive,ElementRef, HostListener }} from '@angular/core';

@Directive({{
  selector: '[directive_selector_name_must_be_changed]',
}})
export class {lExportName}Directive {{ 

  constructor(private element: ElementRef) {{
  }}

}}
";
        }

        #endregion

    }
    #endregion
}
