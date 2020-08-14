using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Threading;

[RequireComponent(typeof(Info))]
public class Enemy : MonoBehaviour, IDamageable, IComm
{
    [SerializeField]
    float speed;
    [SerializeField]
    float health;
    [ShowInInspector]
    float currentHealth;
    [SerializeField]
    float detectionRadius;
    [SerializeField]
    float randomDirDuration = 5;
    Vector3 currentDir;
    Vector3 position;
    [SerializeField]
    Countdown countdownPrefab;
    Countdown countdown;
    [SerializeField]
    Signal signalPrefab;
    EnemyStates currState = EnemyStates.WANDERING;
    public int[] SenderID { get; } = new int[] { 2, 3, 1, 4 };
    public Info Info { get; private set; }

    private void Awake()
    {
        currentHealth = health;
        position = transform.position;
        Info = GetComponent<Info>();
    }
    void Start()
    {
        SetRandomDir(); //SetContinuosly
        CheckPlayerDist();  //SetContinuosly
    }
    void FixedUpdate()
    {
        if (currState == EnemyStates.WANDERING)
        {
            //Wander();
        }
    }
    void CheckPlayerDist()
    {
        if (currState == EnemyStates.WANDERING)
        {
            Callbacks.AddFixed(CheckPlayerDist, 2f, gameObject);
            if (IsCloseToPlayer())
                SendFriendSignal();
        }
    }
    void SendFriendSignal()
    {
        currState = EnemyStates.WAITING;
        CreateCountdown();
        CreateSignal();
    }
    void CreateCountdown()
    {
        countdown = Instantiate(countdownPrefab);
        countdown.transform.position = Util.GetExtentPos(Info, PosEnum.UPPER_MIDDLE) + Vector3.up * GameManager.PixelSize * 2;
        countdown.transform.SetParent(transform, true);
        countdown.NumDots = 4;
        countdown.Duration = 5;
        countdown.Timeout = Timeout;
        countdown.Startup();
    }
    void CreateSignal()
    {
        var signal = Instantiate(signalPrefab);
        signal.Position = transform.position;
        signal.Dir = (GameManager.Player.transform.position - transform.position).normalized;
        signal.Sender = this;
        signal.Receiver = GameManager.Player;
    }
    void Timeout()
    {
        currState = EnemyStates.ATTACKING;
        Destroy(countdown.gameObject);
        countdown = null;
        Debug.Log("Grrrr, attack!");
    }
    bool IsCloseToPlayer()
    {
        return (GameManager.Player.transform.position - transform.position).magnitude < detectionRadius;
    }
    void Wander()
    {
        position += currentDir * speed * Time.fixedDeltaTime;
        transform.position = Util.Round(position);
    }
    void SetRandomDir()
    {
        if (currState != EnemyStates.WANDERING)
            return;
        currentDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        Callbacks.AddFixed(SetRandomDir, randomDirDuration, gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void ReceiveMessage(int[] message, IComm sender)
    {

    }
}

enum EnemyStates
{
    WANDERING,
    WAITING,
    ATTACKING
}