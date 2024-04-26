using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private IConfiguration _configuration;

        public TaskController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Hakee tehtävät tietokannasta frontendiin käsiteltäväksi
        [HttpGet]
        [Route("GetTasksWithDetails")]
        public JsonResult GetTasksWithDetails()
        {
            string query = @"
                    SELECT 
                        Task.*, 
                        Status.Name AS StatusName,
                        ActivityType.Name AS ActivityTypeName
                    FROM 
                        dbo.Task 
                    INNER JOIN 
                        dbo.Status ON Task.StatusId = Status.Id
                    INNER JOIN 
                        dbo.ActivityType ON Task.ActivityTypeId = ActivityType.Id";

            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            SqlDataReader myReader;
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(table);
        }

        // Hakee aktiviteetit tietokannasta frontendiin käsiteltäväksi
        [HttpGet]
        [Route("GetActivityWithDetails")]
        public JsonResult GetActivityWithDetails()
        {
            string query = @"
                    SELECT 
                        Activity.*, 
                        Status.Name AS StatusName,
                        ActivityType.Name AS ActivityTypeName
                    FROM 
                        dbo.Activity 
                    INNER JOIN 
                        dbo.Status ON Activity.StatusId = Status.Id
                    INNER JOIN 
                        dbo.ActivityType ON Activity.ActivityTypeId = ActivityType.Id";

            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            SqlDataReader myReader;
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(table);
        }

        // Hakee statuksen tietokannasta frontendiin käsiteltäväksi
        [HttpGet]
        [Route("GetStatus")]
        public JsonResult GetStatus()
        {
            string query = "select * from dbo.Status";
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            SqlDataReader myReader;
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult(table);
        }

        // Lisää tietokantaan tehtävän käyttäjän antamilla syötteillä
        [HttpPost]
        [Route("AddTask")]
        public IActionResult AddTask([FromBody] JObject data)
        {
            Task task = data.ToObject<Task>();
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            SqlDataReader myReader;
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                int statusId = InsertStatus(task.StatusName, task.StatusTheme);
                int ActivityTypeId = InsertActivityType(task.ActivityTypeName);
                string insertQuery = @"INSERT INTO Task (Name, Description, StartDate, EndDate, StatusId, ActivityTypeId)
                                       VALUES (@Name, @Description, @StartDate, @EndDate, @StatusId, @ActivityTypeId)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", task.Name);
                    command.Parameters.AddWithValue("@Description", task.Description ?? DBNull.Value.ToString());
                    command.Parameters.AddWithValue("@StartDate", task.StartDate);
                    command.Parameters.AddWithValue("@EndDate", task.EndDate);
                    command.Parameters.AddWithValue("@StatusId", statusId);
                    command.Parameters.AddWithValue("@ActivityTypeId", ActivityTypeId);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult("Tehtävä lisätty");
        }

        // Aktiviteetin lisäys käyttäjän antamilla syötteillä tietokantaan
        [HttpPost]
        [Route("AddActivity")]
        public IActionResult AddActivity([FromBody] JObject data)
        {
            Console.WriteLine(data);
            Activity activity = data.ToObject<Activity>();
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            SqlDataReader myReader;
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                int statusId = InsertStatus(activity.StatusName, activity.StatusTheme);
                int ActivityTypeId = InsertActivityType(activity.ActivityTypeName);
                string insertQuery = @"INSERT INTO Activity (Name, Description, Url, StartDate, EndDate, StatusId, ActivityTypeId) 
                                       VALUES (@Name, @Description, @Url, @StartDate, @EndDate, @StatusId, @ActivityTypeId)";

                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", activity.Name);
                    command.Parameters.AddWithValue("@Description", activity.Description ?? DBNull.Value.ToString());
                    command.Parameters.AddWithValue("@Url", activity.Url ?? DBNull.Value.ToString());
                    command.Parameters.AddWithValue("@StartDate", activity.StartDate);
                    command.Parameters.AddWithValue("@EndDate", activity.EndDate);
                    command.Parameters.AddWithValue("@StatusId", statusId);
                    command.Parameters.AddWithValue("@ActivityTypeId", ActivityTypeId);
                    myReader = command.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    connection.Close();
                }
            }
            return new JsonResult("Aktiviteetti lisätty");

        }

        // Lisää aktiviteettiin aktiviteetin tyypin
        private int InsertActivityType(string ActivityName)
        {
            string insertQuery = "INSERT INTO ActivityType (Name) OUTPUT INSERTED.Id VALUES (@ActivityName)";
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");

            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    if (ActivityName == "")
                    {
                        command.Parameters.AddWithValue("@ActivityName", "Ei asetettu");
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@ActivityName", ActivityName);
                    }
                    int ActivityTypeId = (int)command.ExecuteScalar();
                    connection.Close();
                    return ActivityTypeId;
                }
            }
        }

        // Statuksen luonti joka lisätään aktiviteetin tai tehtävän luonnin yhteydessä
        private int InsertStatus(string StatusName, string StatusTheme)
        {
            string insertQuery = "INSERT INTO Status (Name, Theme) OUTPUT INSERTED.Id VALUES (@StatusName, @StatusTheme) ";
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");

            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@StatusName", StatusName);
                    command.Parameters.AddWithValue("@StatusTheme", StatusTheme);
                    int statusId = (int)command.ExecuteScalar();
                    connection.Close();
                    return statusId;
                }
            }

        }

        // Poistaa tehtävän
        [HttpDelete]
        [Route("DeleteTask")]
        public JsonResult DeleteTask(int Id)
        {
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");

            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                int StatusId;
                int ActivityTypeId;

                string getIdsQuery = "SELECT StatusId, ActivityTypeId FROM Task WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(getIdsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StatusId = reader.GetInt32(0);
                            ActivityTypeId = reader.GetInt32(1);
                        }
                        else
                        {
                            return new JsonResult("Tehtävää ei löytynyt");
                        }
                    }
                }

                string deleteTaskQuery = "DELETE FROM Task WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(deleteTaskQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();
                }
                DeleteStatus(StatusId);
                DeleteActivityType(ActivityTypeId);

                connection.Close();
            }
            return new JsonResult("Tehtävä poistettu");
        }

        // Poistaa aktiviteetin
        [HttpDelete]
        [Route("DeleteActivity")]
        public JsonResult DeleteActivity(int Id)
        {
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");

            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();

                int StatusId;
                int ActivityTypeId;

                string getIdsQuery = "SELECT StatusId, ActivityTypeId FROM Activity WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(getIdsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StatusId = reader.GetInt32(0);
                            ActivityTypeId = reader.GetInt32(1);
                        }
                        else
                        {
                            return new JsonResult("Aktiviteettiä ei löydy");
                        }
                    }
                }
                string deleteTaskQuery = "DELETE FROM Activity WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(deleteTaskQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.ExecuteNonQuery();

                }
                DeleteStatus(StatusId);
                DeleteActivityType(ActivityTypeId);

            }
            return new JsonResult("aktiviteetti poistettu onnistuneesti");
        }

        // Poistaa statuksen aktiviteetin tai tehtävän poiston yhteydessä
        private void DeleteStatus(int Id)
        {
            string deleteStatusQuery = "DELETE FROM dbo.Status WHERE Id = @Id";
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                using (SqlCommand deleteCommand = new SqlCommand(deleteStatusQuery, connection))
                {
                    deleteCommand.Parameters.AddWithValue("@Id", Id);
                    deleteCommand.ExecuteNonQuery();
                    connection.Close();

                }
            }

        }

        // Poistaa aktiviteetintyypin aktiviteetin poiston yhteydessä
        private void DeleteActivityType(int Id)
        {
            string deleteActivityTypeQuery = "DELETE FROM dbo.ActivityType WHERE Id = @Id";
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                using (SqlCommand deleteCommand = new SqlCommand(deleteActivityTypeQuery, connection))
                {

                    deleteCommand.Parameters.AddWithValue("@Id", Id);
                    deleteCommand.ExecuteNonQuery();
                    connection.Close();

                }
            }
        }

        // Tehtävän tietojen muokkaus
        [HttpPut]
        [Route("EditTask")]
        public JsonResult EditTask([FromBody] JObject data)
        {
            Console.WriteLine(data);
            EditTask task = data.ToObject<EditTask>();
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            SqlDataReader myReader;
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                int StatusId;
                int ActivityTypeId;
                connection.Open();

                string getIdsQuery = "SELECT StatusId, ActivityTypeId FROM Task WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(getIdsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StatusId = reader.GetInt32(0);
                            ActivityTypeId = reader.GetInt32(1);
                        }
                        else
                        {
                            return new JsonResult("Tehtävää ei löytynyt");
                        }
                    }
                }
                EditStatus(StatusId, task.StatusName, task.StatusTheme);
                EditActivityType(ActivityTypeId, task.ActivityTypeName);
                string updateQuery = "UPDATE Task SET Name = @Name, Description = @Description, StartDate = @StartDate, EndDate = @EndDate, StatusId = @StatusId, ActivityTypeId = @ActivityTypeId WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);
                    command.Parameters.AddWithValue("@Name", task.Name);
                    command.Parameters.AddWithValue("@Description", task.Description ?? DBNull.Value.ToString());
                    command.Parameters.AddWithValue("@StartDate", task.StartDate);
                    command.Parameters.AddWithValue("@EndDate", task.EndDate);
                    command.Parameters.AddWithValue("@StatusId", StatusId);
                    command.Parameters.AddWithValue("@ActivityTypeId", ActivityTypeId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return new JsonResult("Task updated successfully");

                    }
                    else
                    {
                        return new JsonResult("Task not found or no changes made");
                    }
                }
            }
        }

        // Aktiviteetin tietojen muokkaus
        [HttpPut]
        [Route("EditActivity")]
        public JsonResult EditActivity([FromBody] JObject data)
        {
            EditActivity activity = data.ToObject<EditActivity>();
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            SqlDataReader myReader;
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                int StatusId;
                int ActivityTypeId;
                connection.Open();

                string getIdsQuery = "SELECT StatusId, ActivityTypeId FROM Activity WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(getIdsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", activity.Id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StatusId = reader.GetInt32(0);
                            ActivityTypeId = reader.GetInt32(1);
                        }
                        else
                        {
                            return new JsonResult("Aktiviteettiä ei löytynyt");
                        }
                    }
                }

                EditStatus(StatusId, activity.StatusName, activity.StatusTheme);
                EditActivityType(ActivityTypeId, activity.ActivityTypeName);

                string updateQuery = "UPDATE Activity SET Name = @Name, Description = @Description,Url = @Url, StartDate = @StartDate, EndDate = @EndDate, StatusId = @StatusId, ActivityTypeId = @ActivityTypeId WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", activity.Id);
                    command.Parameters.AddWithValue("@Name", activity.Name);
                    command.Parameters.AddWithValue("@Description", activity.Description ?? DBNull.Value.ToString());
                    command.Parameters.AddWithValue("@Url", activity.Url ?? DBNull.Value.ToString());
                    command.Parameters.AddWithValue("@StartDate", activity.StartDate);
                    command.Parameters.AddWithValue("@EndDate", activity.EndDate);
                    command.Parameters.AddWithValue("@StatusId", StatusId);
                    command.Parameters.AddWithValue("@ActivityTypeId", ActivityTypeId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return new JsonResult("Päivitys onnistui");

                    }
                    else
                    {
                        return new JsonResult("Ei löytynyt");
                    }
                }
            }
        }

        private void EditStatus(int Id, string StatusName, string StatusTheme)
        {
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                string updateQuery = "UPDATE Status SET Name = @NewStatusName, Theme = @NewStatusTheme WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@NewStatusName", StatusName);
                    command.Parameters.AddWithValue("@NewStatusTheme", StatusTheme);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void EditActivityType(int Id, string ActivityTypeName)
        {
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");

            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                connection.Open();
                string updateQuery = "UPDATE ActivityType SET Name = @TypeName WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@TypeName", ActivityTypeName);

                    command.ExecuteNonQuery();
                }
            }
        }

        [HttpPut]
        [Route("MarkTaskDone")]
        public JsonResult MarkTaskDone([FromBody] JObject data)
        {
            Console.WriteLine(data);
            MarkDone task = data.ToObject<MarkDone>();
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            SqlDataReader myReader;
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                int StatusId;
                DateTime DoneDate;
                connection.Open();

                string getIdsQuery = "SELECT  StatusId FROM Task WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(getIdsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            StatusId = reader.GetInt32(0);
                        }
                        else
                        {
                            return new JsonResult("Ei löytynyt");
                        }
                    }
                }

                EditStatus(StatusId, "Done", "Done");
                string updateQuery = "UPDATE Task SET StatusId = @StatusId, DoneDate=@DoneDate WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);
                    command.Parameters.AddWithValue("@StatusId", StatusId);
                    command.Parameters.AddWithValue("@DoneDate", task.DoneDate);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return new JsonResult("Päivitys onnistui");
                    }
                    else
                    {
                        return new JsonResult("Ei löytynyt");
                    }
                }
            }
        }

        [HttpPut]
        [Route("MarkActivityDone")]
        public JsonResult MarkActivityDone([FromBody] JObject data)
        {
            Console.WriteLine(data);
            MarkDone task = data.ToObject<MarkDone>();
            DataTable table = new DataTable();
            string sqlDatasource = _configuration.GetConnectionString("taskDBCon");
            SqlDataReader myReader;
            using (SqlConnection connection = new SqlConnection(sqlDatasource))
            {
                int StatusId;
                DateTime DoneDate;

                connection.Open();
                string getIdsQuery = "SELECT  StatusId FROM Activity WHERE Id = @Id";
                using (SqlCommand command = new SqlCommand(getIdsQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StatusId = reader.GetInt32(0);
                        }
                        else
                        {
                            return new JsonResult("Ei löytynyt");
                        }
                    }
                }

                EditStatus(StatusId, "Done", "Done");
                string updateQuery = "UPDATE Activity SET StatusId = @StatusId, DoneDate=@DoneDate WHERE Id = @Id";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);
                    command.Parameters.AddWithValue("@StatusId", StatusId);
                    command.Parameters.AddWithValue("@DoneDate", task.DoneDate);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        return new JsonResult("Task updated successfully");
                    }
                    else
                    {
                        return new JsonResult("Task not found or no changes made");
                    }
                }
            }
        }
    }
}
