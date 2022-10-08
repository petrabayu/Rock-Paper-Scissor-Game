using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{

    public Player player;
    public GameManager gameManager;
    public float choosingInterval;
    private float timer = 0;

    int lastSelected = 0;
    Card[] cards;

    private void Start()
    {
        cards = GetComponentsInChildren<Card>();
    }
    void Update()
    {
        if (gameManager.state != GameManager.GameState.ChooseAttack)
        {
            timer = 0;
            return;
        }
        if (timer < choosingInterval)
        {
            timer += Time.deltaTime;
            return;
        }

        timer = 0;
        ChooseAttack();
    }

    public void ChooseAttack()
    {
        var random = UnityEngine.Random.Range(1, cards.Length);
        var selection = (lastSelected + random) % cards.Length;

        player.SetChoosenCard(cards[selection]);
        lastSelected = selection;
    }
}
