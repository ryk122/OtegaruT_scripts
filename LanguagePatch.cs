using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#pragma warning disable 649

public class LanguagePatch : MonoBehaviour
{
    [SerializeField]
    string[] jp,en;

    [SerializeField]
    Text[] text;


    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("lang") == 1)
        {
            for (int i = 0; i < text.Length; i++)
                text[i].text = jp[i];
        }
        else
        {
            for (int i = 0; i < text.Length; i++)
                text[i].text = en[i];
        }
    }

}
