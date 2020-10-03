using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ComplianceTool.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public object getSearchResult(searchObject searchText)
        {
            var currentPath = System.IO.Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).Parent.FullName + "\\skillMasterLog" + DateTime.Now.ToString("dd-MMM-yy-hh-mm-ss") + ".txt";
            string connetionString, queryString;
            IList<searchResultSet> searchResultSets = new List<searchResultSet>();
            IList<countResult> countResults = new List<countResult>();
            consolidateResult consolidateResult = new consolidateResult();
            SqlDataReader reader;
            connetionString = @"Server=LAPTOP-ROGMLOT7\SQLEXPRESS;Database=ComplianceTool;Trusted_Connection=True;";
            queryString = "GetSearchText";
            using (SqlConnection connection = new SqlConnection(
              connetionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add("@orgId", SqlDbType.VarChar, 100).Value = searchText.orgId;
                command.Parameters.Add("@folId", SqlDbType.VarChar, 100).Value = searchText.folId;
                command.Parameters.Add("@searchText", SqlDbType.VarChar, 100).Value = searchText.searchText;

                command.Connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    searchResultSet searchResultSet = new searchResultSet();
                    searchResultSet.content = reader["content"].ToString();
                    searchResultSet.folId = Convert.ToInt32(reader["folId"]);
                    searchResultSet.folName = reader["folName"].ToString();
                    searchResultSet.orgId = Convert.ToInt32(reader["orgId"]);
                    searchResultSet.orgName = reader["orgName"].ToString();
                    searchResultSet.parentId = Convert.ToInt32(reader["parentId"]);
                    searchResultSet.title = reader["title"].ToString();
                    searchResultSets.Add(searchResultSet);
                }
                reader.NextResult();
                while (reader.Read())
                {
                    countResult countResult = new countResult();
                    countResult.count = Convert.ToInt32(reader["count"]);
                    countResult.name = reader["name"].ToString();
                    countResult.parentId = Convert.ToInt32(reader["parentId"]);
                    countResult.id = reader["Id"].ToString();
                    countResults.Add(countResult);
                }
            }
            consolidateResult.countResult = countResults;
            consolidateResult.searchResultSet = searchResultSets;
            return consolidateResult;
        }
    }

}
