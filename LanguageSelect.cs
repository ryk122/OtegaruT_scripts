using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelect : MonoBehaviour
{

    public void SetLang(int i)
    {
        //1:ja  0:en
        PlayerPrefs.SetInt("lang", i);
        gameObject.SetActive(false);
    }
}
