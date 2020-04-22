﻿using Actio.Common.Commands;
using Actio.Common.Events;
using Actio.Common.Exeptions;
using Actio.Services.Identity.Services;
using Microsoft.Extensions.Logging;
using RawRabbit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Identity.Handlers
{
    public class CreateUserHandler : ICommandHandler<CreateUser>
    {
        private readonly ILogger _logger;
        private readonly IBusClient _busClient;
        private readonly IUserService _userService;

        public CreateUserHandler(ILogger<CreateUser> logger, IBusClient busClient, IUserService userService)
        {
            _logger = logger;
            _busClient = busClient;
            _userService = userService;
        }



        public async Task HandleAsync(CreateUser command)
        {
            _logger.LogInformation($"Creating User : '{command.Email}' with name : '{command.Name}'");
            try
            {
                await _userService.RegisterAsync(command.Email, command.Password, command.Name);
                await _busClient.PublishAsync(new UserCreated(command.Email, command.Name));
                _logger.LogInformation($"User '{command.Email}' was created with name : '{command.Name}'");
                return;
            }
            catch (ActioExeption ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateUserRejected(command.Email, ex.Message, ex.Code));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _busClient.PublishAsync(new CreateUserRejected(command.Email, ex.Message, "Error"));
            }
        }
    }
}
