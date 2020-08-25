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


    public static (BigInteger, BigInteger) GenerateKeys(int size = 64)
    {
        var p = BigIntegerExtensions.GeneratePrime(16);
        var q = BigIntegerExtensions.GeneratePrime(16);
        var n = p * q;
        var phiOfN = (p - 1) * (q - 1);
        var pubKey = PickPubKey(phiOfN);
        var privKey = pubKey.ModInverse(phiOfN); 
        BigInteger message = 123;
        var cipher = BigInteger.ModPow(message, pubKey, n);
        var decrypted = BigInteger.ModPow(cipher, privKey, n);
        return (pubKey, privKey); 
        //var rng = new RNGCryptoServiceProvider();
        //byte[] privBytes = new byte[size / 8];
        //rng.GetBytes(privBytes);
        //BigInteger privKey = new BigInteger(privBytes);
        //byte[] pubBytes = new byte[size / 8];
        //rng.GetBytes(pubBytes);
        //BigInteger pubKey = new BigInteger(pubBytes);
        //return (BigInteger.Abs(pubKey), BigInteger.Abs(privKey));
    }
    static BigInteger PickPubKey(BigInteger phiOfN)
    {
        while (true)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var potentialKey = BigIntegerExtensions.RandomInRange(rng, 3, phiOfN - 1);
                if (BigInteger.GreatestCommonDivisor(potentialKey, phiOfN) == 1)
                {
                    return potentialKey; 
                }
            }
        }
    }
    

}


public static class BigIntegerExtensions
{
    public static BigInteger ModInverse(this BigInteger a, BigInteger m)
    {
        if (m == 1) return 0;
        BigInteger m0 = m;
        (BigInteger x, BigInteger y) = (1, 0);

        while (a > 1)
        {
            BigInteger q = a / m;
            (a, m) = (m, a % m);
            (x, y) = (y, x - q * y);
        }
        return x < 0 ? x + m0 : x;
    }
    public static BigInteger GeneratePrime(int size = 64)
    {
        var rng = new RNGCryptoServiceProvider();
        while (true)
        {
            byte[] randBytes = new byte[size / 8];
            rng.GetBytes(randBytes);
            BigInteger rand = new BigInteger(randBytes);
            if (rand.IsProbablePrime(10))
            {
                return rand;
            }
        }
    }
    public static BigInteger RandomInRange(RandomNumberGenerator rng, BigInteger min, BigInteger max)
    {
        if (min > max)
        {
            var buff = min;
            min = max;
            max = buff;
        }

        // offset to set min = 0
        BigInteger offset = -min;
        min = 0;
        max += offset;

        var value = randomInRangeFromZeroToPositive(rng, max) - offset;
        return value;
    }

    private static BigInteger randomInRangeFromZeroToPositive(RandomNumberGenerator rng, BigInteger max)
    {
        BigInteger value;
        var bytes = max.ToByteArray();

        // count how many bits of the most significant byte are 0
        // NOTE: sign bit is always 0 because `max` must always be positive
        byte zeroBitsMask = 0b00000000;

        var mostSignificantByte = bytes[bytes.Length - 1];

        // we try to set to 0 as many bits as there are in the most significant byte, starting from the left (most significant bits first)
        // NOTE: `i` starts from 7 because the sign bit is always 0
        for (var i = 7; i >= 0; i--)
        {
            // we keep iterating until we find the most significant non-0 bit
            if ((mostSignificantByte & (0b1 << i)) != 0)
            {
                var zeroBits = 7 - i;
                zeroBitsMask = (byte)(0b11111111 >> zeroBits);
                break;
            }
        }

        do
        {
            rng.GetBytes(bytes);

            // set most significant bits to 0 (because `value > max` if any of these bits is 1)
            bytes[bytes.Length - 1] &= zeroBitsMask;

            value = new BigInteger(bytes);

            // `value > max` 50% of the times, in which case the fastest way to keep the distribution uniform is to try again
        } while (value > max);

        return value;
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