using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Vector3 = UnityEngine.Vector3;

public class Signal : MonoBehaviour
{
    [SerializeField]
    float speed = 5;
    public float Speed { get { return speed; } set { speed = value; } }
    [SerializeField]
    float distBeforeSpread = 8;
    float angleLerp = 0.2f;
    Vector3 dir;
    public Vector3 Dir
    {
        get { return dir; }
        set
        {
            dir = value;
            transform.up = dir;
        }
    }
    public float SignalStrength { get; set; } = 64;
    float minStrength = 1;
    float maxStrength = 128;
    Vector3 startPos;
    public Vector3 Position { get; set; }

    public IComm Sender { get; set; }
    public IComm Receiver { get; set; }
    public int MessageID { get; set; }

    public BigInteger[] Message { get; set; }

    void Awake()
    {
        startPos = transform.position;
    }
    void FixedUpdate()
    {
        Move();
    }
    void Clone(Signal parentSig, Vector3 dir)
    {
        Dir = dir;
        Sender = parentSig.Sender;
        Receiver = parentSig.Receiver;
        SignalStrength = parentSig.SignalStrength / 2;
        Position = parentSig.Position;
        Message = parentSig.Message;
    }
    void SpreadSignal()
    {
        if (SignalStrength > minStrength)
        {
            CreateSignal(Vector3.Lerp(transform.up, transform.right, angleLerp));
            CreateSignal(Vector3.Lerp(transform.up, transform.right * -1, angleLerp));
        }
        Destroy(gameObject);
    }
    void CreateSignal(Vector3 dir)
    {
        var signal = Instantiate(this);
        signal.Clone(this, dir);
    }
    void Move()
    {
        Position += Dir * speed * Time.fixedDeltaTime;
        transform.position = Util.Round(Position);
        var distTraveled = (transform.position - startPos).magnitude;
        if (distTraveled > distBeforeSpread)
        {
            SpreadSignal();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == Sender.gameObject)
            return;
        var commComp = other.GetComponents<Component>()
            .Where(comp => comp is IComm)
            .FirstOrDefault();
        if (commComp != null)
            (commComp as IComm).ReceiveMessage(this);
        Destroy(gameObject);
    }
}
