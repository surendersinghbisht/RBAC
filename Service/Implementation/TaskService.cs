using Data.DbContext;
using Data.Entity;
using Microsoft.EntityFrameworkCore;
using Model.Dto;
using Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Service.Implementation
{
    public class TaskService: ITaskService
    {
        private readonly IdentityDbContext _context;
        public TaskService(IdentityDbContext context) {
            _context = context;
        }

        public async Task<AddTaskDto> AddTask(AddTaskDto model)
        {
            var newtask = new UserTask
            {
                AssignedById = model.AssignedById,
                AssignedToId = model.AssignedToId,
                createdOn = DateTime.Now,
                Description = model.Description,
                Status = model.Status,
                Title = model.Title,
                DueDate=model.DueDate,
            };
            await _context.userTask.AddAsync(newtask);
            await _context.SaveChangesAsync();
            return model;
        }

        public async Task<List<AddTaskDto>> GetAllTasks(string loggenInUserId)
        {
            
            var tasks = await _context.userTask.
                Where(t => t.AssignedById == loggenInUserId || t.AssignedToId == loggenInUserId).
                Include(t => t.AssignedBy).
                Include(t => t.AssignedTo).
                Select(t => new AddTaskDto
                {
                    AssignedById = t.AssignedById,
                    AssignedToId = t.AssignedToId,
                    createdOn = t.createdOn,
                    Description = t.Description,
                    Status = t.Status,
                    Title = t.Title,
                    DueDate = t.DueDate,
                    AssignedTouserName = t.AssignedTo.UserName,
                    TaskId = t.Id
                }).ToListAsync();

            return tasks;
        }

       public async Task<bool> DeleteTask(int taskId)
        {
            var task = await _context.userTask.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null)
                return false;
            _context.userTask.Remove(task);
           await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EditTaskDto> EditTask(EditTaskDto editedtask)
        {
            var task = await _context.userTask.FirstOrDefaultAsync(t => t.Id == editedtask.TaskId);
            task.AssignedToId = editedtask.AssignedToId;
            task.Title = editedtask.Title;
            task.Description = editedtask.Description;
            task.DueDate = editedtask.DueDate;
          await _context.SaveChangesAsync();

            return editedtask;
        }

        public async Task<List<AddTaskDto>> GetTaskByStatus(string userId, TStatus status)
        {
            int dbStatus = status switch
            {
                TStatus.Done => 2,    // DB stores Done as 2
                TStatus.Pending => 0, // DB stores Pending as 0
                TStatus.Working => 1, // DB stores Working as 1
                _ => throw new ArgumentException("Invalid status")
            };

            var tasks = await _context.userTask.
                Where(t => t.AssignedToId == userId && (int)t.Status == dbStatus).
                Include(t => t.AssignedBy).
                Select(t => new AddTaskDto
                {
                    AssignedById = t.AssignedById,
                    AssignedToId = t.AssignedToId,
                    createdOn = t.createdOn,
                    Description = t.Description,
                    AssignedByuserName = t.AssignedBy.UserName,
                    Status = t.Status,
                    Title = t.Title,
                    DueDate = t.DueDate,
                    AssignedTouserName = t.AssignedTo.UserName,
                    TaskId = t.Id
                }
                ).ToListAsync();

            return tasks;
        }

        public async Task<bool> UpdateStatus(UpdateTaskDt updateTask)
        {
            var task = await _context.userTask.
                FirstOrDefaultAsync(t => t.Id == updateTask.TaskId);

            task.Status = updateTask.status;
            await _context.SaveChangesAsync();

            var activity = new RecentActivity
            {
                CreatedAt = DateTime.UtcNow,
                Name = $"{task.Status} Task",
                Message = $"{task.Title}",
                UserId = task.AssignedToId
            };

           await _context.activity.AddAsync(activity);
           await _context.SaveChangesAsync();
            return true;              
        }

        public async Task<List<RecentActivity>> GetActivities(string userId)
        {
            var activities = await _context.activity.Where( 
                t => t.UserId == userId).OrderByDescending(t => t.CreatedAt).
                Take(3).ToListAsync();

            return activities;

        }

    }
}
