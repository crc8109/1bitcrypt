using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComm
{
    void ReceiveMessage(Signal signal);
    int[] SenderID { get; } 
    Sprite BaseSprite { get; }  
    Transform transform { get; } 
    GameObject gameObject {get;}
}
