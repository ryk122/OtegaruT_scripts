using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{
    public static Point[] points = new Point[50];
    public static int putnum = 0;

    [SerializeField]
    Point next;

    void Start()
    {
        points[putnum] = this;
        putnum++;
        if (putnum == points.Length)
        {
            putnum = 0;
        }
    }

    
    public Point GetNextPoint()
    {
        return next;
    }

    public void SetNextPoint(Point n)
    {
        next = n;
    }
}
