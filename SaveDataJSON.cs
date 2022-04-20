using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SaveDataJSON : MonoBehaviour
{
    [SerializeField]
    Text up, down;

    string svstring;

    [System.Serializable]
    public class SaveData
    {
        public int money;
        public int gcar;
        public int gem;
        public int redblue;

        public CarData[] carData;
    }

    [System.Serializable]
    public class CarData
    {
        public int id;
        public int lv;
        public string tune;
        public float shakou, camber, width;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Upload()
    {
        SaveData sv = new SaveData();
        sv.money = PlayerPrefs.GetInt("money");
        sv.gcar = PlayerPrefs.GetInt("gcar");
        sv.gem = PlayerPrefs.GetInt("gem");
        sv.redblue = PlayerPrefs.GetInt("redblue");

        //ë‰êîäÑÇËèoÇµ
        int i = 0;
        int p = 1;
        int gc = PlayerPrefs.GetInt("gcar");
        for (int j = 0; j < 32; j++)
        {
            if((gc & p) == p)
            {
                i++;
            }
            p *= 2;
        }
        sv.carData = new CarData[i];

        //idÇ≤Ç∆Ç…ãLò^
        i = 0;
        p = 1;
        for (int j = 0; j < 32; j++)
        {
            if ((gc & p) == p)
            {
                sv.carData[i] = new CarData();
                sv.carData[i].id = j;
                if (PlayerPrefs.HasKey("car" + j))
                {
                    sv.carData[i].tune = PlayerPrefs.GetString("car" + j);
                }
                sv.carData[i].shakou = PlayerPrefs.GetFloat("shakou" + j);
                sv.carData[i].camber = PlayerPrefs.GetFloat("camber" + j);
                sv.carData[i].width = PlayerPrefs.GetFloat("width" + j);
                sv.carData[i].lv = PlayerPrefs.GetInt("carlev" + j);

                i++;
            }
            p *= 2;
        }

        svstring = JsonUtility.ToJson(sv);
        NCMBPlayerPrefs.SetString("data", svstring);
        NCMBPlayerPrefs.Save();
        up.text = "...";
        Invoke("SaveCheck", 1);
    }

    void SaveCheck()
    {
        if (NCMBPlayerPrefs.GetString("data").Equals(svstring))
        {
            if (PlayerPrefs.GetInt("lang") == 1)
            {
                up.text = "äÆóπ";
            }
            else
            {
                up.text = "Done";
            }
        }
        else
        {
            up.text = "ERROR";
        }
    }

    public void Download()
    {
        string ddata = NCMBPlayerPrefs.GetString("data");

        if (ddata.Length < 3)
        {
            down.text = "ERROR";
            return;
        }
        SaveData load = JsonUtility.FromJson<SaveData>(ddata);

        PlayerPrefs.SetInt("money", load.money);
        PlayerPrefs.SetInt("gcar", load.gcar);
        PlayerPrefs.SetInt("gem", load.gem);
        PlayerPrefs.SetInt("redblue", load.redblue);
        
        for(int i=0; i < load.carData.Length; i++)
        {
            int j = load.carData[i].id;
            PlayerPrefs.SetString("car" + j, load.carData[i].tune);
            PlayerPrefs.SetFloat("shakou" + j, load.carData[i].shakou);
            PlayerPrefs.SetFloat("camber" + j, load.carData[i].camber);
            PlayerPrefs.SetFloat("width" + j, load.carData[i].width);
            PlayerPrefs.SetInt("carlev" + j, load.carData[i].lv);
        }

        SceneManager.LoadScene("title");

    }
}
