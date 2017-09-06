using System;
using System.Collections.Generic;
using System.Text;

/*
    Володько В.И. 60331-1 Вариант 2 

    Предметная область: Вокзал. Касса вокзала имеет список тарифов на различные направления. При покупке билета регистрируются паспортные данные пассажира. Пассажир покупает билеты на различные направления. 
        Все пассажиры имеют скидки. У одних пассажиров скидка задана в процентах, у других скидка фиксированная.
        Добавить обработку исключительных ситуаций:
    •    длина строки наименования тарифа больше 10 символов.
    •    стоимость билета с учетом скидки меньше нуля.
    Добавить перегруженный бинарный  оператор для уменьшения стоимости тарифа.
    Система должна:
    •    позволять вводить данные о тарифах;
    •    позволять вводить паспортные данные пассажира и регистрировать покупку билета;
    •    рассчитать среднюю стоимость проданных билетов;
    •    по введенному наименованию направления высчитать сумму проданных билетов с учетом предоставленных скидок;

*/

namespace Control_Work
{
    /// <summary>
    /// Класс Билет
    /// </summary>
    public class Ticket : IEquatable<Ticket>
    {
        /// <summary>
        /// Констуктор класса Билет
        /// </summary>
        public Ticket(string name, string locationFrom, string locationTo, double cost)
        {
            Name = name;
            Cost = cost;
            LocationFrom = locationFrom;
            LocationTo = locationTo;
        }

        /// <summary>
        /// Направление
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Направление Откуда
        /// </summary>
        public string LocationFrom { get; set; }

        /// <summary>
        /// Направление Куда
        /// </summary>
        public string LocationTo { get; set; }

        /// <summary>
        /// Цена
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// Перегружаем бинарный оператор -
        /// </summary>
        public static Ticket operator -(Ticket obj1, int cost)
        {
            if (cost >= obj1.Cost) // если значение уменьшения стоимости тарифа >= тарифа
                throw new InvalidOperationException("Тариф не может быть меньше либо равно нулю"); // вызываем исключение

            obj1.Cost -= cost;

            return obj1;
        }

        /// <summary>
        /// Сравнение 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Ticket other)
        {
            if (this.Name == other.Name) // если имена совпадают
            {
                return true;
            }
            else // если имена не совпадают
            {
                return false;
            }
        }

        /// <summary>
        /// Вывод данных по билету
        /// </summary>
        public override string ToString()
        {
            return string.Format("{0,10} | {1,10} | {2,10} |{3,4}\n", Name,LocationFrom,LocationTo,Cost);
        }

    }

    /// <summary>
    /// Класс пассажир
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Конструктор класса Пассажир
        /// </summary>
        /// <param name="FIO">ФИО</param>
        /// <param name="seria">Серия паспорта</param>
        /// <param name="passportnumber">Номер паспорта</param>
        /// <param name="bpercentdiscount">Процентная скидка</param>
        /// <param name="discountvalue">Скидка</param>
        public Person(string FIO, string seria, long passportnumber,bool bpercentdiscount,double discountvalue)
        {
            this.FIO = FIO;
            this.seria = seria;
            this.passportnumber = passportnumber;
            this.bPercentDiscount = bpercentdiscount;
            this.discountValue = discountvalue;

        }

        /// <summary>
        /// Использование процентной скидки (true - процентная, false - фиксированая)
        /// </summary>
        public bool bPercentDiscount = false;

        /// <summary>
        /// Скидка
        /// </summary>
        public double discountValue;

        /// <summary>
        /// ФИО пассажира
        /// </summary>
        public string FIO;

        /// <summary>
        /// Серия паспорта
        /// </summary>
        public string seria;

        /// <summary>
        /// Номер пасспорта
        /// </summary>
        public long passportnumber;

        /// <summary>
        /// Список билетов пассажира
        /// </summary>
        public List<Ticket> Tickets = new List<Ticket>();

        /// <summary>
        /// Вывод информации о пассажире и приобретенных билетах
        /// </summary>
        public override string ToString()
        {
            string temp = string.Format("{0},{1}{2}\n", FIO, seria, passportnumber);

            temp += "Билеты:\n";

            foreach (Ticket item in Tickets) // Проходим по коллекции Tickets в цикле
            {
                temp+= item.ToString();
            }

            return temp;
        }

