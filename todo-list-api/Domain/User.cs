using System;

namespace TodoListApi.Domain;
public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? LastModifiedAt { get; set; }
}