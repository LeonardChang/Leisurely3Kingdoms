using UnityEngine;
using System.Collections;

public interface IPPPMMBillingEventListener {

	void onAfterApply();
	void onAfterDownload();
	void onBeforeApply();
	void onBeforeDownload();
	void onInitFinish( string encodedParameter );
	void onBillingFinish( string encodedParameter );
	void onQueryFinish( string encodedParameter );
}
