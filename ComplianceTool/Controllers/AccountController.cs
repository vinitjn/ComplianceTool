using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using ComplianceTool.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ComplianceTool.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public object registerUser(Account account)
        {
            //var currentPath = System.IO.Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).Parent.FullName + "\\skillMasterLog" + DateTime.Now.ToString("dd-MMM-yy-hh-mm-ss") + ".txt";
            string connetionString, queryString;
            SqlDataReader reader;
            connetionString = @"Server=LAPTOP-ROGMLOT7\SQLEXPRESS;Database=ComplianceTool;Trusted_Connection=True;";
            queryString = "RegisterUser";
            using (SqlConnection connection = new SqlConnection(
              connetionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@firstName", SqlDbType.VarChar, 100).Value = account.firstName;
                command.Parameters.Add("@lastName", SqlDbType.VarChar, 100).Value = account.lastName;
                command.Parameters.Add("@userName", SqlDbType.VarChar, 100).Value = account.userName;
                command.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = account.password;
                command.Parameters.Add("@instituteName", SqlDbType.VarChar, 100).Value = account.instituteName;
                command.Parameters.Add("@contactNumber", SqlDbType.VarChar, 100).Value = account.contactNumber;
                command.Parameters.Add("@businessNumber", SqlDbType.VarChar, 100).Value = account.businessEmail;

                command.Connection.Open();
                reader = command.ExecuteReader();
            }

            return "";
        }

        [AllowAnonymous]
        [HttpPost]
        public Account loginUser(Account account)
        {
            //var currentPath = System.IO.Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).Parent.FullName + "\\skillMasterLog" + DateTime.Now.ToString("dd-MMM-yy-hh-mm-ss") + ".txt";
            string connetionString, queryString;
            Account accountDetails = new Account();
            SqlDataReader reader;
            connetionString = @"Server=LAPTOP-ROGMLOT7\SQLEXPRESS;Database=ComplianceTool;Trusted_Connection=True;";
            queryString = "LoginUser";
            using (SqlConnection connection = new SqlConnection(
              connetionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@userName", SqlDbType.VarChar, 100).Value = account.userName;
                command.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = account.password;

                command.Connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                    accountDetails.businessEmail = reader["businessEmail"].ToString();
                    accountDetails.contactNumber = reader["contactNumber"].ToString();
                    accountDetails.firstName = reader["firstName"].ToString();
                    accountDetails.instituteName = reader["instituteName"].ToString();
                    accountDetails.lastName = reader["lastName"].ToString();
                    accountDetails.userDetailId = reader["userDetailId"].ToString();
                    accountDetails.userName = reader["userName"].ToString();
                }

            }
            return accountDetails;
        }
    }
}
