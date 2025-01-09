using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupObject : InteractiveManager
{
    [SerializeField] private MeshRenderer[] renderers;

    [SerializeField] private Material enablePreviewMat;
    [SerializeField] private Material disablePreviewMat;

    [SerializeField] private UnityEvent OnGrabbed;
    [SerializeField] private UnityEvent OnPlaced;

    [SerializeField] private int countOfCollision;

    private List<Material> cachedMaterials = new();
    private BoxCollider bCollider;

    private void Awake()
    {
        bCollider = GetComponent<BoxCollider>(); 

        for (int i = 0; i < renderers.Length; i++)
        {
            for(int j = 0; j < renderers[i].materials.Length; j++)
            {
                cachedMaterials.Add(renderers[i].materials[j]);
            }
        }
    }

    public void ChangeLayer(int layer)
    {
        for (int i = 0;i < renderers.Length; i++)
        {
            renderers[i].gameObject.layer = layer;
        }
    }

    public void Grab()
    {
        OnGrabbed?.Invoke();
    }
    public void Place()
    {
        OnPlaced?.Invoke();
        SetDefaut();
    }
    public void SetDefaut()
    {
        OutlineEnabled(false);

        int matIndex = 0;

        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;

            for (int j = 0; j < 1; j++)
            {
                materials[j] = cachedMaterials[matIndex];
                matIndex++;
            }

            renderers[i].materials = materials;
        }

        OutlineEnabled(true);
    }

    private void Draw(bool value)
    {
        OutlineEnabled(false);

        Material mat = value ? enablePreviewMat : disablePreviewMat;

        for (int i = 0;i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].materials;

            for (int j = 0; j < 1; j++)
            {
                materials[j] = mat;
            }

            renderers[i].materials = materials;
        }

        OutlineEnabled(true);
    }

    public bool CheckAvailableToPlace()
    {
        Vector3 position = bCollider.bounds.center;
        Vector3 halfExtents = bCollider.bounds.size / 2f;

        Collider[] results = Physics.OverlapBox(position, halfExtents, Quaternion.identity);
        print(results);

        var result = results.Length == countOfCollision;

        Draw(result);

        return result;
    }

    private void print(Collider[] colls)
    {
        string result = "";

        for (int i = 0; i < colls.Length; i++)
        {
            result += $"{colls[i].name} ";
        }

        Debug.Log(result);
    }

    public override void Interact()
    {
        Debug.Log("interacting");
    }
}
