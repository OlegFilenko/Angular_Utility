using System;
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
            Console.WriteLine("Путь к Angular проекту: ");
            _projectPath = Console.ReadLine();
            _projectPath += "/ClientApp/src/app/";
            _processingRequest();
        }

        #endregion
        //===================================================== PRIVATE_METHODS ================================================>
        #region Private_Methods

        //------------| QUERY_HANDLER |-------------------------------------------------------------------------------------
        private static void _processingRequest() {
            string lQuery = Console.ReadLine();
            string[] lQueryPartsArr = lQuery.Split(' ');

            if(Regex.IsMatch(lQueryPartsArr[0], _generateComponentPattern)) {
                _generateComponent(lQueryPartsArr.Skip(1).ToArray());
                return;
            } else if(Regex.IsMatch(lQueryPartsArr[0], _removeComponentPattern)){
                return;
            } else if(Regex.IsMatch(lQueryPartsArr[0], _renameComponentPattern)) {
                return;
            }
        }

        //------------| GENERATE_COMPONENT |-------------------------------------------------------------------------------------
        private static void _generateComponent(string[] params_) {
            string lPath = _getParamValue(params_, "path");
            bool lRouting = _checkParamExists(params_, "routing");
            Console.WriteLine("path: " + lPath);
            Console.WriteLine("routing: " + lRouting.ToString());
            //Console.WriteLine("generate compoent");
            //Console.WriteLine(string.Join("\n", params_));
            Console.ReadKey();
        }

        //------------| RENAME_COMPONENT |-------------------------------------------------------------------------------------
        private static void _renameComponent(string[] params_) {
            Console.WriteLine("rename compoent");
            Console.WriteLine(string.Join("\n", params_));
            Console.ReadKey();
        }

        //------------| REMOVE_COMPONENT |-------------------------------------------------------------------------------------
        private static void _removeComponent(string[] params_) {
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

        #endregion
    }
    #endregion
}
