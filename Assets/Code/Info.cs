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
    public int Width { get; private set; }

    public int Height { get; private set; }

    public Vector2 Center => new Vector2(transform.position.x, transform.position.y); 

    bool CanBeDamaged => Damageable == null; 

    void Awake()
    {
        SpriteColl = GetComponent<SpriteColl>();
        Damageable = GetComponents<Component>().FirstOrDefault(comp => comp is IDamageable) as IDamageable;
    }
}
