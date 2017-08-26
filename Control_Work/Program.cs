using System;
using System.Collections.Generic;
using System.Text;

namespace Control_Work
{

    public class Ticket : IEquatable<Ticket>
    {

        public Ticket(string location, double cost)
        {
            Location = location;
            Cost = cost;
        }


        public string Location { get; set; }

        public double Cost { get; set; }

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
            return string.Format("{0,10} |{1,4}\n", Location, Cost);
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

        public Person(string FIO, string seria, long passportnumber,bool bpercentdiscount,double discountvalue)
        {
            this.FIO = FIO;
            this.seria = seria;
            this.passportnumber = passportnumber;
            this.bPercentDiscount = bpercentdiscount;
            this.discountValue = discountvalue;

        }

        public bool bPercentDiscount = false;

        public double discountValue;

        public string FIO;

        public string seria;
        public long passportnumber;

        DateTime dateIn;
        DateTime dateOut;

        public List<Ticket> Tickets = new List<Ticket>();

        public override string ToString()
        {
            string temp = string.Format("{0},{1}{2}\n", FIO, seria, passportnumber);

            temp += "Билеты:\n";

            foreach (Ticket item in Tickets)
            {
                temp+= item.ToString();
            }

            return temp;
        }

        public bool AddTicket(string location, double cost)
        {
            if (discountValue >= 0)
            {
                if (bPercentDiscount)
                {
                    cost -= discountValue*cost/100;
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

        List<Person> PersonList = new List<Person>();

        List<Ticket> TarifList = new List<Ticket>();


        public void AddTarif(double cost, string location)
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

        public double GetSumTicket(string Location)
        {
            double sum = 0.0f;

            foreach (Person person in PersonList)
            {
                foreach(Ticket ticket in person.Tickets)
                {
                    if(ticket.Location == Location)
                    {
                        sum += ticket.Cost;
                    }
                }
            }

                return sum;
        }

        public double ShowAverangeCostTicket()
        {
            double sum = 0;
            double count = 0;

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

            return avsum;
        }

        public override string ToString()
        {
            string temp = string.Empty;

            if (TarifList.Count > 0)
            {
                foreach (Ticket tarif in TarifList)
                {
                    temp+=tarif.ToString();
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
                    temp+=person.ToString();
                }
            }

            else
            {
                Console.WriteLine("Список пассажиров пуст");
            }


            return temp;
        }

        public void ShowListTarif()
        {
            int i = 0;

            if (TarifList.Count > 0)
            {
                foreach (var Item in TarifList)
                {
                    Console.Write(i.ToString()+" "+Item.ToString());

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
                Console.WriteLine("4 - Расчет суммы проданных билетов с учетом предоставленных скидок по введенному наименованию направления");
                Console.WriteLine("5 - Вывести всю информацию по кассе");
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
                        double cost = double.Parse(Console.ReadLine());

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
                        Console.Write("Введите направление:");
                        string location = Console.ReadLine();

                        Console.WriteLine("Сумма билетов с направлением {0} - {1}",location,kassa.GetSumTicket(location));
                    }

                    else if (inputMenu == 5)
                    {
                        Console.WriteLine(kassa.ToString());
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
