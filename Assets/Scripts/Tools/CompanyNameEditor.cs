using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompanyNameEditor : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text companyNameText;

    [SerializeField] private Player player;
    [SerializeField] private UIHandler handler;
    private void OnDisable()
    {
        Hide();
    }

    private void Start()
    {
        companyNameText.text = Core.Statistic.GetCompanyName();

        inputField.onSelect.AddListener(OnSelect);
        inputField.onDeselect.AddListener(OnDeselect);
    }
    private void OnSelect(string _)
    {
        player.BlockInteractivity(true, this);
        handler.BlockEnabled(true);
    }
    private void OnDeselect(string _)
    {
        player.BlockInteractivity(false, this);
        handler.BlockEnabled(false);
    }

    private void Hide()
    {
        inputField.gameObject.SetActive(false);
        OnDeselect(null);
    }
    public void OpenInputField() => inputField.gameObject.SetActive(true);
    public void TryConfirm()
    {
        string input = inputField.text;
        string result = input.Replace(" ", "");


        result = "LLC" + " " + result;

        if (result.Length <= 24 && result.Length >= 8)
        {
            Core.Statistic.SetCompanyName(result);
            companyNameText.text = result;
            Hide();
        }
        else
        {
            Core.Clues.Show("Enter between 4 and 20 characters!");
        }
    }
}
