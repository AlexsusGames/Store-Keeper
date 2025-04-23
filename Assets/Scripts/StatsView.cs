using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsView : MonoBehaviour
{
    [SerializeField] private TMP_Text totalShipments;
    [SerializeField] private TMP_Text perfectShipments;
    [SerializeField] private TMP_Text totalEarned;
    [SerializeField] private TMP_Text boxesSpoilded;
    [SerializeField] private TMP_Text boxSold;
    [SerializeField] private TMP_Text totalLosses;

    private string[] actions = { "Ideal shipments:", "Total shipments:", "Total losses:", "Boxes sold:", "Products spoiled:", "Total earned:" };

    private void Start()
    {
        totalShipments.text = $"{Core.Localization.Translate(actions[1])} {Core.Statistic.GetTotalSupplies()}";
        perfectShipments.text = $"{Core.Localization.Translate(actions[0])} {Core.Statistic.GetPerfectSupplies()}";
        totalEarned.text = $"{Core.Localization.Translate(actions[5])} ${Core.Statistic.GetTotalEarned()}";
        boxesSpoilded.text = $"{Core.Localization.Translate(actions[4])} {Core.Statistic.GetProductsSpoiled()}";
        boxSold.text = $"{Core.Localization.Translate(actions[3])} {Core.Statistic.GetBoxSold()}";
        totalLosses.text = $"{Core.Localization.Translate(actions[2])} ${Core.Statistic.GetTotalLosses()}";
    }
}
