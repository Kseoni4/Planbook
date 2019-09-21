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
 * |____Метод сортировки по полю
 * 
 */



#endregion


namespace Homework_07
{
    struct Planbook
    {
        public string Owner { get; set; }

        public DateTime MakeDate { get; set; }

        private int index { get; set; }

        private string path;

        private Note[] Notes;

        private void Resize(bool Flag)
        {
            if (Flag)
            {
                Array.Resize(ref this.Notes, this.Notes.Length * 2);
            }
        }

        public void AddNote(Note _Note)
        {
            this.Resize(index >= this.Notes.Length);
            this.Notes[index] = _Note;
            index++;
        }

        public void PrintNotes()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"Название игры",20} {"Жанр",20} {"Платформы",20} {"Пройдена",20} {"Дата выхода",20}");
            Console.ResetColor();
            for(int i = 0; i < index; i++)
            {
                Console.Write(i);
                Notes[i].Print();
            }
        }

        public void LoadNotes()
        {
            using (StreamReader sr = new StreamReader(this.path))
            {
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(',');

                    AddNote(new Note(line[0], line[1], line[2], Convert.ToBoolean(line[3]), Convert.ToDateTime(line[4])));
                }
            }
        }

        public void SaveNotes(string Path)
        {
            // File.Create(@"notes.csv");

            Path = $@"{Owner}-notes.csv";

            string temp = String.Format("{0},{1},{2},{3},{4}",
                                         "Название игры",
                                          "Жанр",
                                          "Платформы",
                                          "Пройдена",
                                          "Дата выхода");
            File.AppendAllText(Path, $"{temp}\n");

            for (int i = 0; i < this.index; i++)
            {
                temp = String.Format("{0},{1},{2},{3},{4}",
                                    this.Notes[i].GameTitle,
                                    this.Notes[i].Genre,
                                    this.Notes[i].Platforms,
                                    this.Notes[i].Complete,
                                    this.Notes[i].Release
                                    );
                File.AppendAllText(Path, $"{temp}\n");
            }

        }

        public void LoadNotesFromDates()
        {
            Console.Write("Введите диапазон дат через пробел (DD-MM-YYYY): ");
            string[] dates = Console.ReadLine().Split(' ');

            using (StreamReader sr = new StreamReader(this.path))
            {
                sr.ReadLine();

                while (!sr.EndOfStream)
                {
                    string[] line = sr.ReadLine().Split(',');

                    Array.Clear(Notes, 0 ,Notes.Length);
                    Array.Resize(ref Notes, 1);
                    index = 0;
                    
                    if(Convert.ToDateTime(line[4]) <= Convert.ToDateTime(dates[0]) 
                       && Convert.ToDateTime(line[4]) >= Convert.ToDateTime(dates[1]))
                    {
                        AddNote(new Note(line[0], line[1], line[2], Convert.ToBoolean(line[3]), Convert.ToDateTime(line[4])));
                    }
                }
            }

        }

        public void EditNote()
        {

        }
        
        public void SortNote()
        {

        }

        public void DeleteNote()
        {

        }

        public int Count { get { return this.index; } }

        private void savePB()
        {
            string temp = String.Format("{0},{1},{2},{3}",
                                        Owner,
                                        MakeDate,
                                        index,
                                        path);
            File.AppendAllText(path, $"{temp}");
        }

        public Planbook(string owner, DateTime makeDate, string Path)
        {
            this.Owner = owner;
            this.MakeDate = makeDate;
            this.index = 0;
            this.path = Path;
            this.Notes = new Note[5];
            savePB();
            if (File.Exists($@"{Owner}-notes.csv"))
            {
                LoadNotes();
            }
        }
    }
}
