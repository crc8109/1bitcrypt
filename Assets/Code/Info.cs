using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
public class Info : MonoBehaviour, ISize
{
    public SpriteColl SpriteColl { get; private set; }
    SpriteRenderer rend;
    public IDamageable Damageable { get; private set; }

    [SerializeField]
    int width = 8;
    public int Width => (int)(width / rend.sprite.pixelsPerUnit);

    [SerializeField]
    int height = 8;
    public int Height => (int)(height / rend.sprite.pixelsPerUnit);

    public Vector2 Center => new Vector2(transform.position.x, transform.position.y);

    public bool CanBeDamaged => Damageable != null;

    void Awake()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        SpriteColl = GetComponent<SpriteColl>();
        Damageable = GetComponents<Component>().FirstOrDefault(comp => comp is IDamageable) as IDamageable;
    }
}
