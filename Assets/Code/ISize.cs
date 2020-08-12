using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISize
{
    int Width { get; }
    int Height { get; }
    Vector2 Center { get; }
}

public enum PosEnum
{
    UPPER_LEFT = 0,
    UPPER_MIDDLE = 1,
    UPPER_RIGHT = 2,
    MIDDLE_LEFT = 3,
    MIDDLE = 4,
    MIDDLE_RIGHT = 5,
    LOWER_LEFT = 6,
    LOWER_MIDDLE = 7,
    LOWER_RIGHT = 8
}