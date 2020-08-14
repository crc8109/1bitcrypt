using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class EncryptManager : MonoBehaviour
{
    static EncryptManager t;
    [SerializeField]
    List<Color> encryptColors;
    public static List<Color> EncryptColors => t.encryptColors;
    public static List<Color> NumberToColor(BigInteger num, int colors = 8)
    {
        List<int> numsToCovert = new List<int>();
        while (num > 0)
        {
            numsToCovert.Add((int)(num % colors));
            num = num / colors;
        }
        return numsToCovert.Select(num => EncryptColors[num]).ToList();
    }
    void Awake()
    {
        t = this;
    }
}
