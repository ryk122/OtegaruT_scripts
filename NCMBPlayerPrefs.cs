using UnityEngine;
using NCMB;

public class NCMBPlayerPrefs : MonoBehaviour
{
    public static int GetInt(string keyName)
    {
        //キーが存在する場合は値を取得します//
        //存在しない場合は defaultValue を返します。//
        int value;

        if (NCMBUser.CurrentUser != null)
        {
            value = (int)NCMBUser.CurrentUser[keyName];
        }
        else
        {
            value = 0;
        }
        return value;
    }

    public static string GetString(string keyName)
    {
        //キーが存在する場合は値を取得します//
        //存在しない場合は defaultValue を返します。//
        string value;

        if (NCMBUser.CurrentUser != null)
        {
            value = (string)NCMBUser.CurrentUser[keyName];
        }
        else
        {
            value = "";
        }
        return value;
    }

    public static void SetInt(string keyName, int value)
    {
        //キーを保存します//
        NCMBUser.CurrentUser[keyName] = value;
    }

    public static void SetString(string keyName, string value)
    {
        //キーを保存します//
        NCMBUser.CurrentUser[keyName] = value;
    }

    public static void Save()
    {
        //操作結果を保存します//
        NCMBUser.CurrentUser.SaveAsync();
    }

    public static bool HasKey(string key)
    {
        //キーが存在するか確認します//
        return NCMBUser.CurrentUser.ContainsKey(key);
    }

    public static void DeleteKey(string key)
    {
        //対応するキー(フィールド)を削除します//
        NCMBUser.CurrentUser.Remove(key);
    }
}