using Animoment.Features.Parse;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
    
namespace Animoment.Controllers;

public class ParseController : Controller
{
    [HttpPost]
    public async void Index()
    {
        var content = await System.IO.File.ReadAllTextAsync("data.json");
        var shows = JsonConvert.DeserializeObject <List<Show>>(content);
        
    }
}