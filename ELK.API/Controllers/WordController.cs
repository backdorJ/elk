using ELK.API.Models;
using ELK.API.Models.Requests.AddWord;
using ELK.API.Models.Requests.DeleteWord;
using ELK.API.Models.Requests.GetAllWords;
using ELK.API.Models.Requests.GetWords;
using ELK.API.Models.Requests.UpdateWord;
using ELK.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace ELK.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WordController : ControllerBase
{
    private readonly IWordElasticSearchService _wordElasticSearchService;

    public WordController(IWordElasticSearchService wordElasticSearchService)
    {
        _wordElasticSearchService = wordElasticSearchService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(GetWordRequest request)
    {
        var response = await _wordElasticSearchService.GetAsync(request);
        
        return response.IsSuccess
            ? Ok(response.Value)
            : BadRequest(response.ErrorMessage);
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddWordRequest request)
    {
        var response = await _wordElasticSearchService.AddAsync(request);
        
        if (!response.IsSuccess)
            return BadRequest(response.ErrorMessage);
        
        return Ok(response);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchWordsRequest request)
    {
        var response = await _wordElasticSearchService.SearchAsync(request);
        
        return response.IsSuccess
            ? Ok(response.Value)
            : BadRequest(response.ErrorMessage);
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult> Delete([FromRoute] DeleteWordRequest request)
    {
        var response = await _wordElasticSearchService.DeleteAsync(request);
        
        return response.IsSuccess
            ? Ok()
            : BadRequest(response.ErrorMessage);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateWordRequest request)
    {
        var response = await _wordElasticSearchService.UpdateAsync(request);
        
        return response.IsSuccess
            ? Ok()
            : BadRequest(response.ErrorMessage);
    }

    [HttpGet("get-grouped-names")]
    public async Task<IActionResult> GetGroupedNames()
    {
        var response = await _wordElasticSearchService.GetGroupedNamesAsync();
        
        return response.IsSuccess
            ? Ok(response.Value)
            : BadRequest(response.ErrorMessage);
    }
}