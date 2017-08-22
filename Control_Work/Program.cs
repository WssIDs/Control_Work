using System;
using System.Collections.Generic;
using System.Text;

namespace Control_Work
{

    public class Ticket : IEquatable<Ticket>
    {

        public Ticket(string location, int cost)
        {
            Location = location;
            Cost = cost;
        }


        public string Location { get; set; }

        public int Cost { get; set; }

        public bool Equals(Ticket other)
        {
            if (this.Location == other.Location)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override string ToString()
        {
            return string.Format("Направление:{0} - Цнеа:{1} ", Location, Cost);
        }

    }


    public class Person
    {
        public Person(string FIO, string seria,long passportnumber, DateTime dateIn, DateTime dateOut)
        {
            this.FIO = FIO;
            this.seria = seria;
            this.passportnumber = passportnumber;
            this.dateIn = dateIn;
            this.dateOut = dateOut;
        }

        public Person(string FIO, string seria, long passportnumber)
        {
            this.FIO = FIO;
            this.seria = seria;
            this.passportnumber = passportnumber;
        }

        public Person(string FIO, string seria, long passportnumber,bool bpercentdiscount,int discountvalue)
        {
            this.FIO = FIO;
            this.seria = seria;
            this.passportnumber = passportnumber;
            this.bPercentDiscount = bpercentdiscount;
            this.discountValue = discountvalue;

        }

        public bool bPercentDiscount = false;

        public int discountValue;

        public string FIO;

        public string seria;
        public long passportnumber;

        DateTime dateIn;
        DateTime dateOut;

        public List<Ticket> Tickets = new List<Ticket>();

        public void ShowInfoTicket()
        {
            Console.WriteLine("Билеты:");

            foreach (Ticket item in Tickets)
            {
                Console.WriteLine("{0} {1}", item.Location, item.Cost);
            }

            Console.WriteLine();
        }

        public bool AddTicket(string location, int cost)
        {
            if (discountValue >= 0)
            {
                if (bPercentDiscount)
                {
                    cost -= cost / discountValue;
                }

                else
                {
                    if ((cost - discountValue) >= 0)
                    {
                        cost -= discountValue;
                    }

                    else
                    {
                        Console.WriteLine("Стоимость билета с учетом скидки меньше нуля \n Билет невозможно зарегистрировать");
                        return false;
                    }
                }

                Ticket ticket = new Ticket(location, cost);

                Tickets.Add(ticket);

                return true;
            }

            return false;
        }
    }

    public class Kassa
    {
        public Kassa()
        {

        }

        List<Person> PersonList = new List<Person>();

        List<Ticket> TarifList = new List<Ticket>();


        public void AddTarif(int cost, string location)
        {
            Ticket tarif = new Ticket(location, cost);

            if (!TarifList.Contains(tarif))
            {
                TarifList.Add(tarif);
            }
        }

        public Person AddPerson(string fio,string seria, long passportnumber,bool bpercentdiscount, int discountvalue)
        {
            Person person = new Person(fio, seria, passportnumber,bpercentdiscount,discountvalue);

            PersonList.Add(person);

            return person;
        }

        public Ticket GetTarif(int number)
        {
            return TarifList[number];
        }

        public void ShowAverangeCostTicket()
        {
            int sum = 0;
            int count = 0;

            foreach(Person person in PersonList)
            {
                foreach(Ticket ticket in person.Tickets)
                {
                    sum += ticket.Cost;
                    count++;
                }
            }

            double avsum = 0.0f;

            if (count > 0)
            {
                avsum = sum / count;
            }

            Console.WriteLine("Расчет средней стоимости проданных билетов {0}",avsum);
        }

        public void ShowAllKassaInfo()
        {
            if (TarifList.Count > 0)
            {
                foreach (Ticket tarif in TarifList)
                {
                    Console.WriteLine("{0} {1}", tarif.Location, tarif.Cost);
                }
            }

            else
            {
                Console.WriteLine("Список тарифов пуст");
            }

            if (PersonList.Count > 0)
            {
                foreach (Person person in PersonList)
                {
                    Console.WriteLine("{0},{1}{2}", person.FIO, person.seria, person.passportnumber);
                    person.ShowInfoTicket();
                }
            }

            else
            {
                Console.WriteLine("Список пассажиров пуст");
            }
        }

        public void ShowListTarif()
        {
            int i = 0;

            if (TarifList.Count > 0)
            {
                foreach (var Item in TarifList)
                {
                    Console.WriteLine("{0}: {1} - {2}",i,Item.Location, Item.Cost);

                    i++;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Kassa kassa = new Kassa();

            while (true)
            {
                int inputMenu = 0;

                Console.WriteLine();
                Console.WriteLine("МЕНЮ");
                Console.WriteLine("1 - Добавить тариф");
                Console.WriteLine("2 - Добавить пассажира и загеристрировать билет");
                Console.WriteLine("3 - Рассчет средней стоимости проданных билетов");
                Console.WriteLine("4 - Вывести всю информацию по кассе");
                Console.WriteLine("0 - Выход");
                Console.Write("Выберите пункт меню: ");

                try
                {
                    inputMenu = int.Parse(Console.ReadLine());

                    if (inputMenu == 1)
                    {
                        Console.WriteLine("Введите тариф");
                        Console.Write("Направление:");
                        string location = Console.ReadLine();
                        Console.Write("Цена:");
                        int cost = int.Parse(Console.ReadLine());

                        kassa.AddTarif(cost, location);


                        Console.WriteLine();
                        Console.WriteLine("______________________________________________________");
                    }

                    else if (inputMenu == 2)
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

                        if(discount == 0)
                        {
                            bpercentdiscount = false; 
                        }

                        if (discount == 1)
                        {
                            bpercentdiscount = true;
                        }

                        Console.Write("Скидка:");
                        int discountValue = int.Parse(Console.ReadLine());

                        Person person = kassa.AddPerson(fio, seria, passportnumber,bpercentdiscount,discountValue);

                        while (true)
                        {
                            int inputsubMenu = 0;

                            Console.WriteLine();
                            Console.WriteLine("Регистрация билета:");
                            Console.WriteLine("Выберите направление:");

                            kassa.ShowListTarif();

                            Console.WriteLine("для выхода из меню нажмите любой символ");

                            try
                            {
                                inputsubMenu = int.Parse(Console.ReadLine());
                                person.AddTicket(kassa.GetTarif(inputsubMenu).Location, kassa.GetTarif(inputsubMenu).Cost);
                            }

                            catch (FormatException ex)
                            {
                                Console.WriteLine(ex.Message);
                                break;
                            }
                        }
                    }

                    else if (inputMenu == 3)
                    {
                        kassa.ShowAverangeCostTicket();
                    }

                    else if (inputMenu == 4)
                    {
                        kassa.ShowAllKassaInfo();
                    }

                    else if (inputMenu == 0)
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

            Console.ReadKey();
    }
    }
}
