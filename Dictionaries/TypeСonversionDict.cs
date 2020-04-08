namespace Angular_Utility.Dictionaries {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| TypeСonversionDict |=============================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class TypeСonversionDict

    public class TypeСonversionDict: AbstractDict<string, string> {

        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        static TypeСonversionDict() {
            string
                STRING = "string",
                NUMBER = "number";

            _dict = new ReadonlyDict<string, string>(
                new (string, string)[] {
                ("string", STRING),
                ("Guid",STRING),
                ("short", NUMBER),
                ("byte", NUMBER),
                ("int", NUMBER),
                ("long", NUMBER),
                ("Boolean", "boolean"),
                ("DateTime", "Date"),
                ("string?", STRING),
                ("Guid?",STRING),
                ("short?", NUMBER),
                ("byte?", NUMBER),
                ("int?", NUMBER),
                ("long?", NUMBER),
                ("Boolean?", "boolean"),
                ("DateTime?", "Date"),
            });

        }

        #endregion
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        //------------| INIT |-------------------------------------------------------------------------------------
        public static bool init() {
            return true;
        }

        #endregion
    }
    #endregion
}
