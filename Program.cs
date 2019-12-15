using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
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
            //string lPath = 
            Console.WriteLine("generate compoent");
            Console.WriteLine(string.Join("\n", params_));
            Console.ReadKey();
        }

        //------------| REMOVE_COMPONENT |-------------------------------------------------------------------------------------
        private static void _removeComponent(string[] params_) {
            Console.WriteLine("remove compoent");
            Console.WriteLine(string.Join("\n", params_));
            Console.ReadKey();
        }

        #endregion
    }
    #endregion
}
