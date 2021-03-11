using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AadFriendsPickerAPI.Data
{
    public class FriendsContext : DbContext
    {

        public DbSet<FriendShip> FriendShips { get; set; }
        public FriendsContext(DbContextOptions<FriendsContext> options)
            :base(options)
        { }

        public async Task<FriendShip> AddFriend(FriendShip newFriend)
        {
            FriendShips.Add(newFriend);
            await SaveChangesAsync();
            return newFriend;
        }
        public async Task RemoveFriend(FriendShip friendToRemove)
        {
            FriendShips.Remove(friendToRemove);
            await SaveChangesAsync();
        }

    }
}
