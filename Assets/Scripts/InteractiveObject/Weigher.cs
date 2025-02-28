using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Weigher : MonoBehaviour
{
    [SerializeField] private TMP_Text textLine;
    [SerializeField] private Transform parent;

    private Dictionary<BoxType, float> boxWeights = new Dictionary<BoxType, float>()
    {
        { BoxType.YellowBox, 1.5f },
        { BoxType.BlackBox, 1f },
        { BoxType.CartonBox, 0.3f },
        { BoxType.CartonPlane, 0.3f },
        { BoxType.CartonOpened, 0.3f },
        { BoxType.Other, 0.5f }
    };

    private int calculatedCount;
    private float cachedWeight;

    private void FixedUpdate()
    {
        CalculateWeight();
    }

    private void CalculateWeight()
    {
        int currentCount = CheckObjectCount(parent);

        if (currentCount != calculatedCount)
        {
            float totalWeight = 0f;

            for (int i = 0; i < parent.childCount; i++)
            {
                if (parent.GetChild(i).TryGetComponent(out StoreBox box))
                {
                    totalWeight += CheckWeight(box);
                }
            }

            totalWeight = MathF.Round(totalWeight, 2);
            
            StartCoroutine(WeightValueAnimation(cachedWeight, totalWeight));

            calculatedCount = currentCount;
            cachedWeight = totalWeight;
        }
    }

    private IEnumerator WeightValueAnimation(float startValue, float targetValue)
    {
        float elapsedTime = 0f;
        float time = 1f;
        float currentValue = startValue;

        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            currentValue = Mathf.Lerp(currentValue, targetValue, elapsedTime / time);

            textLine.text = $"{currentValue:F2} kg.";
            yield return null; 
        }

        textLine.text = $"{targetValue:F2} kg.";
    }

    private int CheckObjectCount(Transform parent)
    {
        int result = 0;

        for (int i = 0; i < parent.childCount; i ++)
        {
            if(parent.GetChild(i).TryGetComponent(out StoreBox box))
            {
                result++;

                if (box.IsHasChild)
                {
                    result += CheckObjectCount(box.ChildPoint);
                }
            }
        }

        return result;
    }

    private float CheckWeight(StoreBox box)
    {
        float result = box.TotalWeight;
        result += boxWeights[box.BoxType];

        if (box.IsHasChild)
        {
            for (int i = 0; i < box.ChildPoint.childCount; i++)
            {
                if (box.ChildPoint.GetChild(i).TryGetComponent(out StoreBox childBox))
                    result += CheckWeight(childBox);
            }
        }

        return result;
    }

    private void OnDisable() => textLine.text = "";
    private void OnEnable()
    {
        calculatedCount = 0;
        textLine.text = $"{0} kg.";
    }
}
