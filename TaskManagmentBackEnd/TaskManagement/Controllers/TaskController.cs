using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Dtos.Tasks;
using TaskManagement.Exceptions;
using TaskManagement.Models;
using TaskManagement.Services;

namespace TaskManagement.Controllers;

[ApiController]
[Route("tasks")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TaskController : ControllerBase
{
    private readonly TaskService _taskService;

    public TaskController(TaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetTasks(CancellationToken cancellationToken)
    {
        var dto = await _taskService.GetTasks(cancellationToken);
        
        return Ok(new AppResponse
        {
            Code = 200,
            Data = dto
        });
    }
    
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetTask(int id, CancellationToken cancellationToken)
    {
        TaskReadDto task;
        try
        {
            task = await _taskService.GetTask(id, cancellationToken);
        }
        catch (TaskNotFoundException)
        {
            return NotFound(new AppResponse
            {
                Code = 404,
                Message = "Task not found"
            });
        }
        
        return Ok(new AppResponse
        {
            Code = 200,
            Data = task
        });
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateTask(TaskCreateDto taskCreateDto, CancellationToken cancellationToken)
    {
        try
        {
            await _taskService.CreateTask(taskCreateDto, cancellationToken);
        }
        catch (UserNotFoundException)
        {
            return NotFound(new AppResponse
            {
                Code = 404,
                Message = "User not found"
            });
        }
        
        return Ok(new AppResponse
        {
            Code = 201,
            Message = "Task created successfully"
        });
    }
    
    [HttpPut("{id:int}/update")]
    public async Task<IActionResult> UpdateTask(TaskUpdateDto taskUpdateDto, int id, CancellationToken cancellationToken)
    {
        try
        {
            await _taskService.UpdateTask(taskUpdateDto, id, cancellationToken);
        }
        catch (TaskNotFoundException)
        {
            return NotFound(new AppResponse
            {
                Code = 404,
                Message = "Task not found"
            });
        }
        
        return Ok(new AppResponse
        {
            Code = 200,
            Message = "Task updated successfully"
        });
    }
    
    [HttpPut("{id:int}/reassign/{assignedToId}")]
    public async Task<IActionResult> ReassignTask(int id, string assignedToId, CancellationToken cancellationToken)
    {
        try
        {
            await _taskService.ReassignTask(id, assignedToId, cancellationToken);
        }
        catch (TaskNotFoundException)
        {
            return NotFound(new AppResponse
            {
                Code = 404,
                Message = "Task not found"
            });
        }
        catch (UserNotFoundException)
        {
            return NotFound(new AppResponse
            {
                Code = 404,
                Message = "User not found"
            });
        }
        
        return Ok(new AppResponse
        {
            Code = 200,
            Message = "Task reassigned successfully"
        });
    }
    
    [HttpPut("{id:int}/status/{status}")]
    public async Task<IActionResult> CompleteTask(int id, Status status, CancellationToken cancellationToken)
    {
        try
        {
            await _taskService.CompleteTask(id, status, cancellationToken);
        }
        catch (TaskNotFoundException)
        {
            return NotFound(new AppResponse
            {
                Code = 404,
                Message = "Task not found"
            });
        }
        
        return Ok(new AppResponse
        {
            Code = 200,
            Message = "Task status updated successfully"
        });
    }
    
    [HttpDelete("{id:int}/delete")]
    public async Task<IActionResult> DeleteTask(int id, CancellationToken cancellationToken)
    {
        try
        {
            await _taskService.DeleteTask(id, cancellationToken);
        }
        catch (TaskNotFoundException)
        {
            return NotFound(new AppResponse
            {
                Code = 404,
                Message = "Task not found"
            });
        }
        
        return Ok(new AppResponse
        {
            Code = 200,
            Message = "Task deleted successfully"
        });
    }
}