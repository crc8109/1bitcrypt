using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using System.Security.Cryptography;
using Vector3 = UnityEngine.Vector3;
using System.Linq;

public class RSA : MonoBehaviour
{
    [SerializeField]
    BigInteger pubKey = 0;
    [SerializeField]
    BigInteger privKey = 0;
    [SerializeField]
    int colorDepth = 3;
    int numColors => (int)Mathf.Pow(2, colorDepth);
    [SerializeField]
    int blockWidth = 4;
    [SerializeField]
    int blockHeight = 2;
    [SerializeField]
    float _pixelSize;
    float pixelSize
    {
        get
        {
            if (_pixelSize > 0)
                return _pixelSize;
            return GameManager.PixelSize;
        }
    }
    void Awake()
    {
        if (pubKey == 0)
        {
            (pubKey, privKey) = GenerateKeys(blockWidth * blockHeight * colorDepth);
        }
    }
    void Start()
    {
        SetupVisualsForKeys();
    }
    void SetupVisualsForKeys()
    {
        var pubTrans = GenerateVisualForKey(pubKey, "Public Key");
        var privTrans = GenerateVisualForKey(privKey, "Private Key");
        pubTrans.SetParent(transform, true);
        privTrans.SetParent(transform, true);
        pubTrans.position += Vector3.up * pixelSize * (blockHeight + 2);
    }
    Transform GenerateVisualForKey(BigInteger key, string name = "key")
    {
        var go = new GameObject();
        go.name = name;
        go.transform.position = transform.position;
        var colors = EncryptManager.NumberToColor(key, numColors);
        for (int i = 0; i < colors.Count; i++)
        {
            var x = i % blockWidth;
            var y = Mathf.Floor(i / blockWidth);
            var pixel = Instantiate(GameManager.PixelPrefab);
            pixel.color = colors[i];
            pixel.transform.position = transform.position + new Vector3(x * pixelSize, y * pixelSize, 0);
            pixel.transform.localScale = new Vector3(pixelSize, pixelSize, pixelSize);
            pixel.transform.SetParent(go.transform, true);
        }
        return go.transform;
    }


    public static (BigInteger, BigInteger) GenerateKeys(int size)
    {

        var rng = new RNGCryptoServiceProvider();
        byte[] privBytes = new byte[size / 8];
        rng.GetBytes(privBytes);
        BigInteger privKey = new BigInteger(privBytes);
        byte[] pubBytes = new byte[size / 8];
        rng.GetBytes(pubBytes);
        BigInteger pubKey = new BigInteger(pubBytes);
        return (BigInteger.Abs(pubKey), BigInteger.Abs(privKey));
    }
    
}


public static class BigIntegerExtensions
{
    public static BigInteger GeneratePrime(int size = 64){
        var rng = new RNGCryptoServiceProvider();
        while(true){
            byte[] randBytes = new byte[size / 8];
            rng.GetBytes(randBytes);
            BigInteger rand = new BigInteger(randBytes);
            if(rand.IsProbablePrime(10)){
                return rand; 
            }
        }
    }
    public static bool IsProbablePrime(this BigInteger source, int certainty)
    {
        if (source == 2 || source == 3)
            return true;
        if (source < 2 || source % 2 == 0)
            return false;

        BigInteger d = source - 1;
        int s = 0;

        while (d % 2 == 0)
        {
            d /= 2;
            s += 1;
        }

        // There is no built-in method for generating random BigInteger values.
        // Instead, random BigIntegers are constructed from randomly generated
        // byte arrays of the same length as the source.
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] bytes = new byte[source.ToByteArray().LongLength];
        BigInteger a;

        for (int i = 0; i < certainty; i++)
        {
            do
            {
                // This may raise an exception in Mono 2.10.8 and earlier.
                // http://bugzilla.xamarin.com/show_bug.cgi?id=2761
                rng.GetBytes(bytes);
                a = new BigInteger(bytes);
            }
            while (a < 2 || a >= source - 2);

            BigInteger x = BigInteger.ModPow(a, d, source);
            if (x == 1 || x == source - 1)
                continue;

            for (int r = 1; r < s; r++)
            {
                x = BigInteger.ModPow(x, 2, source);
                if (x == 1)
                    return false;
                if (x == source - 1)
                    break;
            }

            if (x != source - 1)
                return false;
        }

        return true;
    }
}