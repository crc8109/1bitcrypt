using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Motion : MonoBehaviour
{
    static Motion t; 

    void Awake()
    {
        t = this; 
    }
    [SerializeField]
    AnimationCurve curve; 

    public static MotionCurve GetCurve(float duration, Vector3 dir, float speed)
    {
        return new MotionCurve(t.curve, duration, dir, speed);
    }
}

public class MotionCurve
{
    public Vector3 Direction { get; set; }
    public float Speed { get; set; } 
    float time = 0;
    float duration;
    public AnimationCurve Curve { get; set; }
    float percentDone => time / duration;
    public bool IsDone => percentDone > 1;
    public float Value => Curve.Evaluate(percentDone);
    public Vector3 Movement => Value * Speed * Direction; 
    public MotionCurve(AnimationCurve _curve, float _duration, Vector3 _dir, float _speed)
    {
        Curve = _curve;
        duration = _duration;
        Speed = _speed;
        Direction = _dir; 
    }
    public void PassTime(float delta)
    {
        time += delta;
    }
}