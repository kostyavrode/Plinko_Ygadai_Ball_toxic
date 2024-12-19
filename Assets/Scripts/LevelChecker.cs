using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelChecker : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    private void OnEnable()
    {
        CheckButtons();
    }

    private void CheckButtons()
    {
        int levels = PlayerPrefs.GetInt("Levels");
        if (levels > buttons.Length)
        {
            levels = buttons.Length;
        }
        if (levels == 0)
        {
            buttons[0].interactable = true;
        }
        for (int i = 0; i < levels; i++)
        {
            buttons[i].interactable = true;
        }
    }
}
