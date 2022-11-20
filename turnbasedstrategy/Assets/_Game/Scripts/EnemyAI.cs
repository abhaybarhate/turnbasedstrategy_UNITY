using System;
using UnityEngine;


public class EnemyAI : MonoBehaviour
{




    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy,
    }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanges += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            return;
        }
        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // No more enemies have actions they can take, end enemy turn
                        Debug.Log("Ending the Enemy Turn");
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        Debug.Log("Take Enemy AI Action");
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                Debug.Log("Action Taken by Enemy");
                return true;
            }
        }

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        Debug.Log("Trying to take the Action");
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach(BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            Debug.Log("Inside the foreach loop");
            if(!enemyUnit.CanSpendActionPoints(baseAction))
            {
                // Enemy cannot afford this action
                Debug.Log("cannot spend the action points");
                continue;
            }
            if(bestEnemyAIAction == null)
            {
                Debug.Log("If Statement");
                bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                bestBaseAction = baseAction;
            }
            else
            {
                Debug.Log("Else Statement");
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if(testEnemyAIAction != null && testEnemyAIAction.actionValue < bestEnemyAIAction.actionValue)
                {
                    bestEnemyAIAction = testEnemyAIAction;
                    bestBaseAction = baseAction;
                }
            }

        }

        
            //bestEnemyAIAction != null &&
        if ( enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            Debug.Log("At the end of the loop");
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            Debug.Log("Action Taken : " + enemyUnit.gameObject.name);
            return true;
        }
        else
            return false;

        //SpinAction spinAction = enemyUnit.GetSpinAction();

        //GridPosition actionGridPosition = enemyUnit.GetGridPosition();

        //if (!spinAction.IsValidActionGridPosition(actionGridPosition))
        //{
        //    return false;
        //}

        //if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
        //{
        //    return false;
        //}

        //Debug.Log("Spin Action!");
        //spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        //return true;
    }

}
