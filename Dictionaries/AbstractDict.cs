namespace Angular_Utility.Dictionaries {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| AbstractDict |===================================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class AbstractDict

    public abstract class AbstractDict<K,V> {
        //=================================================== PRIVATE_VARIABLES ================================================>
        #region Private_Variables

        protected static ReadonlyDict<K, V> _dict;

        #endregion
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        //------------| VALUE |-------------------------------------------------------------------------------------
        public static V value(K keyValue_) {
            if(_dict != null && keyValue_ != null) {
                return _dict[keyValue_];
            } else {
                return default(V);
            }
        }

        //------------| TRY_GET_VALUE |-------------------------------------------------------------------------------------
        public static bool tryGetValue(K key_, out V value_) {
            return _dict.tryGetValue(key_, out value_);
        }

        #endregion
    }
    #endregion
}
