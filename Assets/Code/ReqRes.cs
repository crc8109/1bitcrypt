using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReqRes : MonoBehaviour
{
    public float TimeoutAt{ get; set; }
    public float TimeElasped { get; set; }
    public Action Response { get; set; }
    public Action Timeout { get; set; } 

    void Update()
    {
        TimeElasped += Time.deltaTime; 
        if(TimeElasped > TimeoutAt)
        {
            Timeout();
            Destroy(gameObject); 
        }
    }
}
