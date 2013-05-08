using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;

namespace OOEditor
{

    [Serializable]
    public class ArrayData
    {
        public int mRowCount = 0;
        public int mLineCount = 0;

        public List<List<string>> mData = new List<List<string>>();

        /// <summary>
        /// 添加一列
        /// </summary>
        /// <param name="_index"></param>
        public void InsertRow(int _index)
        {
            List<string> tmp = new List<string>();
            for (int i = 0; i < mLineCount; i++)
            {
                tmp.Add("");
            }
            mData.Insert(_index, tmp);
            mRowCount++;
        }

        /// <summary>
        /// 删除一列
        /// </summary>
        public void DeleteRow(int _index)
        {

            mData.RemoveAt(_index);
            mRowCount--;
            if (mRowCount == 0)
            {
                mLineCount = 0;
            }
        }

        /// <summary>
        /// 添加一行
        /// </summary>
        /// <param name="_index"></param>
        public void InsertLine(int _index)
        {
            foreach (List<string> list in mData)
            {
                list.Insert(_index, "");
            }
            mLineCount++;
        }

        /// <summary>
        /// 删除一行
        /// </summary>
        /// <param name="_index"></param>
        public void DeleteLine(int _index)
        {
            foreach (List<string> list in mData)
            {
                list.RemoveAt(_index);
            }
            mLineCount--;
        }


        public ArrayData()
        {

        }

        public ArrayData(string _asset)
        {

        }


        public static ArrayData ReadData(string _asset)
        {
            TextAsset txt = (TextAsset)Resources.Load(_asset);
            string path = "";

            if (!txt)
            {
                return null;
            }
            path = AssetDatabase.GetAssetPath(txt.GetInstanceID());

            ArrayData mArrayData = new ArrayData();
            try
            {
                StreamReader reader = new StreamReader(File.Open(path, FileMode.Open), System.Text.Encoding.UTF8);
                XmlSerializer xmls = new XmlSerializer(typeof(ArrayData));
                mArrayData = xmls.Deserialize(reader) as ArrayData;
                reader.Close();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
            }
            Resources.UnloadAsset(txt);
            return mArrayData;
        }

        public void SaveData(string _asset)
        {
            TextAsset txt = (TextAsset)Resources.Load(_asset);
            string path = "";

            if (!txt)
            {
                return;
            }
            path = AssetDatabase.GetAssetPath(txt.GetInstanceID());


            try
            {
                StreamWriter writer = new StreamWriter(File.Open(path, FileMode.Create), System.Text.Encoding.UTF8);
                XmlSerializer xmls = new XmlSerializer(typeof(ArrayData));
                xmls.Serialize(writer, this);
                writer.Close();
                AssetDatabase.Refresh();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
            }
        }

    }

}
