using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

namespace Web_WorkTrackingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkTrackingApiController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public WorkTrackingApiController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            mysqlTableCreate();
        }


        private void mysqlTableCreate()
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString);
                connection.Open();

                MySqlCommand commandTable = new MySqlCommand();

                commandTable = new MySqlCommand(@"CREATE TABLE project (id INT PRIMARY KEY AUTO_INCREMENT, name VARCHAR(255) NOT NULL UNIQUE, code VARCHAR(255) NOT NULL UNIQUE, status BOOLEAN NOT NULL DEFAULT(0));", connection);
                commandTable.ExecuteNonQuery();

                commandTable = new MySqlCommand(@"CREATE TABLE task  ( id INT PRIMARY KEY AUTO_INCREMENT, name VARCHAR(255) NOT NULL UNIQUE, project_id INT NOT NULL,  CONSTRAINT fk_task_project FOREIGN KEY (project_id) REFERENCES project (id), status BOOLEAN NOT NULL DEFAULT(0));", connection);
                commandTable.ExecuteNonQuery();

                commandTable = new MySqlCommand(@"CREATE TABLE job_post ( id INT PRIMARY KEY AUTO_INCREMENT, date DATE NOT NULL, hours INT NOT NULL CHECK (hours > 0 AND hours < 25), description VARCHAR(255) NOT NULL, task_id INT NOT NULL, CONSTRAINT fk_task_job_post FOREIGN KEY (task_id) REFERENCES task (id));", connection);
                commandTable.ExecuteNonQuery();

                connection.Close();
            }
            catch { };
        }

        /*
        * GET REQUEST
        */

        [HttpGet("project/get")]
        public IEnumerable<WorkTracking_Project> GetProjects()
        {
            using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
            using MySqlCommand command = new MySqlCommand("SELECT * FROM project", connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                WorkTracking_Project item = new WorkTracking_Project()
                {
                    Index = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    Code = reader.GetString("code"),
                    Status = reader.GetBoolean("status"),
                };
                yield return item;
            }
            connection.Close();
            reader.Close();
            yield break;
        }

        [HttpGet("task/get")]
        public IEnumerable<WorkTracking_Task> GetTask()
        {
            using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
            using MySqlCommand command = new MySqlCommand("SELECT * FROM task", connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                WorkTracking_Task item = new WorkTracking_Task()
                {
                    Index = reader.GetInt32("id"),
                    Name = reader.GetString("name"),
                    ProjectIndex = reader.GetInt32("project_id"),
                    Status = reader.GetBoolean("status"),
                };
                yield return item;
            }
            connection.Close();
            reader.Close();
            yield break;
        }
        [HttpGet("job_post/get")]
        public IEnumerable<WorkTracking_JobPost> GetJobPost()
        {
            using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
            using MySqlCommand command = new MySqlCommand("SELECT * FROM job_post", connection);
            using MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                WorkTracking_JobPost item = new WorkTracking_JobPost()
                {
                    Index = reader.GetInt32("id"),
                    Date = reader.GetDateTime("date").ToString("yyyy-MM-dd"),
                    Hours = reader.GetInt32("hours"),
                    Description = reader.GetString("description"),
                    TaskIndex = reader.GetInt32("task_id")
                };
                yield return item;
            }
            connection.Close();
            reader.Close();
            yield break;
        }
        /*
        * POST REQUEST
        */

        [HttpPost("project/post")]
        public ActionResult<IEnumerable<WorkTracking_Project>> Post(WorkTracking_Project project)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
                using MySqlCommand command = new MySqlCommand("INSERT INTO project (`name`, `code`, `status`) VALUES (@Name, @Code, @Status);", connection);

                command.Parameters.AddWithValue("@Name", project.Name);
                command.Parameters.AddWithValue("@Code", project.Code);
                command.Parameters.AddWithValue("@Status", project.Status);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok(GetProjects());
            }
            catch { return BadRequest(); };
        }

        [HttpPost("task/post")]
        public ActionResult<IEnumerable<WorkTracking_Task>> Post(WorkTracking_Task task)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
                using MySqlCommand command = new MySqlCommand(@"INSERT INTO task (`name`, `project_id`, `status`) VALUES (@Name, @ProjectIndex, @Status);", connection);
                command.Parameters.AddWithValue("@Name", task.Name);
                command.Parameters.AddWithValue("@ProjectIndex", task.ProjectIndex);
                command.Parameters.AddWithValue("@Status", task.Status);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok(GetTask());
            }
            catch { return BadRequest(); };
        }

        [HttpPost("job_post/post")]
        public ActionResult<IEnumerable<WorkTracking_JobPost>> Post(WorkTracking_JobPost jobPost)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
                using MySqlCommand command = new MySqlCommand(@"INSERT INTO job_post (`date`, `hours`, `description`, `task_id`) SELECT @Date, @Hours, @Description, t.`id` FROM `task` t WHERE t.`id` = @TaskIndex;", connection);
                command.Parameters.AddWithValue("@Date", jobPost.Date);
                command.Parameters.AddWithValue("@Hours", jobPost.Hours);
                command.Parameters.AddWithValue("@Description", jobPost.Description);
                command.Parameters.AddWithValue("@TaskIndex", jobPost.TaskIndex);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok(GetJobPost());
            }
            catch { return BadRequest(); };
        }

        /*
        * PUT REQUEST
        */

        [HttpPut("project/put")]
        public ActionResult<IEnumerable<WorkTracking_Project>> Put(WorkTracking_Project project)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
                using MySqlCommand command = new MySqlCommand(@"UPDATE project SET name=@Name, code=@Code, status=@Status WHERE(id=@Index);", connection);

                command.Parameters.AddWithValue("@Index", project.Index);
                command.Parameters.AddWithValue("@Name", project.Name);
                command.Parameters.AddWithValue("@Code", project.Code);
                command.Parameters.AddWithValue("@Status", project.Status);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok(GetProjects());
            }
            catch { return BadRequest(); };
        }

        [HttpPut("task/put")]
        public ActionResult<IEnumerable<WorkTracking_Task>> Put(WorkTracking_Task task)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
                using MySqlCommand command = new MySqlCommand(@"UPDATE task SET name=@Name, project_id=@ProjectIndex, status=@Status WHERE(id=@Index);", connection);
                command.Parameters.AddWithValue("@Index", task.Index);
                command.Parameters.AddWithValue("@Name", task.Name);
                command.Parameters.AddWithValue("@ProjectIndex", task.ProjectIndex);
                command.Parameters.AddWithValue("@Status", task.Status);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok(GetTask());
            }
            catch { return BadRequest(); };
        }

        [HttpPut("job_post/put")]
        public ActionResult<IEnumerable<WorkTracking_JobPost>> Put(WorkTracking_JobPost jobPost)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
                using MySqlCommand command = new MySqlCommand(@"UPDATE job_post SET date=@Date, hours=@Hours, description=@Description, task_id=@TaskIndex WHERE(id=@Index);", connection);
                command.Parameters.AddWithValue("@Index", jobPost.Index);
                command.Parameters.AddWithValue("@Date", jobPost.Date);
                command.Parameters.AddWithValue("@Hours", jobPost.Hours);
                command.Parameters.AddWithValue("@Description", jobPost.Description);
                command.Parameters.AddWithValue("@TaskIndex", jobPost.TaskIndex);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok(GetJobPost());
            }
            catch { return BadRequest(); };

        }


        /*
        * PUT REQUEST
        */


        [HttpDelete("project/delete")]
        public ActionResult<IEnumerable<WorkTracking_Project>> DeleteProject(int index)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
                using MySqlCommand command = new MySqlCommand(@"DELETE FROM project WHERE(id=@Index)", connection);

                command.Parameters.AddWithValue("@Index", index);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok(GetProjects());
            }
            catch { return BadRequest(); };
        }

        [HttpDelete("task/delete")]
        public ActionResult<IEnumerable<WorkTracking_Task>> DeleteTask(int index)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
                using MySqlCommand command = new MySqlCommand(@"DELETE FROM task WHERE(id=@Index)", connection);

                command.Parameters.AddWithValue("@Index", index);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok(GetTask());
            }
            catch { return BadRequest(); };
        }

        [HttpDelete("job_post/delete")]
        public ActionResult<IEnumerable<WorkTracking_JobPost>> DeleteJobPost(int index)
        {
            try
            {
                using MySqlConnection connection = new MySqlConnection(_connectionString); connection.Open();
                using MySqlCommand command = new MySqlCommand(@"DELETE FROM job_post WHERE(id=@Index)", connection);

                command.Parameters.AddWithValue("@Index", index);
                command.ExecuteNonQuery();
                connection.Close();

                return Ok(GetJobPost());
            }
            catch { return BadRequest(); };
        }
    }
}