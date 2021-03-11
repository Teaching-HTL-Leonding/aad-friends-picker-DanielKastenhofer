using AadFriendsPickerAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AadFriendsPickerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendsController : ControllerBase
    {

        FriendsContext context;
        public FriendsController(FriendsContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult GetAllFriends()
        {
            var friends = context.Friends.ToList();
            return Ok(friends);
        }

        [HttpPatch]
        public IActionResult StoreFriends([FromBody] List<Friend> friends)
        {
            context.Friends.AddRange(friends);
            return Created("", friends);
        }

        [HttpDelete]
        public IActionResult DeleteFriend([FromBody] Friend friend)
        {
            var deleted = context.Friends.Remove(friend);
            return Ok(deleted);
        }
    }
}
