using UnityEngine;
using System.Collections;

namespace PPP.Unity3D.Plugins.Billing.GFan {
    
public interface IPPPGFanBillingEventListener {

    void onIAPSuccess(string encodedParameter);
    void onIAPError(string encodedParameter);
    void onIAPErrorUserNotLoggedIn();
    void onGFanChargeSuccess( string encodedParameter );
    void onGFanChargeError( string encodedParameter );
    void onGFanChargeErrorUserNotLoggedIn();
}
    
}