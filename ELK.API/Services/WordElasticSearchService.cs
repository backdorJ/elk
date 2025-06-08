
using Elastic.Clients.Elasticsearch;
using ELK.API.Models;
using ELK.API.Models.Requests.AddWord;
using ELK.API.Models.Requests.DeleteWord;
using ELK.API.Models.Requests.GetAllWords;
using ELK.API.Models.Requests.GetGroupedNames;
using ELK.API.Models.Requests.GetWords;
using ELK.API.Models.Requests.UpdateWord;
using Result = ELK.API.Models.Result;

namespace ELK.API.Services;

public class WordElasticSearchService(ElasticsearchClient client) : IWordElasticSearchService
{
    public async Task<Result> AddAsync(AddWordRequest request)
    {
        var response = await client
            .IndexAsync(new Word(request.Name), i => i.Index(nameof(Word).ToLowerInvariant()));

        return response.IsValidResponse
            ? Result.Success()
            : Result.Failure(errorMessage: $"Status from elastic {response.Result}");
    }

    public async Task<Result> UpdateAsync(UpdateWordRequest request)
    {
        var response = await client.UpdateAsync<Word, Word>(nameof(Word).ToLowerInvariant(), request.Id, u => u
            .Doc(new Word(request.Name))
        );

        return response.IsValidResponse
            ? Result.Success()
            : Result.Failure(errorMessage: $"Status from elastic {response.Result}");
    }

    public async Task<Result> DeleteAsync(DeleteWordRequest request)
    {
        var response = await client.DeleteAsync<Word>(request.Id, d => d.Index(nameof(Word).ToLowerInvariant()));
        
        return response.IsValidResponse
            ? Result.Success()
            : Result.Failure(errorMessage: $"Status from elastic {response.Result}");
    }

    public async Task<Result<List<GetGroupedNamesResponse>>> GetGroupedNamesAsync()
    {
        var response = await client.SearchAsync<Word>(search => search
            .Indices(nameof(Word).ToLowerInvariant())
            .Query(query => query.MatchAll(_ => { }))
            .Aggregations(aggregations => aggregations
                .Add("agg_name", aggregation => aggregation
                    .Terms(group => group
                        .Field("name.keyword")
                        .Size(10_000)
                    )
                )
            )
        );
        
        var buckets = response.Aggregations?
            .GetStringTerms("agg_name")?
            .Buckets?
            .Select(bucket => new
            {
                Name = bucket.Key.ToString() ?? string.Empty,
                Count = bucket.DocCount
            })
            .ToList();
        
        var result = buckets?
            .Select(bucket => new GetGroupedNamesResponse
            {
                Name = bucket.Name,
                Count = bucket.Count,
            })
            .ToList() ?? new();

        return response.IsValidResponse
            ? Result<List<GetGroupedNamesResponse>>.Success(result)
            : Result<List<GetGroupedNamesResponse>>.Failure("Произошла ошибка");
    }

    public async Task<Result<List<SearchWordsResponse>>> SearchAsync(SearchWordsRequest request)
    {
        var response = await client.SearchAsync<Word>(s => s
            .Indices(nameof(Word).ToLowerInvariant())
            .From(request.From)
            .Size(request.Size)
            .Query(q => q
                .Prefix(t => t
                    .Field(x => x.Name)
                    .Value(request.Search ?? string.Empty)
                )
            )
        );
        
        var result = response.Hits.Select(word => new SearchWordsResponse
            {
                Name = word.Source?.Name ?? "null",
                Id = word.Id ?? "null",
            })
            .ToList();
        
        return Result<List<SearchWordsResponse>>.Success(result);
    }

    public async Task<Result<GetWordResponse>> GetAsync(GetWordRequest request)
    {
        var response = await client.GetAsync<Word>(request.Id, d => d.Index(nameof(Word).ToLowerInvariant()));

        return response.IsValidResponse
            ? Result<GetWordResponse>.Success(new GetWordResponse
            {
                Name = response.Source?.Name ?? "сори не нашел",
                Id = response.Source?.Id ?? "сори не нашел"
            })
            : Result<GetWordResponse>.Failure("Произошла ошибка");
    }
}