﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class UserStage : MakeRoad
{
    //Moji_dispのコードを生かすために、継承
    //setroadは最初に一回呼ぶだけにしよう。つまり既存checkpointは使用させない
    public StartCtrl strtctrl;
    public GameObject lcanvas;
    public GameObject starttext;
    public GameObject lightb;
    public GameObject android;
    public AndroidCtrl adcrl;
    public GameObject cone, section, stop, streetlamp;
    public bool pc;
    string stagestr;
    protected string[] data;
    public InputField inputField;
    public Text laptext; public static Text ltext;
    public Text buttontext;

    protected bool err;
    protected float scale;

    [SerializeField]
    GameObject rainobj;
    protected bool raining;

    [SerializeField]
    TimeSeteer timeSeteer;

    private GameObject targettmp;

    private int roadnum;

    private Vector3 lastTargetPos;

    private Color usercolor;

    // Start is called before the first frame update
    void Start()
    {
        err = false;
        raining = false;
        ltext = laptext;
        scale = 1;
        roadnum = 0;

        Point.putnum = 0;
        Point.points= new Point[1000];
        lastTargetPos = transform.position;

        usercolor = new Color(1, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (raining)
        {
            if (strtctrl.cm.speed > 10)
                strtctrl.cm.k = true;
            else
                strtctrl.cm.k = false;
        }
    }

    //UserStageのSetRoad オーバーライドはしていない点注意
    public void SetRoad()
    {
        for (int i = 0; i < data.Length; i++)
        {
            GameObject Endobj,road;

            string roaddata = Loader(i);
            if (roaddata == null)
                continue;

            if (VS && roadnum > 0 && Vector3.SqrMagnitude(transform.position - lastTargetPos) > 0.01f)
            {
                targettmp = Instantiate(Target, transform.position, transform.rotation);
                lastTargetPos = transform.position;
            }

            road = Interpreter(roaddata);


            if (road == null)
            {//err
                err = true;
                buttontext.text = "Reset";
                break;
            }

            if (road == this.gameObject)
            {
                //Destroy(targettmp);
                //Point.putnum--;
                continue;
            }


            Endobj = road.transform.Find("End").gameObject;
            transform.position = Endobj.transform.position;
            transform.rotation = Endobj.transform.rotation;
            roadnum++;

            if (roaddata[2] == '1')//登りなら
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 180, transform.eulerAngles.z);
            }
        }
    }

    //読み込んだデータから、置くべきオブジェクトを返す
    GameObject Interpreter(string str)
    {
        GameObject putroad=null;
        //途中でエラー時は表示してボタン復活
        
        //構文エラー ただし特殊コマンドをのぞく
        if (str.Length != 3)
        {
            if (str.Equals("#STOP"))
            {
                Debug.Log("stop");
                return Instantiate(stop, transform.position, transform.rotation);
            }
            else if (str.Equals("#SECTION"))
            {
                Debug.Log("section");
                return Instantiate(section, transform.position, transform.rotation);
            }
            else if (str.Length>11 && str.Substring(0,11).Equals("#ENDSECTION"))
            {
                Debug.Log("Endsection");
                GameObject g = Instantiate(section, transform.position, transform.rotation);
                Section s = g.transform.GetChild(0).GetComponent<Section>();
                s.end = true; s.stagename = str.Substring(11, str.Length - 11);
                return g;
            }
            else if (str.Equals("#LAMPL"))
            {
                Debug.Log("section");
                return Instantiate(streetlamp, transform.position, transform.rotation);
            }
            else if (str.Equals("#LAMPR"))
            {
                Debug.Log("section");
                GameObject p = Instantiate(streetlamp, transform.position, transform.rotation);
                Vector3 size = p.transform.localScale;
                size.y *= -1;
                p.transform.localScale = size;
                return p;
            }
            else if (str.Equals("#CONE"))
            {
                Debug.Log("cone");
                return Instantiate(cone, transform.position, transform.rotation);
            }
            else if (str.Equals("#RAIN"))
            {
                Debug.Log("rain");
                Instantiate(rainobj, Camera.main.transform.position + Camera.main.transform.up * 15 + Camera.main.transform.forward * 20, transform.rotation).transform.parent = Camera.main.transform;
                timeSeteer.SetRain();
                raining = true;
                return gameObject;
            }
            else if (str.Substring(0, 3).Equals("#VS"))
            {
                int num;
                if (int.TryParse(str.Substring(4, str.Length - 4), out num))
                {
                    Debug.Log(num);
                    VS = true;
                    strtctrl.vs = true;
                    Title.vscarNum = num;
                    return this.gameObject;
                }
                else
                {
                    ErrPrint(1, str);
                    Debug.Log("syntax err");
                    return null;
                }
            }
            else if (str.Equals("#CUBE"))
            {
                var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = transform.position;
                cube.GetComponent<Renderer>().material.SetColor("_Color", usercolor);
                return gameObject;
            }
            else if (str.Substring(0, 6).Equals("#COLOR"))
            {
                if (ColorUtility.TryParseHtmlString("#"+str.Substring(6, str.Length - 6), out usercolor))
                {
                    Debug.Log(usercolor);
                }
                return gameObject;
            }
            else if (str.Substring(0, 6).Equals("#SCALE"))
            {
                float s;
                if(float.TryParse(str.Substring(6, str.Length-6), out s))
                {
                    scale = s/100;
                    Debug.Log(scale);
                    return this.gameObject;
                }
                else
                {
                    ErrPrint(1, str);
                    Debug.Log("syntax err");
                    return null;
                }
            }
            else if (str.Substring(0, 5).Equals("#POFS"))
            {
                float ofs;
                if (float.TryParse(str.Substring(6, str.Length - 6), out ofs))
                {
                    Debug.Log(ofs);
                    Vector3 pos = transform.position;
                    switch (str[5])
                    {
                        case 'X': pos.x += ofs; break;
                        case 'Y': pos.y += ofs; break;
                        case 'Z': pos.z += ofs; break;
                        default:
                            ErrPrint(1, str);
                            Debug.Log("syntax err");
                            return null;

                    }
                    transform.position = pos;
                    return this.gameObject;
                }
                else
                {
                    ErrPrint(1, str);
                    Debug.Log("syntax err");
                    return null;
                }
            }
            else if (str.Substring(0, 5).Equals("#ROFS"))
            {
                float ofs;
                if (float.TryParse(str.Substring(6, str.Length - 6), out ofs))
                {
                    Debug.Log(ofs);
                    Vector3 rot = transform.eulerAngles;
                    switch (str[5])
                    {
                        case 'X': rot.x += ofs; break;
                        case 'Y': rot.y += ofs; break;
                        case 'Z': rot.z += ofs; break;
                        default:
                            ErrPrint(1, str);
                            Debug.Log("syntax err");
                            return null;

                    }
                    transform.eulerAngles = rot;
                    return this.gameObject;
                }
                else
                {
                    ErrPrint(1, str);
                    Debug.Log("syntax err");
                    return null;
                }
            }
            else
            {
                if (str[0] == '$'||str[0]=='h')
                    JudgeKey(str);
                ErrPrint(1, str);
                Debug.Log("syntax err");
                return null;
            }
        }

        //up down の判定ののち どのパーツを返すかroadにをきめ、

        Debug.Log(str);
        if (str[2] == '0')//down
        {
            switch (str[0])
            {
                case '0': putroad = Road[0]; break;
                case '1': putroad = Road[1]; break;
                case '2': putroad = Road[2]; break;
                case '3': putroad = Road[3]; break;
                case '4': putroad = Road[9]; break;
                case '5': putroad = Road[11];break;
                case '6': putroad = Road[12]; break;
                case '7': putroad = Road[13]; break;
                case '9': putroad = Road[8]; break;
                default://未定義エラー
                    ErrPrint(0, str);
                    Debug.Log("data err");
                    return null;
            }
        }
        else if(str[2] == '1')//up
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y+180, transform.eulerAngles.z);
            switch (str[0])
            {
                case '0': putroad = Road[4]; break;
                case '1': putroad = Road[5]; break;
                case '2': putroad = Road[6]; break;
                case '3': putroad = Road[7]; break;
                case '4': putroad = Road[10]; break;
                case '9': putroad = Road[8]; break;
                default://未定義エラー
                    ErrPrint(0, str);
                    Debug.Log("data err");
                    return null;
            }
        }
        else
        {
            ErrPrint(0, str);
            Debug.Log("data err");
            return null;
        }

        GameObject put = Instantiate(putroad, transform.position, transform.rotation);

        //もし、反転の必要があるのなら返すものを反転させて
        if (str[1] == '1')
        {
            Vector3 size = put.transform.localScale;
            size.y *= -1;
            put.transform.localScale = size;
        }

        Vector3 siz = put.transform.localScale;
        siz.y *= scale;
        siz.x *= scale;
        put.transform.localScale = siz;


        return put;
    }

    public void LoadButton()
    {
        if (err)//reset button
        {
            SceneManager.LoadScene("loadstage");
            return;
        }
        //文字列を解釈し、int配列に入れる　ボタンを消しその後setroadを呼びレース開始
        stagestr = inputField.text;
        data = stagestr.Split(',');
        SetRoad();
        if (!err)
        {
            Debug.Log("ok");
            lightb.SetActive(true);
            starttext.SetActive(true);
            strtctrl.enabled = true;
            lcanvas.SetActive(false);
            if (!pc)
                android.SetActive(true);

            if (raining)
            {
                Invoke("RainBehavior", 2);
            }
        }
    }

    void RainBehavior()
    {
        strtctrl.cm.str *= 1.7f;
        strtctrl.cm.slip += 3;
    }

    void ErrPrint(int i,string s)
    {
        string e;
        switch (i)
        {
            case 0:
                e = "data err 解釈不能な命令";
                break;
            case 1:
                e = "syntax err 文法エラー";
                break;
            case 9:
                e = "get coin";
                break;
            default:
                e = "unknow err 不明エラー";
                break;
        }

        inputField.text = e + "\n >" + s + "\n\n" + "Resetを押してください。\nPush Reset button.";
        inputField.interactable = false;
    }

    string Loader(int i)
    {
        /*読込み仕様
         * カンマ区切りで読み込んだものを
         * 以下は動的に対応
         * 先頭末尾の無効文字無視 https://dobon.net/vb/dotnet/string/trim.html
         * 空文字列も無視
         * 構文は(map)(lr)(udn)
         * コメント記述は#スタートの1命令とする: #xxx,
         * 
         * */

        data[i] = data[i].Trim();
        if (data[i].Equals(""))
            return null;
        if (data[i][0] == '%')
            return null;

        Debug.Log(data[i]);
        return data[i];
    }

    public void WikiButton()
    {
        Application.OpenURL("https://otegarut.wiki.fc2.com/wiki/%E4%BD%BF%E3%81%84%E6%96%B9");
    }

    public void DataPark()
    {
        Application.OpenURL("https://otegarut.wiki.fc2.com/wiki/%E3%82%B9%E3%83%86%E3%83%BC%E3%82%B8%E3%83%87%E3%83%BC%E3%82%BF%E5%85%AC%E9%96%8B%E5%BA%83%E5%A0%B4%28%E7%B7%A8%E9%9B%86%E5%8F%AF%E8%83%BD%29");
    }

    void JudgeKey(string s)
    {
        int[] hash = { 13, 10, 9, 23, 37, 4, 22, 2, 3, 7, 30 };

        if (PlayerPrefs.HasKey("code") && PlayerPrefs.GetString("code").Equals(s))
        {
            return;
        }

        //
        if (s.Equals("$coinmax"))
        {
            inputField.text = "allmax cheat\n";
            PlayerPrefs.SetInt("money", int.MaxValue/2);
            PlayerPrefs.SetInt("gcar", 262143);
            return;
        }

        if (s.Equals("$levmax"))
        {
            for (int i = 0; i < 20; i++)
                PlayerPrefs.SetInt("carlev" + i, 99);
            PlayerPrefs.SetInt("cheat", 1);
        }

        if (s.Equals("$gemmax"))
        {
            PlayerPrefs.SetInt("gem", 100);
        }

        if(s.Substring(0, 7).Equals("$setcar"))
        {
            Debug.Log(s.Substring(7, s.Length-7));
            int num;
            if (int.TryParse(s.Substring(7, s.Length - 7), out num))
            {
                PlayerPrefs.SetInt("dcar", num);
                if (num == 99)
                    SceneManager.LoadScene("PunBasics-Launcher");
            }
        }

        if (s.Equals("$allreset"))
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("notice");
        }

        if (s.Substring(0, 4).Equals("http"))
        {
            StartCoroutine(CheckURL(s));
        }

        Debug.Log("judge key");
        int x = 0;
        for (int i = 1; i < s.Length-1; i++)
        {
            x += s[i];
            if (s[i] % hash[i - 1] != 0)
                return;
        }
        if ((x % 26 + 65) != s[s.Length-1])
            return ;

        ErrPrint(9, "");
        Debug.Log("get coin");
        int c = PlayerPrefs.GetInt("money");
        PlayerPrefs.SetInt("money", c + 100000);
        PlayerPrefs.SetString("code", s);
        SceneManager.LoadScene("garage");

    }

    /* java code
     *	public String makeKey(int i) {
		String s = "";
		int x=0;
		for(int p=0;p<i-1;p++) {
			while(true) {
				int r = random.nextInt(26) + 65 + random.nextInt(2)*32;
				if(judgeKey(p,(char)r)) {
					x+=r;
					s += (char)r;
					break;
				}
			}
		}
		s+=(char)(x%26 +65);
		return s;
	}*/

    IEnumerator CheckURL(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                int num;
                if (int.TryParse(webRequest.downloadHandler.text, out num)) {
                    PlayerPrefs.SetInt("dcar", num);
                    SceneManager.LoadScene("PunBasics-Launcher");
                }
                //ErrPrint(99, webRequest.downloadHandler.text);
            }
        }
    }

}
