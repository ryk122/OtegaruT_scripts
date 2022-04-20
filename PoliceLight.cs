using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceLight : MonoBehaviour
{
    [SerializeField]
    GameObject flight;
    [SerializeField]
    GameObject r, l;
    

    bool s;

    // Start is called before the first frame update
    void Start()
    {
        s = true;
        InvokeRepeating("Light", 0, 0.25f);
    }

    void Light()
    {
        if (!flight.activeSelf)
        {
            r.SetActive(false);
            l.SetActive(false);
            return;
        }
        r.SetActive(s);
        l.SetActive(!s);
        s = !s;
    }
}
