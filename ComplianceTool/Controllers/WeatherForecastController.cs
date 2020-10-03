using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ComplianceTool.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public object Get()
        {
            return getSearchResult(null);
        }
        public class OtpData
        {
            //[JsonPropertyName("orgId")]
            public string orgId { get; set; }

            //[JsonPropertyName("folId")]
            public string folId { get; set; }

            //[JsonPropertyName("searchText")]
            public string searchText { get; set; }

        }
        [HttpPost]
        public object getSearchResult(dynamic title)
        {
            var data = JsonConvert.SerializeObject(title, Formatting.Indented);

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
                command.Parameters.Add("@orgId", SqlDbType.VarChar,100).Value = "1";
                command.Parameters.Add("@folId", SqlDbType.VarChar,100).Value = "1";
                command.Parameters.Add("@searchText", SqlDbType.VarChar,100).Value = "is";

                command.Connection.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    searchResultSet searchResultSet = new searchResultSet();
                    searchResultSet.content = reader["content"].ToString();
                    searchResultSet.folId = Convert.ToInt32( reader["folId"]);
                    searchResultSet.folName = reader["folName"].ToString();
                    searchResultSet.orgId = Convert.ToInt32( reader["orgId"]);
                    searchResultSet.orgName = reader["orgName"].ToString();
                    searchResultSet.parentId = Convert.ToInt32( reader["parentId"]);
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
            //if(title.title.ToString() == "1")
            //{
            //    return consolidateResult;
            //}
            //else
            //{
            //    return null;
            //}
            return consolidateResult;
        }
    }

    public class searchResultSet
    {
        public int folId { get; set; }
        public string folName { get; set; }
        public int parentId { get; set; }
        public string orgName { get; set; }
        public int orgId { get; set; }
        public string title { get; set; }
        public string content { get; set; }
    }

    public class countResult
    {
        public int count { get; set; }
        public string name { get; set; }
        public int parentId { get; set; }
        public string id { get; set; }
    }

    public class consolidateResult
    {
        public IList<searchResultSet> searchResultSet { get; set; }
        public IList<countResult> countResult { get; set; }
    }

    public class searchObject
    {
        public string orgId { get; set; }
        public string folId { get; set; }
        public string searchText { get; set; }
    }
}
