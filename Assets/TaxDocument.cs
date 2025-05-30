using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaxDocument : MonoBehaviour
{
    [SerializeField] private GameObject root;

    [SerializeField] private TMP_Text companyName;
    [SerializeField] private TMP_Text dateText;
    [SerializeField] private TMP_Text dayText;

    [SerializeField] private TaxSegment[] segments;
    [SerializeField] private TaxInvoiceUnit[] units;

    [SerializeField] private Canvas canvas;

    public event Action OnFilled;
    
    public FillingTool FillingTool { set; private get; }
    public FillingToolType CurrentTool { set; get; }

    private void OnEnable()
    {
        ResetSetments();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ResetTool();

            Cursor.visible = true;
        }

        if(FillingTool != null)
        {
            Move(FillingTool.Rect);

            Cursor.visible = false;
        }
    }

    public void FillUnits(List<string> names, List<float> amounts)
    {
        for (int i = 0; i < units.Length; i++)
        {
            if(i < names.Count)
            {
                units[i].SetData(names[i], amounts[i]);
            }
            else units[i].Hide();
        }

        FillGeneralInfo();

        root.SetActive(true);
    }

    private void FillGeneralInfo()
    {
        dayText.text = $"{Core.Localization.Translate("Day")}:{Core.Statistic.GetDaysPassed()}";
        dateText.text = DateTime.Now.ToString("dd:MM:yyyy");
        companyName.text = Core.Statistic.GetCompanyName();
    }

    private void ResetSetments()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            segments[i].Init(this);
            segments[i].ResetSegment();
        }
    }

    public void CheckFilling()
    {
        for (int i = 0; i < segments.Length; i++)
        {
            if (!segments[i].IsFilled)
                return;
        }

        ResetTool();

        OnFilled?.Invoke();

        root.SetActive(false);
    }

    private void Move(RectTransform transform)
    {
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out pos
        );

        transform.anchoredPosition = pos;
    }

    private void ResetTool()
    {
        if(FillingTool != null)
        {
            FillingTool.StopUsing();

            CurrentTool = FillingToolType.None;
            FillingTool = null;
        }
    }
}
public enum FillingToolType
{
    None,
    Pen,
    Stamp
}
