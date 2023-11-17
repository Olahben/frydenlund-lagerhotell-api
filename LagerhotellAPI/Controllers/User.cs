using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

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

    [Route("is-phone-number-registered-registration")]
    [HttpPost]
    public IActionResult CheckPhoneNumberExistence([FromBody] string phoneNumber)
    {
        // reads text from JSON and checks if phone number is there
        // returns true if it registered and false if not
        string existingJson = System.IO.File.ReadAllText(filePath);
        // Parse the JSON data
        JObject jsonObject = JObject.Parse(existingJson);

        // Access the "users" array
        JArray usersArray = (JArray)jsonObject["users"];
        Console.WriteLine(usersArray);

        foreach (var item in usersArray)
        {
            Console.WriteLine(item["phoneNumber"]);
            Console.WriteLine(phoneNumber);
            if (phoneNumber == item["phoneNumber"].ToString())
            {
                return BadRequest("False");
            }
        }

        return Ok();
    }
}