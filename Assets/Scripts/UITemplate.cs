using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UITemplate : MonoBehaviour
{
    public static UITemplate instance;

    [SerializeField] private GameObject inGamePanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private GameObject winPanel;
    
    [SerializeField] private TMP_Text moneyBar;
    [SerializeField] private TMP_Text passesBar;
    [SerializeField] private TMP_Text winningsBar;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ShowMoney(Player.instance.GetPlayerMoney().ToString());
    }

    public void ShowMoney(string data)
    {
        moneyBar.text = data;
    }
    public void StartGame()
    {
        GameManager.instance.ChangeGameState(GameState.PLAYING);
    }
    public void PauseGame()
    {
        GameManager.instance.ChangeGameState(GameState.PAUSE);
    }
    public void UnPauseGame()
    {
        GameManager.instance.ChangeGameState(GameState.PLAYING);
    }
    public void EndGame(bool isWin)
    {
        GameManager.instance.ChangeGameState(GameState.END);
        ShowMoney(Player.instance.GetPlayerMoney().ToString());
        inGamePanel.SetActive(false);
        if (isWin)
        {
            winPanel.SetActive(true);
            PlayerPrefs.SetInt("Levels",PlayerPrefs.GetInt("Levels")+1);
            Player.instance.AddMoney(10);
            winningsBar.text="+10";
            ShowMoney(Player.instance.GetPlayerMoney().ToString());
        }
        else
        {
            losePanel.SetActive(true);
        }
    }

    public void ShowPasses(string target, string success)
    {
        passesBar.text = success + " | " + target;
    }
    public void RestartGame()
    {
        GameManager.instance.ChangeGameState(GameState.RESTART);
    }
    public void ExitApp()
    {
        Application.Quit();
    }
}
