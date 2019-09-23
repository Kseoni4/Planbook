using System;
using System.IO;
using System.Collections.Generic;

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
    struct Planbook
    {

        #region Поля структуры ежедневника

        public string Owner { get; set; }

        public DateTime MakeDate { get; set; }

        private int index { get; set; }

        private string path;

        private Note[] Notes;

        #endregion

        #region Работа с заметками

        public void AddNote(Note _Note, bool flag)
        {
            this.Resize(index >= this.Notes.Length);
            this.Notes[index] = _Note;
            if (flag)
            {
                SaveNote();
            }
            index++;
            SavePB();
        }

        public void SaveNote()
        {
            string Path = $@"{Owner}-notes.csv";
            string temp = String.Format("{0},{1},{2},{3},{4}",
                                    this.Notes[index].GameTitle,
                                    this.Notes[index].Genre,
                                    this.Notes[index].Platforms,
                                    this.Notes[index].Complete,
                                    this.Notes[index].Release
                                    );
            File.AppendAllText(Path, $"{temp}\n");
        }

        public void EditNote(int c, int i)
        {
            string s = "Введите новое значение: ";
            Console.Write(s);
            switch (c)
            {
                case 1:
                    {
                        Notes[i].GameTitle = Console.ReadLine();
                        break;
                    }
                case 2:
                    {
                        Notes[i].Genre = Console.ReadLine();
                        break;
                    }
                case 3:
                    {
                        Notes[i].Platforms = Console.ReadLine();
                        break;
                    }
                case 4:
                    {
                        Notes[i].Complete = (Console.ReadLine() == "Y" ? true : false);
                        break;
                    }
                case 5:
                    {
                        Notes[i].Release = Convert.ToDateTime(Console.ReadLine());
                        break;
                    }
            }
            ResaveAllNotes();
        }

        public void SortNote(int c)
        {
            switch (c)
            {
                case 1:
                    {
                        Array.Sort(Notes, new SortBy().Compare);
                        break;
                    }
                case 2:
                    {
                        Array.Sort(Notes, new SortBy().CompareG);
                        break;
                    }
                case 3:
                    {
                        Array.Sort(Notes, new SortBy().CompareP);
                        break;
                    }
                case 4:
                    {
                        Array.Sort(Notes, new SortBy().CompareC);
                        break;
                    }
                case 5:
                    {
                        Array.Sort(Notes, new SortBy().CompareR);
                        break;
                    }
            }
        }

        public void DeleteNote(int idx)
        {
            Array.Clear(Notes, idx - 1, 1);
            index--;
            ResaveAllNotes();
        }

        #endregion

        #region Печать заметок
        public void PrintNotes()
        {
            if(index > 0)
            {
                DrawTitles();

                for (int i = 0; i < index; i++)
                {
                    if(Notes[i].GameTitle != null && Notes[i].GameTitle != "")
                    {
                        Console.Write($"{i + 1, 3}");
                        Notes[i].Print();
                    }
                }
            }
            else
            {
                Console.WriteLine("Записи отсутствуют");
            }
        }

        public void PrintNote(int i, bool flag)
        {
            Console.Clear();

            if (flag) { Console.WriteLine("Изменённая запись:"); }

            DrawTitles();
            Notes[i].Print();
        }

        #endregion

        #region Загрузка заметок

        public void LoadNotes(string pathNotes)
        {

            ClearNotes();

            using (StreamReader sr = new StreamReader(pathNotes))
            {
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(',');

                    AddNote(new Note(line[0], line[1], line[2], Convert.ToBoolean(line[3]), Convert.ToDateTime(line[4])), false);
                }
            }
        }

        public void LoadNotesFromDates(string pathNotes)
        {
            Console.Write("Введите диапазон дат через пробел (DD-MM-YYYY): ");
            string[] dates = Console.ReadLine().Split(' ');

            ClearNotes();

            using (StreamReader sr = new StreamReader(pathNotes))
            {
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(',');

                    if (Convert.ToDateTime(line[4]) >= Convert.ToDateTime(dates[0])
                       && Convert.ToDateTime(line[4]) <= Convert.ToDateTime(dates[1]))
                    {
                        AddNote(new Note(line[0], line[1], line[2], Convert.ToBoolean(line[3]), Convert.ToDateTime(line[4])), false);
                    }
                }
            }

        }

        public void LoadNotesFromFile(string pathNotes)
        {
            using (StreamReader sr = new StreamReader(pathNotes))
            {
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(',');

                    AddNote(new Note(line[0], line[1], line[2], Convert.ToBoolean(line[3]), Convert.ToDateTime(line[4])), false);
                }
            }
            ResaveAllNotes();
        }

        #endregion

        #region Дополнительные функции ежедневника

        public int Count { get { return this.index; } }

        public void Save() { SavePB(); }

        #endregion

        #region Закрытые служебные функции

        private void SavePB()
        {
            string temp = String.Format("{0},{1},{2},{3}",
                                        Owner,
                                        MakeDate,
                                        index,
                                        path);
            File.WriteAllText(path, $"{temp}");
        }

        private void ResaveAllNotes()
        {
            CreateNotesFile();

            string Path = $@"{Owner}-notes.csv";

            for (int i = 0; i < index; i++)
            {
                if (Notes[i].GameTitle != null && Notes[i].GameTitle != "")
                {
                    string temp = String.Format("{0},{1},{2},{3},{4}",
                                    this.Notes[i].GameTitle,
                                    this.Notes[i].Genre,
                                    this.Notes[i].Platforms,
                                    this.Notes[i].Complete,
                                    this.Notes[i].Release
                                    );
                    File.AppendAllText(Path, $"{temp}\n");
                }
            }
        }

        private void CreateNotesFile()
        {
            string Path = $@"{Owner}-notes.csv";

            string temp = String.Format("{0},{1},{2},{3},{4}",
                                         "Название игры",
                                          "Жанр",
                                          "Платформы",
                                          "Пройдена",
                                          "Дата выхода");
            File.WriteAllText(Path, $"{temp}\n");
        }

        private class SortBy : IComparer<Note>
        {
            public int Compare(Note n1, Note n2)
            {
                if (n1.GameTitle != null && n2.GameTitle != null)
                {
                    return n1.GameTitle.CompareTo(n2.GameTitle);
                }
                else
                {
                    return 0;
                }
            }

            public int CompareG(Note n1, Note n2)
            {
                if (n1.Genre != null && n2.Genre != null)
                {
                    return n1.Genre.CompareTo(n2.Genre);
                }
                else
                {
                    return 0;
                }
            }

            public int CompareP(Note n1, Note n2)
            {
                if (n1.Platforms != null && n2.Platforms != null)
                {
                    return n1.Platforms.CompareTo(n2.Platforms);
                }
                else
                {
                    return 0;
                }
            }

            public int CompareC(Note n1, Note n2)
            {
                if (n1.GameTitle != null && n2.GameTitle != null)
                {
                    return n1.Complete.CompareTo(n2.Complete);
                }
                else
                {
                    return 0;
                }
            }

            public int CompareR(Note n1, Note n2)
            {
                if (n1.GameTitle != null && n2.GameTitle != null)
                {
                    return n1.Release.CompareTo(n2.Release);
                }
                else
                {
                    return 0;
                }
            }

        }

        private void ClearNotes()
        {
            Array.Clear(Notes, 0, Notes.Length);
            Array.Resize(ref Notes, 1);
            index = 0;
        }

        private void Resize(bool Flag)
        {
            if (Flag)
            {
                Array.Resize(ref this.Notes, this.Notes.Length * 2);
            }
        }

        private void DrawTitles()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"№", 3} {"Название игры",20} {"Жанр",20} {"Платформы",20} {"Пройдена",20} {"Дата выхода",20}");
            Console.ResetColor();
        }

        #endregion

        public Planbook(string owner, DateTime makeDate, string Path)
        {
            this.Owner = owner;
            this.MakeDate = makeDate;
            this.index = 0;
            this.path = Path;
            this.Notes = new Note[5];
            SavePB();
            if (File.Exists($@"{Owner}-notes.csv"))
            {
                LoadNotes($@"{Owner}-notes.csv");
            }
            else
            {
                CreateNotesFile();
            }
        }
    }
}
