using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 预设对象池
/// </summary>
public class PerfabsPool : MonoBehaviour {
    private class PoolObject
    {
        private bool mActive = false;
        private GameObject mObj = null;

        public bool Active
        {
            get { return mActive; }
            set { mActive = value; }
        }

        public GameObject Obj
        {
            get { return mObj; }
            set { mObj = value; }
        }
    }

    private class PoolSetting
    {
        private string mPerfabName = "";
        private GameObject mPerfab = null;
        private int mPoolSize = 30;

        public string PerfabName
        {
            get { return mPerfabName; }
            set { mPerfabName = value; }
        }

        public GameObject Perfab
        {
            get { return mPerfab; }
            set { mPerfab = value; }
        }

        public int PoolSize
        {
            get { return mPoolSize; }
            set { mPoolSize = value; }
        }
    }

    private Dictionary<string, List<PoolObject>> mPools = new Dictionary<string, List<PoolObject>>();
    private Dictionary<string, PoolSetting> mPoolSettings = new Dictionary<string, PoolSetting>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// 从某个池创建一个perfab
    /// </summary>
    /// <param name="_perfab"></param>
    /// <param name="_poolName"></param>
    /// <returns></returns>
    public GameObject CreateObject(string _perfab, string _poolName)
    {
        if (!mPools.ContainsKey(_poolName))
        {
            mPools[_poolName] = new List<PoolObject>();
            PoolSetting setting = new PoolSetting();
            setting.PerfabName = _perfab;
            mPoolSettings[_poolName] = setting;

            //print("Pool is created: " + _poolName);
        }

        return GetFreeObject(_poolName);
    }

    /// <summary>
    /// 从某个池销毁一个perfab
    /// </summary>
    /// <param name="_object"></param>
    /// <param name="_poolName"></param>
    public void DestoryObject(GameObject _object, string _poolName)
    {
        if (mPools.ContainsKey(_poolName) && !SetFreeObject(_object, _poolName))
        {
            Destroy(_object);
        }
    }

    private GameObject GetFreeObject(string _poolName)
    {
        foreach (PoolObject obj in mPools[_poolName])
        {
            if (!obj.Active)
            {
                obj.Obj.SetActiveRecursively(true);
                obj.Active = true;
                //print("Pool send an object: " + _poolName);
                return obj.Obj;
            }
        }

        if (mPoolSettings[_poolName].Perfab == null)
        {
            mPoolSettings[_poolName].Perfab = GlobalModule.Instance.LoadResource(mPoolSettings[_poolName].PerfabName) as GameObject;
        }
        GameObject newObj = Instantiate(mPoolSettings[_poolName].Perfab) as GameObject;
        
        if (mPools[_poolName].Count < mPoolSettings[_poolName].PoolSize)
        {
            PoolObject po = new PoolObject();
            po.Active = true;
            po.Obj = newObj;
            mPools[_poolName].Add(po);

            //print("Pool add a new object: " + _poolName);
        }
        else
        {
            //print("Pool is max: " + _poolName);
        }

        return newObj;
    }

    private bool SetFreeObject(GameObject _object, string _poolName)
    {
        foreach (PoolObject obj in mPools[_poolName])
        {
            if (obj.Active && _object.GetInstanceID() == obj.Obj.GetInstanceID())
            {
                _object.SetActiveRecursively(false);
                _object.transform.parent = gameObject.transform;
                obj.Active = false;
                //print("Pool free an object: " + _poolName);
                return true;
            }
        }

        //print("Pool can't free an object: " + _poolName);
        return false;
    }
}
