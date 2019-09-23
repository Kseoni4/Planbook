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
        #region Основные поля заметки

        public string GameTitle { get; set; } // Поле название игры

        public string Genre { get; set; } // Поле жанр

        public string Platforms { get; set; } // Поле платформы

        public bool Complete { get; set; } // Поле статуса прохождения игры 

        public DateTime Release { get; set; } // Дата выхода игры

        #endregion

        /// <summary>
        /// Конструктор заметки
        /// </summary>
        /// <param name="title">Название игры</param>
        /// <param name="genre">Жанр</param>
        /// <param name="platforms">Платформа</param>
        /// <param name="complete">Статус пройденности игры</param>
        /// <param name="date">Дата выхода</param>
        public Note(string title, string genre, string platforms, bool complete, DateTime date)
        {
            this.GameTitle = title;
            this.Genre = genre;
            this.Platforms = platforms;
            this.Complete = complete;
            this.Release = date;
        }
        /// <summary>
        /// Метод вывода на экран заметки
        /// </summary>
        public void Print()
        {
            
            string cOut = Complete == true ? "Да" : "Нет";
            Console.Write($"{GameTitle, 20} {Genre, 20} {Platforms, 20}");
            Console.ForegroundColor = Complete == true ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write($" {cOut, 20} ");
            Console.ResetColor();
            Console.Write($"{Release.ToShortDateString(), 20}\n");
        }

    }
}
