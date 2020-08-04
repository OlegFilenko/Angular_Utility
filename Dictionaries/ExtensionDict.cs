using Angular_Utility.Enums;

namespace Angular_Utility.Dictionaries {

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //=====| ExtensionDict |==================================================================================================>>
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region class ExtensionDict

    public class ExtensionDict : AbstractDict<NgElement, string> {
        //====================================================| CONSTRUCTOR |===================================================>
        #region Constructor

        static ExtensionDict() {
            _dict = new ReadonlyDict<NgElement, string>(
                new (NgElement, string)[] {
                    (NgElement.component, ".component.ts"),
                    (NgElement.html, ".component.html"),
                    (NgElement.style, ".component.scss"),
                    (NgElement.module, ".module.ts"),
                    (NgElement.moduleRouting, "-routing.module.ts"),
                    (NgElement.model, ".model.ts"),
                    (NgElement.service, ".service.ts"),
                    (NgElement.directive, ".directive.ts"),
                    (NgElement.validator, ".validator.ts"),
                    (NgElement.guard, ".guard.ts"),
                    (NgElement.interceptor, ".interceptor.ts"),
                    (NgElement.animation, ".animation.ts"),
                    (NgElement.ngEnum, ".enum.ts"),
                    (NgElement.ngClass, ".ts"),
                    (NgElement.ngInterface, ".ts")
                }
            );
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
