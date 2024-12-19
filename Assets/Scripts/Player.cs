using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    private int score;

    private void Awake()
    {
        instance = this;
        UITemplate.instance.ShowMoney(GetPlayerMoney().ToString());
    }
    public int GetPlayerMoney()
    {
        return PlayerPrefs.GetInt("Money");
    }    
    public int GetPlayerScore()
    {
        return score;
    }
    public void AddMoney(int moneyToAdd)
    {
        PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + moneyToAdd);
        Debug.Log(PlayerPrefs.GetInt("Money").ToString());
    }
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }
}
