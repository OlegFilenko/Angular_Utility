using System.Collections.Generic;

namespace Angular_Utility.Dictionaries {


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| ReadonlyDict |===================================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class ReadonlyDict

    public sealed class ReadonlyDict<K, V> {
        //=================================================== PRIVATE_VARIABLES ================================================>
        #region Private_Variables

        private IReadOnlyDictionary<K, V> _dict;

        #endregion
        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        public ReadonlyDict(params (K, V)[] pairs_) {
            Dictionary<K, V> lDict = new Dictionary<K, V>();
            foreach((K, V) pair_ in pairs_) {
                lDict[pair_.Item1] = pair_.Item2;
            }
            _dict = lDict;
        }
        #endregion
        //==================================================== PUBLIC_METHODS ==================================================>
        #region Public_Methods

        //------------| TRY_GET_VALUE |-------------------------------------------------------------------------------------
        public bool tryGetValue(K key_, out V value_) {
            return _dict.TryGetValue(key_, out value_);
        }

        #endregion
        //=================================================== GETTERS_&_SETTERS ================================================>
        #region GettersSetters

        public V this[K value] {
            get { return _dict[value]; }
        }

        #endregion
    }
    #endregion
}
