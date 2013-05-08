using System.Collections;

namespace PPP.Unity3D.Plugins.Billing {
    
public class PPPGFanResultUser {
    
    public long userid;			// User ID
    public string username;		// Username
    
	public override string ToString()
	{
		string retValue = "";
		retValue += "userid:"+userid.ToString();
		retValue += " username:"+username;
		return retValue;
	}
}
 
}