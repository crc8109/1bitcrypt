using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageUI : MonoBehaviour
{
    [SerializeField]
    Image sender;
    [SerializeField]
    Image receiver;
    [SerializeField]
    TMPro.TextMeshProUGUI message;
    [SerializeField]
    TMPro.TextMeshProUGUI key;

    public void DispalySignal(Signal signal)
    {
        sender.sprite = signal.Sender.BaseSprite;
        receiver.sprite = signal.Receiver.BaseSprite;
        message.text = EncryptManager.BigIntToString(signal.Message[0]);

    }
}
