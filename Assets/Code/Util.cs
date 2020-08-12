using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static float Round(float num)
    {
        return Mathf.Round(num); 
    }
    public static Vector3 Round(Vector3 vec)
    {
        return new Vector3(Round(vec.x), Round(vec.y), Round(vec.z)); 
    }
    public static Vector2 Round(Vector2 vec)
    {
        return new Vector2(Round(vec.x), Round(vec.y));
    }

    public static Vector3 GetExtentPos(ISize obj, PosEnum pos)
    {
        float x, y; 
        if((int)pos < 3)
        {
            y = obj.Center.y + obj.Height / 2f; 
        }else if((int)pos < 6)
        {
            y = obj.Center.y;
        }
        else
        {
            y = obj.Center.y - obj.Height / 2f;
        }
        if ((int)pos % 3 == 0)
        {
            x = obj.Center.x - obj.Width / 2f; 
        }
        else if ((int)pos % 3 == 1)
        {
            x = obj.Center.x;
        }
        else
        {
            x = obj.Center.x + obj.Width / 2f;
        }
        return Round(new Vector3(x, y, 0)); 
    }

    public static PosEnum PosFromVec(Vector2 dir) => dir switch
    {
        _ when dir.y > 0 => dir switch{
            _ when dir.x < 0 => PosEnum.UPPER_LEFT,
            _ when dir.x == 0 => PosEnum.UPPER_MIDDLE,
            _=> PosEnum.UPPER_RIGHT
        },
        _ when dir.y == 0 => dir switch
        {
            _ when dir.x < 0 => PosEnum.MIDDLE_LEFT,
            _ when dir.x == 0 => PosEnum.MIDDLE,
            _ => PosEnum.MIDDLE_RIGHT
        },
        _=> dir switch
        {
            _ when dir.x < 0 => PosEnum.LOWER_LEFT,
            _ when dir.x == 0 => PosEnum.LOWER_MIDDLE,
            _ => PosEnum.LOWER_RIGHT
        }
    };
}
