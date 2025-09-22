using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Service.Contract
{
    public interface ITaskService
    {
        Task<AddTaskDto> AddTask(AddTaskDto newtask);
        Task<List<AddTaskDto>> GetAllTasks(string loggenInUserId);
        Task<bool> DeleteTask(int taskId);
        Task<EditTaskDto> EditTask(EditTaskDto editedTask);
        Task<List<AddTaskDto>> GetTaskByStatus(string userId, TStatus status);

        Task<bool> UpdateStatus(UpdateTaskDt updateTask);

    }
}
