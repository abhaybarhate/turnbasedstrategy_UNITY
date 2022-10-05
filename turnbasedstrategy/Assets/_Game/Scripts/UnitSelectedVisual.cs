using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] Unit unit;
    
    private void Start() 
    {
        //transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        SpriteRenderer[] sr = transform.GetComponentsInChildren<SpriteRenderer>();
        if(UnitActionSystem.Instance.GetSelectedUnit() == unit)
        {
            sr[0].enabled = true;
            //sr[1].enabled = true;
        }
        else
        {
            sr[0].enabled = false;
            //sr[1].enabled = false;
        }
        
    }

    private void OnDisable() 
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;   
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
    }

}
