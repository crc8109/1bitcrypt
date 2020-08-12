using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DebugManager : MonoBehaviour
{
    static DebugManager t; 
    [SerializeField]
    Transform debugSprite;

    void Awake()
    {
        t = this; 
    }

    public static void DrawPoint(Vector3 pos, float pixelsPerUnit = 1, float duration = 3)
    {
        var trans = Instantiate(t.debugSprite);
        trans.position = pos; 
        trans.localScale = new Vector3(1 / pixelsPerUnit, 1 / pixelsPerUnit, 1 / pixelsPerUnit);
        Callbacks.Add(() => Destroy(trans.gameObject), duration); 
    } 
    public static void DrawPoints(IEnumerable<Vector2> points, Vector3 offset, float width, float height, float duration, float pixelsPerUnit = 1f)
    {
        List<Transform> transArr = new List<Transform>();
        points.ForEach(point =>
        {
            var trans = Instantiate(t.debugSprite);
            trans.position = new Vector3(
                (point.x - width /2) / pixelsPerUnit,
                (point.y - height/2) / pixelsPerUnit, 0) 
            + offset
            + new Vector3((1/pixelsPerUnit) /2, (1/pixelsPerUnit) /2, 0);
            trans.localScale = new Vector3(1 / pixelsPerUnit, 1 / pixelsPerUnit, 1 / pixelsPerUnit);
            transArr.Add(trans); 
        });
        Callbacks.Add(() => transArr.ForEach(trans => Destroy(trans.gameObject)), duration); 
    }
}
