using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEncrypt
{
    int[] Encrypt (int[] message); 
    int[] Decrypt (int[] cipher); 
}
