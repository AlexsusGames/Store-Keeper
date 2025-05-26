using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RatingBlock : MonoBehaviour
{
    [SerializeField] private TMP_Text ratingText;

    public void Block(int rating, bool value)
    {
        gameObject.SetActive(value);

        float ratingFloat = rating / 1000;
        int rounded = (int)ratingFloat;

        ratingText.text = rounded.ToString();
    }

    public void Hide() => gameObject.SetActive(false);
}
