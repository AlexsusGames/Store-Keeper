using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour
{
    [SerializeField] private string roomKey;
    [SerializeField] private int MatIndex;

    [SerializeField] private FadeScreen fadeScreen;

    [SerializeField] private MeshRenderer mRenderer;

    [SerializeField] private Material[] variants;

    private void Awake()
    {
        int curentIndex = PlayerPrefs.GetInt(roomKey);
        
        ChangeMaterial(curentIndex, false);
    }

    public void ChangeMaterial(int index, bool showScreen = true)
    {
        Action action = () =>
        {
            var material = variants[index];

            var materials = mRenderer.sharedMaterials;

            materials[MatIndex] = material;

            mRenderer.materials = materials;

            PlayerPrefs.SetInt(roomKey, index);
        };

        if(showScreen)
        {
            fadeScreen.TryShowLoadingScreen(action);
        }
        else action?.Invoke();
    }
}
