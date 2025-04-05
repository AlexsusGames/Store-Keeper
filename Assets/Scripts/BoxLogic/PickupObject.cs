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
    [SerializeField] private GameObject cap;

    private List<Material[]> cachedMaterials = new();
    private BoxCollider bCollider;

    private StoreBox box;
    public bool IsFreeToPlace;
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
        OutlineEnabled(false);
        OutlineBlock = true;
        OnGrabbed?.Invoke();
    }
    public void Place()
    {
        OnPlaced?.Invoke();
        SetDefaut();
        OutlineBlock = false;
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

        Material mat = value ? enablePreviewMat : disablePreviewMat;

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

    private Collider[] GetOverlappingColliders()
    {
        Vector3 worldCenter = transform.TransformPoint(bCollider.center);

        Vector3 halfExtents = Vector3.Scale(bCollider.size * 0.5f, transform.lossyScale);

        Quaternion rotation = transform.rotation;

        return Physics.OverlapBox(worldCenter, halfExtents, rotation);
    }

    public override void Interact()
    {
        Debug.Log("interacting");
    }
}
