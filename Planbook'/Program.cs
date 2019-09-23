using System;
using System.IO;

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
 * |____Метод загрузки из определённого файла
 * |
 * |____Метод сортировки по полю
 * |
 * |____Куча служебных методов
 * 
 */



#endregion


namespace Homework_07
{

    class Program
    {
        #region Основные переменные программы

        static Planbook[] pblist;

        static string pblistcsv = @"pbs.csv";

        static string Path = $@"";

        static int ind = ChkCount();

        #endregion

        #region Главное меню и интерфейс меню ежедневника

        static void MainMenu()
        {
            bool q = true;
            byte c = 0;
            while (q)
            {
                DrawMenu();

                c = byte.Parse(Read());

                switch (c)
                {
                    case 1:
                        {
                            CreatePlanBook();
                            break;
                        }
                    case 2:
                        {
                            LoadPlanBook();
                            break;
                        }
                    case 3:
                        {
                            q = false;
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            break;
                        }
                }

            }
        }

        static void PBMenu(int idx)
        {
            bool pbQ = true;
            while (pbQ)
            {
                DrawPBMenu(idx);
                switch (int.Parse(Read()))
                {
                    case 1:
                        {
                            CreateNoteManual();
                            break;
                        }
                    case 2:
                        {
                            pblist[idx].PrintNotes();
                            EditNoteManual(idx);
                            break;
                        }
                    case 3:
                        {
                            pblist[idx].PrintNotes();
                            DeleteNoteManual(idx);
                            break;
                        }
                    case 4:
                        {
                            pblist[idx].LoadNotes($@"{pblist[idx].Owner}-notes.csv");
                            break;
                        }
                    case 5:
                        {
                            pblist[idx].LoadNotesFromDates($@"{pblist[idx].Owner}-notes.csv");
                            break;
                        }
                    case 6:
                        {
                            Console.Write("Введите путь к файлу или его название: ");
                            pblist[idx].LoadNotesFromFile($@"{Read()}");
                            break;
                        }
                    case 7:
                        {
                            Console.Write($"Выберите поле для сортировки (1.Название, 2.Жанр, 3.Платформы, 4.Пройдена, 5.Дата выхода): ");
                            pblist[idx].SortNote(int.Parse(Read()));
                            break;

                        }
                    case 8:
                        {
                            pblist[idx].PrintNotes();
                            Console.ReadLine();
                            break;
                        }
                    case 9:
                        {
                            pbQ = false;
                            pblist[idx].Save();
                            break;
                        }
                }
            }
        }

        #endregion

        #region Создание ежедневника и работа с заметками

        static void CreatePlanBook()
        {
            Console.Write("Введите имя владельца: ");

            string owner = Console.ReadLine();
            Path = $@"{owner}.csv";

            Resize(ind >= ChkLng());

            pblist[ind] = new Planbook(owner, DateTime.Now, Path);

            addToList();

            ind++;
        }

        static void CreateNoteManual()
        {
            Console.Clear();

            Console.Write("Введите название игры: ");

            string title = Read();

            Console.Write("Введите жанр игры: ");

            string genre = Read();

            Console.Write("Введите платформы: ");

            string platforms = Read();

            Console.Write("Пройденна ли игра? [Y/N]: ");

            bool complete = Read() == "Y" ? true : false;

            Console.Write("Введите дату выхода игры (DD-MM-YYYY): ");

            DateTime date = DateTime.Parse(Read());

            pblist[ind].AddNote(new Note(title, genre, platforms, complete, date), true);

            Console.WriteLine($"Запись игры {title} создана.");
        }

        static void EditNoteManual(int idx)
        {
            Console.Write("Выберите запись: ");
            int ind = int.Parse(Read()) - 1;
            pblist[idx].PrintNote(ind, false);
            Console.Write("Выберите поле для редактирования (от 1 до 5): ");
            pblist[idx].EditNote(int.Parse(Read()), ind);
            pblist[idx].PrintNote(ind, true);
            Console.ReadLine();
        }

