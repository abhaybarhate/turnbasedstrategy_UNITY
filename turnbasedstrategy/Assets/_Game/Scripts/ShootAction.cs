using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAction : BaseAction
{

    private enum State
    {
        Aiming,
        Shooting,
        CoolOff
    }

    private State state;
    private int maxShootDistance = 8;
    private float stateTimer;
    private Unit targetUnit;
    private bool canShootBullet;

    public event EventHandler<OnShootEventArgs> onShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(!isActive)
        {
            return;
        }
        stateTimer -= Time.deltaTime;
        switch(state)
        {
            case State.Aiming:
                Vector3 aimDirection = targetUnit.transform.position - transform.position;
                aimDirection.Normalize();
                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, rotateSpeed * Time.deltaTime);
                break;
            case State.Shooting:
                if(canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.CoolOff:
            break;
        }

        if(stateTimer <= 0f)
        {
            NextState();
        }

    }

    private void NextState()
    {
        switch(state)
        {
            case State.Aiming:
                state = State.Shooting;
                float shootingStateTime = 0.1f;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.CoolOff;
                float coolOffStateTime = 0.5f;
                stateTimer = coolOffStateTime;
                break;
            case State.CoolOff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        onShoot?.Invoke(this, new OnShootEventArgs{
            targetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.Damage(80f);
    }

    public override string GetActionName()
    {
        return "SHOOT";
    }

    public override List<GridPosition> GetValidGridPositionsList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetValidGridPositionsList(unitGridPosition);
    }

    public List<GridPosition> GetValidGridPositionsList(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for(int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for(int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);     // The circular shooting distance
                if(testDistance > maxShootDistance)
                {
                    continue;
                }
                //if(unitGridPosition == testGridPosition) continue;
                if(!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;     // Grid Position is empty .. No unit
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if(targetUnit.IsEnemy() == unit.IsEnemy())
                {
                    // Both chads on same team
                    continue;
                }
                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        state = State.Aiming;
        float aimingStateTime = 1f;
        stateTimer = aimingStateTime;
        canShootBullet = true;

    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction {
            gridPosition = gridPosition,
            actionValue = 100,
        };
    }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidGridPositionsList(gridPosition).Count;
    }

}
