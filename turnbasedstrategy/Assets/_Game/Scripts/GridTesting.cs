using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTesting : MonoBehaviour
{

    [SerializeField] Unit unit;

    private void Start() 
    {
        
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.T))
        {
            GridSystemVisual.Instance.HideAllGridPositions();
            GridSystemVisual.Instance.ShowGridPositionList(unit.GetMoveAction().GetValidGridPositionsList());
        }
    }

}
