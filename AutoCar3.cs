/*
 * Copyright (c) 2022 ryk
 *
 * Released under the MIT license.
 * see https://opensource.org/licenses/MIT
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoCar3 : MonoBehaviour
{
    public GameObject nextTarget;
    public Carmain cm;
    private int targetnum;
    private float deg;

    Vector3 lastpos;

    int time=0;

    float stuckTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        float stren = PlayerPrefs.GetFloat("cpstren");
        if (stren < 0.9f) stren -= 1;
        cm.maxs *= 0.9f + (stren * 0.1f);
        targetnum = 0;
        ChangeTarget();
        lastpos = Vector3.zero;
        InvokeRepeating("StuckCheck", 5, 1);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Vector3.SqrMagnitude(nextTarget.transform.position - transform.position) > 9000)
        {
            cm.transform.LookAt(nextTarget.transform);
        }

        //calc deg
        Vector3 t = (nextTarget.transform.position - transform.position);

        Vector3 vel = (cm.rb.velocity/Vector3.Magnitude(cm.rb.velocity) +transform.forward);
        //float dot = Vector3.Dot(t, transform.forward) / ( Vector3.Magnitude(t)*Vector3.Magnitude(transform.forward) );
        float dot = Vector3.Dot(t, vel) / ( Vector3.Magnitude(t)*Vector3.Magnitude(vel) );
        float theata = Mathf.Acos(dot);

        float dot2 = Vector3.Dot(t, transform.right) / (Vector3.Magnitude(t) * Vector3.Magnitude(transform.right));

        deg = theata * 180 / Mathf.PI;
        if (dot2 < 0) deg *= -1;
        //Debug.Log(deg);
        /*
            }

            private void FixedUpdate()
            {
            */
        //drive car
        //=turn

        if (stuckTime > 40f)
        {
            cm.back = -1; cm.Back();
            stuckTime -= 1f;
            return;
        }

        if (deg > 5)
        {
            cm.TR(1);
        }
        else if (deg < -5)
        {
            cm.TR(-1);
        }
        else
        {
            cm.AndrC();
        }

       if (time>20 && Mathf.Abs(deg) > 45) { cm.back = -1; cm.Back(); time = 0; cm.Run();  }
       else { time++; cm.h = 1; if (Mathf.Abs(deg) < 45 || cm.speed < 5) { cm.Run();  } else {cm.N();} }
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == nextTarget.name)
        {
            targetnum++;
            if (targetnum == 50) targetnum = 0;
            Debug.Log(targetnum);
            ChangeTarget();
        }
    }
    */
    void ChangeTarget()
    {
        //NextTarget= GameObject.Find("Target" + targetnum.ToString()).gameObject;
        /*
        nextTarget = TargetScript.target[targetnum];
        if (nextTarget == null) { ChangeTarget(); }*/

        nextTarget = GameObject.Find("MovingTarget");
        nextTarget.GetComponent<MovingTarget>().ac = this;
        nextTarget.GetComponent<MovingTarget>().EngineStart();
        //Debug.Log("Target" + targetnum.ToString()+";"+targetnum);
    }

    void StuckCheck()
    {
        if (Vector3.SqrMagnitude(transform.position - lastpos) < 1)
        {
            stuckTime += 1.0f;
            if (stuckTime > 4)
            {
                stuckTime = 100;
            }
        }
        else
        {
            stuckTime = 0;
        }

        lastpos = transform.position;
    }
}
