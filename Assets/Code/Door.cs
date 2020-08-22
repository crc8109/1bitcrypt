using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Transform to;

    void Interact(Transform interacter)
    {
        interacter.position = to.position;
    }
}
