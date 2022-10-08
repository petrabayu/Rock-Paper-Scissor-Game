using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public Player P1;
    public Player P2;
    public GameState state = GameState.ChooseAttack;
    public GameObject gameOverPanel;
    public TMP_Text winnerText;

    private Player damagedPlayer;
    private Player winner;
    public enum GameState
    {
        ChooseAttack,
        Attacks,
        Damages,
        Draw,
        GameOver,
    }
    private void Start()
    {
        gameOverPanel.SetActive(false);
    }
    private void Update()
    {

        switch (state)
        {

            case GameState.ChooseAttack:
                if (P1.AttackValue != null && P2.AttackValue != null)
                {
                    P1.AnimateAttack();
                    P2.AnimateAttack();
                    P1.isClickable(false);
                    P2.isClickable(false);
                    state = GameState.Attacks;
                }
                break;

            case GameState.Attacks:
                if (P1.isAnimating() == false && P2.isAnimating() == false)
                {
                    damagedPlayer = GetDamagedPlayer();
                    if (damagedPlayer != null)
                    {
                        damagedPlayer.AnimateDamage();
                        state = GameState.Damages;
                    }
                    else
                    {
                        P1.AnimateDraw();
                        P2.AnimateDraw();
                        state = GameState.Draw;
                    }
                }
                break;

            case GameState.Damages:
                if (P1.isAnimating() == false && P2.isAnimating() == false)
                {
                    if (damagedPlayer == P1)
                    {
                        P1.ChangeHealth(-15);
                        P2.ChangeHealth(7);
                    }
                    else
                    {
                        P1.ChangeHealth(7);
                        P2.ChangeHealth(-15);
                    }

                    var winner = GetWinner();

                    if (winner == null)
                    {
                        ResetPlayers();
                        P1.isClickable(true);
                        P2.isClickable(true);
                        state = GameState.ChooseAttack;
                    }
                    else
                    {

                        gameOverPanel.SetActive(true);
                        winnerText.text = winner == P1 ? "Player 1 Wins" : "Player 2 Wins";
                        ResetPlayers();
                        state = GameState.GameOver;
                    }
                }
                break;
            case GameState.Draw:
                if (P1.isAnimating() == false && P2.isAnimating() == false)
                {

                    ResetPlayers();
                    P1.isClickable(true);
                    P2.isClickable(true);
                    state = GameState.ChooseAttack;
                }
                break;
        }
    }

    private void ResetPlayers()
    {
        damagedPlayer = null;
        P1.Reset();
        P2.Reset();
    }

    private Player GetDamagedPlayer()
    {
        Attack? PlayerAtk1 = P1.AttackValue;
        Attack? PlayerAtk2 = P2.AttackValue;

        if (PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Paper)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Rock && PlayerAtk2 == Attack.Scissor)
        {
            return P2;
        }
        else if (PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Rock)
        {
            return P2;
        }
        else if (PlayerAtk1 == Attack.Paper && PlayerAtk2 == Attack.Scissor)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Rock)
        {
            return P1;
        }
        else if (PlayerAtk1 == Attack.Scissor && PlayerAtk2 == Attack.Paper)
        {
            return P2;
        }

        return null;
    }

    private Player GetWinner()
    {
        if (P1.Health == 0)
        {
            return P2;
        }
        else if (P2.Health == 0)
        {
            return P1;
        }
        else
        {
            return null;
        }
    }

    public void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
