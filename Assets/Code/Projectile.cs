using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, ISize
{
    [SerializeField]
    float speed;
    [SerializeField]
    Sprite straightSprite;
    [SerializeField]
    Sprite diagonalSprite; 
    SpriteRenderer rend; 

    Vector3 position;
    Vector3 dir; 

    public void Setup(Vector3 _pos, Vector3 _dir)
    {
        position = _pos;
        dir = _dir;
        rend = GetComponent<SpriteRenderer>();
        if(dir.x == 0 || dir.y == 0)
        {
            rend.sprite = straightSprite;
            transform.up = dir;
            position += dir * Height / 2; 
        }
        else
        {
            rend.sprite = diagonalSprite;
            float rot = 0;
            if (dir.x > 0 && dir.y > 0)
                rot = 270;
            else if (dir.x < 0 && dir.y < 0)
                rot = 90;
            else if (dir.x > 0 && dir.y < 0)
                rot = 180;
            transform.Rotate(Vector3.forward, rot); 
        }
        transform.position = Util.Round(position); 
    }


    void FixedUpdate()
    {
        position += dir * speed * Time.fixedDeltaTime;
        transform.position = Util.Round(position); 
    }

    public int Width => 8;

    public int Height => 8;

    public Vector2 Center => new Vector2(transform.position.x, transform.position.y);
}
