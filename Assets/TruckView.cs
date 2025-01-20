using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckView : MonoBehaviour
{
    [SerializeField] private Material[] companiesMaterials;
    [SerializeField] private MeshRenderer[] renderers;

    private Vector3 beckyardPosition = new Vector3(96.4199982f, 0, 140.399994f);

    public void SetBeckyardPosition()
    {
        transform.localPosition = beckyardPosition;
    }

    public void ChangeSkin(CarType type)
    {
        var material = companiesMaterials[(int)type];

        for (int i = 0; i < renderers.Length; i++)
        {
            int lastMaterialIndex = renderers[i].materials.Length - 1;

            Material[] materials = renderers[i].materials;
            materials[lastMaterialIndex] = material;

            renderers[i].materials = materials;
        }
    }
}
public enum CarType { Grocceries = 0, Bread = 1, Milk = 2 };
