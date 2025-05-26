using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Localization 
{
    private const string KEY = "LOCALIZATION_DATA";

    public Action OnChanged;
    public int LocalizationIndex
    {
        get => PlayerPrefs.GetInt(KEY);
        set
        {
            PlayerPrefs.SetInt(KEY, value);
            OnChanged?.Invoke();
        }
    }

    private Dictionary<string, string[]> localization = new Dictionary<string, string[]>()
    {
        {"Continue", new string[] {"Continue", "����������"} },
        {"Throw", new string[] { "Throw", "��������"} },
        {"Paint", new string[] { "Paint", "���������"} },
        {"Shipments", new string[] { "Shipments", "��������" } },
        {"Management", new string[] { "Management", "����������" } },
        {"Edit Storages", new string[] { "Edit Storages", "������������� �����" } },
        {"Buy Storages", new string[] { "Buy Storages", "������ ���������" } },
        {"Payment successful, units delivered to your warehouse!", new string[] { "Payment successful, units delivered to your warehouse!", "����� ������, ��������� ���������� �� ��� �����!" } },
        {"Payment failed, not enough money!", new string[] { "Payment failed, not enough money!", "����� �� ������, ������������ �����!" } },
        {"The truck has already arrived", new string[] { "The truck has already arrived", "�������� ��� ������." } },
        {"Some of the loaded items do not match the store type.", new string[] { "Some of the loaded items do not match the store type:", "��������� ����������� ������ �� ������������� ���� ��������:" } },
        {"Successful", new string[] { "Successful", "�������" } },
        {"Failure", new string[] { "Failure", "������" } },
        {"Supply", new string[] { "Supply", "���������" } },
        {"LLC 'MarketWay Distributors'", new string[] { "LLC 'MarketWay Distributors'", "��� '���������'" } },
        {"Inc. 'Bakery'", new string[] { "Inc. 'Bakery'", "�� '�������'" } },
        {"Corp. 'Dairy'", new string[] { "Corp. 'Dairy'", "���������� '�����'" } },
        {"Inc. 'Brillex'", new string[] { "Inc. 'Brillex'", "�� '��������'" } },
        {"Corp. 'Nordwell'", new string[] { "Corp. 'Nordwell'", "���������� '��������'" } },
        {"Buyer rating", new string[] { "Buyer rating", "������� ���������" } },
        {"Day", new string[] { "Day", "����" } },
        {"LLC", new string[] { "LLC", "���" } },
        {"Sign out", new string[] { "Sign out", "�����" } },
        {"Mail", new string[] { "Mail", "�����" } },
        {"We pay:", new string[] { "We pay:", "�� ��������:" } },
        {"kg", new string[] { "kg", "��" } },
        {"Close", new string[] { "Close", "�������" } },
        {"Complete", new string[] { "Complete", "���������" } },
        {"Deliver", new string[] { "Deliver", "���������" } },
        {"Cancel", new string[] { "Cancel", "��������" } },
        {"Ideal shipments:", new string[] { "Ideal shipments:", "��������� ��������:" } },
        {"Total shipments:", new string[] { "Total shipments:", "����� ��������:" } },
        {"Total losses:", new string[] { "Total losses:", "����� ������:" } },
        {"Boxes sold:", new string[] { "Boxes sold:", "������� �������:" } },
        {"Products spoiled:", new string[] { "Products spoiled:", "��������� �������:" } },
        {"Total earned:", new string[] { "Total earned:", "����� ����������:" } },
        {"FreshMart LLC", new string[] { "FreshMart LLC", "��� ���������" } },
        {"The Veg Bar Inc.", new string[] { "The Veg Bar Inc.", "�� ���� ���" } },
        {"MooMart LLC", new string[] { "MooMart LLC", "��� ��������" } },
        {"Could you deliver some items, please?", new string[] { "Could you deliver some items, please?", "�� �� ����� �� ��������� ������� �������?" } },
        {"Can I get a delivery of goods?", new string[] { "Can I get a delivery of goods?", "����� �������� ��������?" } },
        {"Would you mind bringing over some supplies?", new string[] { "Would you mind bringing over some supplies?", "�� ��������� �������� �������?" } },
        {"I need a delivery � can you help?", new string[] { "I need a delivery � can you help?", "��� ����� �������� � ��������?" } },
        {"Any chance you could drop off my order?", new string[] { "Any chance you could drop off my order?", "���� ����, ��� �������� ��� �����?" } },
        {"Can you bring me some stuff?", new string[] { "Can you bring me some stuff?", "�������� ��� ���-������?" } },
        {"Could I have some goods delivered?", new string[] { "Could I have some goods delivered?", "����� ��������� ������� �������?" } },
        {"Would you be able to deliver a package?", new string[] { "Would you be able to deliver a package?", "������� �������� �������?" } },
        {"Can I place a delivery order?", new string[] { "Can I place a delivery order?", "���� �������� ��������." } },
        {"I�m looking to get something delivered.", new string[] { "I�m looking to get something delivered.", "���, ��� �� ��� ��������� �����." } },
        {"Apple", new string[] { "Apple", "������" } },
        {"Bread", new string[] { "Bread", "����" } },
        {"Broccoli", new string[] { "Broccoli", "��������" } },
        {"Cabbage", new string[] { "Cabbage", "�������" } },
        {"Carrot", new string[] { "Carrot", "�������" } },
        {"Chili Peper", new string[] { "Chili Peper", "����� ����" } },
        {"Cucumber", new string[] { "Cucumber", "������" } },
        {"Yogurt 'Dessert' 60g.", new string[] { "Yogurt 'Dessert' 60g.", "������ 'Dessert' 60�." } },
        {"Garlic", new string[] { "Garlic", "������" } },
        {"Cheese 'Lunch' 40g.", new string[] { "Cheese 'Lunch' 40g.", "��� 'Lunch' 40�." } },
        {"Yogurt 'Lunch' 30g.", new string[] { "Yogurt 'Lunch' 30g.", "������ 'Lunch' 30�." } },
        {"Mango", new string[] { "Mango", "�����" } },
        {"Milk 'Dessert' 700ml.", new string[] { "Milk 'Dessert' 700ml.", "������ 'Dessert' 700��." } },
        {"Milk 'Milky' 700ml.", new string[] { "Milk 'Milky' 700ml.", "������ 'Milky' 700��." } },
        {"Milk 'Milky' 500ml.", new string[] { "Milk 'Milky' 500ml.", "������ 'Milky' 500��." } },
        {"Cheese 'Milky' 40g.", new string[] { "Cheese 'Milky' 40g.", "��� 'Milky' 40�." } },
        {"Onion", new string[] { "Onion", "���" } },
        {"Orange", new string[] { "Orange", "��������" } },
        {"Peach", new string[] { "Peach", "������" } },
        {"Potato", new string[] { "Potato", "���������" } },
        {"Pumpkin", new string[] { "Pumpkin", "�����" } },
        {"Red Bell Peper", new string[] { "Red Bell Peper", "������� �����" } },
        {"Reset", new string[] { "Reset", "�����" } },
        {"Note", new string[] { "Note", "������" } },
        {"Cart", new string[] { "Cart", "�����" } },
        {"Details", new string[] { "Details", "������" } },
        {"Place", new string[] { "Place", "����������" } },
        {"Fold", new string[] { "Fold", "�������" } },
        {"Take", new string[] { "Take", "�����" } },
        {"Off", new string[] { "Off", "���������" } },
        {"On", new string[] { "On", "��������" } },
        {"Open", new string[] { "Open", "�������" } },
        {"Swap the pallets", new string[] { "Swap the pallets", "����������� �������" } },
        {"Confirm", new string[] { "Confirm", "�����������" } },
        {"Rotate", new string[] { "Rotate", "�������" } },
        {"Transfer", new string[] { "Transfer", "�����������" } },
        {"Use", new string[] { "Use", "������������" } },
        {"Tutorial", new string[] { "Tutorial", "��������" } },
        {"Pay your debt to Andy.", new string[] { "Pay your debt to Andy.", "��������� ���� ����." } },
        {"Find a computer. [Press 'P' for details]", new string[] { "Find a computer. [Press 'P' for details]", "������� ���������. [������� 'P' ��� �������]" } },
        {"Buy storage units. [Press 'P' for details]", new string[] { "Buy storage units. [Press 'P' for details]", "������ ���������. [������� 'P' ��� �������]" } },
        {"Place storage units. [Press 'P' for details]", new string[] { "Place storage units. [Press 'P' for details]", "���������� ���������. [������� 'P' ��� �������]" } },
        {"Order a shipment. [Press 'P' for details]", new string[] { "Order a shipment. [Press 'P' for details]", "�������� ��������. [������� 'P' ��� �������]" } },
        {"Grab the clipboard. [Press 'P' for details]", new string[] { "Grab the clipboard. [Press 'P' for details]", "�������� �������. [������� 'P' ��� �������]" } },
        {"Complete Shipment. [Press 'P' for details]", new string[] { "Complete shipment. [Press 'P' for details]", "��������� ��������. [������� 'P' ��� �������]" } },
        {"End the day. [Press 'P' for details]", new string[] { "End the day. [Press 'P' for details]", "��������� ����. [������� 'P' ��� �������]" } },
        {"Check the mail. [Press 'P' for details]", new string[] { "Check the mail. [Press 'P' for details]", "��������� �����. [������� 'P' ��� �������]" } },
        {"Deliver products. [Press 'P' for details]", new string[] { "Deliver products. [Press 'P' for details]", "��������� ������. [������� 'P' ��� �������]" } },
        {"Date", new string[] { "Date", "����" } },
        {"Stamp", new string[] { "Stamp", "�����" } },
        {"Name", new string[] { "Name", "��������" } },
        {"Price(pcs)", new string[] { "Price(pcs)", "����(��.)" } },
        {"Qty", new string[] { "Qty", "���.��" } },
        {"Price(All)", new string[] { "Price(All)", "����(���)" } },
        {"Supplier:", new string[] { "Supplier:", "���������:" } },
        {"Buyer:", new string[] { "Buyer:", "��������:" } },
        {"New Game", new string[] { "New Game", "����� ����" } },
        {"Settings", new string[] { "Settings", "���������" } },
        {"Quit", new string[] { "Quit", "�����" } },
        {"Starting a new game will erase progress. Proceed?", new string[] { "Starting a new game will erase progress. Proceed?", "������ ����� ���� ����� ������� ��������. ����������?" } },
        {"Yes", new string[] { "Yes", "��" } },
        {"No", new string[] { "No", "���" } },
        {"Back", new string[] { "Back", "�����" } },
        {"Music:", new string[] { "Music:", "������:" } },
        {"Sound:", new string[] { "Sound:", "����:" } },
        {"Mouse sensitivity:", new string[] { "Mouse sensitivity:", "���������������� ����:" } },
        {"Resolution:", new string[] { "Resolution:", "����������:" } },
        {"Quality:", new string[] { "Quality:", "�������:" } },
        {"Full screen:", new string[] { "Full screen:", "�� ���� �����:" } },
        {"Language:", new string[] { "Language:", "����:" } },

        {"Enter between 4 and 20 characters!", 
            new string[] { "Enter between 4 and 20 characters!", "������� �� 4 �� 20 ��������!" } },

        {"You cannot end the day while a truck is waiting to be loaded.",
            new string[] { "You cannot end the day while a truck is waiting to be loaded.", "������ ��������� ����, ���� �������� ������� ��������." } },

        {"All trucks are currently in use. A truck will be available the next day after delivery.",
            new string[] { "All trucks are currently in use. A truck will be available the next day after delivery.",
                "��� ��������� ������ ������. �������� ����� �������� �� ��������� ���� ����� ��������." } },

        {"A drawer cannot be placed here.", 
            new string[] { "A drawer cannot be placed here.", "���� ������ ���������� �����." } },

        {"First, remove the top drawer", 
            new string[] { "First, remove the top drawer", "������� ������� ������� ����." } },

        {"Products can only be transferred between identical drawers",
            new string[] { "Products can only be transferred between identical drawers", "������ ����� ���������� ������ ����� ����������� �������." } },

        {"Only result can be noted!",
            new string[] { "Only result can be noted!", "����� ������ ������ ���������!" } },

        {"You cannot start a delivery during a shipment.", 
            new string[] { "You cannot start a delivery during a shipment.", "������ ������ �������� �� ����� ��������." } },

        {"Before sending the delivery, unload any extra goods",
            new string[] { "Before sending the delivery, unload any extra goods", "����� ��������� �������� ��������� ������ ������." } },

        {"You cannot send out an empty truck.", 
            new string[] { "You cannot send out an empty truck.", "������ ��������� ������ ��������." } },

        {"You cannot start a shipment during a delivery.", 
            new string[] { "You cannot start a shipment during a delivery.", "������ ������ �������� �� ����� ��������." } },

        {"To cancel the delivery, you need to unload the products you just loaded.",
            new string[] { "To cancel the delivery, you need to unload the products you just loaded.", 
                "����� �������� ��������, ���������� ��������� ������ ��� ����������� ������." } },

        {"Not enough money to pay for the shipment.", new string[] { "Not enough money to pay for the shipment.", "������������ ������� ��� ������ ��������." } },

        {"Before completing the delivery, make sure that all notes in the invoice are filled in.",
            new string[] { "Before completing the delivery, make sure that all notes in the invoice are filled in.",
                "����� ����������� �������� ���������, ��� ��� ������� � ��������� ���������." } },

        {"Before completing the delivery, make sure no products are left in the truck.",
            new string[] { "Before completing the delivery, make sure no products are left in the truck.", 
                "����� ����������� �������� ���������, ��� � ��������� �� �������� �������." } },

        {"First, confirm the storage container's placement",
            new string[] { "First, confirm the storage's placement", "������� ����������� ���������� ���������." } },

        {"Before editing the storage unit's placement, remove all products from it.", 
            new string[] { "Before editing the storage unit's placement, remove all products from it.", 
                "����� ���������� ���������� ��������� ������� �� ���� ��� ������." } },

        {"Andy", new string[] { "Andy", "����" } },
        {"Got it!", new string[] { "Got it!", "�����!" } },
        {"Are you at the address yet?", new string[] { "Are you at the address yet?", "�� ��� �� �����?" } },
        {"Yeah, I�m here.", new string[] { "Yeah, I�m here.", "���, � ���." } },

        {"Honestly, I thought you�d bail on this. But trust me, if you put in the effort, you can go much further. I�ll help you get started.",
            new string[] { "Honestly, I thought you�d bail on this. But trust me, if you put in the effort, you can go much further. I�ll help you get started.",
                "������, �����, �� ���������. �� ������, ���� ������������ - ������ ������. � ������ ���� ������." } },

        {"First, head to the office - it�s down the left hallway. Find the computer, and we�ll go from there!",
            new string[] { "First, head to the office - it�s down the left hallway. Find the computer, and we�ll go from there!",
                "������� ��� � ������� � �� � ����� ��������. ����� ���������, � ������ �����!" } },

        {"On it!", new string[] { "On it!", "������." } },

        {"I�m at the computer.", new string[] { "I�m at the computer.", "� � ����������." } },

        {"Long story short - you�ll be storing goods until you find buyers.", 
            new string[] { "Long story short - you�ll be storing goods until you find buyers.", 
                "������ ������ � �� ������ ������� ������, ���� �� ������ �����������." } },

        {"Later on, you can even supply your own stores.", new string[] { "Later on, you can even supply your own stores.", "����� �� ���� ������� �������� ���� ��������." } },
        {"So, where do I start?", new string[] { "So, where do I start?", "��� � ���� ������?" } },

        {"I just sent you $5,000. Pay me back when you start making money.",
            new string[] { "I just sent you $5,000. Pay me back when you start making money.",
                "� ������ ��� ������ ���� $5000. ������, ����� ������ ������������." } },

        {"Buy a couple of storage units so you have space for the boxes.",
            new string[] { "Buy a couple of storage units so you have space for the boxes.",
                "���� ���� ��������, ����� ���� ���� ���������� �������." } },

        {"You�ll find them in the app installed on the computer.",
            new string[] { "You�ll find them in the app installed on the computer.", "������ �� � ���������� �� �����." } },

        {"I�ll text you when it�s done!", new string[] { "I�ll text you when it�s done!", "������, ����� �� ����� ������!" } },

        {"Now I need to place them, right?", new string[] { "Now I need to place them, right?", "������ �� ����� ����������, ��?" } },
        {"Exactly!", new string[] { "Exactly!", "������!" } },

        {"You can do it from the same computer - just open the right app.",
            new string[] { "You can do it from the same computer - just open the right app.",
                "��� ���� ����� ������� � ���������� � ������ ������ ������ ����������." } },

        {"Remember, before you exit the editor, don�t forget to confirm the changes with this button.", 
            new string[] { "Remember, before you exit the editor, don�t forget to confirm the changes with this button.", 
                "� �� ������ ����������� ��������� ����� ������� �� ��������� � ������ �����." } },

        {"Okay.", new string[] { "Okay.", "����." } },

        {"Done.", new string[] { "Done.", "������." } },

        {"Now your warehouse is ready to receive its first shipments.", 
            new string[] { "Now your warehouse is ready to receive its first shipments.", "�������, ����� ����� � ������ ��������." } },

        {"Open the 'Shipments' app on the computer and pick a truck from the list.",
            new string[] { "Open the 'Shipments' app on the computer and pick a truck from the list.", 
                "������ ���������� '��������' �� ���������� � ������ �������� �� ������." } },

        {"Do I need to pick a specific one?", new string[] { "Do I need to pick a specific one?", "����� ������?" } },

        {"Any will do - you�ll be able to order all of them throughout the day.", 
            new string[] { "Any will do - you�ll be able to order all of them throughout the day.",
                "����� ������� � � ������� ��� ����� ���������� ���." } },

        {"Let me know when the truck arrives, and we�ll move on to the hardest part!", 
            new string[] { "Let me know when the truck arrives, and we�ll move on to the hardest part!.",
                "��� �����, ����� �������� �������, � ������� � ������ ��������!" } },

        {"Grab the clipboard - it�s on the table next to the computer. Take a look at what�s inside.", 
            new string[] { "Grab the clipboard - it�s on the table next to the computer. Take a look at what�s inside.",
                "������ ������� � �� �� ����� ����� � ������. �������, ��� ��� ������." } },

        {"Let�s do this.", new string[] { "Let�s do this.", "�������." } },

        {"I�m ready.", new string[] { "I�m ready.", "� �����." } },
        {"Let�s begin.", new string[] { "Let�s begin.", "�����." } },
        {"Here�s a list of the delivered products.", new string[] { "Here�s a list of the delivered products.", "��� ������ ������������ �������." } },
        {"Got it?", new string[] { "Got it?", "�����?" } },
        {"Got it.", new string[] { "Got it.", "�����" } },
        {"Yeah.", new string[] { "Yeah.", "��." } },

        {"Your job is to unload the products into the warehouse and log the quantities in the notes.", 
            new string[] { "Your job is to unload the products into the warehouse and log the quantities in the notes.",
                "���� ������ � ���������� ������ �� ����� � �������� ���������� � �������." } },

        {"Why do we even need these notes?", new string[] { "Why do we even need these notes?", " � ����� ������ ��� �������?" } },

        {"Suppliers can make mistakes too - they might list items on paper but forget to load them.",
            new string[] { "Suppliers can make mistakes too - they might list items on paper but forget to load them.",
                "���������� ���� ������� � � ������� �������, � ������� ��������." } },

        {"Enter the quantity, then hit the �Note� button.", new string[] { "Enter the quantity, then hit the �Note� button.", "������� ���������� � ���� ������ '������'." } },
        {"Got it, I think.", new string[] { "Got it, I think.", "�������, �����.." } },

        {"Alright then, get moving - the truck�s already waiting in the yard!", 
            new string[] { "Alright then, get moving � the truck�s already waiting in the yard! Remember, you need to complete the delivery through the computer.",
                "�� ����� ����� � �������� ��� ��� �� �����! �����, �������� ����� ��������� ����� ���������." } },

        {"See you then!", new string[] { "See you then!", "��������!" } },

        {"Looks like you did great!", new string[] { "Looks like you did great!", "������, �� ��������� �������!" } },
        {"I knew you�d pull it off.", new string[] { "I knew you�d pull it off.", "� ����, ��� � ���� ���������." } },
        {"Thanks.", new string[] { "Thanks.", "�������." } },

        {"Take care of the last truck, then go get some rest. We�ll continue tomorrow!", 
            new string[] { "Take care of the last truck, then go get some rest. We�ll continue tomorrow!",
                "��������� � ��������� ���������� � �������. ������ ���������!" } },

        {"I see it didn�t go without losses...", new string[] { "I see it didn�t go without losses...", "����, �� �������� ��� �������" } },

        {"We all started like this. Take your time, and you�ll be a pro in no time!",
            new string[] { "We all started like this. Take your time, and you�ll be a pro in no time!", "� ���� ��� � ������. �� �����, ����� ������ �����!" } },

        {"Yeah...", new string[] { "Yeah...", "���..." } },
        {"See you.", new string[] { "See you.", "�� �����." } },

        {"Almost forgot - before you leave, make sure to confirm it in the �Management� section. Just press the �Sign out� button.", 
            new string[] { "Almost forgot - before you leave, make sure to confirm it in the �Management� section. Just press the �Sign out� button..",
                "���� �� ����� � ����� ������ �� ������ ����������� ��� � ��������� '����������'. ������ ����� ������ '�����'." } },

        {"Oh, and some dairy products will spoil overnight if they are not kept in the fridge. See you.",
            new string[] { "Oh, and some dairy products will spoil overnight if they are not kept in the fridge. See you.",
                "� ��, �������� �������� ���������� �� ����, ���� �� �������� � �����������. �� �������." } },

        {"I�m here. Ready to get started?", new string[] { "I�m here. Ready to get started?", "� ���. ����� ��������?" } },
        {"Yes! Today we start to reap the rewards.", new string[] { "Yes! Today we start to reap the rewards.", "��! ������� ����� �������� �����." } },
        {"First, check your mail in the �Management� app.", new string[] { "First, check your mail in the �Management� app.", "������� ������� ����� � ���������� '����������'." } },

        {"Let�s do it!", new string[] { "Let�s do it!", "�������!" } },

        {"Whoa, that�s a lot of messages.", new string[] { "Whoa, that�s a lot of messages.", "��� ������� �����." } },

        {"Keep in mind, the number of trucks is limited. Choose orders wisely.", 
            new string[] { "Keep in mind, the number of trucks is limited. Choose orders wisely.", "���� � ����, ���������� ������������ ����������. ������� ������ � ����." } },

        {"How long does delivery take?", new string[] { "How long does delivery take?", "������� �������� ��������?" } },

        {"All dispatched couriers will be available again the next day.", 
            new string[] { "All dispatched couriers will be available again the next day.", "��� ������������ ������� ����� ����� �������� �� ��������� ����." } },

        {"Now, here�s the deal. Your task is to accept an order and then load it onto a truck.", 
            new string[] { "Now, here�s the deal. Your task is to accept an order and then load it onto a truck.",
                "������ � ����. ���� ������ � ������� ����� � ��������� ��� � ��������." } },

        {"Like a delivery, in reverse.", new string[] { "Like a delivery, in reverse.", "���� �������� ��������." } },

        {"Exactly! Try to load fresh products - at least as much as was ordered.",
            new string[] { "Exactly! Try to load fresh products - at least as much as was ordered.", 
                "������! ���������� ��������� ������ �������� � ���� �� �������, ������� ��������." } },

        {"Mistakes can affect your reputation. Ready to start?", new string[] { "Mistakes can affect your reputation. Ready to start?", "������ ����� �������� �� ���� ���������. �����?" } },

        {"Good luck, Andy!", new string[] { "Good luck, Andy!", "�����, ����!" } },

        {"Looks like I made my first money!", new string[] { "I made my first money!", "�������, � ��������� ������!" } },
        {"I knew you�d do great.", new string[] { "I knew you�d do great.", "� ����, ��� � ���� ���������." } },
        {"If you�ve got any questions, just message me!", new string[] { "If you�ve got any questions, just message me!", "���� ����� ������� � ����!" } },
        {"About the money...", new string[] { "About the money...", "������ �����..." } },

        {"Pay me back when you�re on your feet. No rush. We�ll stay in touch.", 
            new string[] { "Pay me back when you�re on your feet. No rush. We�ll stay in touch.", "������, ����� �������� �� ����. �� �����. ����� �� �����." } },

        {"Load last save", new string[] { "Replay", "����������" } },
        {"Losses:", new string[] { "Losses:", "������:" } },
        {"Calculating losses...", new string[] { "Calculating losses...", "������� �������..." } },
        {"Status: Significant loss.", new string[] { "Status: Significant loss.", "������: ������������ ������." } },
        {"Status: Minor loss.", new string[] { "Status: Minor loss.", "������: �������������� ������." } },
        {"Status: No loss detected.", new string[] { "Status: No loss detected.", "������: ������ �� ����������." } },
        {"Hello, you recently sent goods to our store.", new string[] { "Hello, you recently sent goods to our store.", "������������, �� ������� ��������� ��� ������." } },
        {"I noticed that some products were missing.", new string[] { "I noticed that some products were missing.", "� �������, ��� ��������� ������� �� �������." } },
        {"Some items were spoiled�this is unacceptable!", new string[] { "Some items were spoiled�this is unacceptable!", "��������� ������ ���� ��������� � ��� �����������!" } },
        {"The payment will reflect that.", new string[] { "The payment will reflect that.", "������ ����� ���������������." } },

        {"The quantity and quality are just right. Thank you!",
            new string[] { "The quantity and quality are just right. Thank you!", "���������� � �������� � ������ �������. �������!" } },

        {"Demo", new string[] { "Demo", "����" } },

        {"You've reached the end of the demo. Thank you for playing!", 
            new string[] { "You've reached the end of the demo.", "�� ����� �� ����� ����-������." } },

        {"To show the clipboard, pick it up from the table when the truck arrives.",
            new string[] { "To show the clipboard, pick it up from the table when the truck arrives.", "����� �������� �������, �������� ��� �� �����, ����� ������� ��������." } },

        {"Unknown", new string[] { "Unknown", "����������" } },
        {"Alright, I�ll take a look!", new string[] { "Alright, I�ll take a look!", "������, � ��������!" } },
        {"Hello! You�ve bought storage units from us before.", new string[] { "Hello! You�ve bought storage units from us before.", "������������! �� �������� � ��� ���������." } },
        {"Yes, I remember.", new string[] { "Yes, I remember.", "��, � �����." } },

        {"I just wanted to let you know we�ve got some special offers for your warehouse!", 
            new string[] { "I just wanted to let you know we�ve got some special offers for your warehouse!",
                "� ������ ����� ��������, ��� � ��� ���� ����������� ����������� ��� ������ ������!" } },

        {"Like what?", new string[] { "Like what?", "��������?" } },

        {"For example, you can now buy a freight scale � it�s a must-have for receiving vegetables!",
            new string[] { "For example, you can now buy a freight scale � it�s a must-have for receiving vegetables!",
                "��������, ������ �� ������ ������ �������� ���� � ��� ����������� ���� ��� ������ ������!" } },

        {"Store in <color=blue>a dry place", new string[] { "Store in <color=blue>a dry place", "������� � <color=blue>����� �����" } },
        {"Keep <color=blue>refrigerated", new string[] { "Keep <color=blue>refrigerated", "������� � <color=blue>������������" } },
        {"Keep <color=blue>frozen", new string[] { "Keep <color=blue>frozen", "������� � <color=blue>������������ ����" } },
        {"<color=red>Spoiled", new string[] { "<color=red>Spoiled", "<color=red>���������" } },
        {"<color=green>Fresh", new string[] { "<color=green>Fresh", "<color=green>������" } },
        {"Scales", new string[] { "Scales", "����" } },
        {"Ventilation", new string[] { "Ventilation", "����������" } },
        {"Security", new string[] { "Security", "������������" } },

        {"You can continue playing with limited content. Please note that progress will be reset in the full version. Thanks for trying the demo � we hope you enjoyed it!", 
            new string[] { "You can continue playing with limited content. Please note that progress will be reset in the full version. Thanks for trying the demo � we hope you enjoyed it!",
                "�� ������ ���������� ���� � ������������ ���������. �������� ��������: � ������ ������ �������� ����� �������. �������, ��� ����������� ���� � ��������, ��� �����������!" } },

        {"Products", new string[] { "Goods", "������" } },

        {"You can't transfer a product into a box of spoiled goods.",
            new string[] { "You can't transfer a product into a box of spoiled goods.",
                "������ ����������� ����� � ���� � ������������ ����������." } },

        {"pcs", new string[] { "pcs.", "��." } },
        {"Store", new string[] { "Store", "�����" } },
        {"Retail outlets", new string[] { "Stores", "������" } },
        {"Pricing", new string[] { "Pricing", "���������������" } },
        {"Product not selected", new string[] { "No product selected", "������� �� ������" } },
        {"Price on delivery:", new string[] { "Price on delivery:", "���� ��� ��������:" } },
        {"Retail price:", new string[] { "Retail price:", "���� � ���������:" } },
        {"Supplier price:", new string[] { "Supplier price:", "���� ����������:" } },
        {"Box weight:", new string[] { "Box weight:", "��� �������:" } },
        {"Raising the price without prior agreement is unacceptable!", 
            new string[] { "Raising the price without prior agreement is unacceptable!", "��������� ���� ��� ������������ - ��� �����������!" } },

        {"Rating", new string[] {"Rating", "�������" } },
        {"Cheese 'Luxury' 40g.", new string[] { "Cheese 'Luxury' 40g.", "��� 'Luxury' 40�." } },
        {"Yogurt 'Luxury'' 30g.", new string[] { "Yogurt 'Luxury'' 30g.", "������ 'Luxury'' 30�." } },
        {"Milk 'Luxury' 700ml.", new string[] { "Milk 'Luxury' 700ml.", "������ 'Luxury' 700��." } },
        {"Milk 'Luxury' 500ml.", new string[] { "Milk 'Luxury' 500ml.", "������ 'Luxury' 500��." } },

        {"CityHall", new string[] { "City Hall", "�����" } },

        {"Baguette", new string[] { "Baguette", "�����" } },
        {"Bread roll", new string[] { "Bread roll", "�����" } },
        {"Donut 'Donato' Blueb", new string[] { "Donut 'Donato' Blueb.", "������ 'Donato' ����." } },
        {"Donut 'Donato' Rasp", new string[] { "Donut 'Donato' Rasp", "������ 'Donato' ������" } },
        {"Donut 'Donato' Strawb", new string[] { "Donut 'Donato' Strawb.", "������ 'Donato' �����." } },
        {"Donut 'Donni' �aramel", new string[] { "Donut 'Donni' �aramel", "������ 'Donni' ��������" } },
        {"Donut 'Donni' Chocolate", new string[] { "Donut 'Donni' Chocolate", "������ 'Donni' ���." } },
        {"Donut 'Donni' Lime", new string[] { "Donut 'Donni' Lime", "������ 'Donni' ����" } },
        {"Donut 'Luxury' Lemon", new string[] { "Donut 'Luxury' Lemon", "������ 'Luxury' �����" } },
        {"Donut 'Luxury' Kiwi", new string[] { "Donut 'Luxury' Kiwi", "������ 'Luxury' ����" } },

        {"For Sale:", new string[] { "For Sale:", "� �������:" } },

        {"To close the place down, you'll have to lay off all the employees.",
            new string[] { "To close the place down, you'll have to lay off all the employees. Hall",
                "����� ���������� ������ ���������, ���������� ������� ���� �����������." } },

        {"Bread Nook", new string[] { "Bread Nook", "������� ������" } },
        {"Tech Town", new string[] { "Tech Town", "Tech Town" } },
        {"Business", new string[] { "Business", "������" } },
        {"Rental price:", new string[] { "Rental price:", "���� ������:" } },
        {"Renter:", new string[] { "Renter:", "���������:" } },

        {"ProductShop", new string[] { "Supermarket", "�����������" } },
        {"Grocceries", new string[] { "Vegetable store", "������� �������" } },
        {"MilkShop", new string[] { "Dairy store", "�������� �������" } },
        {"BreadStore", new string[] { "Bakery store", "������� �����" } },
        {"TechShop", new string[] { "Electronics store", "������� �������" } },

        {"Rent", new string[] { "Rent", "����������" } },
        {"Info", new string[] { "Info", "����������" } },
        {"Map", new string[] { "Map", "�����" } },
        {"Employees", new string[] { "Employees", "���������" } },
        {"Revenue Graph", new string[] { "Revenue Graph", "�������" } },
        {"Select a store location on the map", new string[] { "Select a store location on the map", "�������� �������������� �������� �� �����." } },

        {"Cashier", new string[] { "Cashier", "�����" } },
        {"Salesperson", new string[] { "Salesperson", "��������" } },
        {"Manager", new string[] { "Manager", "�����������" } },
        {"Loader", new string[] { "Loader", "�������" } },
        {"Accountant", new string[] { "Accountant", "���������" } },
        {"DriverHelper", new string[] { "Driver Helper", "�������� ��������" } },
        {"SecurityGuard", new string[] { "Security Guard", "��������" } },

        {"Pay", new string[] { "Pay", "��������" } },
        {"Salary", new string[] { "Salary", "��������" } },
        {"Unpaid Rent", new string[] { "Unpaid Rent", "������" } },

        {"Hire", new string[] { "Hire", "������" } },
        {"lay off", new string[] { "Lay off", "�������" } },
        {"Unrent", new string[] { "Unrent", "������" } },

        {"Truck is ready for loading. ", new string[] { "Truck is ready for loading. ", "�������� ������." } },
        {"Press 'Supply' to send it off.", new string[] { "Press 'Supply' to send it off.", "������� '���������' ����� ���������." } },
        {"Only load products that match the <color=yellow>store type", new string[] { "Only load products that match the <color=yellow>store type", "������� ������ �� ������, ��� ������������� <color=yellow>���� ��������" } },

        {"You need a rented space to hire employees.", new string[] { "You need a rented space to hire employees.", "����� �������� �����������, ����� ���������� ���������." } },

        {"manager_info", new string[] { "Works in the store, required to ensure the efficient and organized functioning of large retail stores.",
            "�������� � ��������, ��������� ��� ����������� ����������� � �������������� ������ ������� ��������� ���������." } },
        {"accountant_info", new string[] { "Works in the warehouse, handles paperwork, pays workers' salaries, and pays the rent.",
            "�������� �� ������, ���������� �������� �������, ������ �������� �������, ���������� ������." } },
        {"salesperson_info", new string[] { "Works in the store and increases the number of items that can be sold per day.",
            "�������� � ��������, ��������� ��������� ������ ������� � ����." } },
        {"security_info", new string[] { "Works in the store, reduces the risk of product theft and decreases the number of conflict situations.",
            "�������� � ��������, ��������� ���� ����� ������, ��������� ���������� ����������� ��������." } },
        {"loader_info", new string[] { "Works in the warehouse and loads products for delivery. (only works if the products are not blocked)",
            "P������� �� ������, ��������� �������� �� ��������. (��������, ���� �������� �� ����������)" } },
        {"cashier_info", new string[] { "Works in the store and sells products to customers.", "P������� � ��������, ������� ������ �����������." } },
        {"driver_info", new string[] { "Works during deliveries and checks suppliers during loading.",
            "P������� ��� ���������, ��������� ����������� ��� ��������." } },
    };

    public string Translate(string text)
    {
        if (!localization.ContainsKey(text))
        {
            Debug.Log($"[Translate] - [{text}]");
            return text;
        }

        if (localization[text].Length <= LocalizationIndex)
        {
            return text;
        }

        return localization[text][LocalizationIndex];
    }
}
