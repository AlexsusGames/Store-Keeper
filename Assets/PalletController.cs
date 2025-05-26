using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PalletController : MonoBehaviour
{
    [SerializeField] private Transform[] paletaPoints;
    [SerializeField] private DeliveredProducts palletPrefab;

    private List<DeliveredProducts> pallets;

    private Coroutine swapCoroutine;

    public bool HasProducts()
    {
        for (int i = 0; i < pallets.Count; i++)
        {
            if (pallets[i].HasProducts())
                return true;
        }

        return false;
    }

    public void ClearPallets()
    {
        if(pallets != null)
        {
            for (int i = 0; i < pallets.Count; i++)
            {
                Destroy(pallets[i].gameObject);
            }

            pallets.Clear();
        }
    }
    public Transform GetPalletParent(int index) => paletaPoints[index];
    public void AddPallet(DeliveredProducts products)
    {
        if(pallets == null)
        {
            pallets = new List<DeliveredProducts>();
        }

        pallets.Add(products);
    }

    public void CreateEmptyPallets()
    {
        pallets = new List<DeliveredProducts>();

        for (int i = 0; i < paletaPoints.Length; i++)
        {
            var pallet = Instantiate(palletPrefab.gameObject, paletaPoints[i]);
            pallets.Add(pallet.GetComponent<DeliveredProducts>());
        }
    }

    public DeliveryReport GetDeliveryReport(Dictionary<string, float> expectedProducts)
    {
        var pricingInteractor = Core.Interactors.GetInteractor<PricingInteractor>();

        Dictionary<string, float> report = new();

        bool isHasChanged = false;
        bool isHasSpoiled = false;

        for (int i = 0; i < pallets.Count; i++)
        {
            pallets[i].Init(false);

            if (pallets[i].ConsistSpoilled)
                isHasSpoiled = true;

            var order = pallets[i].GetOrder();

            foreach (var unit in order.Keys)
            {
                if (!expectedProducts.ContainsKey(unit))
                {
                    Core.Clues.Show("Before sending the delivery, unload any extra goods");

                    return new DeliveryReport(isSuccess: false);
                }
                else report[unit] = report.GetValueOrDefault(unit, 0) + order[unit];

                if (!isHasChanged)
                    isHasChanged = pricingInteractor.WasChanged(unit);
            }
        }

        if (report.Count == 0 && !isHasSpoiled)
        {
            Core.Clues.Show("You cannot send out an empty truck.");

            return new DeliveryReport(isSuccess: false);
        }

        foreach (var item in report.Keys)
        {
            Core.ProductList.RemoveProduct(item, report[item]);
        }

        foreach (var unit in expectedProducts.Keys)
        {
            report[unit] = report.GetValueOrDefault(unit, 0) - expectedProducts[unit];
        }

        return new DeliveryReport(report, isHasSpoiled, isHasChanged);
    }

    public bool Swapped()
    {
        if (swapCoroutine == null)
        {
            swapCoroutine = StartCoroutine(Timer());
            return true;
        }

        return false;
    }

    private IEnumerator Timer()
    {
        List<Vector3> localPositions = new List<Vector3>();
        foreach (var obj in paletaPoints)
        {
            localPositions.Add(obj.transform.localPosition);
        }

        for (int i = 0; i < paletaPoints.Length; i++)
        {
            int swapIndex = (i + 1) % paletaPoints.Length;

            paletaPoints[i].transform.localPosition = localPositions[swapIndex];
        }

        for (int i = 0; i < paletaPoints.Length; i++)
        {
            yield return StartCoroutine(PalletAnim(paletaPoints[i]));
        }

        swapCoroutine = null;
    }

    private IEnumerator PalletAnim(Transform palletTransform)
    {
        Vector3 startPosition = palletTransform.localPosition;
        Vector3 targetPosition = startPosition + new Vector3(0f, 0.5f, 0f);

        float timeElapsed = 0f;
        float duration = 0.2f;

        while (timeElapsed < duration)
        {
            palletTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        palletTransform.localPosition = targetPosition;

        timeElapsed = 0f;
        duration = 0.1f;

        while (timeElapsed < duration)
        {
            palletTransform.localPosition = Vector3.Lerp(targetPosition, startPosition, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        palletTransform.localPosition = startPosition;
    }

    public Dictionary<string, float> GetLoadedProducts()
    {
        Dictionary<string, float> map = new Dictionary<string, float>();

        for (int i = 0; i < pallets.Count; i++)
        {
            pallets[i].Init(false);

            var order = pallets[i].GetOrder();

            foreach(var key in order.Keys)
            {
                map[key] = map.GetValueOrDefault(key, 0) + order[key];
            }
        }

        return map;
    }
}
