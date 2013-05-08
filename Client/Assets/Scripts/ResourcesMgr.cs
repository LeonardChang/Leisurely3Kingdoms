using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

/// <summary>
/// ��Դ������
/// ���ھ����ᱻ���õ���Դ���������Cache�У����ⱻϵͳ����
/// </summary>
public class ResourcesMgr : MonoBehaviour {
#if UNITY_EDITOR
    private Dictionary<string, int> mUseCount = new Dictionary<string, int>();
#endif

    private Dictionary<string, UnityEngine.Object> mLv1Resources = new Dictionary<string, UnityEngine.Object>();
    private Dictionary<string, UnityEngine.Object> mLv2Resources = new Dictionary<string, UnityEngine.Object>();
    private Dictionary<string, UnityEngine.Object> mLv3Resources = new Dictionary<string, UnityEngine.Object>();

    private enum SystemLevel : int
    {
        Level1 = 0,
        Level2,
        Level3,
        Unknow,
    }

    void Awake()
    {
        TextAsset asset = Resources.Load("ResourcesLoadLevel") as TextAsset;
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(asset.text);

        XmlNode node = doc.SelectSingleNode("Files");
        XmlNode lv1Node = node.SelectSingleNode("Level1");
        XmlNode lv2Node = node.SelectSingleNode("Level2");
        XmlNode lv3Node = node.SelectSingleNode("Level3");

        mLv1Resources.Clear();
        mLv2Resources.Clear();
        mLv3Resources.Clear();

        if (lv1Node != null)
        {
            foreach (XmlNode elem in lv1Node.SelectNodes("File"))
            {
                mLv1Resources[elem.InnerText] = null;
            }
        }

        if (lv2Node != null)
        {
            foreach (XmlNode elem in lv2Node.SelectNodes("File"))
            {
                mLv2Resources[elem.InnerText] = null;
            }
        }

        if (lv3Node != null)
        {
            foreach (XmlNode elem in lv3Node.SelectNodes("File"))
            {
                mLv3Resources[elem.InnerText] = null;
            }
        }
    }

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
	    if (Input.GetKeyDown(KeyCode.C))
	    {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.dataPath + "\\..\\resfile.txt", false);

            foreach (string key in mUseCount.Keys)
            {
                writer.WriteLine(key + "," + mUseCount[key].ToString());
            }
            writer.Close();
            print(Application.dataPath + "\\resfile.txt file saved");
	    }
