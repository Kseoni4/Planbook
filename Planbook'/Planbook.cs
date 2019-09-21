using System;

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
        private string Owner { get; set; }
        private DateTime MakeDate { get; set; }
        private int index { get; set; }

        private Note[] Notes;

        private void Resize(bool Flag)
        {
            if (Flag)
            {
                Array.Resize(ref this.Notes, this.Notes.Length * 2);
            }
        }

        public void CreateNote()
        {
            Console.WriteLine();

            this.Resize(index >= this.Notes.Length);

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

            Notes[index] = new Note(title, genre, platforms, complete, date);

            index++;

            Console.WriteLine($"Запись игры {title} создана.");

        }

        public void PrintNotes()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{"Название игры",20} {"Жанр",20} {"Платформы",20} {"Пройдена",20} {"Дата выхода",20}");
            Console.ResetColor();
            for(int i = 0; i < index; i++)
            {
                Notes[i].Print();
            }
        }

        public void LoadNotes()
        {

        }

        public void SaveNotes()
        {

        }

        public void LoadNotesFromDates()
        {

        }

        public void EditNote()
        {

        }
        
        public void SortNote()
        {

        }

        public Planbook(string owner, DateTime makeDate)
        {
            this.Owner = owner;
            this.MakeDate = makeDate;
            this.index = 0;
            this.Notes = new Note[5];
        }
    }
}
