using System;
using System.Collections.Generic;
namespace TodoListApi.Domain;

public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? LastModifiedAt { get; set; }
    public int UserId { get; set; }
}