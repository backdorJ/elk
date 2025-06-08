namespace ELK.API.Models.Requests.GetWords;

public class SearchWordsRequest
{
    public int From { get; set; }

    public int Size { get; set; }

    public string? Search { get; set; }
}