        static void DeleteNoteManual(int i)
        {
            Console.Write("Выберите номер записи для удаления: ");
            pblist[i].DeleteNote(int.Parse(Read()));
        }

        static void LoadPlanBook()
        {
            try
            {
                using (StreamReader sr = new StreamReader(pblistcsv))
                {
                    byte indP = 0;
                    while (!sr.EndOfStream)
                    {
                        string[] pbls = sr.ReadLine().Split(" - ");

                        Resize(int.Parse(pbls[0]) >= ChkLng());

                        pblist[int.Parse(pbls[0])] = new Planbook(pbls[1], Convert.ToDateTime(pbls[2]), pbls[3]);
                    }
                    for (int i = 0; i < pblist.Length; i++)
                    {
                        Console.WriteLine($"{i + 1} - {pblist[i].Owner} - {pblist[i].MakeDate.ToShortDateString()}");
                    }
                    Console.Write("Выберите номер ежедневника: ");

                    indP = byte.Parse(Console.ReadLine());

                    PBMenu(indP - 1);

                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Ежедневников не найдено, создайте свой первый!");
            }
        }

        #endregion

        #region Служебные методы проверки ввода, добавления в файл и переопределение размера массива

        static void Resize(bool Flag)
        {
            if (Flag)
            {
                Array.Resize(ref pblist, ChkLng() + 1);
            }
        }

        static int ChkCount()
        {
            try
            {
                if (File.Exists(pblistcsv))
                {
                    using (StreamReader sr = new StreamReader(pblistcsv))
                    {
                        int count = 0;
                        while (!sr.EndOfStream)
                        {
                            sr.ReadLine();
                            count++;
                        }
                        return count;
                    }
                }
                else
                {
                    return 0;
                }
            }
            catch(NullReferenceException)
            {
                return 0;
            }
        }

        static int ChkLng()
        {
            try
            {
                return pblist.Length;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }

        static string Read()
        {
            try
            {
                return Console.ReadLine();
            }
            catch (FormatException)
            {
                return "0";
            }
        }

        static void addToList()
        {
            string data = String.Format("{0} - {1} - {2} - {3}",
                                        ind,
                                        pblist[ind].Owner,
                                        pblist[ind].MakeDate,
                                        Path.ToString());
            File.AppendAllText(pblistcsv, $"{data} \n");
        }

        static void DrawMenu()
        {
            Console.Clear();
            DrawWhitePanel();
            Console.Write("Игровой ежедневник пройденных игр ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("Ten Games Planbook\n");
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine($"Созданных ежедневников {ChkCount()}");
            Console.WriteLine("[1] Создать новый ежедневник. \n" +
                              "[2] Загрузить имеющийся. \n" +
                              "[3] Выйти из программы.");
            Console.Write("Выбор действия: ");
        }

        static void DrawPBMenu(int idx)
        {
            Console.Clear();
            DrawWhitePanel();
            Console.Write("Меню ежедневника владельца ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{pblist[idx].Owner}. ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("Записей ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{pblist[idx].Count}\n");
            Console.ResetColor();
            Console.WriteLine("[1] Добавить запись.\n" +
                              "[2] Изменить запись.\n" +
                              "[3] Удалить запись.\n" +
                              "[4] Загрузить записи.\n" +
                              "[5] Загрузить записи по дате.\n" +
                              "[6] Загрузить записи из другого файла.\n" +
                              "[7] Сортировка записей по полю.\n" +
                              "[8] Вывести записи.\n" +
                              "[9] Выйти в главное меню.\n");
            Console.Write("Выберите действие: ");
        }

        static void DrawWhitePanel()
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
        }

        #endregion

        static void Main(string[] args)
        {
        MainMenu();    
        Console.ReadLine();

        }
    }
}
