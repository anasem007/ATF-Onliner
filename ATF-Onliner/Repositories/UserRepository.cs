using System.Collections.Generic;
using System.Linq;
using ATF_Onliner.Context;
using ATF_Onliner.Models;

namespace ATF_Onliner.Repositories
{
    public class UserRepository
    {
        private readonly UserContext _userContext = new UserContext();

        public IList<User> UsersList()
        {
            return _userContext.Users.ToList();
        }
    }
}