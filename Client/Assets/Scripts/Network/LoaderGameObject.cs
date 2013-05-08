using UnityEngine;
using System.Collections;

/// <summary>
/// 资源加载的更新节点
/// </summary>
public class LoaderGameObject : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

	void Start () 
    {
        ResourceLoader.Init();
	}
	
	void Update () 
    {
        ResourceLoader.UpdateProcess();
	}
}