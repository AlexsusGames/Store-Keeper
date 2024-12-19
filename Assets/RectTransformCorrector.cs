using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RectTransformCorrector : MonoBehaviour
{
    [SerializeField] private Vector3 aimPos;
    private RectTransform rectTransform;

    private void Awake() => rectTransform = GetComponent<RectTransform>();
    private void Start() => rectTransform.anchoredPosition = aimPos;
}
