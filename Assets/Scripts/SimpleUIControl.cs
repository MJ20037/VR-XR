using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
using System;

public class SimpleUIControl : MonoBehaviour
{
    [SerializeField] XRButtonInteractable startButton;
    [SerializeField] GameObject keyIndicatorLight;
    [SerializeField] string[] msgStrings;
    [SerializeField] TMP_Text[] msgText;

    void Start()
    {
        if (startButton != null)
        {
            startButton.selectEntered.AddListener(StartButtonPressed);
        }
    }

    private void StartButtonPressed(SelectEnterEventArgs arg0)
    {
        SetText(msgStrings[1]);
        if (keyIndicatorLight != null)
        {
            keyIndicatorLight.SetActive(true);   
        }
    }

    public void SetText(string msg)
    {
        for (int i = 0; i < msgText.Length; i++)
        {
            msgText[i].text = msg;
        }
    } 
}
