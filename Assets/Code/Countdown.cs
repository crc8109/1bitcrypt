using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    [SerializeField]
    Transform dotPrefab;
    [SerializeField]
    int padding = 1; 
    public Action Timeout { get; set; }
    public int NumDots { get; set; }
    public float Duration { get; set; }
    List<GameObject> dots = new List<GameObject>();
    float timeSinceLastDelete = 0; 
    public void Startup()
    {
        float xOffsetStart = (Mathf.FloorToInt(NumDots / 2) * (padding + 1)) * -1 * GameManager.PixelSize;
        for(int i = 0; i < NumDots; i++)
        {
            float x = xOffsetStart + (padding + 1) * i * GameManager.PixelSize;
            var dot = Instantiate(dotPrefab);
            dot.position = new Vector3(x + transform.position.x, transform.position.y, transform.position.z);
            dots.Add(dot.gameObject);
            dot.SetParent(transform, true); 
        }
    }
    public void FixedUpdate()
    {
        if(dots.Count > 0)
        {
            timeSinceLastDelete += Time.fixedDeltaTime; 
            if(timeSinceLastDelete > Duration / NumDots)
            {
                timeSinceLastDelete -= Duration / NumDots;
                var go = dots.Last();
                dots.RemoveAt(dots.Count - 1);
                Destroy(go); 
            }
        }
        if(dots.Count == 0)
        {
            Timeout(); 
        }

    }
}
