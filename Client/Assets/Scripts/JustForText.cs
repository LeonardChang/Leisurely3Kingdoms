using UnityEngine;
using System.Collections;

public class JustForText : MonoBehaviour {

	// Use this for initialization
    public WebCamTexture cam;
    public UILabel label;
    public GameObject BtnPrefab;
	void Start () {
        /*
	    WebCamDevice[] devices = WebCamTexture.devices;
        for( int i = 0 ; i < devices.Length ; i++ )
        Debug.Log(devices[i].name);

        cam = new WebCamTexture(devices[0].name, 2048, 2048, 30);

        renderer.material.mainTexture = cam;
        cam.Play();  
         */
        /*
        for (int i = 0; i < 8; i++)
        { 
            for(int j = 0; j < 8; j ++)
            {
                GameObject obj = (GameObject)Instantiate(BtnPrefab);

                obj.transform.parent = transform;
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = new Vector3(-280f + i * 80f, j * (-80f) + 120f, 0);
            }
        }
         * */
	}
	
	// Update is called once per frame
	void Update () {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            Texture2D tx = new Texture2D(640, 640);

            for(int i = 0; i < cam.width; i ++)
                for(int j = 0; j < cam.height; j ++)
                {
                    tx.SetPixel(i, j, cam.GetPixel(i, j));
                }

            byte[] bytes = tx.EncodeToPNG();

            System.IO.File.WriteAllBytes(Application.persistentDataPath + "\\hhj.png", bytes);
            print(Application.dataPath);
        }
         */
	}
    void OnHover()
    {
        print("oooooooooooooooooooooooooooooo");
    }
}
