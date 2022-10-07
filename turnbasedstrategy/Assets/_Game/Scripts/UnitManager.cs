using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{

    private List<Unit> unitList;
    private List<Unit> friendlyUnitList;
    private List<Unit> enemyUnitList;


    public static UnitManager Instance {get; private set;}

    
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one UnitManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
        unitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
        enemyUnitList = new List<Unit>();    
    }

    void Start()
    {
        
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        
        unitList.Add(unit); 

        if(unit.IsEnemy())
        {
            enemyUnitList.Add(unit);
        }
        else
        {
            friendlyUnitList.Add(unit);
        }

        Debug.Log("All Unit Count" + unitList.Count);
        Debug.Log("Friendly Unit Count" + friendlyUnitList.Count);
        Debug.Log("enemy Unit Count" + enemyUnitList.Count);
        
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;
        
        unitList.Remove(unit); 

        if(unit.IsEnemy())
        {
            enemyUnitList.Remove(unit);
        }
        else
        {
            friendlyUnitList.Remove(unit);
        }

        Debug.Log("All Unit Count" + unitList.Count);
        Debug.Log("Friendly Unit Count" + friendlyUnitList.Count);
        Debug.Log("enemy Unit Count" + enemyUnitList.Count);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return enemyUnitList;
    }

}
