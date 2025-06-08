namespace ELK.API.Models;

public class Word
{
    public Word(string? name)
    {
        Name = name;
    }

    /// <summary>
    /// Идентификатор
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// Слово
    /// </summary>
    public string? Name { get; set; }
}