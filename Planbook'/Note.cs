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
    struct Note
    {
        private string GameTitle { get; set; }
        private string Genre { get; set; }
        private string Platforms { get; set; }
        private bool Complete { get; set; }
               
        DateTime Release { get; set; }

        public Note(string title, string genre, string platforms, bool complete, DateTime date)
        {
            this.GameTitle = title;
            this.Genre = genre;
            this.Platforms = platforms;
            this.Complete = complete;
            this.Release = date;
        }

        public void Print()
        {
            
           string cOut = Complete == true ? "Да" : "Нет";
           Console.WriteLine($"{GameTitle} {Genre} {Platforms} {cOut} {Release}");
        }

    }
}
