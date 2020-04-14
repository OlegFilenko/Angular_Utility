using Angular_Utility.Data;
using Angular_Utility.Dictionaries;
using Angular_Utility.Enums;
using System;
using System.IO;

namespace Angular_Utility {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| Program |=======================================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class Program

    class Program {
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        static void Main(string[] args) {
            _initialize();
            _processingRequest();
        }

        #endregion
        //===================================================== PRIVATE_METHODS ================================================>
        #region Private_Methods

        //------------| INITIALIZE_DICTIONARIES |-------------------------------------------------------------------------------------
        private static void _initialize() {
            TypeСonversionDict.init();
            ExtensionDict.init();
            Utility.init();
        }

        //------------| PROCESSING_REQUEST |-------------------------------------------------------------------------------------
        private static void _processingRequest() {
            do {
                ConsoleWriter.accentMessageLine("Введите запрос");
                string lQuery = Console.ReadLine();
                QueryData lQueryData = new QueryData(lQuery);

                switch(lQueryData.actionType) {
                    case NgAction.generateComponent: {
                            _generateComponent(new GenerateComponentData(lQueryData));
                            break;
                        }
                    case NgAction.generateDirective: {
                            _generateDirective(new GenerateDirectiveData(lQueryData));
                            break;
                        }
                    case NgAction.generateElement: {
                            _generateElement(new GenerateElementData(lQueryData));
                            break;
                        }
                    case NgAction.reflectModels: {
                            _reflectModels(new ReflectModelsData(lQueryData));
                            break;
                        }
                    case NgAction.reflectController: {
                            _reflectController(new ReflectControllerData(lQueryData));
                            break;
                        }
                    case NgAction.renameComponent: {
                            break;
                        }
                    case NgAction.deleteComponent: {
                            break;
                        }
                    case NgAction.generateModel: {
                            break;
                        }
                    case NgAction.errorQuery: {
                            ConsoleWriter.warnMessageLine("Incorrect query");
                            break;
                        }
                }
            } while(true);
        }

        //------------| GENERATE_COMPONENT |-------------------------------------------------------------------------------------
        private static void _generateComponent(GenerateComponentData componentData_) {
            if(componentData_.isValid) {
                foreach(NgElement element in componentData_.elementsSet) {
                    _generateElement(new GenerateElementData(element, componentData_.name, componentData_.path, componentData_ as object));
                }
                if(componentData_.type == NgComponent.page) {
                    Utility.setToAppRouting(componentData_.path + componentData_.name + ExtensionDict.value(NgElement.module));
                }
                if(componentData_.parentModule != "") {
                    Utility.addToParent(componentData_.parentModule, componentData_.path + componentData_.name + ExtensionDict.value(NgElement.model));
                }
            }
        }

        //------------| GENERATE_DIRECTIVE |-------------------------------------------------------------------------------------
        private static void _generateDirective(GenerateDirectiveData directiveData_) {
            if(directiveData_.isValid) {
                foreach(NgElement element in directiveData_.elements) {
                    _generateElement(new GenerateElementData(element, directiveData_.name, directiveData_.path, directiveData_ as object));
                }
            }
        }

        //------------| GENERATE_ELEMENT |-------------------------------------------------------------------------------------
        private static void _generateElement(GenerateElementData elementData_) {
            if(!Directory.Exists(elementData_.path)) {
                Directory.CreateDirectory(elementData_.path);
            }
            File.WriteAllText(elementData_.path + elementData_.name, elementData_.content);
        }

        //------------| REFLECT_MODELS |-------------------------------------------------------------------------------------
        private static void _reflectModels(ReflectModelsData reflectModelsData_) {
            if(reflectModelsData_.isValid) {
                FileInfo lFileInfo;
                foreach(string filePath_ in reflectModelsData_.filePathArray) {
                    lFileInfo = new FileInfo(filePath_);
                    string lClientFileName = Path.GetFileNameWithoutExtension(lFileInfo.Name);
                    int lLastIndex = lClientFileName.LastIndexOf("Model");
                    if(lLastIndex != -1) {
                        lClientFileName = lClientFileName.Remove(lLastIndex);
                    }
                    lClientFileName = Utility.getClientFileName(lClientFileName);
                    _generateElement(new GenerateElementData(NgElement.model, lClientFileName, reflectModelsData_.pathTo, lFileInfo as object));
                }
            }
        }

        //------------| REFLECT_CONTROLLER |-------------------------------------------------------------------------------------
        private static void _reflectController(ReflectControllerData reflectControllerData_) {
            if(reflectControllerData_.isValid) {
                string lName = Path.GetFileNameWithoutExtension(reflectControllerData_.controllerPath).Replace("Controller", "");
                _generateElement(new GenerateElementData(NgElement.service, Utility.getClientFileName(lName), reflectControllerData_.servicePath, reflectControllerData_ as object));
            }
        }

        #endregion
    }
    #endregion
}
