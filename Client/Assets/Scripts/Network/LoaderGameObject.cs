using UnityEngine;
using System.Collections;

/// <summary>
/// ��Դ���صĸ��½ڵ�
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