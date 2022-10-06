using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    private enum State{
        WaitingForTurn,
        TakingATurn,
        Busy
    }

    private State state;

    void Awake()
    {
        state = State.WaitingForTurn;    
    }

    private float timer;
    
    void Start()
    {
        TurnSystem.Instance.OnTurnChanges += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        if(TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }

        switch(state)
        {
            case State.WaitingForTurn :
                break;
            case State.TakingATurn :
                timer -= Time.deltaTime;
                Debug.Log("Its the Enemy's Chance");
                if(timer <= 0f)
                {
                    state = State.WaitingForTurn;
                    TurnSystem.Instance.NextTurn();
                }
                break;
            case State.Busy :
                break;
        }

            
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingATurn;
            timer = 2f;
        }
    }

}
