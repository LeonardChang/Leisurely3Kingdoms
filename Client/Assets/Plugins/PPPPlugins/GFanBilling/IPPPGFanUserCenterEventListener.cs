using UnityEngine;
using System.Collections;

namespace PPP.Unity3D.Plugins.Billing.GFan {
    
public interface IPPPGFanUserCenterEventListener {

    void onLoginSuccess(string encodedParameter);
    void onRegisterSuccess(string encodedParameter);
    void onLoginError();
    void onRegisterError();

}
    
}