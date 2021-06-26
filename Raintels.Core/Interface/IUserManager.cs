using Raintels.Entity.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raintels.Core.Interface
{
    public interface IUserManager
    {
        int CreateUser(UserModel user);
    }
}
