using UnityEngine;
using NCMB;

public class NCMBPlayerPrefs : MonoBehaviour
{
    public static int GetInt(string keyName)
    {
        //�L�[�����݂���ꍇ�͒l���擾���܂�//
        //���݂��Ȃ��ꍇ�� defaultValue ��Ԃ��܂��B//
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
        //�L�[�����݂���ꍇ�͒l���擾���܂�//
        //���݂��Ȃ��ꍇ�� defaultValue ��Ԃ��܂��B//
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
        //�L�[��ۑ����܂�//
        NCMBUser.CurrentUser[keyName] = value;
    }

    public static void SetString(string keyName, string value)
    {
        //�L�[��ۑ����܂�//
        NCMBUser.CurrentUser[keyName] = value;
    }

    public static void Save()
    {
        //���쌋�ʂ�ۑ����܂�//
        NCMBUser.CurrentUser.SaveAsync();
    }

    public static bool HasKey(string key)
    {
        //�L�[�����݂��邩�m�F���܂�//
        return NCMBUser.CurrentUser.ContainsKey(key);
    }

    public static void DeleteKey(string key)
    {
        //�Ή�����L�[(�t�B�[���h)���폜���܂�//
        NCMBUser.CurrentUser.Remove(key);
    }
}