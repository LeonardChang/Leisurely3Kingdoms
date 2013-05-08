using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GtGameStore : MonoBehaviour {
	#if UNITY_IPHONE
	private ArrayList productIds;
	
	
	public delegate void GtGetProduct();
	public static event GtGetProduct GtGetProductEvent;
	
	public delegate void GtGetProductFail();
	public static event GtGetProductFail GtGetProductFailEvent;
	
	public delegate void GtPurchaseSuccessful(int id);
	public static event GtPurchaseSuccessful GtPurchaseSuccessfulEvent;
	
	public delegate void GtPurchaseFailed();
	public static event GtPurchaseFailed GtPurchaseFailedEvent;
	
	public delegate void GTPurchaseCancelled();
	public static event GTPurchaseCancelled GtPurchaseCancelledEvent;
	
	private bool canPay = false;
	private bool productIdOk = false;
	
	
	
	// Use this for initialization
	void Start () {
		productIds = new ArrayList();
		StoreKitManager.purchaseSuccessful += PurchaseSuccessful;
		StoreKitManager.purchaseCancelled += PurchaseCancelled;
		StoreKitManager.purchaseFailed += PurchaseFailed;
		StoreKitManager.productListReceived += ProductListReceived;
		StoreKitManager.productListRequestFailed += ProductListRequestFailed;
        if(CanMakePayments())
        {
            ArrayList args = new ArrayList();
            args.Add("IapKoz001");
            args.Add("IapKoz002");
            args.Add("IapKoz003");
            args.Add("IapKoz004");
            GetProductData(args);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public bool CanMakePayments()
	{
		canPay = StoreKitBinding.canMakePayments();
		Debug.Log("Can Pay " + canPay );
		return canPay;
	}
	
	public void GetProductData(ArrayList _ids)
	{
		productIds = _ids;
		string ids = "";
		for(int i = 0; i< _ids.Count; i++)
		{
			if(i != 0)
			{
				ids += ",";
			}
			
			ids += _ids[i].ToString();
		}
		Debug.Log(ids);
		productIdOk = true;
		StoreKitBinding.requestProductData( ids );
	}
	
	public bool PurchaseProduct(int index)
	{
		if(canPay && productIdOk)
		{
			string productId = "IapKoz00" + (index+ 1).ToString();
			Debug.Log(productId);
			StoreKitBinding.purchaseProduct( productId, 1 );
			return true;
		}
		else
		{
            if(GtPurchaseFailedEvent != null)
		    {
			    GtPurchaseFailedEvent();
			    Debug.Log("GtGetProductFailEvent");
		    }
			return false;
		}
		
	}
	
	void PurchaseSuccessful( string productIdentifier, string receipt, int quantity )
	{
		Debug.Log("PurchaseSuccessful");
		if(GtPurchaseSuccessfulEvent != null )
		{
			string sId = productIdentifier.Replace("IapKoz00","");
			int id = int.Parse(sId) -1;
			GtPurchaseSuccessfulEvent(id);
			Debug.Log("PurchaseSuccessful");
		}
	}
	
	void PurchaseCancelled( string error )
	{
		Debug.Log("PurchaseCancelled");
		if(GtPurchaseCancelledEvent != null)
		{
			GtPurchaseCancelledEvent();	
			Debug.Log("PurchaseCancelled");
		}
	}
	
	void PurchaseFailed( string error )
	{
		Debug.Log("GtGetProductFailEvent");
		if(GtPurchaseFailedEvent != null)
		{
			GtGetProductFailEvent();
			Debug.Log("GtGetProductFailEvent");
		}
	}
	
	void ProductListReceived( List<StoreKitProduct> productList )
	{
		Debug.Log("go to ProductListReceived");
		if(productList == null || productList.Count == 0)
		{
			if(GtGetProductFailEvent != null)
			{
				GtGetProductFailEvent();
				Debug.Log("GtGetProductFailEvent");
			}
		}
		if(GtGetProductEvent != null)
		{
			productIdOk = true;
			GtGetProductEvent();
			Debug.Log("ProductListReceived " + productList.Count);
		}
		Debug.Log(productList.Count);
	}
	
	void ProductListRequestFailed( string error )
	{
		Debug.Log("go to ProductListRequestFailed");
		if(GtGetProductFailEvent != null)
		{
			GtGetProductFailEvent();
			Debug.Log("GtGetProductFailEvent");
		}
	}
	
#endif
                                         }
