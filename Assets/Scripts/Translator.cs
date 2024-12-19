using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Translator : MonoBehaviour
{
    public static Translator instance;

    public bool isPortu;

    public Sprite portuSprite;

    public Image imaheHolder;

    public string[] portugales;

    public TMP_Text[] textBars;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    private void OnLevelWasLoaded(int level)
    {
        if (PlayerPrefs.GetInt("isPortu") == 1)
        {
            Translate();
        }
    }

    public void Translate()
    {
        for (int i = 0; i < portugales.Length; i++)
        {
            textBars[i].text = portugales[i];
        }
//        imaheHolder.sprite = portuSprite;
        isPortu = true;
        PlayerPrefs.SetInt("isPortu", 1);
    }
    public void NotTranslate()
    {
        PlayerPrefs.SetInt("isPortu", 0);
    }
}