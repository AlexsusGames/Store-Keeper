using System;
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
    public Dictionary<string, float> ExpectedOrder { get; private set; }
    public Dictionary<string, float> ActualOrder { get; private set; }

    private List<string> itemsList;
    private List<float> notesList;
    private bool isEditing => orderPanel.activeInHierarchy;
    private int lastIndex => ExpectedOrder.Count - 1;
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

        if(notesList.Count > 0)
        {
            notes.SetData(notesList[currentIndex]);

            Core.Sound.PlayClip(AudioType.Tablet);
        }
    }

    private void Note(float value)
    {
        int index = currentIndex;
        var item = itemsList[index];

        value = value > ExpectedOrder[itemsList[index]] ? ExpectedOrder[itemsList[index]] : value;

        notesList[index] = value;
        ChangeIndex();

        float different = notesList[index] - ExpectedOrder[item];

        bool isEqual = Math.Abs(different) < 0.1;

        for ( int i = 0; i < views.Length; i++)
        {
            views[i].SetEqualStatusByIndex(index, isEqual);
        }
    }

    public Dictionary<string, float> GetNotes()
    {
        Dictionary<string, float> result = new();

        for ( int i = 0; i < itemsList.Count; i++)
        {
            result[itemsList[i]] = notesList[i];
        }

        return result;
    }

    public void Init(Dictionary<string, float> expectedOrder, Dictionary<string, float> actualOrder, CarType type)
    {
        this.ExpectedOrder = expectedOrder;
        this.ActualOrder = actualOrder;

        itemsList = actualOrder.Keys.ToList();
        notesList = Enumerable.Repeat(0f, actualOrder.Keys.Count).ToList();

        UpdateViews(expectedOrder, type);
        ChangeIndex();
    }

    public bool IsFilledIn()
    {
        for ( int i = 0;i < notesList.Count;i++)
        {
            if (notesList[i] == 0)
                return false;
        }

        return true;
    }

    private void UpdateViews(Dictionary<string, float> order, CarType type)
    {
        for (int i = 0; i < views.Length; i++)
        {
            views[i].SetData(order, type);
        }
    }
}
