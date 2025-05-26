using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ProductViewPool : MonoBehaviour
{
    [SerializeField] private RectTransform viewPrefab;
    [Inject] private DiContainer container;

    private List<ProductView> viewPool;

    public ProductView Get(RectTransform parent)
    {
        if(viewPool == null)
        {
            viewPool = new List<ProductView>();
        }

        for (int i = 0; i < viewPool.Count; i++)
        {
            if (!viewPool[i].gameObject.activeInHierarchy)
            {
                viewPool[i].transform.SetParent(parent);
                viewPool[i].gameObject.SetActive(true);

                return viewPool[i];
            }
        }

        return CreateNew(parent);
    }

    public void HideAll()
    {
        if(viewPool != null)
        {
            for (int i = 0; i < viewPool.Count; i++)
            {
                viewPool[i].transform.SetParent(transform);
                viewPool[i].gameObject.SetActive(false);
            }
        }
    }

    private ProductView CreateNew(RectTransform parent)
    {
        var rect = container.InstantiatePrefab(viewPrefab, parent);
        var view = rect.GetComponent<ProductView>();
        viewPool.Add(view);

        return view;
    }
}
