using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelWing : MonoBehaviour
{
    [SerializeField]
    Transform left, right;
    [SerializeField]
    ParticleSystem particle;

    float t;
    float s;

    bool isOpen, isFOVEffect;

    float fov;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
        t = 0;
        s = 0;
        isOpen = false;
        fov = Camera.main.fieldOfView;
        isFOVEffect = false;
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        right.transform.localEulerAngles = new Vector3(right.localEulerAngles.x, Mathf.Sin(t*2)*10+10, 0);
        left.transform.localEulerAngles = new Vector3(left.localEulerAngles.x, -Mathf.Sin(t*2)*10-10, 0);
    }

    public void GetWing(bool fov)
    {
        if (isOpen) return;
        isOpen = true;
        isFOVEffect = fov;
        s = -1.57f;
        InvokeRepeating("OpeningWing", 0, 0.01f);
        Invoke("ParticleStart", 0.15f);
    }

    void OpeningWing()
    {
        if (s > 1.57f)
        {
            CancelInvoke("OpeningWing");
            return;
        }
        ChangeFOV(fov + (s + 1.57f) * 5);
        float size = (Mathf.Sin(s) + 1) * 0.7f;
        transform.localScale = new Vector3(size, size, size);
        right.transform.localEulerAngles = new Vector3(-size * 30 + 41, right.localEulerAngles.y, 0);
        left.transform.localEulerAngles = new Vector3(size * 30 - 41, left.localEulerAngles.y, 0);
        s += 0.1f;
    }

    public void CloseWing(bool fov)
    {
        isOpen = false;
        isFOVEffect = fov;
        InvokeRepeating("ClosingWing", 0, 0.02f);
        t = 0;
    }
    void ClosingWing()
    {
        s -= 0.1f;
        if (s < 0)
        {
            s = 0;
            CancelInvoke("ClosingWing");
            ChangeFOV(fov);
        }
        ChangeFOV(fov + s * 5);
        float size = (Mathf.Sin(s)) * 0.7f;
        transform.localScale = new Vector3(size, size, size);
    }

    private void ChangeFOV(float s)
    {
        if (isFOVEffect)
        {
            Camera.main.fieldOfView = s;
        }
    }

    public bool IsOpen()
    {
        return isOpen;
    }

    private void ParticleStart()
    {
        particle.Play();
    }


}
