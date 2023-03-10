using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using TestTechnical.Models;

namespace TestTechnical.Controllers
{
    public class TestController : Controller
    {
        private string mainconn = ConfigurationManager.ConnectionStrings["TestConnection"].ConnectionString;
        // GET: Test
        public ActionResult Index()
        {
            // Creating connection to the database
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string query = "SELECT * FROM [TESTDB].[dbo].[TEST_SCHEDULE_TBL]";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            
            // Opening the connection
            sqlconn.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            List<TestModel> schedule = new List<TestModel>();

            // Fill the model with the ds
            foreach (DataRow dr in ds.Tables[0].Rows) {
                schedule.Add(new TestModel
                {
                    id = Convert.ToInt32(dr["ID"]),
                    title = dr["TITLE"].ToString(),
                    Category = dr["CATEGORY"].ToString(),
                    Start_date = Convert.ToDateTime(dr["TESTDATE"]),
                    Location = dr["LOCATION"].ToString(),
                    Description = dr["DESCRIPTION"].ToString()
                });
            }

            // Closing the connection
            sqlconn.Close();

            return View(schedule);
        }

        [HttpPost]
        public ActionResult Index(TestModel model) {
            // Creating connection to the database
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string title = "%" + model.title + "%";
            string query = "SELECT * FROM [TESTDB].[dbo].[TEST_SCHEDULE_TBL] WHERE TITLE LIKE @title AND CATEGORY = @category AND TESTDATE BETWEEN @start_date and @end_date";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@title", title);
            cmd.Parameters.AddWithValue("@category", model.Category);
            cmd.Parameters.AddWithValue("@start_date", model.Start_date);
            cmd.Parameters.AddWithValue("@end_date", model.End_date);

            // Opening the connection
            sqlconn.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            List<TestModel> schedule = new List<TestModel>();

            // Fill the model with the ds
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                schedule.Add(new TestModel
                {
                    id = Convert.ToInt32(dr["ID"]),
                    title = dr["TITLE"].ToString(),
                    Category = dr["CATEGORY"].ToString(),
                    Start_date = Convert.ToDateTime(dr["TESTDATE"]),
                    Location = dr["LOCATION"].ToString(),
                    Description = dr["DESCRIPTION"].ToString()
                });
            }

            // Closing the connection
            sqlconn.Close();
            return View(schedule);
        }

        [HttpGet]
        public ActionResult form_page(TestModel model)
        {
            if (model == null)
            {
                ViewData["id"] = null;
                ViewData["title_schedule"] = null;
                ViewData["Category"] = null;
                ViewData["Date"] = null;
                ViewData["Location"] = null;
                ViewData["Description"] = null;
                return View();
            }
            else
            {
                ViewData["id"] = model.id;
                ViewData["title_schedule"] = model.title;
                ViewData["Category"] = model.Category;
                ViewData["Date"] = Convert.ToDateTime(model.Start_date).ToShortDateString();
                ViewData["Location"] = model.Location;
                ViewData["Description"] = model.Description;
                return View();
            }
        }

        public JsonResult DeleteData(TestModel model)
        {
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string query = "DELETE [TESTDB].[dbo].[TEST_SCHEDULE_TBL] WHERE ID = @id";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@id", model.id);
            sqlconn.Open();
            cmd.ExecuteNonQuery();
            sqlconn.Close();
            return Json(true);
        }

        public JsonResult SaveorUpdate(TestModel model) {
            // Creating connection to the database
            SqlConnection sqlconn = new SqlConnection(mainconn);
            string query = "SELECT * FROM [TESTDB].[dbo].[TEST_SCHEDULE_TBL] WHERE ID = @id";
            SqlCommand cmd = new SqlCommand(query, sqlconn);
            cmd.Parameters.AddWithValue("@id", model.id);

            // Opening the connection
            sqlconn.Open();
            SqlDataAdapter sda = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            sda.Fill(ds);
            sqlconn.Close();

            if (ds.Tables[0].Rows.Count > 0) {
                query = "UPDATE [TESTDB].[dbo].[TEST_SCHEDULE_TBL] SET TITLE = @title, CATEGORY = @category, TESTDATE = @testdate , LOCATION = @location, DESCRIPTION = @description WHERE ID = @id";
                cmd = new SqlCommand(query, sqlconn);
                cmd.Parameters.AddWithValue("@title", model.title);
                cmd.Parameters.AddWithValue("@category", model.Category);
                cmd.Parameters.AddWithValue("@testdate", model.Start_date);
                cmd.Parameters.AddWithValue("@location", model.Location);
                cmd.Parameters.AddWithValue("@description", model.Description);
                cmd.Parameters.AddWithValue("@id", model.id);
                sqlconn.Open();
                cmd.ExecuteNonQuery();
                sqlconn.Close();
            }
            else
            {
                query = "INSERT INTO [TESTDB].[dbo].[TEST_SCHEDULE_TBL] VALUES(@title, @category, @testdate, @location, @description)";
                cmd = new SqlCommand(query, sqlconn);
                cmd.Parameters.AddWithValue("@title", model.title);
                cmd.Parameters.AddWithValue("@category", model.Category);
                cmd.Parameters.AddWithValue("@testdate", model.Start_date);
                cmd.Parameters.AddWithValue("@location", model.Location);
                cmd.Parameters.AddWithValue("@description", model.Description);
                cmd.Parameters.AddWithValue("@id", model.id);
                sqlconn.Open();
                cmd.ExecuteNonQuery();
                sqlconn.Close();
            }


            return Json(true);
        }
    }
}