using System.IO;

namespace Angular_Utility.Data {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| ReflectControllerData |==========================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class ReflectControllerData

    public sealed class ReflectControllerData {
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        public readonly string
            controllerPath,
            servicePath;

        public bool isValid => _isValid;

        #endregion
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        private bool _isValid = true;

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        public ReflectControllerData(QueryData queryData_) {
            controllerPath = queryData_.dataDictionary["controllerPath"];
            servicePath = queryData_.dataDictionary["servicePath"];

            if(controllerPath == null || servicePath == null) {
                _isValid = false;
                return;
            }

            controllerPath = Utility.toValidPath(controllerPath);
            servicePath = Utility.toValidPath(servicePath);

            if(!File.Exists(controllerPath)) {
                ConsoleWriter.warnMessageLine("ERROR. Incorrect controller path");
                _isValid = false;
                return;
            }

            if(Path.GetFileName(servicePath) != "") {
                ConsoleWriter.warnMessageLine("ERROR. Service path must be a path to directory, not file");
                _isValid = false;
                return;
            }
        }

        #endregion
    }
    #endregion
}
