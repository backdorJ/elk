using ELK.API.Models;
using ELK.API.Models.Requests.AddWord;
using ELK.API.Models.Requests.DeleteWord;
using ELK.API.Models.Requests.GetAllWords;
using ELK.API.Models.Requests.GetGroupedNames;
using ELK.API.Models.Requests.GetWords;
using ELK.API.Models.Requests.UpdateWord;

namespace ELK.API.Services;

public interface IWordElasticSearchService
{
    public Task<Result> AddAsync(AddWordRequest request);
    public Task<Result> UpdateAsync(UpdateWordRequest request);
    public Task<Result> DeleteAsync(DeleteWordRequest request);

    public Task<Result<List<GetGroupedNamesResponse>>> GetGroupedNamesAsync();
    
    public Task<Result<List<SearchWordsResponse>>> SearchAsync(SearchWordsRequest request);

    public Task<Result<GetWordResponse>> GetAsync(GetWordRequest request);
}