using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Info))]
public class SpriteColl : MonoBehaviour
{
    [SerializeField]
    CollTypes collType;
    [SerializeField] 
    LayerMask mask; 
    Sprite sprite;
    Vector3 lastPos;
    float pixelsPerUnit;
    Rect rect;
    HashSet<Vector2> points = new HashSet<Vector2>();
    List<(SpriteColl spriteColl, Vector3 prevPos)> collidedSprites = new List<(SpriteColl spriteColl, Vector3 prevPos)>();
    bool shouldUpdatePoints = true;
    public Info Info { get; private set; }
    IDamageDealer damageDealer;
    void Awake()
    {
        Info = GetComponent<Info>();
        sprite = GetComponentInChildren<SpriteRenderer>().sprite;
        GenerateSpriteColl();
        damageDealer = GetComponents<Component>()
            .FirstOrDefault(comp => comp is IDamageDealer) as IDamageDealer;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(!mask.Contains(other.gameObject.layer))
            return;
        var otherColl = other.GetComponent<SpriteColl>();
        if (otherColl == null || otherColl.collType.HasPriorityOver(collType))
            return;
        //if (shouldUpdatePoints)
        //    GenerateSpriteColl();
        //if(IsColliding(Util.Round(transform.position), points, pixelsPerUnit))
        //{
        //    Collided(otherColl); 
        //}
        Collided(otherColl);
    }
    void Collided(SpriteColl collided)
    {
        damageDealer?.Hit(collided.Info);
    }
    public void SpriteChanged()
    {
        shouldUpdatePoints = true;
    }
    /*
     * 0.25    .5    1    2
     *   4 -> 2 ->   1  -> 1
     *   13 -> 6 -> 3 -> 2
     * 
     */

    public bool IsColliding(Vector3 pos, HashSet<Vector2> otherPoints, float otherPixelsPerUnit)
    {
        if (shouldUpdatePoints)
            GenerateSpriteColl();
        Vector2 diff = new Vector2(pos.x - lastPos.x, pos.y - lastPos.y);
        return otherPoints.Any(point =>
        {
            var modPixel = (point - diff) * otherPixelsPerUnit / pixelsPerUnit;
            return points.Contains(point);
        });
    }



    void GenerateSpriteColl()
    {
        points.Clear();
        rect = sprite.rect;
        pixelsPerUnit = sprite.pixelsPerUnit;
        var tex = sprite.texture;
        var pixelsWide = Mathf.FloorToInt(rect.width / sprite.pixelsPerUnit);
        var pixelsTall = Mathf.FloorToInt(rect.height / sprite.pixelsPerUnit);
        var xStart = Mathf.FloorToInt(rect.xMin / pixelsPerUnit);
        var yStart = Mathf.FloorToInt(rect.yMin / pixelsPerUnit);
        var pixels = tex.GetPixels((int)rect.xMin, (int)rect.yMin, (int)rect.width, (int)rect.height);
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r > 0.5f)
            {
                points.Add(new Vector2(i % (int)rect.width, Mathf.FloorToInt(i / rect.height)));
            }
        }
    }
}

public enum CollTypes{
    INERT = 0,
    PLAYER = 1,
    ENEMY = 2,
    PROJECTILE = 3
}
public static class CollHelper{
    public static bool HasPriorityOver(this CollTypes coll, CollTypes otherColl){
        return (int)coll > (int)otherColl; 
    }
}