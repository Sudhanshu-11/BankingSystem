using Banking_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking_System.UsersData
{
     public interface IUsersData
    {
        List<Users> GetAllUsers();
        Users GetUsers(Guid id);
        List<UserAccounts> GetAllUserAccount();
        UserAccounts AddUserAccount(UserAccounts userAccounts);
        void DeleteUserAccount(UserAccounts userAccounts);
        UserAccounts UpdateUserAccount(UserAccounts userAccounts);

        UserAccounts GetUserAccountByID(Guid id);
    }
}
