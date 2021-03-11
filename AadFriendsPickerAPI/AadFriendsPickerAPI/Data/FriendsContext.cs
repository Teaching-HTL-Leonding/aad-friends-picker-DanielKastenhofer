using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AadFriendsPickerAPI.Data
{
    public class FriendsContext : DbContext
    {

        public DbSet<Friend> Friends { get; set; }
        public FriendsContext(DbContextOptions<FriendsContext> options)
            :base(options)
        { }

        public async Task<Friend> AddFriend(Friend newFriend)
        {
            Friends.Add(newFriend);
            await SaveChangesAsync();
            return newFriend;
        }
        public async Task RemoveFriend(Friend friendToRemove)
        {
            Friends.Remove(friendToRemove);
            await SaveChangesAsync();
        }

    }
}
