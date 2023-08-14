var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Person> personList = new List<Person>();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    if (request.Path == "/api/user")
    {
        var message = "некорректні дані";
        try
        {
            var person = await request.ReadFromJsonAsync<Person>();
            if (person != null)
            {
                personList.Add(person);
                message = "Користувач доданий успішно.";
            }
        }
        catch { }
        await response.WriteAsJsonAsync(new { text = message });
    }
    else if (request.Path == "/users")
    {
        response.ContentType = "text/html; charset=utf-8";
        var userListHtml = "<h2>User List</h2>";
        foreach (var person in personList)
        {
            userListHtml += $"<p>Name: {person.Name}, Age: {person.Age}</p>";
        }
        await response.WriteAsync(userListHtml);
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
});

app.Run();

public record Person(string Name, int Age);
