namespace TodoListApi.Application.Dtos;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public LoginRequest() { }

    public LoginRequest(string username, string password)
    {
        Username = username;
        Password = password;
    }
}