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
                NUMBER = "number",
                BOOLEAN = "boolean",
                DATE = "Date";

            _dict = new ReadonlyDict<string, string>(
                new (string, string)[] {
                ("string", STRING),
                ("Guid",STRING),
                ("short", NUMBER),
                ("byte", NUMBER),
                ("int", NUMBER),
                ("long", NUMBER),
                ("bool", BOOLEAN),
                ("DateTime", DATE),
                ("string?", STRING),
                ("Guid?",STRING),
                ("short?", NUMBER),
                ("byte?", NUMBER),
                ("int?", NUMBER),
                ("long?", NUMBER),
                ("bool?", BOOLEAN),
                ("DateTime?", DATE),
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