#endif
	}

    /// <summary>
    /// ����һ����Դ
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public UnityEngine.Object Load(string _path)
    {
#if UNITY_EDITOR
        if (mUseCount.ContainsKey(_path))
        {
            mUseCount[_path] += 1;
        }
        else
        {
            mUseCount[_path] = 1;
        }
#endif

        return TryStoreIntoCache(_path, Resources.Load(_path));
    }

    /// <summary>
    /// ����һ����Դ
    /// </summary>
    /// <param name="_path"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    public UnityEngine.Object Load(string _path, System.Type _type)
    {
#if UNITY_EDITOR
        if (mUseCount.ContainsKey(_path))
        {
            mUseCount[_path] += 1;
        }
        else
        {
            mUseCount[_path] = 1;
        }
#endif

        return TryStoreIntoCache(_path, Resources.Load(_path, _type));
    }

    /// <summary>
    /// ����һ��AudioClip
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public AudioClip LoadAudioClip(string _path)
    {
#if UNITY_EDITOR
        if (mUseCount.ContainsKey(_path))
        {
            mUseCount[_path] += 1;
        }
        else
        {
            mUseCount[_path] = 1;
        }
#endif

        return TryStoreIntoCache(_path, Resources.Load(_path, typeof(AudioClip))) as AudioClip;
    }

    /// <summary>
    /// ����һ��TextAsset
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public TextAsset LoadTextAsset(string _path)
    {
#if UNITY_EDITOR
        if (mUseCount.ContainsKey(_path))
        {
            mUseCount[_path] += 1;
        }
        else
        {
            mUseCount[_path] = 1;
        }
#endif

        return TryStoreIntoCache(_path, Resources.Load(_path, typeof(TextAsset))) as TextAsset;
    }

    /// <summary>
    /// ����һ��GameObject
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public GameObject LoadGameObject(string _path)
    {
#if UNITY_EDITOR
        if (mUseCount.ContainsKey(_path))
        {
            mUseCount[_path] += 1;
        }
        else
        {
            mUseCount[_path] = 1;
        }
#endif

        return TryStoreIntoCache(_path, Resources.Load(_path, typeof(GameObject))) as GameObject;
    }

    /// <summary>
    /// ����һ��Material
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public Material LoadMaterial(string _path)
    {
#if UNITY_EDITOR
        if (mUseCount.ContainsKey(_path))
        {
            mUseCount[_path] += 1;
        }
        else
        {
            mUseCount[_path] = 1;
        }
#endif

        return TryStoreIntoCache(_path, Resources.Load(_path, typeof(Material))) as Material;
    }

    /// <summary>
    /// ����һ��Texture2D
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public Texture2D LoadTexture2D(string _path)
    {
#if UNITY_EDITOR
        if (mUseCount.ContainsKey(_path))
        {
            mUseCount[_path] += 1;
        }
        else
        {
            mUseCount[_path] = 1;
        }
#endif

        return TryStoreIntoCache(_path, Resources.Load(_path, typeof(Texture2D))) as Texture2D;
    }

    /// <summary>
    /// ����ϵͳ�ڴ��С�ж�Cache��С
    /// </summary>
    private SystemLevel mSysLevel = SystemLevel.Unknow;
    private SystemLevel SysLevel
    {
        get
        {
            if (mSysLevel == SystemLevel.Unknow)
            {
                if (SystemInfo.systemMemorySize <= 128)
                {
                    mSysLevel = SystemLevel.Level1;
                }
                else if (SystemInfo.systemMemorySize <= 512)
                {
                    mSysLevel = SystemLevel.Level2;
                }
                else
                {
                    mSysLevel = SystemLevel.Level3;
                }
            }

            return mSysLevel;
        }
    }

    /// <summary>
    /// ����Load��Դ������뻺��
    /// �ڴ�С��128m��ϵͳֻ����Level3����Դ
    /// �ڴ�С��512m��ϵͳֻ����Level2��Level3����Դ
    /// ����ϵͳ����Level1��Level2��Level3����Դ
    /// </summary>
    /// <param name="_file"></param>
    /// <param name="_object"></param>
    /// <returns></returns>
    private UnityEngine.Object TryStoreIntoCache(string _file, UnityEngine.Object _object)
    {
        if (_object == null)
        {
            return null;
        }

        switch (SysLevel)
        {
            case SystemLevel.Level1:
                if (mLv3Resources.ContainsKey(_file))
                {
                    if (mLv3Resources[_file] == null)
                    {
                        mLv3Resources[_file] = _object;
                    }
                }
                break;
            case SystemLevel.Level2:
                if (mLv2Resources.ContainsKey(_file))
                {
                    if (mLv2Resources[_file] == null)
                    {
                        mLv2Resources[_file] = _object;
                    }
                }
                else if (mLv3Resources.ContainsKey(_file))
                {
                    if (mLv3Resources[_file] == null)
                    {
                        mLv3Resources[_file] = _object;
                    }
                }
                break;
            case SystemLevel.Level3:
                if (mLv1Resources.ContainsKey(_file))
                {
                    if (mLv1Resources[_file] == null)
                    {
                        mLv1Resources[_file] = _object;
                    }
                }
                else if (mLv2Resources.ContainsKey(_file))
                {
                    if (mLv2Resources[_file] == null)
                    {
                        mLv2Resources[_file] = _object;
                    }
                }
                else if (mLv3Resources.ContainsKey(_file))
                {
                    if (mLv3Resources[_file] == null)
                    {
                        mLv3Resources[_file] = _object;
                    }
                }
                break;
            default:
                break;
        }

        return _object;
    }
}
