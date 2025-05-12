using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class BoxInfoView : MonoBehaviour
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private TMP_Text keepingText;
    [SerializeField] private TMP_Text spoiltText;

    [Inject] private ProductFinder productFinder;

    private string[] keepConditions = { "Store in <color=blue>a dry place", "Keep <color=blue>refrigerated", "Keep <color=blue>frozen" };
    private string[] status = { "<color=red>Spoiled", "<color=green>Fresh" };

    public void ShowInfo(StoreBox box)
    {
        gameObject.SetActive(true);

        string translatedName = Core.Localization.Translate(box.ProductName);

        nameText.text = translatedName;
        countText.text = box.UseWeight ? "??? - kg." : $"{box.GetItemsAmount()}/{box.GetCapacity()}";

        var conditionIndex = (int)productFinder.FindByName(box.ProductName).StorageType;

        string translatedKeepCondition = Core.Localization.Translate(keepConditions[conditionIndex]);

        keepingText.text = translatedKeepCondition;

        spoiltText.text = box.IsSpoilt ? status[0] : status[1];

        spoiltText.text = Core.Localization.Translate(spoiltText.text);
    }

    public void Hide() => gameObject.SetActive(false);
}
