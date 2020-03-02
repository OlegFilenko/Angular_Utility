using Angular_Utility.Data;
using Angular_Utility.Enums;
using System;
using System.IO;

namespace Angular_Utility {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| Program |=======================================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class Programm

    class Program {
        //=================================================== PRIVATE_VARIABLES ================================================>
        #region Private_Variables

        private static string
            _projectPath = string.Empty;

        #endregion
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        static void Main(string[] args) {
            _projectPath = File.ReadAllLines("Projects")[0];
            ConsoleWriter.accentMessageLine(_projectPath);
            _projectPath += "/ClientApp/src/app/";
            _processingRequest();
        }

        #endregion
        //===================================================== PRIVATE_METHODS ================================================>
        #region Private_Methods

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
                    case NgAction.generateElement: {
                            _generateElement(new GenerateElementData(lQueryData));
                            break;
                        }
                    case NgAction.reflectModels: {
                            _reflectModels(new ReflectModelsData(lQueryData));
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
                    string lClientFileName = lFileInfo.Name.Replace(lFileInfo.Extension, "");
                    int lIndexLast = lClientFileName.LastIndexOf("Model");
                    if(lIndexLast != -1) {
                        lClientFileName = lClientFileName.Remove(lIndexLast);
                    }
                    lClientFileName = Utility.getClientFileName(lClientFileName);
                    _generateElement(new GenerateElementData(NgElement.model, lClientFileName, reflectModelsData_.pathTo, lFileInfo as object));
                }
            }

        }

        #endregion
    }
    #endregion
}
