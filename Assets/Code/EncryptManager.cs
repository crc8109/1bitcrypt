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
        return numsToCovert.Select(numz => EncryptColors[numz]).ToList();
    }
    void Awake()
    {
        t = this;
    }
    public static string BigIntToString(BigInteger num)
    {
        string message = "";
        while (num > 0)
        {
            BigInteger bigKey;
            num = BigInteger.DivRem(num, NUMCHAR + 1, out bigKey);
            int key = (int)bigKey;
            message += intToChar[key];
        }
        return message;
    }
    public static BigInteger StringToBigInt(string message)
    {
        BigInteger result = 0;
        for (int i = message.Length - 1; i >= 0; i--)
        {
            var c = message[i];
            var num = charToInt[c];
            result += num;
            if (i != 0)
                result *= NUMCHAR + 1;
        }
        return result;
    }
    static int NUMCHAR = 71;
    static Dictionary<int, char> intToChar = new Dictionary<int, char>(){
        {0, 'a'},
        {1, 'b'},
        {2, 'c'},
        {3, 'd'},
        {4, 'e'},
        {5, 'f'},
        {6, 'g'},
        {7, 'h'},
        {8, 'i'},
        {9, 'j'},
        {10, 'k'},
        {11, 'l'},
        {12, 'm'},
        {13, 'n'},
        {14, 'o'},
        {15, 'p'},
        {16, 'q'},
        {17, 'r'},
        {18, 's'},
        {19, 't'},
        {20, 'u'},
        {21, 'v'},
        {22, 'w'},
        {23, 'x'},
        {24, 'y'},
        {25, 'z'},
        {26, 'A'},
        {27, 'B'},
        {28, 'C'},
        {29, 'D'},
        {30, 'E'},
        {31, 'F'},
        {32, 'G'},
        {33, 'H'},
        {34, 'I'},
        {35, 'J'},
        {36, 'K'},
        {37, 'L'},
        {38, 'M'},
        {39, 'N'},
        {40, 'O'},
        {41, 'P'},
        {42, 'Q'},
        {43, 'R'},
        {44, 'S'},
        {45, 'T'},
        {46, 'U'},
        {47, 'V'},
        {48, 'W'},
        {49, 'X'},
        {50, 'Y'},
        {51, 'Z'},
        {52, '.'},
        {53, ','},
        {54, '!'},
        {55, '?'},
        {56, '<'},
        {57, '>'},
        {58, '('},
        {59, ')'},
        {60, ':'},
        {61, '0'},
        {62, '1'},
        {63, '2'},
        {64, '3'},
        {65, '4'},
        {66, '5'},
        {67, '6'},
        {68, '7'},
        {69, '8'},
        {70, '9'},
        {71, ' '},
    };

    static Dictionary<char, int> charToInt = new Dictionary<char, int>(){
        {'a',0},
        {'b',1},
        {'c',2},
        {'d',3},
        {'e',4},
        {'f',5},
        {'g',6},
        {'h',7},
        {'i',8},
        {'j',9},
        {'k',10},
        {'l',11},
        {'m',12},
        {'n',13},
        {'o',14},
        {'p',15},
        {'q',16},
        {'r',17},
        {'s',18},
        {'t',19},
        {'u',20},
        {'v',21},
        {'w',22},
        {'x',23},
        {'y',24},
        {'z',25},
        {'A',26},
        {'B',27},
        {'C',28},
        {'D',29},
        {'E',30},
        {'F',31},
        {'G',32},
        {'H',33},
        {'I',34},
        {'J',35},
        {'K',36},
        {'L',37},
        {'M',38},
        {'N',39},
        {'O',40},
        {'P',41},
        {'Q',42},
        {'R',43},
        {'S',44},
        {'T',45},
        {'U',46},
        {'V',47},
        {'W',48},
        {'X',49},
        {'Y',50},
        {'Z',51},
        {'.', 52},
        {',',53},
        {'!',54},
        {'?',55},
        {'<',56},
        {'>',57},
        {'(',58},
        {')',59},
        {':',60},
        {'0',61},
        {'1',62},
        {'2',63},
        {'3',64},
        {'4',65},
        {'5',66},
        {'6',67},
        {'7',68},
        {'8',69},
        {'9',70},
        {' ', 71},
    };
}
