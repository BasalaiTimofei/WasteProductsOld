using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WasteProducts.Logic.Common.Models.Groups;
using WasteProducts.Logic.Common.Services.Groups;

namespace WasteProducts.Web.Controllers.Api.Groups
{
    [RoutePrefix("group")]
    public class GroupController : ApiController
    {
        private IGroupService _groupService;
        private Group _group;
        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        public IEnumerable<Group> Get(string userId)
        {
            return _groupService.GetIds<Group>(userId);
        }

        [Route("{id}")]
        [HttpGet]
        public Group Get(Guid id)
        {
            return _groupService.FindById<Group>(id);
        }

        [HttpPost]
        public void Create(string userId, string name, string information)
        {
            _group.Name = name;
            _group.Information = information;
            _group.AdminId = userId;
            _groupService.Create(_group);
        }
        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, string name, string information, string userId)
        {
            _group.Id = id;
            _group.Name = name;
            _group.Information = information;
            _group.AdminId = userId;
            _groupService.Update(_group);
        }

        [HttpDelete]
        public void Delete(Guid id, string userId)
        {
            _group.Id = id;
            _group.AdminId = userId;
            _groupService.Delete(_group);
        }
    }
}