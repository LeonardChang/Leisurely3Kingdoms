using UnityEngine;
using System.Collections;

/// <summary>
/// ������������½ڵ�
/// </summary>
public class NetworkGameObject : MonoBehaviour 
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	void Start () 
    {
        NetworkCtrl.Init();
	}
	
	void Update ()
    {
        NetworkCtrl.UpdateHandler();
	}
}
