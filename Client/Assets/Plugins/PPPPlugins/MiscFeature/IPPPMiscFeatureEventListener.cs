using UnityEngine;
using System.Collections;

public interface IPPPMiscFeatureEventListener {

	void albumChooserCancelled( string empty );
	void albumChooserSucceeded( string path );
	void photoChooserCancelled( string empty );
	void photoChooserSucceeded( string path );
	void videoRecordingSucceeded( string path );
	void videoRecordingCancelled( string empty );
	void alertButtonClicked( string clickedButtonText );
	void alertCancelled();
}
