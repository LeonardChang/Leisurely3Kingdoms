using System.Collections;

namespace PPP.Unity3D.Plugins.Billing {
    
public class PPPGFanResultUserWithOrder {

    public long userid;				// User ID
    public string username;			// Username
    public string orderid;			// Order ID
    public string transactionid;	// Transaction ID
    public int price;    			// Price
    
	public override string ToString()
	{
		string retValue = "";
		retValue += "userid:"+userid.ToString();
		retValue += " username:"+username;
		retValue += " orderid:"+orderid;
		retValue += " transactionid:"+transactionid;
		retValue += " price:"+price.ToString();
		return retValue;
	}
}

}