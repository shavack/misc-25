using AutoMapper;
using TodoListApi.Application.Dtos;
using TodoListApi.Domain;
namespace TodoListApi.Application.Mapping;

public class TaskMappingProfile : Profile
{
    public TaskMappingProfile()
    {
        CreateMap<TaskItem, TaskItemDto>().ReverseMap();
    }
}