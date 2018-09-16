using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Web.Controllers.Api.Groups
{
    public class GroupBoardController : BaseApiController
    {
        private readonly IGroupBoardService _groupService;

        public GroupBoardController(IGroupBoardService groupService, ILogger logger) : base(logger)
        {
            _groupService = groupService;
        }

    }
}
