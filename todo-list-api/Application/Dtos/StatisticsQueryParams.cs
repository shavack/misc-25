using System;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace TodoListApi.Application.Dtos;

public class StatisticsQueryParams
{
    public bool? IsCompleted { get; set; }
    public string Title { get; set; }
    [SwaggerSchema(Format = "date")]
    public DateOnly? FromDate { get; set; }
    [SwaggerSchema(Format = "date")]
    [DataType(DataType.Date)]
    public DateOnly? ToDate { get; set; }
}