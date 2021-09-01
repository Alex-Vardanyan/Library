using System;
using System.Collections.Generic;

namespace LibraryData
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Example Program
            var Table1 = new ReadingDesk();
            var Table2 = new ReadingDesk();

            var TheWarOfTheWorlds = new Book("The War Of The Worlds", "Herbert George Wells", 287, 1897, true);
            var AroundTheWorldIn80Days = new Book("Around The World In 80 Days", "Jules Verne", publishedYear: 1872, availability: true);
            var TheGoodSoldierSvejk = new Book("The Good Soldier Svejk", "Jaroslav Hasek");
            var MurderOnTheOrientExpress1 = new Book("The Murder On The Orient Express", "Agatha Christie", pages:256);
            var MurderOnTheOrientExpress2 = new Book("The Murder On The Orient Express", "Agatha Christie", pages: 256);

            BookCollection.AddToCollection(TheWarOfTheWorlds);
            BookCollection.AddToCollection(AroundTheWorldIn80Days);
            BookCollection.AddToCollection(TheGoodSoldierSvejk);
            BookCollection.AddToCollection(MurderOnTheOrientExpress1);
            BookCollection.AddToCollection(MurderOnTheOrientExpress2);

            BookCollection.ViewCollection();

            var Alex = new Member("Alexander Vardanyan");
            Alex.AddBook(MurderOnTheOrientExpress1);
            Alex.AddBook(TheGoodSoldierSvejk);
            Console.WriteLine(Alex);
            Alex.ViewBorrowedBooks();

            Alex.Sit(Table2);

            Alex.ReturnBook(TheGoodSoldierSvejk); //you have to stand up from the desk first
            Alex.StandUp();
            Alex.ReturnBook(TheWarOfTheWorlds); //you don't have that book
            Alex.ReturnBook(TheGoodSoldierSvejk);
            Alex.AddBook(AroundTheWorldIn80Days);

            BookCollection.ViewCollection();
            #endregion
        }
    }

    class Book
    {
        public bool Available{ get; set; }
        
        private int _pages;
        private string _author;
        private string _title;
        private int _publishYear;

        public Book(string title, string author, int pages = 0, int publishedYear = 0, bool availability = true)
        {
            Title = title;
            Author = author;
            Pages = pages;
            PublicationYear = publishedYear;
            Available = availability;
        }

        public int Pages 
        {
            get => _pages;
            private set
            {
                if (value > 5000 && value < 0)
                {
                    Console.WriteLine("Invalid number of pages, it can't exceed 5000!");
                }
                _pages = value;
            }
        }

        public string Author
        {
            get => _author;
            private set
            {
                if (value.Length > 50)
                {
                    Console.WriteLine("Authors name is too long!");
                }
                _author = value;
            }
        }

        public string Title
        {
            get => _title;
            private set
            {
                if (value.Length > 50)
                {
                    Console.WriteLine("Title is too long!");
                }
                _title = value;
            }
        }

        public int PublicationYear
        {
            get => _publishYear;
            private set
            {
                if (value < 0)
                {
                    Console.WriteLine("Invalid year!");
                }
                _publishYear = value;
            }
        }

        public override string ToString()
        {
            return $"{_title} by {_author}";
        }
    }

    class Member
    {
        private static int _maxBooks = 10;
        private string _name;
        private List<Book> _bookList = new List<Book>();
        private string _accountCreationDate;
        private bool _isSitting = false;
        private ReadingDesk SomeDesk = new ReadingDesk();

        public Member(string name)
        {
            Name = name;
            _accountCreationDate = DateTime.Now.ToString("G");
        }

        public void AddBook(Book Book)
        {
            if (_isSitting == false)
            {
                if (_bookList.Count >= _maxBooks)
                {
                    Console.WriteLine("You reached the limit of 10 borrowed books! You can't borrow more, please return some books!");
                }
                else if (Book.Available == false)
                {
                    Console.WriteLine($"{Book} is unavailable!");
                }
                else
                {
                    _bookList.Add(Book);
                    Book.Available = false;
                }
            }
            else
            {
                Console.WriteLine("Please stand up first to be able to do other actions!");
            }
        }

        public void ReturnBook(Book Book)
        {
            if (_isSitting == false)
            {
                if (_bookList.Contains(Book) == true)
                {
                    _bookList.Remove(Book);
                    Book.Available = true;
                    Console.WriteLine("The book succesfuly returned!");
                }
                else if (_bookList.Count == 0)
                {
                    Console.WriteLine("You don't have any books to return!");
                }
                else
                {
                    Console.WriteLine($"You don't have {Book}");
                }
            }
            else
            {
                Console.WriteLine("Please stand up first to be able to do other actions!");
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                if (value.Length > 50)
                {
                    Console.WriteLine("Member's name is too long!");
                }
                else
                { 
                _name = value;
                }
            }
        }

        public void Sit(ReadingDesk desk)
        {
            if (_isSitting == false)
            {
                SomeDesk = desk;
                _isSitting = true;
                desk.OccupyChair();
            }
            else
            {
                Console.WriteLine("You have already done that!");
            }
        }

        public void StandUp()
        {
            if (_isSitting == true)
            {
                _isSitting = false;
                SomeDesk.LeaveChair();
            }
            else
            {
                Console.WriteLine("You are already standing!");
            }
        }

        public void ViewBorrowedBooks()
        {
            foreach (var Book in _bookList)
            {
                Console.WriteLine("• " + Book + ",");
                Console.WriteLine(nameof(Book));
            }
        }

        override public string ToString()
        {
            return $"{_name}\nAccount created on {_accountCreationDate}\nBooks in use: {_bookList.Count}.";
        }
    }

    class ReadingDesk
    {
        public static int ChairCount = 7;
        private int availableChairs = ChairCount;
        
        public void OccupyChair()
        {
            if (availableChairs > 0)
            {
                availableChairs -= 1;
            }
            else
            {
                Console.WriteLine("Sorry, all chairs are accupied!");
            }
        }

        public void LeaveChair()
        {
            if (availableChairs < ChairCount)
            {
                availableChairs += 1;
            }
            else
            {
                Console.WriteLine("Sorry, but you can't do that!");
            }
        }
    }

    static class BookCollection
    {
        static private List<Book> _books = new List<Book>();
        
        static public void AddToCollection(Book Book)
        {
            if (_books.Contains(Book) == true)
            {
                Console.WriteLine("The same book is already in collection!");
                Console.WriteLine("But there COULD be multiple Copies of the same book!");
            }
            else
            {
                _books.Add(Book);
            }
        }

        static public void RemoveBook(Book Book)
        {
            if (_books.Contains(Book) == true)
            {
                _books.Remove(Book);
                Console.WriteLine($"{Book} Removed from Collection!");
            }
            else
            {
                Console.WriteLine($"{Book} is not in collection");
            }
        }

        static public void ViewCollection()
        {
            Console.WriteLine("--------------------------------");
            foreach (var Book in _books)
            {
                Console.WriteLine(Book + " ; Availability: " + (Book.Available?"Available":"Unavailable"));
            }
            Console.WriteLine("--------------------------------");
        }
    }
}
