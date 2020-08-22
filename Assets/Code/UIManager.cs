using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static UIManager t;

    [SerializeField]
    MessageUI messageUIPrefab;
    Canvas canvas;

    Transform currentMessage;
    float defaultDuration = 5f;
    float timeRemaining = 0;

    public static void DisplaySignal(Signal signal, float duration = 0)
    {
        t.ClearOldMessage();
        t.timeRemaining = duration == 0 ? t.defaultDuration : duration;
        var message = Instantiate(t.messageUIPrefab);
        message.transform.SetParent(t.canvas.transform, false);
        message.DispalySignal(signal);
        t.currentMessage = message.transform;
    }

    void ClearOldMessage()
    {
        if (currentMessage != null)
        {
            Destroy(currentMessage.gameObject);
        }
        currentMessage = null;
        timeRemaining = 0;
    }

    void Awake()
    {
        t = this;
        canvas = FindObjectOfType<Canvas>();
    }
    void Update()
    {
        if (currentMessage != null)
            ToastCountdown();
    }
    void ToastCountdown()
    {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining < 0)
            ClearOldMessage();
    }
}
