using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using LMS.Application.Common.Interfaces;

namespace LMS.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest> : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    private readonly ILogger _logger;
    private readonly IUser _user;
    private readonly IIdentityService _identityService;

    public LoggingBehaviour(ILogger<TRequest> logger, IUser user, IIdentityService identityService)
    {
        _logger = logger;
        _user = user;
        _identityService = identityService;
    }

    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _user.Id;
        string? userName = string.Empty;

        if (userId > 0)
        {
            userName = await _identityService.GetUserNameAsync(userId);
        }

        _logger.LogInformation("LMS Request: {Name} {@UserId} {@UserName} {@Request}",
            requestName, userId, userName, request);
    }
}
