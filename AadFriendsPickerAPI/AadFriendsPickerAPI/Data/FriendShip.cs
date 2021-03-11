using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AadFriendsPickerAPI.Data
{
    public class FriendShip
    {
        public int ID { get; set; }
        public string FirstUserId { get; set; }
        public string SecondUserId { get; set; }
    }
}
