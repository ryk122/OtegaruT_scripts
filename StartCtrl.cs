using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class StartCtrl : MonoBehaviour {
    int i,dcar;
    float w;
    public TextMeshProUGUI count;
    public GameObject[] car;
    public GameObject[] acar;
    [SerializeField] GameObject vscars;
    public GameObject rcam;
    public Carmain cm;
    public UnityStandardAssets.Utility.SmoothFollow smf;
    public bool vs;
    public int vscar=-1;
    public AutoCar3 atc;
    AudioSource ads;

    bool pov;
    [SerializeField]
    Transform maincmr,pos1,pos2;

    [SerializeField]
    GameObject resultWindow;
    [SerializeField]
    Text result;
    

	// Use this for initialization
	void Start () {
        dcar = PlayerPrefs.GetInt("dcar");
        if (dcar > 65)
        {
            dcar = 0;
            PlayerPrefs.SetInt("dcar", 0);
        }
        Debug.Log(dcar);
        car[dcar].SetActive(true);
        
        smf.target = car[dcar].transform;
        cm = car[dcar].GetComponent<Carmain>();
        ads = GetComponent<AudioSource>();
        float vol = PlayerPrefs.GetFloat("sev");
        ads.volume = vol * 0.3f;
        w = cm.b;
        cm.b = 0;
        i = 3;
        InvokeRepeating("S", 0.1f, 1);

        if (vs)
        {
            rcam.transform.parent = car[dcar].transform;

            GameObject acars = Instantiate(vscars);
            AutocarList al = acars.GetComponent<AutocarList>();

            int x = Random.Range(0, al.acar.Length);
            vscar = Title.vscarNum;
            if (vscar != -1)
            {
                x = vscar;
            }

            /*
            acar[x].SetActive(true);
            atc = acar[x].GetComponentInChildren<AutoCar3>();
            atc.enabled = false;
            */
            al.acar[x].SetActive(true);
            atc = al.acar[x].GetComponentInChildren<AutoCar3>();
            atc.enabled = false;
            GetComponent<PutCar>().SetAutoCarMj(atc.GetComponent<HitChecker>());
            if (PlayerPrefs.GetInt("time") == 0) { atc.cm.LightOnOff(); }
        }

        TargetScript.num = 0;
        TargetScript.putnum = 0;

        //if (PlayerPrefs.GetInt("time") == 0) { Light(); }

        Change.Reset();

        pov = false;
    }
	
    void S()
    {
        if (i == 0)
        {
            count.text = "GO";
            cm.b = w;
            ads.pitch = 2;
            ads.Play();
            if (vs)
            {
                atc.enabled = true;
            }
        }
        else if (i == -1)
        {
            count.text = "";
            CancelInvoke("S");
            //Destroy(this.gameObject);

            if (vs && PlayerPrefs.GetInt("dailyvs") != -1)
            {
                PlayerPrefs.SetInt("dailyvs", PlayerPrefs.GetInt("dailyvs") + 1);
            }

            if (PlayerPrefs.GetInt("dailyt") != -1)
            {
                PlayerPrefs.SetInt("dailyt", PlayerPrefs.GetInt("dailyt") + 1);
            }
        }
        else
        {
            ads.Play();
            count.text = i.ToString();
        }

        i--;

    }

    public void Light()
    {
        cm.LightOnOff();
    }

    public void CamChange()
    {
        /*cm.POV();
        if (maincam.activeSelf)
            maincam.SetActive(false);
        else
            maincam.SetActive(true);*/
        if (pov)
        {
            pov = false;
            smf.rotationDamping -= 500;
            smf.heightDamping -= 500;
            maincmr.position = pos1.position;
            smf.distance = 3;
            smf.height = 0.5f;
        }
        else
        {
            smf.rotationDamping += 500;
            smf.heightDamping += 500;
            smf.distance = 2;
            smf.height = 0.2f;
            pov = true;
            maincmr.position = pos2.position;
        }
    }

    public void SetResult(int c)
    {
        result.text = c.ToString() + "coin";
        resultWindow.SetActive(true);
    }
    public void CloseResult()
    {
        resultWindow.SetActive(false);
    }

}
