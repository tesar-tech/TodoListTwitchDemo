﻿using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Newtonsoft.Json;
using Todo.AzureFunctions.Entities;
using Todo.AzureFunctions.Factories;
using Todo.AzureFunctions.Services.Interfaces;
using Todo.Shared.Constants;
using Todo.Shared.Dto;

namespace Todo.AzureFunctions.Functions
{
    public class UpdateTodoItemFunction
    {
        private readonly CloudTable _cloudTable;
        private readonly IMapper _mapper;
        private readonly IAuthService _authService;

        public UpdateTodoItemFunction(ICloudTableFactory cloudTableFactory, IMapper mapper, IAuthService authService)
        {
            _mapper = mapper;
            _authService = authService;
            _cloudTable = cloudTableFactory.CreateCloudTable();
        }

        [FunctionName(FunctionConstants.UpdateTodoItemFunction)]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = null)]
            HttpRequest req)
        {
            var user = _authService.GetClientPrincipalFromRequest(req);

            var listId = user.UserId;

            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JsonConvert.DeserializeObject<UpdateTodoItemDto>(requestBody);
            var entity = _mapper.Map<TodoListEntity>(data);
            entity.PartitionKey = listId;
            entity.ETag = "*";

            _cloudTable.Execute(TableOperation.Merge(entity));

            return new OkResult();
        }

    }
}
