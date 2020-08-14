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
        Debug.Log(privBytes.Length);
        privBytes.ToList().ForEach(z => Debug.Log(z));
        BigInteger privKey = new BigInteger(privBytes);
        byte[] pubBytes = new byte[size / 8];
        rng.GetBytes(pubBytes);
        BigInteger pubKey = new BigInteger(pubBytes);
        return (BigInteger.Abs(pubKey), BigInteger.Abs(privKey));
    }
}
