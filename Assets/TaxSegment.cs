using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TaxSegment : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private FillingToolType requiredType;

    private CanvasGroup canvasGroup;
    private TaxDocument document;

    private bool isFilled;

    private event Action OnFill;
    public bool IsFilled {  get => isFilled || !gameObject.activeInHierarchy; set => isFilled = value; }

    public void Init(TaxDocument document)
    {
        if(this.document == null)
        {
            this.document = document;

            OnFill += document.CheckFilling;

            canvasGroup = GetComponent<CanvasGroup>();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(document.CurrentTool == requiredType && !isFilled)
        {
            IsFilled = true;
            canvasGroup.alpha = 1;

            Invoke(nameof(CallOnFill), 1);

            AudioType type = requiredType == FillingToolType.Pen ? AudioType.Note : AudioType.BoxFold;
            Core.Sound.PlayClip(type);
        }
    }

    private void CallOnFill() => OnFill?.Invoke();

    public void ResetSegment()
    {
        IsFilled = false;
        canvasGroup.alpha = 0.2f;
    }
}