        /// <summary>
        /// Добавление билета с учетом скидки
        /// </summary>
        public bool AddTicket(Ticket ticket)
        {
            if (discountValue >= 0) // если скидка >= 0
            {
                if (bPercentDiscount) // используем процентную скидку
                {
                    if (discountValue <= 100) // если скидка <= 100%
                    {
                        ticket.Cost -= discountValue * ticket.Cost / 100;
                    }
                    else // если скидка > 100%
                    {
                        Console.WriteLine("Процентная скидка не может превышать больше 100%");
                        return false;
                    }
                }

                else // используем фиксированную скидку
                {
                    if ((ticket.Cost - discountValue) >= 0) // Разность цены билета и скидки >= 0
                    {
                        ticket.Cost -= discountValue;
                    }

                    else // Разность цены билета и скидки < 0
                    {
                        Console.WriteLine("Стоимость билета с учетом скидки меньше нуля \n Билет невозможно зарегистрировать");
                        return false;
                    }
                }

                Tickets.Add(ticket); // Добавление билета в коллекцию

                return true;
            }

            else // если скидка < 0
            {
                Console.WriteLine("Скидка не может быть меньше нуля");
            }

            return false;
        }
    }

    /// <summary>
    /// Класс Касса
    /// </summary>
    public class Kassa
    {

        /// <summary>
        /// Список пассажиров
        /// </summary>
        List<Person> PersonList = new List<Person>();

        /// <summary>
        /// Список тарифов
        /// </summary>
        List<Ticket> TarifList = new List<Ticket>();


        /// <summary>
        /// Добавить тариф
        /// </summary>
        /// <param name="cost">Цена</param>
        /// <param name="location">Направление</param>
        public void AddTarif(double cost,string locationFrom, string locationTo, string name)
        {
            if (cost > 0) // Стоимость > 0
            {
                if (name.Length < 10) // Количество символов < 10
                {
                    Ticket tarif = new Ticket(name, locationFrom, locationTo, cost);

                    if (!TarifList.Contains(tarif)) // если не содержит данного тарифа
                    {
                        TarifList.Add(tarif); // добавление тарифа в коллекцию
                    }

                    else // если данный тариф уже присутствует
                    {
                        Console.WriteLine("Тариф уже существует. Невозможно добавить тариф");
                    }
                }

                else  // Количество символов >= 10
                {
                    Console.WriteLine("Количество символов названия тарифа должно быть меньше 10. Невозможно добавить тариф");
                }
            }
            else // Стоимость <= 0
            {
                Console.WriteLine("Цена тарифа не может быть меньше либо равно нулю. Невозможно добавить тариф");
            }
        }

        /// <summary>
        /// Добавить пассажира
        /// </summary>
        /// <param name="fio">ФИО</param>
        /// <param name="seria">серия паспорта</param>
        /// <param name="passportnumber">номер паспорта</param>
        /// <param name="bpercentdiscount">процентная скидка</param>
        /// <param name="discountvalue">скидка</param>
        public Person AddPerson(string fio,string seria, long passportnumber,bool bpercentdiscount, int discountvalue)
        {
            Person person = new Person(fio, seria, passportnumber,bpercentdiscount,discountvalue);

            PersonList.Add(person); // Добавление пассажира в коллекцию

            return person;
        }

        /// <summary>
        /// Получить тариф по значению
        /// </summary>
        /// <param name="number">номер тарифа</param>
        /// <returns></returns>
        public Ticket GetTarif(int number)
        {
            return TarifList[number];
        }

        /// <summary>
        /// Получить сумму приобретенных билетов по направлению
        /// </summary>
        /// <param name="Location">Направление</param>
        public double GetSumTicket(string locationTo)
        {
            double sum = 0.0f;

            foreach (Person person in PersonList)
            {
                foreach(Ticket ticket in person.Tickets)
                {
                    if(ticket.LocationTo == locationTo) // если направления совпадают
                    {
                        sum += ticket.Cost; // Высчитываем сумму билетов
                    }
                }
            }

                return sum;
        }

        /// <summary>
        /// Расчет средней стоимости проданных билетов
        /// </summary>
        /// <returns>Средняя стоимость</returns>
        public double ShowAverangeCostTicket()
        {
            double sum = 0;
            double count = 0;

            foreach(Person person in PersonList)
            {
                foreach(Ticket ticket in person.Tickets)
                {
                    sum += ticket.Cost; // сумма проданных билетов
                    count++; // количество
                }
            }

            double avsum = 0.0f;

            if (count > 0) // если количество > 0
            {
                avsum = sum / count; // расчет средней стоимости
            }

            Console.WriteLine("Расчет средней стоимости проданных билетов {0}",avsum);

            return avsum;
        }

        /// <summary>
        /// Вывод списка тарифов и пассажирова с приобретенными билетами
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string temp = string.Empty;

            if (TarifList.Count > 0)  // если количество тарифов > 0
            {
                foreach (Ticket tarif in TarifList)
                {
                    temp+=tarif.ToString();
                }
            }

            else  // если количество тарифов <= 0
            {
                Console.WriteLine("Список тарифов пуст");
            }

            if (PersonList.Count > 0)  // если количество пассажиров > 0
            {
                foreach (Person person in PersonList)
                {
                    temp+=person.ToString();
                }
            }

            else  // если количество пассажиров <= 0
            {
                Console.WriteLine("Список пассажиров пуст");
            }


            return temp;
        }

        /// <summary>
        /// Вывод списка тарифов
        /// </summary>
        public void ShowListTarif()
        {
            int i = 0;

            if (TarifList.Count > 0) // если количество тарифов > 0
            {
                foreach (Ticket Item in TarifList)
                {
                    Console.Write(i.ToString()+" "+Item.ToString()); // Вывод определенного тарифа

                    i++; // счетчик
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Kassa kassa = new Kassa(); // Создание объекта класса Kassa

            while (true) // пока true выводим меню
            {
                int inputMenu = 0;

                Console.WriteLine();
                Console.WriteLine("МЕНЮ");
                Console.WriteLine("1 - Добавить тариф");
                Console.WriteLine("2 - Добавить пассажира и зарегистрировать билет");
                Console.WriteLine("3 - Рассчет средней стоимости проданных билетов");
                Console.WriteLine("4 - Расчет суммы проданных билетов с учетом предоставленных скидок по введенному наименованию направления");
                Console.WriteLine("5 - Уменьшение стоимости тарифа по номеру тарифа");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите пункт меню: ");

                try
                {
                    inputMenu = int.Parse(Console.ReadLine());

                    if (inputMenu == 1)   // если ввели 1
                    {
                        Console.WriteLine("Введите тариф");
                        Console.Write("Наименование:");
                        string name = Console.ReadLine();
                        Console.Write("Откуда:");
                        string locationFrom = Console.ReadLine();
                        Console.Write("Куда:");
                        string locationTo = Console.ReadLine();
                        Console.Write("Цена:");
                        double cost = double.Parse(Console.ReadLine());

                        kassa.AddTarif(cost,locationFrom,locationTo, name);


                        Console.WriteLine();
                        Console.WriteLine("______________________________________________________");
                    }

                    else if (inputMenu == 2)   // если ввели 2
                    {
                        Console.WriteLine("Введите пасспортные данные");
                        Console.Write("ФИО:");
                        string fio = Console.ReadLine();
                        Console.Write("Серия:");
                        string seria = Console.ReadLine();
                        Console.Write("Номер:");
                        long passportnumber = long.Parse(Console.ReadLine());

                        Console.Write("Скидка 0-фиксированная, 1-процентная:");

                        bool bpercentdiscount = false;
                        int discount = int.Parse(Console.ReadLine());

                        if(discount == 0) // если скидка фиксированная
                        {
                            bpercentdiscount = false; 
                        }

                        if (discount == 1)  // если скидка процентная
                        {
                            bpercentdiscount = true;
                        }

                        Console.Write("Скидка:");
                        int discountValue = int.Parse(Console.ReadLine());

                        Person person = kassa.AddPerson(fio, seria, passportnumber,bpercentdiscount,discountValue);

                        while (true) // пока true выводим подменю
                        {
                            int inputsubMenu = 0;

                            Console.WriteLine();
                            Console.WriteLine("Регистрация билета:");
                            Console.WriteLine("Выберите тариф:");

                            kassa.ShowListTarif(); // вывод списка тарифов

                            Console.WriteLine("для выхода из меню нажмите любой символ");

                            try
                            {
                                inputsubMenu = int.Parse(Console.ReadLine());

                                person.AddTicket(kassa.GetTarif(inputsubMenu));
                            }

                            catch (FormatException ex)
                            {
                                Console.WriteLine(ex.Message);
                                break;
                            }
                        }
                    }

                    else if (inputMenu == 3)  // если ввели 3
                    {

                        kassa.ShowAverangeCostTicket(); // вывод средней стомости проданных билетов
                    }

                    else if (inputMenu == 4)  // если ввели 4
                    {
                        Console.Write("Введите направление:");
                        string location = Console.ReadLine();

                        Console.WriteLine("Сумма билетов с направлением {0} - {1}",location,kassa.GetSumTicket(location));
                    }

                    else if (inputMenu == 5)  // если ввели 5
                    {

                        while (true) // пока true выводим подменю
                        {
                            kassa.ShowListTarif(); // выаод списка тарифов

                            int inputsubMenu = 0;

                            Console.WriteLine();
                            Console.WriteLine("Выберите тариф:");

                            Console.WriteLine("для выхода из меню нажмите любой символ");

                            try
                            {
                                inputsubMenu = int.Parse(Console.ReadLine()); 

                                Ticket ticket = kassa.GetTarif(inputsubMenu); // получение тарифа по номеру

                                Console.WriteLine("Уменьшение тарифа:");
                                int lowCost = int.Parse(Console.ReadLine());

                                ticket-=lowCost; // уменьшение тарифа
                            }

                            catch (FormatException ex)
                            {
                                Console.WriteLine(ex.Message);
                                break;
                            }
                        }
                    }

                    else if (inputMenu == 0) // если ввели 0
                    {
                        break;
                    }

                    else
                    {
                        Console.WriteLine("Введите правильный пункт меню");
                    }
                }
                catch (FormatException ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            Console.ReadKey(); // ждем нажатия клавиши
    }
    }
}
