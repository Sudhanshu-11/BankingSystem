using Banking_System.Models;
using Banking_System.UsersData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Banking_System.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersData _iUsersData;
        private IHttpContextAccessor http;
        public UsersController(IUsersData iUsersData)
        {
            this._iUsersData = iUsersData;
        }

        [HttpGet]
        [Route("api/[Controller]/GetUsers")]
        public IActionResult GetUsers()
        {
            return Ok(_iUsersData.GetAllUsers());
        }

        [HttpGet]
        [Route("api/[Controller]/[action]/{id}")]
        public IActionResult GetUsersbyId(int id)
        {
            var Users = _iUsersData.GetUsers(id);
            if(Users != null)
            {
                return Ok(Users);
            }
            return NotFound($"Users with Id: {id} was not found");
        }

        [HttpPost]
        [Route("api/[Controller]/[action]")]
        public IActionResult AddUserAccount(UserAccounts useraccounts)
        {
            useraccounts.AvailableBalance = useraccounts.DepositAmt - useraccounts.WithdrawlAmt;
            if (useraccounts.AvailableBalance < 100)
            {
                return new ObjectResult(new ApiError() { Message = "Available balance should be more than 100$" })
                {
                    StatusCode = StatusCodes.Status405MethodNotAllowed
                };
            }
            _iUsersData.AddUserAccount(useraccounts);
            return Ok(); 
        }

        [HttpGet]
        [Route("api/[Controller]/GetAllUsersAccounts")]
        public IActionResult GetAllUsersAccounts()
        {
            return Ok(_iUsersData.GetAllUserAccount());
        }

        [HttpDelete]
        [Route("api/[Controller]/[action]/{id}")]
        public IActionResult DeleteUsersbyId(int id)
        {
            var UsersAcc = _iUsersData.GetUserAccountByID(id);
            if (UsersAcc != null)
            {
                _iUsersData.DeleteUserAccount(UsersAcc);
                return Ok($"UserAccount with Id: {id} deleted");
            }
            return NotFound($"UsersAccounts with Id: {id} was not found");
        }

        [HttpPatch]
        [Route("api/[Controller]/[action]/{id}")]
        public IActionResult UpdateAccountBalance(int id, UserAccounts useraccounts)
        {
            var ExisitingUsersAcc = _iUsersData.GetUserAccountByID(id);
             if (ExisitingUsersAcc == null)
            {
                return new ObjectResult(new ApiError() { Message = $"Account is not valid" })
                {
                    StatusCode = StatusCodes.Status405MethodNotAllowed
                };
            }
            var remainbal = ExisitingUsersAcc.AvailableBalance - useraccounts.WithdrawlAmt;
            var stat= checkavailablebalance(ExisitingUsersAcc, remainbal);
            if (useraccounts.DepositAmt > 10000)
            {
                return new ObjectResult(new ApiError() { Message = "cannot deposit more than $10,000 in a single transaction" })
                {
                    StatusCode = StatusCodes.Status405MethodNotAllowed
                };
            }           
           else if (remainbal < 100)
            {
                return new ObjectResult(new ApiError() { Message = $"Available balance should be more than 100$ You can withdraw only {ExisitingUsersAcc.AvailableBalance-100}$" })
                {
                    StatusCode = StatusCodes.Status405MethodNotAllowed
                };
            }
           
           else if (!stat)
            {
                return new ObjectResult(new ApiError() { Message = "Cannot withdraw more than 90% of their total balance" })
                {
                    StatusCode = StatusCodes.Status405MethodNotAllowed
                };
            }
            else
            {
                useraccounts.Id = id;               
                _iUsersData.UpdateUserAccount(useraccounts);
                return Ok(ExisitingUsersAcc);
            }
           
        }

        public static bool checkavailablebalance(UserAccounts ExisitingUsersAcc, decimal remainingBal)
        {
            bool status = false;
            //find 90 % of available balance
            var finalpercbal= ExisitingUsersAcc.AvailableBalance * 90 / 100;
            var t =  ExisitingUsersAcc.AvailableBalance- finalpercbal;
            if (remainingBal>t)
            {
                status = true;
            }
            else
            {
                status = false;
            }
            return status;
        }

    }
}
public class ApiError {
    public string Message { get; set; } 
}