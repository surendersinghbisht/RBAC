using Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contract
{
    public interface IAnnouncementService
    {
        Task<bool> AddAnnouncements(List<AddAnnouncementsDto> announcements);
        Task<List<AddAnnouncementsDto>> GetAllAnouncements(string userId);

        Task<bool> DeleteAnnouncements(int announcementId);


        Task<EditAnnouncementDto> EditAnnouncement(EditAnnouncementDto announcement);

        Task<List<UserNotificationDto>> GetAllAnnouncementsForUser(string userId);

        Task<bool> SeenNotification(string userId);
    }
}
