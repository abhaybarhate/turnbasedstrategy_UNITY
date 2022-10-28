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
                    state = State.Busy;
                    if(TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                }
                break;
            case State.Busy :
                break;
        }

            
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingATurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if(!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingATurn;
            timer = 2f;
        }
    }

    private  bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Take Enemy Ai ACtion");
        foreach(Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if(TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;
        foreach(BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if(enemyUnit.CanSpendActionPoints(baseAction))
            {
                // Enemy cannot afford the action
                continue;
            }

            if(bestEnemyAIAction == null)
            {
                baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            } else {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue > bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;
                }
            }   
        }

        if(bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else{
            return false;
        }

        // SpinAction spinAction = enemyUnit.GetSpinAction();
        // GridPosition actionGridPosition = enemyUnit.GetGridPosition();
        // if(!spinAction.IsValidActionGridPosition(actionGridPosition))
        // {
        //     return false;
        // }
        // if(!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
        // {   
        //     return false;   
        // }
        // spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        // return true;
    }

}
