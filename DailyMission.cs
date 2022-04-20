using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyMission : MonoBehaviour
{
    [SerializeField]
    Text vsText, eventText, onlineText, totalText;

    [SerializeField]
    Image button;

    // Start is called before the first frame update
    public void Start()
    {
        Display();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Display()
    {
        int i;

        i = PlayerPrefs.GetInt("dailyt");
        if (i == -1)
        {
            if (PlayerPrefs.GetInt("lang") == 1)
                totalText.text = "受取り済み";
            else
                totalText.text = "Done";
        }
        else if (i > 9)
        {
            totalText.text = "Get 10gem";
            NoticeReceive();
        }
        else
        {
            totalText.text = i.ToString() + "/10";
        }


        i = PlayerPrefs.GetInt("dailye");
        if (i == -1)
        {
            if (PlayerPrefs.GetInt("lang") == 1)
                eventText.text = "受取り済み";
            else
                eventText.text = "Done";
        }
        else if(i > 2)
        {
            eventText.text = "Get 1000coin";
            NoticeReceive();
        }
        else
        {
            eventText.text = i.ToString() + "/3";
        }

        i = PlayerPrefs.GetInt("dailyo");
        if (i == -1)
        {
            if (PlayerPrefs.GetInt("lang") == 1)
                onlineText.text = "受取り済み";
            else
                onlineText.text = "Done";
        }
        else if (i > 59)
        {
            onlineText.text = "Get 5gem";
            NoticeReceive();
        }
        else
        {
            onlineText.text = (i/6).ToString() + "/10min";
        }


        i = PlayerPrefs.GetInt("dailyvs");
        if (i == -1)
        {
            if (PlayerPrefs.GetInt("lang") == 1)
                vsText.text = "受取り済み";
            else
                vsText.text = "Done";
        }
        else if (i > 2)
        {
            vsText.text = "Get 1000coin";
            NoticeReceive();
        }
        else
        {
            vsText.text = i.ToString() + "/3";
        }
    }

    public void GetTotalButton()
    {
        if (PlayerPrefs.GetInt("dailyt") > 9)
        {
            //ここでアイテム追加
            PlayerPrefs.SetInt("gem", PlayerPrefs.GetInt("gem") + 10);
            PlayerPrefs.SetInt("dailyt", -1);
            CancelNotice();
            Display();
        }
    }

    public void GetVSButton()
    {
        if (PlayerPrefs.GetInt("dailyvs") > 2)
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 1000);
            PlayerPrefs.SetInt("dailyvs", -1);
            CancelNotice();
            Display();
        }
    }

    public void GetEventButton()
    {
        if (PlayerPrefs.GetInt("dailye") > 2)
        {
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 1000);
            PlayerPrefs.SetInt("dailye", -1);
            CancelNotice();
            Display();
        }
    }

    public void GetOnlineButton()
    {
        if (PlayerPrefs.GetInt("dailyo") > 59)
        {
            PlayerPrefs.SetInt("gem", PlayerPrefs.GetInt("gem") + 5);
            PlayerPrefs.SetInt("dailyo", -1);
            CancelNotice();
            Display();
        }
    }

    public void OpenDailyMission()
    {
        gameObject.SetActive(true);
        Display();
    }

    public void CloseDailyWindow()
    {
        gameObject.SetActive(false);
    }

    public static void NewDay()
    {
        PlayerPrefs.SetInt("dailyt", 0);
        PlayerPrefs.SetInt("dailyvs", 0);
        PlayerPrefs.SetInt("dailye", 0);
        PlayerPrefs.SetInt("dailyo", 0);
    }

    void NoticeReceive()
    {
        button.color = new Color(1f, 0, 0);
    }

    void CancelNotice()
    {
        button.color = new Color(0.8901961f, 0.8901961f, 0.8901961f);
    }
}
