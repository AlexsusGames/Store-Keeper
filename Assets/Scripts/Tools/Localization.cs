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
        {"Continue", new string[] {"Continue", "Продолжить"} },
        {"Shipments", new string[] { "Shipments", "Поставки" } },
        {"Management", new string[] { "Management", "Менеджмент" } },
        {"Edit Storages", new string[] { "Edit Storages", "Редактировать склад" } },
        {"Buy Storages", new string[] { "Buy Storages", "Купить хранилища" } },
        {"Payment successful, units delivered to your warehouse!", new string[] { "Payment successful, units delivered to your warehouse!", "Платёж прошёл, хранилища доставлены на ваш склад!" } },
        {"Payment failed, not enough money!", new string[] { "Payment failed, not enough money!", "Платёж не прошёл, недостаточно денег!" } },
        {"Successful", new string[] { "Successful", "Успешно" } },
        {"Failure", new string[] { "Failure", "Ошибка" } },
        {"Supply", new string[] { "Supply", "Заказать" } },
        {"LLC 'MarketWay Distributors'", new string[] { "LLC 'MarketWay Distributors'", "ООО 'МаркетВэй'" } },
        {"Inc. 'Bakery'", new string[] { "Inc. 'Bakery'", "АО 'Бейкери'" } },
        {"Corp. 'Dairy'", new string[] { "Corp. 'Dairy'", "Корпорация 'Дэйри'" } },
        {"Inc. 'Brillex'", new string[] { "Inc. 'Brillex'", "АО 'Бриллекс'" } },
        {"Corp. 'Nordwell'", new string[] { "Corp. 'Nordwell'", "Корпорация 'Нордвелл'" } },
        {"Buyer rating", new string[] { "Buyer rating", "Рейтинг заказчика" } },
        {"Day", new string[] { "Day", "День" } },
        {"LLC", new string[] { "LLC", "ООО" } },
        {"Sign out", new string[] { "Sign out", "Выйти" } },
        {"Mail", new string[] { "Mail", "Почта" } },
        {"We pay:", new string[] { "We pay:", "Мы заплатим:" } },
        {"kg", new string[] { "kg", "кг" } },
        {"Close", new string[] { "Close", "Закрыть" } },
        {"Complete", new string[] { "Complete", "Завершить" } },
        {"Deliver", new string[] { "Deliver", "Доставить" } },
        {"Cancel", new string[] { "Cancel", "Отменить" } },
        {"Ideal shipments:", new string[] { "Ideal shipments:", "Идеальные поставки:" } },
        {"Total shipments:", new string[] { "Total shipments:", "Всего поставок:" } },
        {"Total losses:", new string[] { "Total losses:", "Всего потерь:" } },
        {"Boxes sold:", new string[] { "Boxes sold:", "Продано товаров:" } },
        {"Products spoiled:", new string[] { "Products spoiled:", "Испорчено товаров:" } },
        {"Total earned:", new string[] { "Total earned:", "Всего заработано:" } },
        {"FreshMart LLC", new string[] { "FreshMart LLC", "ООО «ФрешМарт»" } },
        {"The Veg Bar Inc.", new string[] { "The Veg Bar Inc.", "АО «Вег Бар»" } },
        {"MooMart LLC", new string[] { "MooMart LLC", "ООО «МууМарт»" } },
        {"Could you deliver some items, please?", new string[] { "Could you deliver some items, please?", "Вы не могли бы доставить немного товаров?" } },
        {"Can I get a delivery of goods?", new string[] { "Can I get a delivery of goods?", "Можно заказать доставку?" } },
        {"Would you mind bringing over some supplies?", new string[] { "Would you mind bringing over some supplies?", "Не затруднит привезти припасы?" } },
        {"I need a delivery — can you help?", new string[] { "I need a delivery — can you help?", "Мне нужна доставка — поможете?" } },
        {"Any chance you could drop off my order?", new string[] { "Any chance you could drop off my order?", "Есть шанс, что привезёте мой заказ?" } },
        {"Can you bring me some stuff?", new string[] { "Can you bring me some stuff?", "Привезёте мне что-нибудь?" } },
        {"Could I have some goods delivered?", new string[] { "Could I have some goods delivered?", "Можно доставить немного товаров?" } },
        {"Would you be able to deliver a package?", new string[] { "Would you be able to deliver a package?", "Сможете привезти посылку?" } },
        {"Can I place a delivery order?", new string[] { "Can I place a delivery order?", "Хочу оформить доставку." } },
        {"I’m looking to get something delivered.", new string[] { "I’m looking to get something delivered.", "Ищу, кто бы мог доставить заказ." } },
        {"Apple", new string[] { "Apple", "Яблоки" } },
        {"Bread", new string[] { "Bread", "Хлеб" } },
        {"Broccoli", new string[] { "Broccoli", "Брокколи" } },
        {"Cabbage", new string[] { "Cabbage", "Капуста" } },
        {"Carrot", new string[] { "Carrot", "Морковь" } },
        {"Chili Peper", new string[] { "Chili Peper", "Перец чили" } },
        {"Cucumber", new string[] { "Cucumber", "Огурец" } },
        {"Yogurt 'Dessert' 60g.", new string[] { "Yogurt 'Dessert' 60g.", "Йогурт 'Dessert' 60г." } },
        {"Garlic", new string[] { "Garlic", "Чеснок" } },
        {"Cheese 'Lunch' 40g.", new string[] { "Cheese 'Lunch' 40g.", "Сыр 'Lunch' 40г." } },
        {"Yogurt 'Lunch' 30g.", new string[] { "Yogurt 'Lunch' 30g.", "Йогурт 'Lunch' 30г." } },
        {"Mango", new string[] { "Mango", "Манго" } },
        {"Milk 'Dessert' 700ml.", new string[] { "Milk 'Dessert' 700ml.", "Молоко 'Dessert' 700мл." } },
        {"Milk 'Milky' 700ml.", new string[] { "Milk 'Milky' 700ml.", "Молоко 'Milky' 700мл." } },
        {"Milk 'Milky' 500ml.", new string[] { "Milk 'Milky' 500ml.", "Молоко 'Milky' 500мл." } },
        {"Cheese 'Milky' 40g.", new string[] { "Cheese 'Milky' 40g.", "Сыр 'Milky' 40г." } },
        {"Onion", new string[] { "Onion", "Лук" } },
        {"Orange", new string[] { "Orange", "Апельсин" } },
        {"Peach", new string[] { "Peach", "Персик" } },
        {"Potato", new string[] { "Potato", "Картофель" } },
        {"Pumpkin", new string[] { "Pumpkin", "Тыква" } },
        {"Red Bell Peper", new string[] { "Red Bell Peper", "Красный перец" } },
        {"Reset", new string[] { "Reset", "Сброс" } },
        {"Note", new string[] { "Note", "Внести" } },
        {"Cart", new string[] { "Cart", "Взять" } },
        {"Details", new string[] { "Details", "Детали" } },
        {"Place", new string[] { "Place", "Разместить" } },
        {"Fold", new string[] { "Fold", "Сложить" } },
        {"Take", new string[] { "Take", "Взять" } },
        {"Off", new string[] { "Off", "Выключить" } },
        {"On", new string[] { "On", "Включить" } },
        {"Open", new string[] { "Open", "Открыть" } },
        {"Swap the pallets", new string[] { "Swap the pallets", "Передвинуть паллеты" } },
        {"Confirm", new string[] { "Confirm", "Подтвердить" } },
        {"Rotate", new string[] { "Rotate", "Вращать" } },
        {"Transfer", new string[] { "Transfer", "Переместить" } },
        {"Use", new string[] { "Use", "Использовать" } },
        {"Tutorial", new string[] { "Tutorial", "Обучение" } },
        {"Pay your debt to Andy.", new string[] { "Pay your debt to Andy.", "Заплатите долг Энди." } },
        {"Find a computer. [Press 'P' for details]", new string[] { "Find a computer. [Press 'P' for details]", "Найдите компьютер. [Нажмите 'P' для деталей]" } },
        {"Buy storage units. [Press 'P' for details]", new string[] { "Buy storage units. [Press 'P' for details]", "Купите хранилища. [Нажмите 'P' для деталей]" } },
        {"Place storage units. [Press 'P' for details]", new string[] { "Place storage units. [Press 'P' for details]", "Разместите хранилища. [Нажмите 'P' для деталей]" } },
        {"Order a shipment. [Press 'P' for details]", new string[] { "Order a shipment. [Press 'P' for details]", "Закажите поставку. [Нажмите 'P' для деталей]" } },
        {"Grab the clipboard. [Press 'P' for details]", new string[] { "Grab the clipboard. [Press 'P' for details]", "Возьмите планшет. [Нажмите 'P' для деталей]" } },
        {"Complete Shipment. [Press 'P' for details]", new string[] { "Complete shipment. [Press 'P' for details]", "Завершите поставку. [Нажмите 'P' для деталей]" } },
        {"End the day. [Press 'P' for details]", new string[] { "End the day. [Press 'P' for details]", "Завершите день. [Нажмите 'P' для деталей]" } },
        {"Check the mail. [Press 'P' for details]", new string[] { "Check the mail. [Press 'P' for details]", "Проверьте почту. [Нажмите 'P' для деталей]" } },
        {"Deliver products. [Press 'P' for details]", new string[] { "Deliver products. [Press 'P' for details]", "Доставьте товары. [Нажмите 'P' для деталей]" } },
        {"Date", new string[] { "Date", "Дата" } },
        {"Stamp", new string[] { "Stamp", "Штамп" } },
        {"Name", new string[] { "Name", "Название" } },
        {"Price(pcs)", new string[] { "Price(pcs)", "Цена(шт.)" } },
        {"Qty", new string[] { "Qty", "Кол.во" } },
        {"Price(All)", new string[] { "Price(All)", "Цена(все)" } },
        {"Supplier:", new string[] { "Supplier:", "Поставщик:" } },
        {"Buyer:", new string[] { "Buyer:", "Заказчик:" } },
        {"New Game", new string[] { "New Game", "Новая игра" } },
        {"Settings", new string[] { "Settings", "Настройки" } },
        {"Quit", new string[] { "Quit", "Выйти" } },
        {"Starting a new game will erase progress. Proceed?", new string[] { "Starting a new game will erase progress. Proceed?", "Начало новой игры сотрёт текущий прогресс. Продолжить?" } },
        {"Yes", new string[] { "Yes", "Да" } },
        {"No", new string[] { "No", "Нет" } },
        {"Back", new string[] { "Back", "Назад" } },
        {"Music:", new string[] { "Music:", "Музыка:" } },
        {"Sound:", new string[] { "Sound:", "Звук:" } },
        {"Mouse sensitivity:", new string[] { "Mouse sensitivity:", "Чувствительность мыши:" } },
        {"Resolution:", new string[] { "Resolution:", "Разрешение:" } },
        {"Quality:", new string[] { "Quality:", "Графика:" } },
        {"Full screen:", new string[] { "Full screen:", "На весь экран:" } },
        {"Language:", new string[] { "Language:", "Язык:" } },

        {"Enter between 4 and 20 characters!", 
            new string[] { "Enter between 4 and 20 characters!", "Введите от 4 до 20 символов!" } },

        {"You cannot end the day while a truck is waiting to be loaded.",
            new string[] { "You cannot end the day while a truck is waiting to be loaded.", "Нельзя завершить день, пока грузовик ожидает загрузки." } },

        {"All trucks are currently in use. A truck will be available the next day after delivery.",
            new string[] { "All trucks are currently in use. A truck will be available the next day after delivery.",
                "Все грузовики сейчас заняты. Грузовик будет доступен на следующий день после доставки." } },

        {"A drawer cannot be placed here.", 
            new string[] { "A drawer cannot be placed here.", "Ящик нельзя разместить здесь." } },

        {"First, remove the top drawer", 
            new string[] { "First, remove the top drawer", "Сначала уберите верхний ящик." } },

        {"Products can only be transferred between identical drawers",
            new string[] { "Products can only be transferred between identical drawers", "Товары можно перемещать только между одинаковыми ящиками." } },

        {"Only result can be noted!",
            new string[] { "Only result can be noted!", "Можно внести только результат!" } },

        {"You cannot start a delivery during a shipment.", 
            new string[] { "You cannot start a delivery during a shipment.", "Нельзя начать доставку во время поставки." } },

        {"Before sending the delivery, unload any extra goods",
            new string[] { "Before sending the delivery, unload any extra goods", "Перед отправкой доставки выгрузите лишние товары." } },

        {"You cannot send out an empty truck.", 
            new string[] { "You cannot send out an empty truck.", "Нельзя отправить пустой грузовик." } },

        {"You cannot start a shipment during a delivery.", 
            new string[] { "You cannot start a shipment during a delivery.", "Нельзя начать поставку во время доставки." } },

        {"To cancel the delivery, you need to unload the products you just loaded.",
            new string[] { "To cancel the delivery, you need to unload the products you just loaded.", 
                "Чтобы отменить доставку, необходимо выгрузить только что загруженные товары." } },

        {"Not enough money to pay for the shipment.", new string[] { "Not enough money to pay for the shipment.", "Недостаточно средств для оплаты поставки." } },

        {"Before completing the delivery, make sure that all notes in the invoice are filled in.",
            new string[] { "Before completing the delivery, make sure that all notes in the invoice are filled in.",
                "Перед завершением доставки убедитесь, что все позиции в накладной заполнены." } },

        {"Before completing the delivery, make sure no products are left in the truck.",
            new string[] { "Before completing the delivery, make sure no products are left in the truck.", 
                "Перед завершением доставки убедитесь, что в грузовике не осталось товаров." } },

        {"First, confirm the storage container's placement",
            new string[] { "First, confirm the storage's placement", "Сначала подтвердите размещение хранилища." } },

        {"Before editing the storage unit's placement, remove all products from it.", 
            new string[] { "Before editing the storage unit's placement, remove all products from it.", 
                "Перед изменением размещения хранилища удалите из него все товары." } },

        {"Andy", new string[] { "Andy", "Энди" } },
        {"Got it!", new string[] { "Got it!", "Понял!" } },
        {"Are you at the address yet?", new string[] { "Are you at the address yet?", "Ты уже на месте?" } },
        {"Yeah, I’m here.", new string[] { "Yeah, I’m here.", "Ага, я тут." } },

        {"Honestly, I thought you’d bail on this. But trust me, if you put in the effort, you can go much further. I’ll help you get started.",
            new string[] { "Honestly, I thought you’d bail on this. But trust me, if you put in the effort, you can go much further. I’ll help you get started.",
                "Честно, думал, ты соскочишь. Но поверь, если постараешься - далеко пойдёшь. Я помогу тебе начать." } },

        {"First, head to the office - it’s down the left hallway. Find the computer, and we’ll go from there!",
            new string[] { "First, head to the office - it’s down the left hallway. Find the computer, and we’ll go from there!",
                "Сначала иди в кабинет — он в левом коридоре. Найди компьютер, и оттуда начнём!" } },

        {"On it!", new string[] { "On it!", "Сейчас." } },

        {"I’m at the computer.", new string[] { "I’m at the computer.", "Я у компьютера." } },

        {"Long story short - you’ll be storing goods until you find buyers.", 
            new string[] { "Long story short - you’ll be storing goods until you find buyers.", 
                "Короче говоря — ты будешь хранить товары, пока не найдёшь покупателей." } },

        {"Later on, you can even supply your own stores.", new string[] { "Later on, you can even supply your own stores.", "Потом ты даже сможешь снабжать свои магазины." } },
        {"So, where do I start?", new string[] { "So, where do I start?", "Так с чего начать?" } },

        {"I just sent you $5,000. Pay me back when you start making money.",
            new string[] { "I just sent you $5,000. Pay me back when you start making money.",
                "Я только что скинул тебе $5000. Вернёшь, когда начнёшь зарабатывать." } },

        {"Buy a couple of storage units so you have space for the boxes.",
            new string[] { "Buy a couple of storage units so you have space for the boxes.",
                "Купи пару хранилищ, чтобы было куда складывать коробки." } },

        {"You’ll find them in the app installed on the computer.",
            new string[] { "You’ll find them in the app installed on the computer.", "Найдёшь их в приложении на компе." } },

        {"I’ll text you when it’s done!", new string[] { "I’ll text you when it’s done!", "Напишу, когда всё будет готово!" } },

        {"Now I need to place them, right?", new string[] { "Now I need to place them, right?", "Теперь их нужно расставить, да?" } },
        {"Exactly!", new string[] { "Exactly!", "Именно!" } },

        {"You can do it from the same computer - just open the right app.",
            new string[] { "You can do it from the same computer - just open the right app.",
                "Это тоже можно сделать с компьютера — просто открой нужное приложение." } },

        {"Remember, before you exit the editor, don’t forget to confirm the changes with this button.", 
            new string[] { "Remember, before you exit the editor, don’t forget to confirm the changes with this button.", 
                "И не забудь подтвердить изменения перед выходом из редактора — кнопка внизу." } },

        {"Okay.", new string[] { "Okay.", "Окей." } },

        {"Done.", new string[] { "Done.", "Готово." } },

        {"Now your warehouse is ready to receive its first shipments.", 
            new string[] { "Now your warehouse is ready to receive its first shipments.", "Отлично, склад готов к первой поставке." } },

        {"Open the 'Shipments' app on the computer and pick a truck from the list.",
            new string[] { "Open the 'Shipments' app on the computer and pick a truck from the list.", 
                "Открой приложение 'Поставки' на компьютере и выбери грузовик из списка." } },

        {"Do I need to pick a specific one?", new string[] { "Do I need to pick a specific one?", "Какой именно?" } },

        {"Any will do - you’ll be able to order all of them throughout the day.", 
            new string[] { "Any will do - you’ll be able to order all of them throughout the day.",
                "Любой подойдёт — в течение дня можно заказывать все." } },

        {"Let me know when the truck arrives, and we’ll move on to the hardest part!", 
            new string[] { "Let me know when the truck arrives, and we’ll move on to the hardest part!.",
                "Дай знать, когда грузовик приедет, и перейдём к самому сложному!" } },

        {"Grab the clipboard - it’s on the table next to the computer. Take a look at what’s inside.", 
            new string[] { "Grab the clipboard - it’s on the table next to the computer. Take a look at what’s inside.",
                "Возьми планшет — он на столе рядом с компом. Загляни, что там внутри." } },

        {"Let’s do this.", new string[] { "Let’s do this.", "Погнали." } },

        {"I’m ready.", new string[] { "I’m ready.", "Я готов." } },
        {"Let’s begin.", new string[] { "Let’s begin.", "Начнём." } },
        {"Here’s a list of the delivered products.", new string[] { "Here’s a list of the delivered products.", "Вот список доставленных товаров." } },
        {"Got it?", new string[] { "Got it?", "Понял?" } },
        {"Got it.", new string[] { "Got it.", "Понял" } },
        {"Yeah.", new string[] { "Yeah.", "Да." } },

        {"Your job is to unload the products into the warehouse and log the quantities in the notes.", 
            new string[] { "Your job is to unload the products into the warehouse and log the quantities in the notes.",
                "Твоя задача — разгрузить товары на склад и записать количество в заметки." } },

        {"Why do we even need these notes?", new string[] { "Why do we even need these notes?", " А зачем вообще эти заметки?" } },

        {"Suppliers can make mistakes too - they might list items on paper but forget to load them.",
            new string[] { "Suppliers can make mistakes too - they might list items on paper but forget to load them.",
                "Поставщики тоже косячат — в бумагах указано, а грузить забывают." } },

        {"Enter the quantity, then hit the “Note” button.", new string[] { "Enter the quantity, then hit the “Note” button.", "Вводишь количество и жмёшь кнопку 'Внести'." } },
        {"Got it, I think.", new string[] { "Got it, I think.", "Кажется, понял.." } },

        {"Alright then, get moving - the truck’s already waiting in the yard!", 
            new string[] { "Alright then, get moving - the truck’s already waiting in the yard!", "Ну тогда вперёд — грузовик уже ждёт во дворе!" } },

        {"See you then!", new string[] { "See you then!", "Увидимся!" } },

        {"Looks like you did great!", new string[] { "Looks like you did great!", "Слушай, ты справился отлично!" } },
        {"I knew you’d pull it off.", new string[] { "I knew you’d pull it off.", "Я знал, что у тебя получится." } },
        {"Thanks.", new string[] { "Thanks.", "Спасибо." } },

        {"Take care of the last truck, then go get some rest. We’ll continue tomorrow!", 
            new string[] { "Take care of the last truck, then go get some rest. We’ll continue tomorrow!",
                "Разберись с последним грузовиком и отдыхай. Завтра продолжим!" } },

        {"I see it didn’t go without losses...", new string[] { "I see it didn’t go without losses...", "Вижу, не обошлось без потерь…" } },

        {"We all started like this. Take your time, and you’ll be a pro in no time!",
            new string[] { "We all started like this. Take your time, and you’ll be a pro in no time!", "У всех так в начале. Не спеши, скоро будешь профи!" } },

        {"Yeah...", new string[] { "Yeah...", "Ага..." } },
        {"See you.", new string[] { "See you.", "До связи." } },

        {"Almost forgot - before you leave, make sure to confirm it in the “Management” section. Just press the “Sign out” button.", 
            new string[] { "Almost forgot - before you leave, make sure to confirm it in the “Management” section. Just press the “Sign out” button..",
                "Чуть не забыл — перед уходом не забудь подтвердить это в программе 'Менеджмент'. Просто нажми кнопку 'Выйти'." } },

        {"Oh, and some dairy products will spoil overnight if they are not kept in the fridge. See you.",
            new string[] { "Oh, and some dairy products will spoil overnight if they are not kept in the fridge. See you.",
                "И да, молочные продукты испортятся за ночь, если не положить в холодильник. До встречи." } },

        {"I’m here. Ready to get started?", new string[] { "I’m here. Ready to get started?", "Я тут. Готов начинать?" } },
        {"Yes! Today we start to reap the rewards.", new string[] { "Yes! Today we start to reap the rewards.", "Да! Сегодня начнём пожинать плоды." } },
        {"First, check your mail in the “Management” app.", new string[] { "First, check your mail in the “Management” app.", "Сначала проверь почту в приложении 'Менеджмент'." } },

        {"Let’s do it!", new string[] { "Let’s do it!", "Поехали!" } },

        {"Whoa, that’s a lot of messages.", new string[] { "Whoa, that’s a lot of messages.", "Ого сколько писем." } },

        {"Keep in mind, the number of trucks is limited. Choose orders wisely.", 
            new string[] { "Keep in mind, the number of trucks is limited. Choose orders wisely.", "Имей в виду, грузовиков ограниченное количество. Выбирай заказы с умом." } },

        {"How long does delivery take?", new string[] { "How long does delivery take?", "Сколько занимает доставка?" } },

        {"All dispatched couriers will be available again the next day.", 
            new string[] { "All dispatched couriers will be available again the next day.", "Все отправленные курьеры будут снова доступны на следующий день." } },

        {"Now, here’s the deal. Your task is to accept an order and then load it onto a truck.", 
            new string[] { "Now, here’s the deal. Your task is to accept an order and then load it onto a truck.",
                "Теперь к делу. Твоя задача — принять заказ и загрузить его в грузовик." } },

        {"Like a delivery, in reverse.", new string[] { "Like a delivery, in reverse.", "Типа доставка наоборот." } },

        {"Exactly! Try to load fresh products - at least as much as was ordered.",
            new string[] { "Exactly! Try to load fresh products - at least as much as was ordered.", 
                "Именно! Постарайся загрузить свежие продукты — хотя бы столько, сколько заказали." } },

        {"Mistakes can affect your reputation. Ready to start?", new string[] { "Mistakes can affect your reputation. Ready to start?", "Ошибки могут повлиять на твою репутацию. Готов?" } },

        {"Good luck, Andy!", new string[] { "Good luck, Andy!", "Удачи, Энди!" } },

        {"Looks like I made my first money!", new string[] { "I made my first money!", "Кажется, я заработал деньги!" } },
        {"I knew you’d do great.", new string[] { "I knew you’d do great.", "Я знал, что у тебя получится." } },
        {"If you’ve got any questions, just message me!", new string[] { "If you’ve got any questions, just message me!", "Если будут вопросы — пиши!" } },
        {"About the money...", new string[] { "About the money...", "Насчёт денег..." } },

        {"Pay me back when you’re on your feet. No rush. We’ll stay in touch.", 
            new string[] { "Pay me back when you’re on your feet. No rush. We’ll stay in touch.", "Вернёшь, когда встанешь на ноги. Не спеши. Будем на связи." } },

        {"Load last save", new string[] { "Replay", "Переиграть" } },
        {"Losses:", new string[] { "Losses:", "Убытки:" } },
        {"Calculating losses...", new string[] { "Calculating losses...", "Подсчёт убытков..." } },
        {"Status: Significant loss.", new string[] { "Status: Significant loss.", "Статус: Существенные потери." } },
        {"Status: Minor loss.", new string[] { "Status: Minor loss.", "Статус: Незначительные потери." } },
        {"Status: No loss detected.", new string[] { "Status: No loss detected.", "Статус: Потерь не обнаружено." } },
        {"Hello, you recently sent goods to our store.", new string[] { "Hello, you recently sent goods to our store.", "Здравствуйте, вы недавно отправили нам товары." } },
        {"I noticed that some products were missing.", new string[] { "I noticed that some products were missing.", "Я заметил, что некоторых товаров не хватало." } },
        {"Some items were spoiled—this is unacceptable!", new string[] { "Some items were spoiled—this is unacceptable!", "Некоторые товары были испорчены — это неприемлемо!" } },
        {"The payment will reflect that.", new string[] { "The payment will reflect that.", "Оплата будет скорректирована." } },

        {"The quantity and quality are just right. Thank you!",
            new string[] { "The quantity and quality are just right. Thank you!", "Количество и качество в полном порядке. Спасибо!" } },

        {"Demo", new string[] { "Demo", "Демо" } },

        {"You've reached the end of the demo. Thank you for playing!", 
            new string[] { "You've reached the end of the demo. Thank you for playing!", "Вы дошли до конца демо-версии. Спасибо, что сыграли!" } },

        {"To show the clipboard, pick it up from the table when the truck arrives.",
            new string[] { "To show the clipboard, pick it up from the table when the truck arrives.", "Чтобы показать планшет, возьмите его со стола, когда приедет грузовик." } },
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
