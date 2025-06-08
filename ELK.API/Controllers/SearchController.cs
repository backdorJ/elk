using System.Collections.Immutable;
using Elastic.Clients.Elasticsearch;
using ELK.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ELK.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SearchController : ControllerBase
{
    private readonly ElasticsearchClient _esClient;

    public SearchController(ElasticsearchClient esClient)
    {
        _esClient = esClient;
    }

    [HttpGet("ping")]
    public async Task<IActionResult> Ping()
    {
        var pingResponse = await _esClient.PingAsync();
        return Ok(pingResponse.IsValidResponse ? "Elasticsearch OK" : "Failed to connect");
    }

    [HttpPost("fill-text")]
    public async Task<IActionResult> FillText()
    {
        var words = Text.GetText()
            .Split([' ', ',', '.', '!', '?'], StringSplitOptions.RemoveEmptyEntries)
            .Select((word, index) => new Word( word.Trim())
            {
                Id = $"{index + 1}",
            })
            .ToImmutableList();

        await _esClient.IndexManyAsync(words);
        
        return Ok(new { FillCount = words.Count });
    }
}