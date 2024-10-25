namespace Library;

public class Book(string name, string author)
{
    public Guid Id
    {
        get; 
        init;
    } = Guid.NewGuid();

    public string Name { get; init; } = name;
    public string Author { get; init; } = author;
    

    public override bool Equals(object b)
    {
        if(b is Book)
        {
            return Name == ((Book)b).Name && Author == ((Book)b).Author;
        }
        return false;
    }
    
    
}