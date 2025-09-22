using Data.DbContext;
using Data.Entity;
using Data.Migrations;
using Microsoft.EntityFrameworkCore;
using Model.Dto;
using Service.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementation
{
    public class AnnouncementService: IAnnouncementService
    {
        private readonly IdentityDbContext _context;
        public AnnouncementService(
            IdentityDbContext context
            ) {
            _context = context;
        }

        public async Task<bool> AddAnnouncements(List<AddAnnouncementsDto> announcements)
        {
            var res = new List<Announcements>();
            var users = await _context.Users.ToListAsync();
            var userNotifications = new List<UserNotification>();
            foreach (var announcement in announcements)
            {
                var singleAnn = new Announcements
                {
                    CreatedAt = DateTime.Now,
                    CreatedBy = announcement.createdBy,
                    Message = announcement.Message,
                    Title = announcement.Title,
                };

                res.Add(singleAnn);
            }
            await _context.Announcements.AddRangeAsync(res);
            await _context.SaveChangesAsync();

            foreach (var announcement in res)
          {
                foreach (var user in users)
                {
                    userNotifications.Add(new UserNotification
                    {
                        AnnouncementId = announcement.Id,
                        IsRead = false,
                        Message = announcement.Message,
                        Title = announcement.Title,
                        UserId = user.Id,
                    });
                }
            }

            await _context.userNotifications.AddRangeAsync(userNotifications);
            await _context.SaveChangesAsync();
         
            return true;
        }

        public async Task<List<AddAnnouncementsDto>> GetAllAnouncements(string userId)
        {
            var announcement = await _context.Announcements.Where(a => a.CreatedBy == userId).
                Select(a => 
           new AddAnnouncementsDto
           {
                AnnouncementId = a.Id,
                createdBy = a.CreatedBy,
                CreatedAt = a.CreatedAt,
                Message = a.Message,
                Title = a.Title
            }
            ).ToListAsync();

            return announcement;
        }

        public async Task<bool> DeleteAnnouncements(int announcementId)
        {
            var data = await _context.Announcements
                                     .FirstOrDefaultAsync(a => a.Id == announcementId);

            if (data == null)
                return false; 

            _context.Announcements.Remove(data);
            await _context.SaveChangesAsync(); 

            return true;
        }

        public async Task<EditAnnouncementDto> EditAnnouncement(EditAnnouncementDto announcementDto)
        {
            var announcement = await _context.Announcements.FirstOrDefaultAsync(a => a.Id == announcementDto.AnnouncementId);
            announcement.Title = announcementDto.Title;
            announcement.Message = announcementDto.Message;
           
            await _context.SaveChangesAsync();
            return announcementDto;
        }

        public async Task<List<UserNotificationDto>> GetAllAnnouncementsForUser(string userId)
        {
            var res = await _context.userNotifications.
                Where(n => n.UserId == userId && n.IsRead == false)
                .Select(a => new UserNotificationDto
            {
                AnnouncementId = a.Id,
                IsRead = a.IsRead,
                Message = a.Message,
                Title = a.Title,
                NotificationId = a.Id,
                UserId = a.UserId
            }).ToListAsync();

            return res;
        }

       public async Task<bool> SeenNotification(string userId)
        {
            var notifications = await _context.userNotifications.Where(n => n.UserId == userId && !n.IsRead).ToListAsync();

            notifications.ForEach(n => {
                n.IsRead = true;
                n.SeenAt = DateTime.UtcNow;
            });

            await _context.SaveChangesAsync();

            return true;
        }

    }
}
