using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public int[] costs;



    public Texture[] textures;

    public Button[] buttons;

    public GameObject notenoug;
    private void OnEnable()
    {
        notenoug.SetActive(false);
    }
    public void Buy(int num)
    {
        if (PlayerPrefs.GetInt("Money") >= costs[num])
        {
            PlayerPrefs.SetInt("CurrentSkin", num);
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - costs[num]);
            buttons[num].interactable = false;
        }
        else
        {
            notenoug.SetActive(true);
        }
    }
}