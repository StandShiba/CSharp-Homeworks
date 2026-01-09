using EntityFramework.Data;
using EntityFramework.Models;
using Microsoft.EntityFrameworkCore;

namespace EntityFramework
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n=== Меню библиотеки ===");
                Console.WriteLine("1. Добавить автора");
                Console.WriteLine("2. Добавить книгу автору");
                Console.WriteLine("3. Показать все книги");
                Console.WriteLine("4. Показать всех авторов");
                Console.WriteLine("5. Показать книги конкретного автора");
                Console.WriteLine("6. Удалить книгу");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine()?.Trim();

                switch (choice)
                {
                    case "1":
                        AddAuthor(); // добавление автора
                        break;

                    case "2":
                        AddBookAuthor(); // добавление книги
                        break;

                    case "3":
                        GetAllBooks(); // показать все книги
                        break;

                    case "4":
                        GetAuthors(); // показать всех авторов
                        break;

                    case "5":
                        GetBooksFromAuthor(); // показать книги конкретного автора
                        break;

                    case "6":
                        DeleteBook(); // удалить книгу
                        break;

                    case "0":
                        exit = true; // выход
                        Console.WriteLine("Выход из программы...");
                        break;

                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }
        static void AddAuthor()
        {
            using (var context = new LibraryContext())
            {
                Console.Write("Введите имя автора: ");
                string name = Console.ReadLine()?.Trim(); // убираем лишние пробелы
                if (string.IsNullOrEmpty(name))
                {
                    Console.WriteLine("Имя автора не может быть пустым.");
                    return; // выходим из функции
                }
                bool isAuthorExist = context.Authors.Any(x => x.Name == name);
                if (!isAuthorExist)
                {
                    Author author = new Author { Name = name };
                    context.Authors.Add(author);
                    context.SaveChanges();
                    Console.WriteLine($"Автор '{name}' успешно добавлен!");
                }
                else
                {
                    Console.WriteLine("Такое имя уже существует");
                }
            }
        }
        static void AddBookAuthor()
        {
            using (var context = new LibraryContext())
            {
                Console.Write("Введите имя автора: ");
                string authorName = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(authorName))
                {
                    Console.WriteLine("Имя автора не может быть пустым.");
                    return;
                }
                var author = context.Authors.FirstOrDefault(x => x.Name == authorName);
                if (author == null)
                {
                    Console.WriteLine("Автор с таким именем не найден.");
                    return;
                }
                Console.Write("Введите название книги: ");
                string bookTitle = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(bookTitle))
                {
                    Console.WriteLine("Название книги не может быть пустым.");
                    return;
                }
                bool isBookExist = context.Books.Any(b => b.Title == bookTitle && b.AuthorId == author.Id);
                if (isBookExist)
                {
                    Console.WriteLine("У этого автора уже есть книга с таким названием.");
                    return;
                }
                Book book = new Book { Title = bookTitle, Author = author };
                context.Books.Add(book);
                context.SaveChanges();
                Console.WriteLine($"Книга '{bookTitle}' добавлена автору '{authorName}'.");
            }
        }
        static void GetAllBooks()
        {
            using (var context = new LibraryContext())
            {
                var allBooks = context.Books.Include(x => x.Author).ToList();

                if (!allBooks.Any())
                {
                    Console.WriteLine("В базе нет книг.");
                    return;
                }

                foreach (var book in allBooks)
                {
                    // Выводим Id, название и имя автора
                    string authorName = book.Author != null ? book.Author.Name : "Неизвестный автор";
                    Console.WriteLine($"{book.Id}, {book.Title}, Автор: {authorName}");
                }
            }
        }

        static void GetAuthors()
        {
            using (var context = new LibraryContext())
            {
                var allAuthors = context.Authors.ToList();

                if (!allAuthors.Any())
                {
                    Console.WriteLine("В базе нет авторов.");
                    return;
                }

                foreach (var auth in allAuthors)
                {
                    Console.WriteLine($"Id: {auth.Id}, Имя: {auth.Name}");
                }
            }
        }

        static void GetBooksFromAuthor()
        {
            using (var context = new LibraryContext())
            {
                Console.Write("Введите Id автора: ");
                string input = Console.ReadLine()?.Trim();

                if (!int.TryParse(input, out int authorId))
                {
                    Console.WriteLine("Некорректный Id автора.");
                    return;
                }

                var author = context.Authors.FirstOrDefault(a => a.Id == authorId);
                if (author == null)
                {
                    Console.WriteLine("Автор с таким Id не найден.");
                    return;
                }

                var books = context.Books
                                   .Where(b => b.AuthorId == authorId)
                                   .ToList();

                if (!books.Any())
                {
                    Console.WriteLine($"У автора '{author.Name}' пока нет книг.");
                    return;
                }

                Console.WriteLine($"Книги автора '{author.Name}':");
                foreach (var book in books)
                {
                    Console.WriteLine($"- {book.Title} (Id: {book.Id})");
                }
            }
        }
        static void DeleteBook()
        {
            using (var context = new LibraryContext())
            {
                Console.Write("Введите Id книги для удаления: ");
                string input = Console.ReadLine()?.Trim();

                if (!int.TryParse(input, out int bookId))
                {
                    Console.WriteLine("Некорректный Id книги.");
                    return;
                }

                var book = context.Books.Include(b => b.Author).FirstOrDefault(b => b.Id == bookId);
                if (book == null)
                {
                    Console.WriteLine("Книга с таким Id не найдена.");
                    return;
                }

                context.Books.Remove(book);
                context.SaveChanges();

                string authorName = book.Author != null ? book.Author.Name : "Неизвестный автор";
                Console.WriteLine($"Книга '{book.Title}' автора '{authorName}' успешно удалена.");
            }
        }

    }
}