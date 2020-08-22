using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
public class Info : MonoBehaviour, ISize
{
    public SpriteColl SpriteColl { get; private set; }

    public IDamageable Damageable { get; private set; }

    [SerializeField]
    int _width = 8;
    public int Width => _width;
    [SerializeField]
    int _height;

    public int Height => _width; 

    public Vector2 Center => new Vector2(transform.position.x, transform.position.y); 

    public bool CanBeDamaged => Damageable == null; 
    IComm commComp; 


    void Awake()
    {
        SpriteColl = GetComponent<SpriteColl>();
        Damageable = GetComponents<Component>().FirstOrDefault(comp => comp is IDamageable) as IDamageable;
    }
}
