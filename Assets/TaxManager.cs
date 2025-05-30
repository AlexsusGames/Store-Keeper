using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxManager : InteractiveManager
{
    [SerializeField] private TaxDocument view;

    private void Start()
    {
        view.OnFilled += OnTaxPayed;
    }

    public override void Interact()
    {
        var interactor = Core.Interactors.GetInteractor<DayProgressInteractor>();

        if (interactor.HasUnpaidBills())
        {
            var bills = interactor.GetTaxInfo();

            view.FillUnits(bills.names, bills.amounts);
        }
        else Core.Clues.Show("All tax invoices have already been filled out.");
    }

    private void OnTaxPayed()
    {
        var interactor = Core.Interactors.GetInteractor<DayProgressInteractor>();

        interactor.PayTaxes();
    }
}
