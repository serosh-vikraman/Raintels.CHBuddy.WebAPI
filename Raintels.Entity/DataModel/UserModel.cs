using System;
using System.Collections.Generic;
using System.Text;

namespace Raintels.Entity.DataModel
{
    public class UserModel
    {
        public int? UserId { get; set; }
        public string GoogleID { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
    }
}
