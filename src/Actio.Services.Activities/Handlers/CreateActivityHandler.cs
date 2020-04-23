using System.Threading.Tasks;
using Actio.Common.Commands;
using RawRabbit;
using System;
using Actio.Common.Events;
using Actio.Services.Activities.Services;
using Microsoft.Extensions.Logging;
using Actio.Common.Exeptions;

namespace Actio.Services.Activities.Handlers
{
    public class CreateActivityHandler : ICommandHandler<CreateActivity>
    {
        private readonly IBusClient _busClient;
        private readonly IActivityService _activityService;
        private readonly ILogger _logger;

        public CreateActivityHandler(IBusClient busClient, IActivityService activityService,
            ILogger<CreateActivityHandler> logger)
        {
            _logger = logger;
            _busClient = busClient;
            _activityService = activityService;
        }
        public async Task HandleAsync(CreateActivity command)
        {
            _logger.LogInformation($"Creating Activity : '{command.Id}' for user: '{command.UserId}'");

            try
            {
                await _activityService.AddAsync(command.Id, command.UserId,
                    command.Category, command.Name, command.Description, command.CreatedAt);
                await _busClient.PublishAsync(new ActivityCreated(command.Id,
                    command.UserId,
                    command.Category,
                    command.Name,
                    command.Description,
                    command.CreatedAt
                ));
                _logger.LogInformation($"Activity {command.Id} was create for user {command.UserId}");
                return;
            }
            catch (ActioExeption ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateActivityRejected(command.Id, ex.Message, ex.Code));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateActivityRejected(command.Id, ex.Message, "error"));
            }
        }
    }
}