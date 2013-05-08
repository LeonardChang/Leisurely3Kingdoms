using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
using OOEditor;

public class TestWin2 : EditorWindow
{
    public static ArrayData mArrayData = new ArrayData();
    public Vector2 scrollPosition = new Vector2();
    public static TextAsset mAsset;
    public string mAssetPath;
    public string mEnumPath = "";

    [MenuItem("Assets/Create/TxT")]
    static void CreateTxt()
    {
        string str = AssetDatabase.GetAssetPath(Selection.activeObject.GetInstanceID());
        int i = 0;
        while (File.Exists(str + "/" + "___newData" + i.ToString() + ".txt"))
        {
            i++;
        }
        FileStream fs;
        fs = File.Create(str + "/" + "___newData" + i.ToString() + ".txt");
        fs.Close();
        AssetDatabase.Refresh();
    }


    [MenuItem("OORoom/------通用-------")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(TestWin2));
    }

    void OnGUI()
    {
        
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("R", GUILayout.Width(30)) && mAsset)
        {
            StreamReader reader = new StreamReader(File.Open(mAssetPath, FileMode.Open), System.Text.Encoding.UTF8);
            XmlSerializer xmls = new XmlSerializer(typeof(ArrayData));
            mArrayData = xmls.Deserialize(reader) as ArrayData;
            reader.Close();
        }
        mAsset = (TextAsset)EditorGUILayout.ObjectField(mAsset, typeof(UnityEngine.TextAsset), false);
        if(mAsset)
        {
            mAssetPath = AssetDatabase.GetAssetPath(mAsset.GetInstanceID());
        }
        GUILayout.EndHorizontal();


         


        if (mArrayData == null)
        {
            return;
        }
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

            if(GUILayout.Button("S", GUILayout.Width(30)) && mAsset != null)
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



            if (mArrayData.mLineCount > 0)
            {
                GUILayout.BeginHorizontal();
                Color color = GUI.backgroundColor;
                GUI.backgroundColor = Color.red;
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
                    GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f, 1f);
                }
                if(i == 0)
                {
                    GUI.backgroundColor = Color.green;
                }
                GUILayout.BeginHorizontal();

                if(GUILayout.Button(i.ToString(), GUILayout.Width(30)))
                {
                    mArrayData.InsertLine(i);
                    break;
                }

                for (int j = 0; j < mArrayData.mRowCount; j ++ )
                {
                    mArrayData.mData[j][i] = EditorGUILayout.TextField(mArrayData.mData[j][i]);
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
