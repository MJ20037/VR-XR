using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;

public class CombinationLock : MonoBehaviour
{
    public UnityAction UnlockAction;
    private void OnUnlocked() => UnlockAction?.Invoke();
    public UnityAction LockAction;
    private void OnLocked()=> LockAction?.Invoke();
    [SerializeField] TMP_Text userinputText;
    [SerializeField] XRButtonInteractable[] comboButtons;
    [SerializeField] TMP_Text infoText;
    private const string startString = "Enter 3 Digit Combo";
    private const string resetString = "Enter 3 Digits To Reset Combo";
    [SerializeField] Image lockedPanel;
    [SerializeField] Color unlockedColor;
    [SerializeField] Color lockedColor;
    [SerializeField] TMP_Text lockedText;
    private const string unlockedString = "Unlocked";
    private const string lockedString = "Locked";
    [SerializeField] bool isLocked;
    [SerializeField] bool isResettable;
    private bool resetCombo;
    [SerializeField] int[] comboValues = new int[3];
    [SerializeField] int[] inputValues;
    private int maxButtonPresses;
    private int buttonPresses;

    void Start()
    {
        maxButtonPresses = comboValues.Length;
        ResetUserValues();
        for (int i = 0; i < comboButtons.Length; i++)
        {
            comboButtons[i].selectEntered.AddListener(OnComboButtonPressed);
        }
    }

    private void OnComboButtonPressed(SelectEnterEventArgs arg0)
    {
        if (buttonPresses >= maxButtonPresses)
        {
            return;
        }
        for (int i = 0; i < comboButtons.Length; i++)
        {
            if (arg0.interactableObject.transform.name == comboButtons[i].transform.name)
            {
                userinputText.text += i.ToString();
                inputValues[buttonPresses] = i;
            }
            else
            {
                comboButtons[i].ResetColor();
            }
        }
        buttonPresses++;
        if (buttonPresses == maxButtonPresses)
        {
            CheckCombo();
        }
    }

    private void CheckCombo()
    {
        if (resetCombo)
        {
            resetCombo = false;
            LockCombo();
            return;
        }
        int matches = 0;

        for (int i = 0; i < maxButtonPresses; i++)
        {
            if (inputValues[i] == comboValues[i])
            {
                matches++;
            }
        }
        if (matches == maxButtonPresses)
        {
            UnlockCombo();    
        }
        else
        {
            ResetUserValues();
        }
    }

    private void UnlockCombo()
    {
        isLocked = false;
        OnUnlocked();
        lockedPanel.color = unlockedColor;
        lockedText.text = unlockedString;
        if (isResettable)
        {
            ResetCombo();
        }
    }

    private void LockCombo()
    {
        isLocked = true;
        OnLocked();
        lockedPanel.color = lockedColor;
        lockedText.text = lockedString;
        infoText.text = startString;
        for (int i = 0; i < maxButtonPresses; i++)
        {
            comboValues[i] = inputValues[i];
        }
        ResetUserValues();
    }

    private void ResetCombo()
    {
        infoText.text = resetString;
        ResetUserValues();
        resetCombo = true;
    }

    private void ResetUserValues()
    {
        inputValues = new int[comboValues.Length];
        userinputText.text = "";
        buttonPresses = 0;
    }
}
