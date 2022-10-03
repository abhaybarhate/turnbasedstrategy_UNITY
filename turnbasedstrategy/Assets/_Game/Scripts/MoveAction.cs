using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{


    [SerializeField] private Animator UnitAnimator;  
    [SerializeField] int maxMoveDistance = 4;
    
    private Vector3 TargetPosition;
    private GridPosition gridPosition;
    
    public event EventHandler onStartMoving;
    public event EventHandler onStopMoving;

    protected override void Awake() {
        base.Awake();
        TargetPosition = transform.position;
    }

    private void Start() {
        
    }

    void Update()
    {
        if(!isActive) return;
        float stoppingDistance = .1f;
        Vector3 moveDirection = (TargetPosition - transform.position).normalized;
        if (Vector3.Distance(transform.position, TargetPosition) > stoppingDistance)
        {
            
            float moveSpeed = 4f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;

            

        }

        else
        {
            onStopMoving?.Invoke(this, EventArgs.Empty);
            ActionComplete();
        }

        float rotateSpeed = 10f;
        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

    }

    public override void TakeAction(GridPosition gridPosition,Action onActionComplete)
    {
        ActionStart(onActionComplete);
        this.TargetPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
        onStartMoving?.Invoke(this, EventArgs.Empty);
        Debug.Log("I m moving");
    }

    // public override bool IsValidActionGridPosition(GridPosition gridPosition)
    // {
    //     List<GridPosition> validGridPositionsList = GetValidGridPositionsList();
    //     return validGridPositionsList.Contains(gridPosition);
    // }

    public override List<GridPosition> GetValidGridPositionsList()
    {
        List<GridPosition> validGridPositionList = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for(int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x,z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                if(!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                if(unitGridPosition == testGridPosition) continue;
                if(LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) continue;
                validGridPositionList.Add(testGridPosition);
            }
        }
        
        return validGridPositionList;
    }

    public override string GetActionName()
    {
        return "Move";
    }

}
