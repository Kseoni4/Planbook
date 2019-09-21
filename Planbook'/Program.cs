using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Задание

//Разработать ежедневник.
/// В ежедневнике реализовать возможность 
/// - создания
/// - удаления
/// - реактирования
/// записей
/// 
/// В отдельной записи должно быть не менее пяти полей
/// 
/// Реализовать возможность 
/// - Загрузки даннах из файла
/// - Выгрузки даннах в файл
/// - Добавления данных в текущий ежедневник из выбранного файла
/// - Импорт записей по выбранному диапазону дат
/// - Упорядочивания записей ежедневника по выбранному полю

#endregion

#region архитектура программы
/*
 * Ежедневник (struct Planbook)
 * |
 * |
 * |____Запись (struct Note)
 * |    |
 * |    |
 * |    |___Название_игры (string)
 * |    |
 * |    |___Жанр (string)
 * |    |
 * |    |___Платформы (string)
 * |    |
 * |    |___Пройдена (string)
 * |    |
 * |    |___Дата выхода (DateTime)
 * |    |
 * |    |___Конструктор записи
 * |
 * |____Владелец (string)
 * |
 * |____Дата создания (DateTime)
 * |
 * |____Конструктор ежедневника
 * |
 * |____Метод загрузки данных из файла
 * |
 * |____Метод сохранения данных в файла
 * |
 * |____Метод создания записи
 * |
 * |____Метод редактирования записи
 * |
 * |____Метод загрузки по диапазону дат
 * |
 * |____Метод сортировки по полю
 * 
 */



#endregion


namespace Homework_07
{

    class Program
    {
        static Planbook[] pblist;
        static int ind = 0;
        static string pblistcsv = @"pbs.csv";
        static string Path = $@"";

        static void Resize(bool Flag)
        {
            if (Flag)
            {
                Array.Resize(ref pblist, chkLng() + 1);
            }
        }

        static int lng = 0;

        static void addToList()
        {
            string data = String.Format("{0} - {1} - {2} - {3}",
                                        ind,
                                        pblist[ind].Owner,
                                        pblist[ind].MakeDate,
                                        Path.ToString());
            File.AppendAllText(pblistcsv, $"{data}\n");
        }

        static void CreateNoteManual(Planbook pb)
        {
            Console.Clear();

            Console.Write("Введите название игры: ");

            string title = Console.ReadLine();

            Console.Write("Введите жанр игры: ");

            string genre = Console.ReadLine();

            Console.Write("Введите платформы: ");

            string platforms = Console.ReadLine();

            Console.Write("Пройденна ли игра? [Y/N]: ");

            bool complete = Console.ReadLine() == "Y" ? true : false;

            Console.Write("Введите дату выхода игры (DD-MM-YYYY): ");

            DateTime date = DateTime.Parse(Console.ReadLine());

            pb.AddNote(new Note(title, genre, platforms, complete, date));

            Console.WriteLine($"Запись игры {title} создана.");
        }

        static void EditNoteManual(int idx, int i)
        {
            Console.Write("Выберите запись: ");
        }

        static int chkLng()
        {
            try
            {
                return pblist.Length;
            }
            catch(NullReferenceException)
            {
                return 0;
            }
        }

        static void MainMenu()
        {
            bool q = true;
            byte c = 0;

            while (q)
            {

                Console.WriteLine("Игровой ежедневник пройденных игр Ten Games Planbook");
                Console.WriteLine($"Созданных ежедневников {chkLng()}");
                Console.WriteLine("[1] Создать новый ежедневник. \n" +
                                  "[2] Загрузить имеющийся. \n" +
                                  "[3] Выйти из программы.");
                Console.Write("Выбор действия: ");
                c = byte.Parse(Console.ReadLine());

                switch (c)
                {
                    case 1:
                        {
                            Console.Write("Введите имя владельца: ");

                            string owner = Console.ReadLine();
                            Path = $@"{owner}.csv";

                            Resize(ind >= chkLng());

                            pblist[ind] = new Planbook(owner, DateTime.Now, Path);

                            addToList();

                            ind++;

                            break;
                        }
                    case 2:
                        {
                            try
                            {
                                using (StreamReader sr = new StreamReader(pblistcsv))
                                {
                                    byte indP = 0;
                                    while (!sr.EndOfStream)
                                    {
                                        string[] pbls = sr.ReadLine().Split(" - ");

                                        Resize(int.Parse(pbls[0]) >= chkLng());

                                        pblist[int.Parse(pbls[0])] = new Planbook(pbls[1], Convert.ToDateTime(pbls[2]), pbls[3]);
                                    }
                                    for (int i = 0; i < pblist.Length; i++)
                                    {
                                        Console.WriteLine($"{i} {pblist[i].Owner} {pblist[i].MakeDate}");
                                    }
                                    Console.Write("Выберите номер ежедневника: ");

                                    indP = byte.Parse(Console.ReadLine());

                                    PBMenu(pblist[indP], indP);

                                }
                            }
                            catch (FileNotFoundException)
                            {
                                Console.WriteLine("Ежедневников не найдено, создайте свой первый!");
                            }
                            break;
                        }
                    case 3:
                        {
                            q = false;
                            break;
                        }
                }

            }
        }

        static void PBMenu(Planbook pb, int idx)
        {
            bool pbQ = true;
            Planbook pbc = pb;
            while(pbQ)
            {
                Console.Clear();
                Console.WriteLine($"Меню ежедневника владельца {pb.Owner}. Записей {pb.Count}");
                Console.WriteLine("[1] Добавить запись.\n" +
                                  "[2] Изменить запись.\n" +
                                  "[3] Удалить запись.\n" +
                                  "[4] Загрузить записи.\n" +
                                  "[5] Загрузить записи по дате.\n" +
                                  "[6] Сортировка записей по полю.\n" +
                                  "[7] Вывести записи.\n" +
                                  "[8] Выйти в главное меню.\n");
                Console.Write("Выберите действие: ");
                switch (int.Parse(Console.ReadLine()))
                {
                    case 1:
                        {
                            CreateNoteManual(pbc);
                            break;
                        }
                    case 2:
                        {
                            pb.PrintNotes();
                            EditNoteManual(idx, 0);
                            break;
                        }
                    case 3:
                        {
                            pb.PrintNotes();
                            pb.DeleteNote();
                            break;
                        }
                    case 4:
                        {
                            pb.LoadNotes();
                            break;
                        }
                    case 5:
                        {
                            pb.LoadNotesFromDates();
                            break;
                        }
                    case 6:
                        {
                            pb.SortNote();
                            break;
                        }
                    case 7:
                        {
                            pb.PrintNotes();
                            break;
                        }
                    case 8:
                        {
                            pbQ = false;
                            break;
                        }
                }
            }
        }

        static void Main(string[] args)
        {
            MainMenu();    
        Console.ReadLine();

        }
    }
}
