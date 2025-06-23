using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoListApi.Application.Dtos;
using TodoListApi.Application.Services;
using TodoListApi.Domain;

namespace TodoListApi.Controllers
{
    [Route("taskscontroller")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResultDto<TaskItem>>> GetTasks([AsParameters] TaskQueryParams taskQueryParams, IValidator<TaskQueryParams> validator)
        {
            validator.ValidateAndThrow(taskQueryParams);
            var tasks = await _taskService.GetAllTasksAsync(taskQueryParams);
            var paginatedResult = new PaginatedResultDto<TaskItem>
            {
                Items = tasks.Items,
                Page = tasks.Page,
                PageSize = tasks.PageSize,
                TotalCount = tasks.TotalCount,
                TotalPages = tasks.TotalPages
            };

            return paginatedResult;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskItem>> GetTaskById(int id)
        {
            var taskItem = await _taskService.GetTaskByIdAsync(id); 

            if (taskItem == null)
            {
                return NotFound();
            }

            return taskItem;
        }

        [HttpPost]
        public async Task<ActionResult<TaskItem>> AddTask(TaskItem taskItem)
        {
            await _taskService.AddTaskAsync(taskItem);

            return CreatedAtAction(nameof(GetTaskById), new { id = taskItem.Id }, taskItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskItem taskItem)
        {
            if (id != taskItem.Id)
            {
                return BadRequest();
            }
            await _taskService.UpdateTaskAsync(id, taskItem);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var taskItem = await _taskService.GetTaskByIdAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }

            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }
    }
}