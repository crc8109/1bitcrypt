using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Callbacks.Add(() => Debug.Log("Ran!"), 3);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
