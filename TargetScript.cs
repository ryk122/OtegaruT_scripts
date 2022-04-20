using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetScript : MonoBehaviour
{
    public static int num=0;
    public static int putnum = 0;
    public static GameObject[] target=new GameObject[50];

    // Start is called before the first frame update
    void Start()
    {
        this.name = "Target"+num.ToString();
        target[putnum] = this.gameObject;
        num++;
        putnum++;
        if (putnum == 50) putnum = 0;
    }

}
