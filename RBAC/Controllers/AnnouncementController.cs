using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Dto;
using Service.Contract;
using Service.Implementation;

namespace RBAC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {
        private readonly IAnnouncementService _announcementService;
        public AnnouncementController(
            IAnnouncementService announcementService
            ) {
        _announcementService = announcementService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost("add-announcement")]
        public async Task<IActionResult> AddAnnouncement(List<AddAnnouncementsDto> announcements)
        {
            var res = await _announcementService.AddAnnouncements(announcements);
            return Ok(new { message = "announcements created successfully" });
        }

        [HttpGet("get-announcements/{userId}")]
        public async Task<ActionResult<List<AddAnnouncementsDto>>> GetAllAnouncements(string userId)
        {
            var res = await _announcementService.GetAllAnouncements(userId);

            if (res == null) return NotFound();

            return Ok(res);
        }

        [HttpDelete("delete-announcement/{announcementId}")]
        public async Task<IActionResult> DeleteAnnouncements(int announcementId)
        {
            var res = await _announcementService.DeleteAnnouncements(announcementId);
            if(!res) return NotFound();

            return Ok(new {message = "user deleted successfully"});
        }

        [HttpPut("edit-announcement")]
        public async Task<ActionResult<EditAnnouncementDto>> EditAnnouncement( EditAnnouncementDto editAnnouncement)
        {
            var res = await _announcementService.EditAnnouncement(editAnnouncement);
            return res;
        }

        [HttpGet("all-announcemets-forusers/{userId}")]
        public async Task<List<UserNotificationDto>> GetAllAnnouncementsForUser(string userId)
        {
            var res = await _announcementService.GetAllAnnouncementsForUser(userId);
            return res;
        }

        [HttpPost("markAsRead")]
        public async Task<IActionResult> SeenNotification(MarkAsReadDto markAsRead)
        {
            
            var res = await _announcementService.SeenNotification(markAsRead.UserId);
            if(!res) return NotFound();
            return Ok(new { message = "Marked as read" });
        }
    }
}
