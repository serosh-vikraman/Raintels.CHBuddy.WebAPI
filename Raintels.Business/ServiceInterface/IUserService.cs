using Raintels.Entity.DataModel;
using Raintels.Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Raintels.Service.ServiceInterface
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
        //User GetByEmail(string email);
        int CreateUser(UserModel user);
    }
}
