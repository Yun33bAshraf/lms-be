using System.Text.Json;
using Project.Domain.Entities;
using Project.Application.Common.Interfaces;

namespace Project.Infrastructure.Repositories;

public class NotificationService : INotificationService
{
    private readonly IApplicationDbContext _context;

    public NotificationService(IApplicationDbContext context)
    {
        _context = context;
    }

    //public async Task CreateNotificationAsync(int toUserId, string title, string description, string messageKey,
    //object? messageParams = null, CancellationToken cancellationToken = default)
    //{
    //    var notification = new Notification
    //    {
    //        ToUserId = toUserId,
    //        Title = title,
    //        Description = description,
    //        CreatedBy = toUserId,
    //        TemplateKey = messageKey,
    //        TemplateArgsJson = messageParams != null
    //        ? JsonSerializer.Serialize(messageParams)
    //        : "{}",
    //    };

    //    _context.Notification.Add(notification);
    //    await _context.SaveChangesAsync(cancellationToken);
    //}
}
