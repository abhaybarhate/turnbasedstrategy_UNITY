using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator UnitAnimator;  
    [SerializeField] private bool isEnemy;
    private const int ACTION_POINTS_MAX = 2;
    
    public static event EventHandler OnAnyActionPointsChanged;
    
    private Vector3 TargetPosition;
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private BaseAction[] baseActionArray;
    private HealthSystem healthSystem;
    private int unitActionPoints = ACTION_POINTS_MAX;

    private void Awake() 
    {

        healthSystem = GetComponent<HealthSystem>();   
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if(LevelGrid.Instance) {
            gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        }

        TurnSystem.Instance.OnTurnChanges += TurnSystem_OnTurnChanges;
        healthSystem.OnDie += HealthSystem_OnDie;
    }

    // Update is called once per frame
    void Update()
    {

        

        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            // Unit changed Grid Position
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }

    }

    private void SpendActionPoints(int amount)
    {
        unitActionPoints -= amount;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public MoveAction GetMoveAction()
    {
        return moveAction;
    }

    public SpinAction GetSpinAction()
    {
        return spinAction;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public BaseAction[] GetBaseActionArray()
    {
        return baseActionArray;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if(CanSpendActionPoints(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointsCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPoints(BaseAction baseAction)
    {
        if(unitActionPoints >= baseAction.GetActionPointsCost())
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    public int GetActionPoints()
    {
        return unitActionPoints;
    }

    public bool IsEnemy()
    {
        return isEnemy;
    }

    private void TurnSystem_OnTurnChanges(object sender, EventArgs e)
    {
        if((IsEnemy() && !TurnSystem.Instance.IsPlayerTurn()) || (!IsEnemy() && TurnSystem.Instance.IsPlayerTurn()))
        {
            unitActionPoints = ACTION_POINTS_MAX;
        }    
    }

    public Vector3 GetWorldPosition()
    {
        return transform.position;
    }
    
    public void Damage(float damageAmount)
    {
        Debug.Log("Bro I'm taking Damage... Here ::" + transform.position);
        healthSystem.TakeDamage(damageAmount);
    }

    private void HealthSystem_OnDie(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }

}
