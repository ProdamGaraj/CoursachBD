using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoursachBD
{
    class Program
    {
        static void PrintMenu()
        {
            Console.WriteLine(
                " 1.Найти все пьесы, в которых занят заданный актер.\n " +
                "2.Выдать афишу театра за заданный период. \n " +
                "3.Можно ли забронировать билеты на определенный спектакль. \n " +
                "4.Список стоимости билетов на указанный спектакль. \n " +
                "5.Программa спектакля \n " +
                "6.Регистрация нового поступления билетов для бронирования. \n " +
                "7.Ежедневное снятие с брони не выкупленных билетов \n " +
                "8.Регистрация заказа на бронирование. \n " +
                "9.Продажа заказанного билета. \n " +
                "0. Выход."
                );
        }
        static void Main(string[] args)
        {
            string input = "";
            
            bool exit = false;
            var context = new Tolkunov_TheatreEntities();
            Билет ticket = new Билет()
            {

            };
            Актер actor = new Актер()
            {
                ФИО = "",
                ЗаслуженныйАртистРФ = false
            };
            Роль role = new Роль()
            {
                Имя = "",
            };
            Пьеса play = new Пьеса()
            {
                Название = ""
            };
            Афиша playbill = new Афиша
            {

            };
            Сцена scene = new Сцена()
            {
                КоличествоМест = "0"
            };
            Театр theatre = new Театр
            {
                Название = ""
            };
            PrintMenu();
            while (!exit)
            {
                int inpt = int.Parse(Console.ReadLine());
                switch (inpt)
                {
                    case 0:
                        {
                            exit=true;
                            break;
                        }
                    case 1:
                        {
                            Console.Write("Введите ФИО актёра: ");
                            input = Console.ReadLine();
                            actor = context.Актер.FirstOrDefault(r => r.ФИО == input);
                            var roles = context.Роль.ToList();
                            var plays = context.Пьеса.ToList();
                            foreach (var r in roles)
                                if (r.Актер.Contains(actor))
                                {
                                    foreach (var p in plays)
                                    {
                                        if (p.Роль.Contains(r))
                                        {
                                            Console.WriteLine(p.Название);
                                        }
                                    }
                                }
                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("Вводите даты в формате год-месяц-день!");
                            Console.Write("Введите дату начала периода: ");
                            string[] dtargs = Console.ReadLine().Split('-', ' ', ':');
                            DateTime startDate = new DateTime(int.Parse(dtargs[0]), int.Parse(dtargs[1]), int.Parse(dtargs[2]));
                            Console.Write("Введите дату конца периода: ");
                            dtargs = Console.ReadLine().Split('-', ' ', ':');
                            DateTime endDate = new DateTime(int.Parse(dtargs[0]), int.Parse(dtargs[1]), int.Parse(dtargs[2]));
                            var playbills = context.Афиша.ToList();
                            foreach (var p in playbills)
                            {
                                if (p.ДатаВремя <= endDate && p.ДатаВремя >= startDate)
                                {
                                    Console.WriteLine(p.Место + ' ' + p.Пьеса.Название + ' ' + p.ДатаВремя.Value.Date);
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            Console.WriteLine("Введите название спектакля.");
                            input = Console.ReadLine();
                            int i = 0;
                            Console.WriteLine("Вводите даты в формате год-месяц-день!");
                            Console.Write("Введите текущую дату: ");
                            string[] dtargs = Console.ReadLine().Split('-', ' ', ':');
                            DateTime date = new DateTime(int.Parse(dtargs[0]), int.Parse(dtargs[1]), int.Parse(dtargs[2]));
                            var tickets = context.Билет.ToList();
                            Console.WriteLine("Доступные для бронирования билеты:");
                            foreach (var t in tickets)
                            {
                                if (t.Пьеса.Название == input && t.Забронирован == false && t.ДатаВремяПремьеры.Value.AddDays(-1) != date)
                                {
                                    Console.WriteLine(t.Пьеса.Название + " " + t.Место + " " + t.Стоимость);
                                    i++;
                                }
                            }
                            if (i == 0)
                            {
                                Console.WriteLine("Нет доступных для бронирования билетов");
                            }
                            break;
                        }
                    case 4:
                        {
                            Console.Write("Введите название спектакля: ");
                            input = Console.ReadLine();
                            play = context.Пьеса.FirstOrDefault(r => r.Название == input);
                            var tickets = context.Билет.ToList();
                            foreach (var t in tickets)
                            {
                                if (t.Пьеса == play)
                                {
                                    Console.WriteLine(t.Стоимость);
                                }
                            }
                            break;
                        }
                    case 5:
                        {
                            Console.Write("Введите название спектакля: ");
                            input = Console.ReadLine();
                            play = context.Пьеса.FirstOrDefault(r => r.Название == input);
                            var roles = context.Роль.ToList();
                            foreach (var r in roles)
                            {
                                if (r.Пьеса == play)
                                {
                                    Console.WriteLine(r.Имя + " - " + r.Актер.FirstOrDefault().ФИО);
                                }
                            }
                            break;
                        }
                    case 6:
                        {
                            ticket.Забронирован = false;
                            ticket.Выкуплен = false;
                            Console.WriteLine("Введите данные билетов.");
                            Console.Write("Название спектакля: ");
                            input = Console.ReadLine();
                            ticket.Пьеса = context.Пьеса.FirstOrDefault(r => r.Название == input);
                            Console.Write("Места с: ");
                            var startPlace = int.Parse(Console.ReadLine());
                            Console.Write("Места до: ");
                            var endPlace = int.Parse(Console.ReadLine());
                            Console.WriteLine("Вводите даты в формате год - месяц - день - час - минута!");
                            Console.Write("Дата и время премьеры: ");
                            string[] dtargs = Console.ReadLine().Split('-', ' ', ':');
                            ticket.ДатаВремяПремьеры = new DateTime(int.Parse(dtargs[0]), int.Parse(dtargs[1]), int.Parse(dtargs[2]), int.Parse(dtargs[3]), int.Parse(dtargs[4]), 0);
                            for (int i = startPlace; i <= endPlace; i++)
                            {
                                Console.Write("Стоимость: ");
                                ticket.Стоимость = decimal.Parse(Console.ReadLine());
                                ticket.Место = i;
                                context.Билет.Add(ticket);
                                context.SaveChanges();
                            }
                            break;
                        }
                    case 7:
                        {
                            Console.WriteLine("Вводите даты в формате год-месяц-день!");
                            Console.Write("Введите текущую дату: ");
                            string[] dtargs = Console.ReadLine().Split('-', ' ', ':');
                            DateTime date = new DateTime(int.Parse(dtargs[0]), int.Parse(dtargs[1]), int.Parse(dtargs[2]));
                            var tickets = context.Билет.ToList();
                            Console.WriteLine("Список изменённых билетов: ");
                            foreach (var t in tickets)
                            {
                                if (t.ДатаВремяПремьеры.Value.AddDays(-1) == date && t.Выкуплен == false && t.Забронирован != false)
                                {
                                    Console.WriteLine(t.Пьеса.Название + " " + t.Место + " " + t.Забронирован);
                                    t.Забронирован = false;
                                    context.SaveChanges();
                                    Console.WriteLine(t.Пьеса.Название + " " + t.Место + " " + t.Забронирован);
                                    Console.WriteLine();
                                }
                            }
                            break;
                        }
                    case 8:
                        {
                            Console.WriteLine("Введите данные бронируемого билета.");
                            Console.Write("Название спектакля: ");
                            input = Console.ReadLine();
                            Console.Write("Номер места: ");
                            var place = int.Parse(Console.ReadLine());
                            ticket = context.Билет.FirstOrDefault(r => r.Пьеса.Название == input && r.Место == place);
                            Console.WriteLine(ticket.Пьеса.Название + " " + ticket.Место + " " + ticket.Забронирован);
                            ticket.Забронирован = true;
                            context.SaveChanges();
                            Console.WriteLine(ticket.Пьеса.Название + " " + ticket.Место + " " + ticket.Забронирован);
                            break;
                        }
                    case 9:
                        {
                            Console.WriteLine("Введите данные бронируемого билета.");
                            Console.Write("Название спектакля: ");
                            input = Console.ReadLine();
                            Console.Write("Номер места: ");
                            var place = int.Parse(Console.ReadLine());
                            ticket = context.Билет.FirstOrDefault(r => r.Пьеса.Название == input && r.Место == place);
                            if (ticket.Забронирован == false)
                            {
                                Console.WriteLine("Данный билет не был забронирован.");
                                break;
                            }
                            else if (ticket.Выкуплен == true)
                            {
                                Console.WriteLine("Данный билет уже куплен.");
                                break;
                            }
                            Console.WriteLine(ticket.Пьеса.Название + " " + ticket.Место + " " + ticket.Выкуплен);
                            ticket.Выкуплен = true;
                            context.SaveChanges();
                            Console.WriteLine(ticket.Пьеса.Название + " " + ticket.Место + " " + ticket.Выкуплен);
                            break;
                        }
                }
            }
        }
    }
}
