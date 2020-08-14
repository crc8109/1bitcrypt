using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager t;
    [SerializeField]
    float pixelSize = 0.25f;
    [SerializeField] 
    SpriteRenderer pixelPrefab; 
    public static SpriteRenderer PixelPrefab => t.pixelPrefab; 
    public static float PixelSize => t.pixelSize; 

    Character player; 
    public static Character Player => t.player; 
    void Awake()
    {
        t = this;
        player = FindObjectOfType<Character>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
