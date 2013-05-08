using UnityEngine;
using System.Collections;

/// <summary>
/// 短连接请求更新节点
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
