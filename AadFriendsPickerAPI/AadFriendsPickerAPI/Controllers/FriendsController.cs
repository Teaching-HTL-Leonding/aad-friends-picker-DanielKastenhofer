using AadFriendsPickerAPI.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
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

        private readonly FriendsContext _context;
        public FriendsController(FriendsContext context) => _context = context;

        public record AddFriendModel(string FriendId);
        public record RemoveFriendModel(string FriendId);

        public async Task<IEnumerable<string>> GetAllFriends()
        {
            var userId = HttpContext.User.Claims.First(c => c.Type == ClaimConstants.ObjectId).Value;
            var friendShips = await _context.FriendShips.Where(f => f.FirstUserId == userId || f.SecondUserId == userId).ToArrayAsync();
            return friendShips.Select(f => (f.FirstUserId == userId) ? f.SecondUserId : f.FirstUserId);
        }

        [Route("getAll")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await GetAllFriends());
        }

        [Route("add")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFriend([FromBody] AddFriendModel model)
        {
            var friendId = model.FriendId;
            // Check if the user is not already friends with the given person
            if ((await GetAllFriends()).All(f => f != friendId))
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                var friendShip = new FriendShip
                {
                    FirstUserId = HttpContext.User.Claims.First(c => c.Type == ClaimConstants.ObjectId).Value,
                    SecondUserId = friendId
                };
                await _context.FriendShips.AddAsync(friendShip);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            return await GetAll();
        }

        [Route("remove")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFriend([FromBody] RemoveFriendModel model)
        {
            var friendId = model.FriendId;
            var userId = HttpContext.User.Claims.First(c => c.Type == ClaimConstants.ObjectId).Value;
            if ((await GetAllFriends()).Any(f => f == friendId))
            {
                await using var transaction = await _context.Database.BeginTransactionAsync();
                _context.FriendShips.RemoveRange(
                    _context.FriendShips.Where(f => (f.FirstUserId == friendId || f.SecondUserId == friendId) &&
                                                    (f.FirstUserId == userId || f.SecondUserId == userId))
                );
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            return await GetAll();
        }
    }
}
