using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public GameState gameState;
    public bool increaseTimeScale;
    private float currentTimeScale;

    private int lastTargetPasses;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        ChangeGameState(GameState.MENU);
    }

    private void Update()
    {
        if (gameState==GameState.PLAYING && increaseTimeScale)
        {
            currentTimeScale += 0.001f;
            Time.timeScale = currentTimeScale;
        }
    }

    public void ChangeGameState(GameState newState)
    {
        gameState = newState;
        switch(newState)
        {
            case GameState.MENU:
                {
                    UITemplate.instance.ShowMoney(Player.instance.GetPlayerMoney().ToString());
                    currentTimeScale=1f;
                    Time.timeScale = 1;
                    break;
                }
                case GameState.PLAYING:
                {
                    //if (!level)
                    //{
                    //    StartGame();
                    //}
                    Time.timeScale = 1;
                    break;
                }
                case GameState.PAUSE:
                {
                    Time.timeScale = 0;
                    break;
                }
            case GameState.END:
                {
                    break;
                }
                case GameState.RESTART:
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    break;
                }
        }    
    }

    public void RestartGame()
    {
        EndGame();
        StartGame();
    }
    public void StartGame()
    {

    }

    public void EndGame()
    {
        ChangeGameState(GameState.END);
    }
}
public enum GameState
{
    MENU,
    PLAYING,
    PAUSE,
    END,
    RESTART
}