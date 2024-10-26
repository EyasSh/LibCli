using System.Linq.Expressions;

namespace Library;

/// <summary>
/// a class Representing a library. This class is a singleton
/// </summary>
public class Library
    {
        static Library? _lib=Library.GetInstance();
        List<Worker?> _workers = new();
        List<Book?> _books = new();
        
        private Library(Admin a)
        {
            _workers.Add(a);
        }
        /// <summary>
        /// a public method to return an instance of the <see cref="Library"/> class 
        /// </summary>
        /// <returns> A singleton instance of the class</returns>
        public static Library GetInstance()
        {

            if (_lib == null)
            {
                Admin a = new("John Doe", "JD","Delay");
                
                _lib = new Library(a);
                Console.WriteLine("Admin added");
                return _lib;
            }
            return _lib;
        }
        /// <summary>
        /// a method that checks if a <see cref="Worker"/> objects exists in the worker list of the library
        /// </summary>
        /// <param name="worker">The worker being checked</param>
        /// <returns>true if a worker does work at the library and false otherwise</returns>
        public bool WorksAtLib(Worker? worker)
        {
           return _workers.Contains(worker);
        }
        public void AddBook(Worker? w, Book? book)
        {
            if (w is not null && WorksAtLib(w) &&book != null)
            {
               _books.Add(book);
                MessageHandler.SuccessMsg($"Book Added by {w.Name}");
            }
            else
            {
                if (w is not null)
                    MessageHandler.FailureMsg(@$"Book was not added. 
                                    Invalid authorization detected by {w.Name}");
            }


        }
        /// <summary>
        /// Removes a book given that it exists in the list
        /// if it doesn't the method prints a message with an exception
        /// </summary>
        /// <param name="worker"> a worker working in the library</param>
        /// <param name="book">the book to be removed</param>
        public void RemoveBook(Worker? worker, Book? book)
        {
            if (book == null || worker == null)
            {
                MessageHandler.FailureMsg("Book or worker are null");
            }
            else
            {
                if (!WorksAtLib(worker))
                {
                    MessageHandler.FailureMsg($"{worker.Name} Does Not Work at the Library");
                    return;
                }
                try
                {
                    _books.Remove(book);
                    MessageHandler.SuccessMsg($"{book.Name} removed by {worker.Name}");
                }
                catch (Exception e)
                {
                    MessageHandler.FailureMsg($" problem removing book exception:-\n{e.Message}");
                }

            }
        }
        /// <summary>
        /// This method enables a <see cref="Manager"/> position or above to hire a new worker
        /// </summary>
        /// <param name="adder">The <see cref="Manager"/> or <see cref="Admin"/> which is hiring the worker</param>
        /// <param name="worker">The <see cref="Worker"/> being hired</param>
        public void AddWorker(Manager? adder, Worker? worker)
        {
            if (adder!=null && worker!=null && WorksAtLib(adder))
            {
                _workers.Add(worker);
                MessageHandler.SuccessMsg($"Worker {worker.Name}" +
                    $"added successfully by {adder.Name}");
            }
            else
                MessageHandler.FailureMsg(@"Failure to add worker either the adder or the worker are null or 
                                          the adder does not work at lib");
        }
        /// <summary>
        /// a method to  fire a <see cref="Worker"/>
        /// </summary>
        /// <param name="remover">an <see cref="Admin"/> who is firing the worker</param>
        /// <param name="worker"> The <see cref="Worker"/> Being  fired</param>
        public void RemoveWorker(Admin? remover, Worker? worker)
        {
            if ((worker != null && remover != null) && (WorksAtLib(remover) && WorksAtLib(worker)))
            {
                _workers.Remove(worker);
                MessageHandler.SuccessMsg($"Worker {worker.Name}" +
                    $"added successfully by {remover.Name}");
            }
            else
                MessageHandler.FailureMsg("Failure to remove worker");
        }
        /// <summary>
        /// A method to promote a worker
        /// </summary>
        /// <param name="admin"> an <see cref="Admin"/> whom works at the library which is promoting the worker</param>
        /// <param name="worker"> The <see cref="Worker"/> being promoted</param>
        public void PromoteWorker(Admin admin, Worker? worker)
        {
            Console.WriteLine("Press 1 to promote worker to Manager" +
                "or 2 to promote worker to Admin");
            char c = Console.ReadKey().KeyChar;
            switch (c)
            {
                case '1':
                    if (worker != null && worker is not Manager && worker is not Admin)
                    {
                        Manager a = new Manager(worker.Name, worker.UserName,worker.Password);
                        _workers.Remove(worker);
                        _workers.Add(a);
                        MessageHandler.SuccessMsg($"Worker {worker.Name}" +
                        $"promoted to manager successfully by {admin.Name}");
                    }

                    break;
                case '2':
                    if (worker != null && worker is not Manager && worker is not Admin)
                    {
                        Admin a = new Admin(worker.Name, worker.UserName, worker.Password);
                        _workers.Remove(worker);
                        _workers.Add(a);
                        MessageHandler.SuccessMsg($"Worker {worker.Name}" +
                    $"promoted to admin successfully by {admin.Name}");
                    }
                    break;
                default:
                    MessageHandler.FailureMsg("Wrong character pressed/ null admin or worker/ worker being promoted is already in admin position");
                    break;


            }
        }
          ///
         ///<summary>
         /// a method the job of which is demote a worker to a lower position within the library
         /// </summary>
         /// <param name="admin"> an <see cref="Admin"/> who is demoting the worker</param>
         ///<param name="worker">a <see cref="Worker"/> object which represents the worker being demoted</param>
        public void DemoteWorker(Admin? admin, Worker? worker)
        {
            bool bothWorkAtLib = (admin != null && worker != null) && _lib is not null && (_lib.WorksAtLib(admin) && _lib.WorksAtLib(worker));
            if (worker is null || admin is null)
            {
                MessageHandler.FailureMsg("One of the workers is null or does not work at library");
                return;
            }

            if (bothWorkAtLib && worker is Admin)
            {

                try
                {
                    Manager m = new(worker.Name,worker.UserName,worker.Password);
                    _workers.Add(m);
                    _workers.Remove(worker);
                }
                    
                catch(Exception e)
                {
                    MessageHandler.FailureMsg("Manager object is null exiting program Library.cs Line:154");
                    return ;
                }
               
                
            }
            else if (bothWorkAtLib && worker is Manager)
            {
                Worker w = new(worker.Name,worker.UserName,worker.Password);
                _workers.Remove(worker);
                _workers.Add(w);
            }
            else
            {
                Console.WriteLine("Worker is already demoted to the lowest level would you like to fire them y/n");
                try
                {
                    char ch = char.Parse(Console.ReadKey().KeyChar.ToString());
                    if (char.ToLower(ch) != 'y' || char.ToLower(ch) != 'n')
                    {
                        Console.WriteLine("Character must be y/n");
                        return;
                    }

                    else if (char.ToLower(ch) == 'y')
                    {
                        RemoveWorker(admin, worker);
                        return;
                    }

                    else
                    {
                        Console.WriteLine("Process finished successfully worker was not fired");
                        return;
                    }
                }
                catch
                {
                    Console.WriteLine("Cannot take more than one character either y/n process will end");
                    return;
                }

            }


        }
    }

    public static class MessageHandler
    {

        public static void SuccessMsg(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void FailureMsg(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
