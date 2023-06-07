using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DadJokesAPIProject.Controllers;

[ApiController]
[Route("[controller]")]
public class DadJokesController : ControllerBase
{
    private readonly ILogger<DadJokesController> _logger;
    private readonly HttpClient _httpClient;

    public DadJokesController(ILogger<DadJokesController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("DadJokeHttpClient");

    }

    [HttpGet("GetRandomJoke")]
    public async Task<IActionResult> GetRandomJoke()
    {
        var response = await _httpClient.GetFromJsonAsync<ApiGetResponse>("");
        if (response!=null)
        {
            if (response.Status == 200)
            {
                var joke = response;
                return Ok(new DadJoke() { Joke = response.Joke });
            }
            return BadRequest(response.Status.ToString());

        }
        return BadRequest("Error Fetching Data!");
    }
    [HttpGet("SearchJokeWithKeyWord")]
    public async Task<IActionResult> SearchJokeWithKeyWord(string keyword)
    {
        var response = await _httpClient.GetFromJsonAsync<ApiSearchResponse>($"search?page=1&limit=30&term={keyword}");
        if (response != null)
        {
            if (response.Status == 200)
            {
                var results = response.Results;
                var jokes = results?.Select(r => new DadJoke() { Joke = r.Joke }).ToList();
                var shortJokes = jokes?.Where(j => j.Joke.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length < 10).Select(obj => {
                    obj.Joke = Regex.Replace(obj.Joke, $@"{Regex.Escape(keyword)}", $@"{keyword.ToUpper()}", RegexOptions.IgnoreCase);
                    return obj;
                }).ToList();
                var mediumJokes = jokes?.Where(j => j.Joke.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length >= 10 && j.Joke.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length < 20).Select(obj => {
                    obj.Joke = Regex.Replace(obj.Joke, $@"{Regex.Escape(keyword)}", $@"{keyword.ToUpper()}", RegexOptions.IgnoreCase);
                    return obj;
                }).ToList();
                var longJokes = jokes?.Where(j => j.Joke.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length >= 20).Select(obj => {
                    obj.Joke = Regex.Replace(obj.Joke, $@"{Regex.Escape(keyword)}", $@"{keyword.ToUpper()}", RegexOptions.IgnoreCase);
                    return obj;
                }).ToList();
                return Ok(new { shortJokes, mediumJokes, longJokes});
            }
            return BadRequest(response.Status.ToString());

        }
        return BadRequest("Error Fetching Data!");
    }
}

