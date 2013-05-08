using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;
//using OOEditor;

public class DeleteSave : EditorWindow
{

    [MenuItem("OORoom/删除存档")]
    static void Init()
    {
        //EditorWindow.GetWindow(typeof(DataWin3));
        string path = Application.persistentDataPath + "/ZombieGameData.gdata";
        FileUtil.DeleteFileOrDirectory(path);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }


    /// <summary>
    /// 设置参数
    /// </summary>
    /// <param name="_str"></param>
    /// <param name="_value"></param>
    static void SetOption(string _str, string _value)
    {
        OOEditor.ArrayData data = OOEditor.ArrayData.ReadData("Data/OptionValue");

        for (int i = 1; i < data.mLineCount; i++)
        {

            if (data.mData[0][i] == _str)
            {
                data.mData[1][i] = _value;
            }
        }

        data.SaveData("Data/OptionValue");
    }

    /// <summary>
    /// 设置语言参数
    /// </summary>
    /// <param name="_value"></param>
    static void SetLanguage(string _value)
    {
        SetOption("Language", _value);
    }

    [MenuItem("OORoom/语言/英文")]
    static void SetToEnglish()
    {
        SetLanguage("English");
    }

    [MenuItem("OORoom/语言/中文")]
    static void SetToChinese()
    {
        SetLanguage("Chinese");
    }

    [MenuItem("OORoom/语言/中文繁体")]
    static void SetToChineseTw()
    {
        SetLanguage("ChineseTw");
    }

    [MenuItem("OORoom/语言/日文")]
    static void SetToJapanese()
    {
        SetLanguage("Japanese");
    }


    [MenuItem("OORoom/语言/多语言")]
    static void SetToLocal()
    {
        SetLanguage("All");
    }


    [MenuItem("OORoom/商店类型/正常")]
    static void ShopNormal()
    {
        SetOption("ShopType", "0");
    }

    [MenuItem("OORoom/商店类型/中国移动+机锋")]
    static void ShopCMCC()
    {
        SetOption("ShopType", "1");
    }

    
    [MenuItem("OORoom/AdMob广告/启用")]
    static void AdMobEnable()
    {
        SetOption("AdMob", "1");
    }

    [MenuItem("OORoom/AdMob广告/禁用")]
    static void AdMobDisable()
    {
        SetOption("AdMob", "0");
    }
}
