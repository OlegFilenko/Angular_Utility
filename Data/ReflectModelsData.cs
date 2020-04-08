using System.IO;

namespace Angular_Utility.Data {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| ReflectModelsData |==============================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class ReflectModelsData

    public sealed class ReflectModelsData {
        //=================================================== PUBLIC_VARIABLES =================================================>
        #region Public_Variables

        public readonly string
            pathFrom,
            pathTo;

        public bool isValid => _isValid;

        public readonly string[] filePathArray;

        #endregion
        //=================================================== PRIVATE_VARIABLES ================================================>
        #region Private_Variables

        private bool _isValid = true;

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        public ReflectModelsData(QueryData queryData_) {
            pathFrom = queryData_.getValue("pathFrom");
            pathTo = queryData_.getValue("pathTo");

            if(pathFrom == null && pathTo == null) {
                _isValid = false;
                return;
            }

            pathFrom = Utility.normalisePath(pathFrom);
            pathTo = Utility.normalisePath(pathTo, true);

            if(!Directory.Exists(pathFrom)) {
                if(!File.Exists(pathFrom)) {
                    ConsoleWriter.warnMessageLine("Incorrect source path");
                    _isValid = false;
                    return;
                } else {
                    filePathArray = new string[] { pathFrom };
                }
            } else {
                filePathArray = Directory.GetFiles(pathFrom);
            }
        }

        #endregion
    }
    #endregion
}
