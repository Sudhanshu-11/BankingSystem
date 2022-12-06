using Banking_System.Controllers;
using Banking_System.Models;
using Banking_System.UsersData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace BankingSystemXUnitTest
{
    public class UnitTest1
    {
      //  private readonly IUsersData _iUsersData;
        private readonly Mock<IUsersData> _iUsersData;
        private readonly UsersController _UsersController;
        public UnitTest1()
        {
            this._iUsersData =new Mock<IUsersData> ();
            _UsersController = new UsersController(this._iUsersData.Object);

        }

        [Fact]
        public  void Task_GetUsers_Return_OkResult()
        {
            //Arrange  
          //  Guid id=

            //Act  
            var data = _UsersController.GetUsers();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_GetUsersbyId_Return_NotFoundResult()
        {
            //Arrange  
            int userid = 1;
            //Act  
            var data = _UsersController.GetUsersbyId(userid);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(data);
        }
        [Fact]
        public void Task_GetUsersbyId_Return_OkResult()
        {
            //Arrange  
            Users us = new Users()
            {
                Id = 1,
                Name = "Sudhanshu"
            };
            int userid = 1;
            //Act  
            _iUsersData.Setup(x => x.GetUsers(userid)).Returns(us);
            var data = _UsersController.GetUsersbyId(userid);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_AddUserAccount_Return_OkResult()
        {
            //Arrange  
            UserAccounts us = new UserAccounts()
            {
               UsersId=2,
               UserAccName="sud",
               DepositAmt=200,
               WithdrawlAmt=0,
               AvailableBalance=200,
               AccountCreationDate=DateTime.Now,
               TransactionDate= DateTime.Now
            };          
     
            //Act  
            _iUsersData.Setup(x => x.AddUserAccount(us)).Returns(us);
            var data = _UsersController.AddUserAccount(us);

            //Assert  
            Assert.IsType<OkResult>(data);
        }

        [Fact]
        public void Task_AddUserAccount_AvailableBalance_lessthan100_Return_405()
        {
            //Arrange  
            UserAccounts us = new UserAccounts()
            {
                UsersId = 2,
                UserAccName = "sud",
                DepositAmt = 95,
                WithdrawlAmt = 0,
                AvailableBalance = 95,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };          

            //Act  
            _iUsersData.Setup(x => x.AddUserAccount(us)).Returns(us);
            ObjectResult data = _UsersController.AddUserAccount(us) as ObjectResult;

            //Assert  
            data.ToString().Equals(HttpStatusCode.MethodNotAllowed);
            //Assert.IsType<ObjectResult>(data);
        }

        [Fact]
        public void Task_GetAllUsersAccount_Return_OkResult()
        {
            //Arrange  
           List<UserAccounts> users = new List<UserAccounts>()
        {
            new UserAccounts()
            {
                 UsersId=1,
               UserAccName="abc",
               DepositAmt=200,
               WithdrawlAmt=0,
               AvailableBalance=200,
               AccountCreationDate=DateTime.Now,
               TransactionDate= DateTime.Now
            },
            new UserAccounts()
            {
               UsersId=2,
               UserAccName="sud",
               DepositAmt=200,
               WithdrawlAmt=0,
               AvailableBalance=200,
               AccountCreationDate=DateTime.Now,
               TransactionDate= DateTime.Now
            }
        };
          _iUsersData.Setup(x => x.GetAllUserAccount()).Returns(users);
            //Act  
            var data = _UsersController.GetAllUsersAccounts();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_DeleteUsersbyId_Return_ok()
        {
            //Arrange  
            UserAccounts users = new UserAccounts()
            {
                UsersId = 1,
                UserAccName = "abc",
                DepositAmt = 200,
                WithdrawlAmt = 0,
                AvailableBalance = 200,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };
            int userid = 1;
            _iUsersData.Setup(x => x.GetUserAccountByID(userid)).Returns(users);
            //Act  
            var data = _UsersController.DeleteUsersbyId(userid);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_DelteUsersbyId_Return_NotFoundResult()
        {
            //Arrange  
            UserAccounts users = null;
            int userid = 1;
            _iUsersData.Setup(x => x.GetUserAccountByID(userid)).Returns(users);
            //Act  
            var data = _UsersController.DeleteUsersbyId(userid);

            //Assert  
            Assert.IsType<NotFoundObjectResult>(data);
        }

        [Fact]
        public void Task_UpdateUserAccount_Return_OkResult()
        {
            //Arrange  
            UserAccounts us = new UserAccounts()
            {
                UsersId = 2,
                UserAccName = "sud",
                DepositAmt = 200,
                WithdrawlAmt = 0,
                AvailableBalance = 200,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };

            //Act  
            _iUsersData.Setup(x => x.GetUserAccountByID(us.UsersId)).Returns(us);
            
            var data = _UsersController.UpdateAccountBalance(us.UsersId,us);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public void Task_UpdateUserAccount_ExisitingUsersAccNull_Return_status405()
        {
            //Arrange  
            UserAccounts usAct = new UserAccounts()
            {
                UsersId = 2,
                UserAccName = "sud",
                DepositAmt = 200,
                WithdrawlAmt = 0,
                AvailableBalance = 200,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };
            UserAccounts us = null;
            int userid = 1;
            //Act  
            _iUsersData.Setup(x => x.GetUserAccountByID(userid)).Returns(us);

            var data = _UsersController.UpdateAccountBalance(userid, usAct);

            //Assert  
            data.ToString().Equals(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public void Task_UpdateUserAccount_DepositAmountShouldNotBegreaterThan10k_Return_status405()
        {
            //Arrange  
            UserAccounts usAct = new UserAccounts()
            {
                UsersId = 2,
                UserAccName = "sud",
                DepositAmt = 12000,
                WithdrawlAmt = 0,
                AvailableBalance = 200,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };
            UserAccounts usExc = new UserAccounts()
            {
                UsersId = 2,
                UserAccName = "sud",
                DepositAmt = 200,
                WithdrawlAmt = 0,
                AvailableBalance = 200,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };
            int userid = 1;
            //Act  
            _iUsersData.Setup(x => x.GetUserAccountByID(userid)).Returns(usExc);

            var data = _UsersController.UpdateAccountBalance(userid, usAct);

            //Assert  
            data.ToString().Equals(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public void Task_UpdateUserAccount_remainingBalShouldNotbelessthan100_Return_status405()
        {
            //Arrange  
            UserAccounts usAct = new UserAccounts()
            {
                UsersId = 2,
                UserAccName = "sud",
                DepositAmt = 200,
                WithdrawlAmt = 120,
                AvailableBalance = 80,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };
            UserAccounts usExc = new UserAccounts()
            {
                UsersId = 2,
                UserAccName = "sud",
                DepositAmt = 200,
                WithdrawlAmt = 0,
                AvailableBalance = 200,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };
            int userid = 1;
            //Act  
            _iUsersData.Setup(x => x.GetUserAccountByID(userid)).Returns(usExc);

            var data = _UsersController.UpdateAccountBalance(userid, usAct);

            //Assert  
            data.ToString().Equals(HttpStatusCode.MethodNotAllowed);
        }

        [Fact]
        public void Task_UpdateUserAccount_UserCannotWithdrawMoreThan90Per_Return_status405()
        {
            //Arrange  
            UserAccounts usAct = new UserAccounts()
            {
                UsersId = 2,
                UserAccName = "sud",
                DepositAmt = 0,
                WithdrawlAmt = 1900,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };
            UserAccounts usExc = new UserAccounts()
            {
                UsersId = 2,
                UserAccName = "sud",
                DepositAmt = 2000,
                WithdrawlAmt = 0,
                AvailableBalance = 2000,
                AccountCreationDate = DateTime.Now,
                TransactionDate = DateTime.Now
            };
            int userid = 1;
            //Act  
            _iUsersData.Setup(x => x.GetUserAccountByID(userid)).Returns(usExc);

            var data = _UsersController.UpdateAccountBalance(userid, usAct);

            //Assert  
            data.ToString().Equals(HttpStatusCode.MethodNotAllowed);
        }
    }
}
