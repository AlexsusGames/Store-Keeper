using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    [SerializeField] private ProductsBlank[] views;
    [SerializeField] private GameObject orderPanel;
    [SerializeField] private Calculator calculator;
    [SerializeField] private Notes notes;

    private Dictionary<string, float> expectedOrder;
    private Dictionary<string, float> actualOrder;

    private List<string> itemsList;
    private List<float> notesList;
    private bool isEditing => orderPanel.activeInHierarchy;
    private int lastIndex => expectedOrder.Count - 1;
    private int currentIndex;

    private void Awake()
    {
        calculator.OnNoted += Note;
    }

    private void Update()
    {
        if (isEditing)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                ChangeIndex(1);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                ChangeIndex(-1);
            }
        }
    }

    private void ChangeIndex(int value = 0)
    {
        currentIndex += value;

        currentIndex = currentIndex < 0 ? lastIndex : currentIndex;
        currentIndex = currentIndex > lastIndex ? 0 : currentIndex;

        views[0].DrawUnit(currentIndex);

        notes.SetData(notesList[currentIndex]);
    }

    private void Note(float value)
    {
        int index = currentIndex;
        var item = itemsList[index];

        notesList[index] = value;
        ChangeIndex();

        bool isEqual = notesList[index] == expectedOrder[item];
        
        for ( int i = 0; i < views.Length; i++)
        {
            views[i].SetEqualStatusByIndex(index, isEqual);
        }
    }

    public void Init(Dictionary<string, float> expectedOrder, Dictionary<string, float> actualOrder, CarType type)
    {
        this.expectedOrder = expectedOrder;
        this.actualOrder = actualOrder;

        itemsList = actualOrder.Keys.ToList();
        notesList = Enumerable.Repeat(0f, actualOrder.Keys.Count).ToList();

        UpdateViews(expectedOrder, type);
        ChangeIndex();
    }

    private void UpdateViews(Dictionary<string, float> order, CarType type)
    {
        for (int i = 0; i < views.Length; i++)
        {
            views[i].SetData(order, type);
        }
    }
}
