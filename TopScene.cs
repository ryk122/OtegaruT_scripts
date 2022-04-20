using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#pragma warning disable 649

public class TopScene : MonoBehaviour
{
    [SerializeField]
    GameObject loading;
    // Start is called before the first frame update
    void Start()
    {
        //rot
        //
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        Screen.autorotateToPortrait = false; // 縦
        Screen.autorotateToLandscapeLeft = true; // 左
        Screen.autorotateToLandscapeRight = true; // 右
        Screen.autorotateToPortraitUpsideDown = false; // 上下逆
        Screen.orientation = ScreenOrientation.AutoRotation;

        //通知
        LocalPushNotification.AllClear();
        LocalPushNotification.RegisterChannel("otegaru", "otegaru", "remind_game");

        //久しぶりの通知
        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            LocalPushNotification.AddSchedule("御手軽T", "他のゲームを楽しくやれよ.. 俺にかまうな...!", 1, 1500000, "otegaru");
        }
        Notifications();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            loading.SetActive(true);
            SceneManager.LoadScene("title");
        }
    }

    void Notifications()
    {
        string content = "";

        if (Application.systemLanguage == SystemLanguage.Japanese)
        {
            // 日本語対応
            int r = Random.Range(0, 7);
            switch (r)
            {
                case 0:
                    content = "峠が俺を呼んでいる...";
                    break;
                case 1:
                    content = "ガソリン飲んでみる？";
                    break;
                case 2:
                    content = "No Touge No Life";
                    break;
                case 3:
                    content = "峠は楽しいよ!";
                    break;
                case 4:
                    content = "仕掛けるのはこの先だ!";
                    break;
                default:
                    content = "峠が俺を呼んでいる...";
                    break;
            }
        }
        else
        {
            // 英語対応
            int r = Random.Range(0, 4);
            switch (r)
            {
                case 0:
                    content = "Don't you wanna step on the gas?";
                    break;
                case 1:
                    content = "No Touge No Life";
                    break;
                case 2:
                    content = "Drift!";
                    break;
                default:
                    content = "Don't you wanna step on the gas?";
                    break;
            }
        }
            LocalPushNotification.AddSchedule("OtegaruT", content, 1, 259000, "otegaru");
    }

}

