﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//carに適用 レース開始時に自動で変更する。
//garage中ではGarageTuneからSettuneを使う
public class TuneSetter : MonoBehaviour
{
    [SerializeField]
    Carmain cm;
    [SerializeField]
    public float pmaxs, pslip, pstr;
    [SerializeField]
    public bool skinmesh,bodytune,turbo,garage;
    [SerializeField]
    public GameObject nomal, tuned;
    [SerializeField]
    public GameObject[] wheel;
    public Material[] material;

    public ChangeColor[] changecolor;

    Vector3 nombasePos,tunbasePos;
    Vector3[] wheelpos = new Vector3[4];
    bool tunedMode;

    // Start is called before the first frame update
    void Start()
    {
        if (wheel.Length == 0)
            Debug.LogError("No Wheel Material");

        if (garage)
            return;

        int tune;
        int dcar = PlayerPrefs.GetInt("dcar");
        GetBasePos(dcar);
        if (PlayerPrefs.HasKey("car" + dcar)){
            string data = PlayerPrefs.GetString("car" + dcar);
            tune = data[0] - '0';
            int m = data[1] - '0';
            if (m < material.Length)
                SetWheel(material[m]);

            float shakou = PlayerPrefs.GetFloat("shakou" + dcar);
            SetShakou(shakou);

            float camber = PlayerPrefs.GetFloat("camber" + dcar);
            SetCamber(camber);

            float width = PlayerPrefs.GetFloat("width" + dcar);
            SetWidth(width);
        }
        else
        {
            PlayerPrefs.SetString("car" + dcar, "00#nnnnnn");//[0]=body,[0]=wheel
            tune = 0;
        }

        cm = GetComponent<Carmain>();
        if(bodytune)
            SetTune(tune);
        if (skinmesh)
        {
            if (tune == 1)
                cm.skm = tuned.GetComponent<SkinnedMeshRenderer>();
            else
                cm.skm = nomal.GetComponent<SkinnedMeshRenderer>();
            cm.LightOnOff(); cm.LightOnOff();
        }
    }

    public void SetTune(int tune)
    {
        Debug.Log("tune :" + tune);

        if (tune == 0)
        {
            tuned.SetActive(false);
            nomal.SetActive(true);
            //nombasePos = nomal.transform.localPosition;
            tunedMode = false;
        }
        else if (tune == 1)
        {
            tuned.SetActive(true);
            nomal.SetActive(false);
            //tunbasePos = tuned.transform.localPosition;
            tunedMode = true;
            if (cm != null)
            {
                cm.maxs += pmaxs;
                cm.slip += (int)pslip;
                cm.str += pstr;
                cm.turbo = turbo;
                cm.GetSoundSource();
            }
        }
    }

    public void SetWheel(Material m)
    {
        foreach(GameObject t in wheel)
        {
            t.GetComponent<Renderer>().material = m;
        }
    }

    public void SetCamber(float angle)
    {
        wheel[0].transform.localEulerAngles = new Vector3(-angle, 90, 0);
        wheel[1].transform.localEulerAngles = new Vector3(-angle, -90, 0);
        wheel[2].transform.localEulerAngles = new Vector3(-angle, 90, 0);
        wheel[3].transform.localEulerAngles = new Vector3(-angle, -90, 0);

    }

    public void SetWidth(float widthDiff)
    {
        wheel[0].transform.localPosition = wheelpos[0] + new Vector3(widthDiff, 0, 0);
        wheel[1].transform.localPosition = wheelpos[1] + new Vector3(-widthDiff, 0, 0);
        wheel[2].transform.localPosition = wheelpos[2] + new Vector3(widthDiff, 0, 0);
        wheel[3].transform.localPosition = wheelpos[3] + new Vector3(-widthDiff, 0, 0);
    }

    public void SetShakou(float diff)
    {
        nomal.transform.localPosition = new Vector3(0, diff, 0) + nombasePos;
        if(tuned!=null)
            tuned.transform.localPosition = new Vector3(0, diff, 0) + tunbasePos;

        if (!garage)
            GetComponent<Carmain>().tlight.transform.localPosition += new Vector3(0, diff, 0);
    }

    public void GetBasePos(int dcar)
    {
        string data = PlayerPrefs.GetString("car" + dcar);
        int tune = data[0] - '0';

        nombasePos = nomal.transform.localPosition;
        if (tuned != null)
            tunbasePos = tuned.transform.localPosition;

        for (int i = 0; i < 4; i++)
        {         
            wheelpos[i] = wheel[i].transform.localPosition;
        }
    }
}
