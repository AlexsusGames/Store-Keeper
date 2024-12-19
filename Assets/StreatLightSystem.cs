using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreatLightSystem : MonoBehaviour
{
    [SerializeField] private Material lightOffMaterial;
    [SerializeField] private Material lightOnMaterial;
    [SerializeField] private Renderer lightRenderer;
    [SerializeField] private Light lightSource;

    private bool isEnabled;

    public void ChangeEnabled()
    {
        isEnabled = !isEnabled;

        lightSource.enabled = isEnabled;

        Material material = isEnabled
            ? lightOnMaterial
            : lightOffMaterial;

        Material[] materials = lightRenderer.materials;
        if (materials.Length > 2)
        {
            materials[2] = material;
            lightRenderer.materials = materials;
        }
    }
}
