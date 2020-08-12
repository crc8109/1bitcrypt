using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComm
{
    void ReceiveMessage(int[] message, IComm sender);
    int[] SenderID { get; } 
    Transform transform { get; } 
}
