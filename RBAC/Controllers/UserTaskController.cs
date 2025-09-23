using Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Dto;
using Service.Contract;
using Utility;

namespace RBAC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        private readonly ITaskService _taskService;
        public UserTaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("create-task")]
        public async Task<ActionResult<AddTaskDto>> AddTask(AddTaskDto taskDetails)
        {

           var res = await _taskService.AddTask(taskDetails);
            return Ok(res);
        }

        
        [HttpGet("get-tasks/{loggenInUserId}")]
        public async Task<ActionResult<AddTaskDto>> GetAllTasks(string loggenInUserId)
        {
            var res = await _taskService.GetAllTasks(loggenInUserId);
            return Ok(res);
        }

        [Authorize(Roles ="Admin")]
        [HttpDelete("delete-task/{taskId}")]
        public async Task<ActionResult> DeleteTask(int taskId) { 
        var res = await _taskService.DeleteTask(taskId);
            return Ok(res);
        }

        [HttpPut("edit-task")]
        public async Task<ActionResult<EditTaskDto>> EditTask(EditTaskDto editedData)
        {
            var res = await _taskService.EditTask(editedData);
            return Ok(res);
        }

        [HttpGet("user-tasks-by-status")]
        public async Task<ActionResult<AddTaskDto>> GetTasksByStatus([FromQuery] string userId, [FromQuery] TStatus status)
        {
            var tasks = await _taskService.GetTaskByStatus(userId, status);
            return Ok(tasks);
        }

        [HttpPut("update-task-status")]
        public async Task<IActionResult> UpdateStatus(UpdateTaskDt updateTask)
        {
            var res = await _taskService.UpdateStatus(updateTask);
           return Ok();
        }

        [HttpGet("recent-activities/{userId}")]
        public async Task<List<RecentActivity>> GetActivities(string userId)
        {
            var res = await _taskService.GetActivities(userId);
            return res;
        }
    }
}
