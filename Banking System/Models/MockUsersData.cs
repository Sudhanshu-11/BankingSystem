using Banking_System.UsersData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Banking_System.Models
{
    public class MockUsersData : IUsersData
    {
        private List<UserAccounts> usersAcc = new List<UserAccounts>();
        private List<Users> users = new List<Users>()
        {
            new Users()
            {
                Id=Guid.NewGuid(),
                Name="Sudhanshu"
            },
            new Users()
            {
                 Id=Guid.NewGuid(),
                Name="Himanshu"
            }
        };
        List<Users> IUsersData.GetAllUsers()
        {
            return users;
        }
        Users IUsersData.GetUsers(Guid id)
        {
            return users.FirstOrDefault(x => x.Id == id);
        }
        UserAccounts IUsersData.AddUserAccount(UserAccounts userAccounts)
        {
            userAccounts.Id =  Guid.NewGuid();
            usersAcc.Add(userAccounts);
            return userAccounts;
        }

        void IUsersData.DeleteUserAccount(UserAccounts userAccounts)
        {
            usersAcc.Remove(userAccounts);
        }

        List<UserAccounts> IUsersData.GetAllUserAccount()
        {
            return usersAcc;
        }                   

        UserAccounts IUsersData.UpdateUserAccount(UserAccounts userAccounts)
        {
            var getExisitingAcc = getauseracc(userAccounts.Id);
            getExisitingAcc.WithdrawlAmt = getExisitingAcc.WithdrawlAmt + userAccounts.WithdrawlAmt;
            getExisitingAcc.DepositAmt = getExisitingAcc.DepositAmt + userAccounts.DepositAmt;
            getExisitingAcc.AvailableBalance = getExisitingAcc.DepositAmt - getExisitingAcc.WithdrawlAmt;
            return getExisitingAcc;
        }

        UserAccounts IUsersData.GetUserAccountByID(Guid id)
        {
            return usersAcc.FirstOrDefault(x => x.Id == id);
        }
        public  UserAccounts getauseracc(Guid id)
        {
            return usersAcc.FirstOrDefault(x => x.Id == id);
        }
    }
}
