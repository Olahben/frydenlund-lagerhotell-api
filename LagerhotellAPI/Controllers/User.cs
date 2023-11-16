using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly string filePath = @"C:\Users\ohage\SKOLE\Programmering\Lagerhotell\wwwroot\Data\users.json";
    [Route("adduser")]
    [HttpPost]
    public IActionResult AddUser([FromBody] Dictionary<string, string> data)
    {
        List<object> _data = new List<object>() { data };

        string existingJson = System.IO.File.ReadAllText(filePath);
        JObject jsonObject = JsonConvert.DeserializeObject<JObject>(existingJson);

        JArray usersArray = jsonObject.GetValue("users") as JArray;
        usersArray.Add(JToken.FromObject(data));

        string updatedJson = jsonObject.ToString();

        System.IO.File.WriteAllText(filePath, updatedJson);


        return Ok();
    }
}