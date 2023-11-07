using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterController : MonoBehaviour
{
    [SerializeField]
    private RectTransform fill;

    public void ChangeFill(float value)
    {
        Vector2 anchorMax = fill.anchorMax;
        anchorMax.y = value;
        fill.anchorMax = anchorMax;
    }
}
