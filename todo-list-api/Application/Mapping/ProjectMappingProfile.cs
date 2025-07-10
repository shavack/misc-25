using AutoMapper;
using TodoListApi.Application.Dtos;
using TodoListApi.Domain;
namespace TodoListApi.Application.Mapping;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<Project, ProjectItemDto>().ReverseMap();
    }
}