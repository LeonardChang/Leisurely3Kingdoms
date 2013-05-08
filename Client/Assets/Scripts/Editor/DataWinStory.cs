using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using OOEditor;

public class DataWinStory : EditorWindow
{
    public static ArrayData mArrayData = new ArrayData();
    public Vector2 scrollPosition = new Vector2();
    public static TextAsset mAsset;
    public string mAssetPath;
    public string mEnumPath = "";

    public static int[] rowWidth = { 100, 100, 400, 400,100};

    [MenuItem("OORoom/剧情/剧情数据")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(DataWinStory));
        mAsset = null;
    }


    void ReadData()
    {
        if (mAsset)
        {
            mAssetPath = AssetDatabase.GetAssetPath(mAsset.GetInstanceID());
        }

        try
        {
            StreamReader reader = new StreamReader(File.Open(mAssetPath, FileMode.Open), System.Text.Encoding.UTF8);
            XmlSerializer xmls = new XmlSerializer(typeof(ArrayData));
            mArrayData = xmls.Deserialize(reader) as ArrayData;
            reader.Close();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
        }
    }


    void OnGUI()
    {
        
        //"R"按钮   以及object吸取器
        GUILayout.BeginHorizontal();

        if(!mAsset)
        {
            mAsset = (TextAsset)Resources.Load("Data/Story/Story1");
            ReadData();
        }

        if (GUILayout.Button("R", GUILayout.Width(30)) && mAsset)
        {
            ReadData();
        }
        mAsset = (TextAsset)EditorGUILayout.ObjectField(mAsset, typeof(UnityEngine.TextAsset), false);        
        GUILayout.EndHorizontal();





        if (mArrayData == null)
        {
            return;
        }
        //新数据表
        if (mArrayData.mData.Count == 0)
        {
            if(GUILayout.Button("+", GUILayout.Width(30)))
            {
                mArrayData.InsertRow(0);
                //mArrayData.InsertLine(0);
            }
        }
        


        if(mArrayData.mData.Count > 0)
        {

            GUILayout.BeginHorizontal();

            //“S”按钮
            if(GUILayout.Button("S", GUILayout.Width(30)) && mAsset != null)
            {
                try
                {

               
                string folder = mAssetPath.Substring(0, mAssetPath.LastIndexOf('/')+1) + "_Back/";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                FileUtil.CopyFileOrDirectoryFollowSymlinks(mAssetPath, folder + mAsset.name + DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss") + ".txt");
                MonoBehaviour.print(folder);

                StreamWriter writer = new StreamWriter(File.Open(mAssetPath, FileMode.Create), System.Text.Encoding.UTF8);
                XmlSerializer xmls = new XmlSerializer(typeof(ArrayData));
                xmls.Serialize(writer, mArrayData);
                writer.Close();
                MonoBehaviour.print(mAssetPath);
                AssetDatabase.Refresh();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.Log(e.Message);
                }
            }


            //按钮行
            for (int i = 0; i < mArrayData.mData.Count; i ++ )
            {
                if(GUILayout.Button("+  " + i.ToString()))
                {
                    mArrayData.InsertRow(i);
                    break;
                }


                if(GUILayout.Button("-"))
                {
                    mArrayData.DeleteRow(i);
                    break;
                }
            }

            if(GUILayout.Button("+", GUILayout.Width(30)))
            {
                mArrayData.InsertRow(mArrayData.mData.Count);
            }
            GUILayout.EndHorizontal();


            //字段名
            if (mArrayData.mLineCount > 0)
            {
                GUILayout.BeginHorizontal();
                Color color = GUI.backgroundColor;
                GUI.backgroundColor = Color.green;
                GUILayout.Button("", GUILayout.Width(30));

                for (int j = 0; j < mArrayData.mRowCount; j++)
                {
                    mArrayData.mData[j][0] = EditorGUILayout.TextField(mArrayData.mData[j][0]);
                }

                GUILayout.Button("", GUILayout.Width(30));

                GUI.backgroundColor = color;
                GUILayout.EndHorizontal();

            }


            scrollPosition = GUILayout.BeginScrollView(scrollPosition);
            for(int i = 0; i < mArrayData.mLineCount; i++)
            {
                if(i %2 == 0)
                {
                    GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f, 1f);
                }
                else
                {
                    GUI.backgroundColor = new Color(0.7f, 0.7f, 0.7f, 1f);
                }

                if (mArrayData.mData[0][i] != "")
                {
                    GUI.backgroundColor = Color.yellow;
                }

                if(i == 0)
                {
                    GUI.backgroundColor = Color.red;
                }
                GUILayout.BeginHorizontal();



                //id
                if(GUILayout.Button(i.ToString(), GUILayout.Width(30)))
                {
                    mArrayData.InsertLine(i);
                    break;
                }

                //数据项
                for (int j = 0; j < mArrayData.mRowCount; j ++ )
                {
                    //int size = GUI.skin.textField.fontSize;


                    //GUI.skin.textField.fontSize = 16;
                    mArrayData.mData[j][i] = EditorGUILayout.TextField(mArrayData.mData[j][i]);
                    //GUI.skin.textField.fontSize = 12;
                }

                if(GUILayout.Button("-", GUILayout.Width(30)))
                {
                    mArrayData.DeleteLine(i);
                    break;
                }
                GUILayout.EndHorizontal();
            }


            
            GUILayout.BeginHorizontal();
            GUI.backgroundColor = Color.gray + Color.grey;
            if(GUILayout.Button("+", GUILayout.Width(30)))
            {
                mArrayData.InsertLine(mArrayData.mLineCount);
            }
            for (int i = 0; i < mArrayData.mData.Count; i++)
            {
                EditorGUILayout.TextField("");
            }
            GUILayout.Button(" ", GUILayout.Width(30));
            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }



        GUILayout.BeginHorizontal();
        if (GUILayout.Button("G", GUILayout.Width(30)))
        {
            string outstr = "public enum " + mEnumPath + "{\n";

            for (int i = 1; i < mArrayData.mLineCount; i++)
            {
                if (mArrayData.mData[0][i] != "")
                {
                    outstr = outstr + "\t" + mArrayData.mData[0][i] + " = " + i.ToString() + "," + "\n";
                }
            }

            outstr = outstr  + "Max\n"  + "}";

            mEnumPath = outstr;
        }
        //mEnumPath = EditorGUILayout.TextField(mEnumPath);
        mEnumPath = EditorGUILayout.TextField(mEnumPath);
        GUILayout.EndHorizontal();

    }
}
