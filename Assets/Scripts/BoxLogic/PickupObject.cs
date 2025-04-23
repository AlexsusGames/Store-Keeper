using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupObject : InteractiveManager
{
    [SerializeField] private BoxSettings settings;
    private MeshRenderer[] renderers => settings.Renderers;
    private GameObject cap => settings.capObject;

    private List<Material[]> cachedMaterials = new();

    private BoxCollider bCollider;
    private StoreBox box;
    public bool IsFreeToPlace => settings.IsFreeToPlace;
    public StoreBox StoreBox => box;

    private void Awake()
    {
        bCollider = GetComponent<BoxCollider>(); 
        box = GetComponent<StoreBox>();

        for (int i = 0; i < renderers.Length; i++)
        {
            cachedMaterials.Add(renderers[i].sharedMaterials);
        }
    }

    public void ChangeLayer(int layer)
    {
        for (int i = 0;i < renderers.Length; i++)
        {
            renderers[i].gameObject.layer = layer;
        }
    }

    public void SetCapActive(bool isActive)
    {
        if(cap != null)
        {
            cap.SetActive(isActive);
        }
    }

    public void Grab()
    {
        CollidersEnabled(false);

        OutlineEnabled(false);
        OutlineBlock = true;
    }
    public void Place()
    {
        CollidersEnabled(true);

        SetDefaut();
        OutlineBlock = false;
    }

    private void CollidersEnabled(bool enabled)
    {
        var surfaceCollider = settings.SurfaceCollider;

        if(surfaceCollider != null)
        {
            surfaceCollider.enabled = enabled;
        }

        settings.CollisionCollider.enabled = enabled;
    }

    public void SetDefaut()
    {
        OutlineEnabled(false);

        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] materials = cachedMaterials[i];
            renderers[i].sharedMaterials = materials;
        }

        OutlineEnabled(true);
    }

    private void Draw(bool value)
    {
        OutlineEnabled(false);

        Material mat = value ? settings.BluePreviewMaterial : settings.RedPreviewMaterial;

        for (int i = 0;i < renderers.Length; i++)
        {
            Material[] materials = renderers[i].sharedMaterials;

            for (int j = 0; j < materials.Length; j++)
            {
                materials[j] = mat;
            }

            renderers[i].sharedMaterials = materials;
        }

        OutlineEnabled(true);
    }

    public bool CheckAvailableToPlace()
    {
        Collider[] results = GetOverlappingColliders();

        var result = results.Length == 1;

        Draw(result);

        Priint(results);

        return result;
    }


    private Collider[] GetOverlappingColliders()
    {
        Vector3 worldCenter = transform.TransformPoint(bCollider.center);

        Vector3 halfExtents = Vector3.Scale(bCollider.size * 0.5f, transform.lossyScale);

        Quaternion rotation = transform.rotation;

        return Physics.OverlapBox(worldCenter, halfExtents, rotation);
    }

    private void Priint(Collider[] colliers)
    {
        string result = "";

        for (int i = 0; i < colliers.Length; i++)
        {
            result += colliers[i].name;
            result += ", ";
        }

        Debug.Log(result);
    }

    public override void Interact()
    {
    }
}
