using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text text;

    public void Activate(string _text)
    {
        text.text = _text;
        gameObject.SetActive(true);
    }
    public void Deactivate()
    {
        text.text = "Interact";
        gameObject.SetActive(false);
    }
}
