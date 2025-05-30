using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillingTool : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TaxDocument document;
    [SerializeField] private FillingToolType type;

    public RectTransform Rect { get; private set; }

    private void Awake()
    {
        Rect = GetComponent<RectTransform>();
    }

    public void Use()
    {
        image.raycastTarget = false;

        document.CurrentTool = type;
        document.FillingTool = this;
    }

    public void StopUsing() => image.raycastTarget = true;
}
