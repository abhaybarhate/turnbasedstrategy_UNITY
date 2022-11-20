using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] Button endTurnButton;
    [SerializeField] TextMeshProUGUI TurnNumberText;
    [SerializeField] GameObject enemyTurnTextUI;

    private void Start() {
        endTurnButton.onClick.AddListener(()=> {
            TurnSystem.Instance.NextTurn();
        });
        UpdateTurnNumberText();
        UpdateEnemyTurnVisual();
        TurnSystem.Instance.OnTurnChanges += TurnSystem_OnTurnChanges;
    }

    private void TurnSystem_OnTurnChanges(object sender, EventArgs e)
    {
        UpdateTurnNumberText();
        UpdateEnemyTurnVisual();
    }

    public void UpdateTurnNumberText()
    {
        int turnNumber = TurnSystem.Instance.GetTurnNumber();
        TurnNumberText.text = "TURN " + turnNumber;
    }

    public void UpdateEnemyTurnVisual()
    {
        enemyTurnTextUI.SetActive(!TurnSystem.Instance.IsPlayerTurn());
        
        endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

}
