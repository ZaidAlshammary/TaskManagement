using System.Linq.Expressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Data;
using TaskManagement.Dtos.Tasks;
using TaskManagement.Exceptions;
using TaskManagement.Models;
using TaskManagement.Resolvers;
using Task = System.Threading.Tasks.Task;

namespace TaskManagement.Services;

public class TaskService
{
    private readonly UserManager<User> _userManager;
    private readonly TaskManagementDbContext _context;
    private readonly IUserContextResolver _userContextResolver;

    public TaskService(UserManager<User> userManager, TaskManagementDbContext context, IUserContextResolver userContextResolver)
    {
        _userManager = userManager;
        _context = context;
        _userContextResolver = userContextResolver;
    }
    
    public async Task<List<TaskReadDto>> GetTasks(CancellationToken cancellationToken)
    {
        Expression<Func<Models.Task, bool>> where = t => true;
        
        var user = await _userManager.FindByIdAsync(_userContextResolver.CurrentUserId);
        if (!user.IsAdmin)
        {
            where = t => t.UserId == user.Id;
        }

        var dto = await _context.Tasks.Where(where)
            .Select(t => new TaskReadDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                Assignee = t.User.FullName
            })
            .ToListAsync(cancellationToken: cancellationToken);
        
        return dto;
    }
    
    public async Task<TaskReadDto> GetTask(int id, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.Where(t => t.Id == id)
            .Select(t => new TaskReadDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Status = t.Status,
                Assignee = t.User.FullName,
                AssigneeId = t.UserId
            }).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        
        if (task == null)
        {
            throw new TaskNotFoundException(id);
        }
        
        return task;
    }
    
    public async Task CreateTask(TaskCreateDto taskCreateDto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(taskCreateDto.AssignedTo);
        if (user == null)
        {
            throw new UserNotFoundException(taskCreateDto.AssignedTo);
        }
        
        var task = new Models.Task
        {
            Title = taskCreateDto.Title,
            Description = taskCreateDto.Description,
            DueDate = taskCreateDto.DueDate,
            Status = taskCreateDto.Status,
            UserId = taskCreateDto.AssignedTo
        };
        
        await _context.Tasks.AddAsync(task, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task UpdateTask(TaskUpdateDto taskUpdateDto, int id, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
        if (task == null)
        {
            throw new TaskNotFoundException(id);
        }
        
        task.Title = taskUpdateDto.Title;
        task.Description = taskUpdateDto.Description;
        task.DueDate = taskUpdateDto.DueDate;
        task.Status = taskUpdateDto.Status;
        
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task ReassignTask(int id, string assignedToId, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(assignedToId);
        if (user == null)
        {
            throw new UserNotFoundException(assignedToId);
        }
        
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
        if (task == null)
        {
            throw new TaskNotFoundException(id);
        }
        
        task.UserId = user.Id;
        
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task CompleteTask(int id, Status status, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
        if (task == null)
        {
            throw new TaskNotFoundException(id);
        }
        
        task.Status = status;
        
        await _context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteTask(int id, CancellationToken cancellationToken)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == id, cancellationToken: cancellationToken);
        if (task == null)
        {
            throw new TaskNotFoundException(id);
        }
        
        task.IsDeleted = true;
        
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync(cancellationToken);
    }
}