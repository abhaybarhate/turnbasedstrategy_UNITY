using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField] Color HiddenRenderingColorInner;
    [SerializeField] Color HiddenRenderingColorOuter;
    [SerializeField] Color ShowRenderingColorInner;
    [SerializeField] Color ShowRenderingColorOuter;
    [SerializeField] SpriteRenderer gridSprite;
    [SerializeField] SpriteRenderer OuterSquareRenderer;

    void Start()
    {
        
    }

    public void Show()
    {
       gridSprite.enabled = true;
        
    }

    public void Hide()
    {
        gridSprite.enabled = false;
    }

}
