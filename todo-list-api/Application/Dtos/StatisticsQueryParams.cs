using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;
using TodoListApi.Types;

namespace TodoListApi.Application.Dtos;

public class StatisticsQueryParams
{
    public TaskState? State { get; set; } = null;
    public string Title { get; set; }
    [SwaggerSchema(Format = "date")]
    public DateOnly? FromDate { get; set; }
    [SwaggerSchema(Format = "date")]
    [DataType(DataType.Date)]
    public DateOnly? ToDate { get; set; }
}