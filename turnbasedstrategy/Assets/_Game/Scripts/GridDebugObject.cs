using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] TextMeshPro textMeshPro;
    private object gridObject;


    public virtual void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;

    }

    private void Update()
    {
        if(textMeshPro != null && gridObject != null)
        {
            textMeshPro.text = gridObject.ToString();
        }
          
    }
}
