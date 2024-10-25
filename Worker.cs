namespace Library;

public class Worker(string name, string userName, string password)
{
   
       
    #region Props
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name
    { get ;set; } = name;

    public string UserName { get; set; } = userName;
    public string Password { get; set; } = password;

    #endregion

    public void AddBook(Library? lib,Book? book)
    {
        if (lib is not null && lib.WorksAtLib(this))
        {
            lib.AddBook(this, book);
            Console.WriteLine("Book added");
            
        }
            
    }
    public void RemoveBook(Library? lib, Book? book)
    {
        if (lib is not null && lib.WorksAtLib(this))
        {
            lib.RemoveBook(this, book);
            Console.WriteLine("Removed book");
            return;
        }

        Console.WriteLine("Book cannot be removed as it is not in library");
        
            
    }
           
}
public class Manager(string name, string userName, string password) : Worker(name, userName, password)
{
    public void Hire(Library? l,Worker? w)
    {
        if (l is not null && l.WorksAtLib(this))
        {
            l.AddWorker(this, w);
            Console.WriteLine("Worker hired");
            return;
        }

        Console.WriteLine("Worker Already hired");
            
    }
      
}
public sealed class Admin(string name, string username, string password) : Manager(name, username, password)
{
    public void Promote(Library? l,Worker? w)
    {
        if (l is not null && l.WorksAtLib(this))
        {
            l.PromoteWorker(this, w);
            return;
        }
            
        Console.WriteLine("Library does not exist.");
    }
    public void Demote(Library? l,Worker? w)
    {
        if(l is not null && l.WorksAtLib(this))
            l.DemoteWorker(this, w);
    }
    public void Fire(Library? l, Worker? w)
    {
        if(l is not null && l.WorksAtLib(this))
            l.RemoveWorker(this, w);
    }
}