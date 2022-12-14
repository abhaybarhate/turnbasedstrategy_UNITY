using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{

    

    private float totalSpinAmount;


    void Update()
    {
        if(!isActive) return;
        
        float spinAddAmount = 360f * Time.deltaTime;
        
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
        totalSpinAmount += spinAddAmount;
        if(totalSpinAmount >= 360f)
        {
            ActionComplete();
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        totalSpinAmount = 0f;
    }

    public override List<GridPosition> GetValidGridPositionsList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition>{ unitGridPosition };
    }

    public override string GetActionName()
    {
        return "Spin";
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 0,
        };
    }

}
