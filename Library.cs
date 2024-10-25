using System.Linq.Expressions;

namespace Library;

public class Library
    {
        private static Library? _lib=Library.GetInstance();
        List<Worker?> _workers = new();
        List<Book?> _books = new();
        
        private Library(Admin a)
        {
            _workers.Add(a);
        }
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
        public bool WorksAtLib(Worker? worker)
        {
            if (_workers.Contains(worker))
                return true;
            return false;
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
        public void RemoveBook(Worker? worker, Book? book)
        {
            if (book == null || worker == null)
            {
                MessageHandler.FailureMsg("Book or worker are null");
            }
            else
            {
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
                    MessageHandler.FailureMsg("Wrong character pressed/ null admin or worker");
                    break;


            }
        }
          ///
         ///<Summary> @param admin an admin whose job is to demote a worker </Summary>
         ///
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
