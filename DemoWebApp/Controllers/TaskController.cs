using Microsoft.AspNetCore.Mvc;
using DemoWebApp.Model;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        public TaskController(IConfiguration config, IWebHostEnvironment env)
        {
            _configuration = config;
            _env = env;
        }
        // GET: api/<TaskController>
        [HttpGet]
        public ResponseModel Get()
        {
            ResponseModel _objResponseModel = new ResponseModel();

            string query = @"
                            select * from
                            Task
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("phonebookDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            List<dynamic> taskList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                Task1 task1 = new Task1();
                task1.Id = Convert.ToInt32(table.Rows[i]["id"]);
                task1.Task_title = table.Rows[i]["task_title"].ToString();
                task1.Task_description =table.Rows[i]["task_description"].ToString();
                task1.Attachment= table.Rows[i]["attachment"].ToString();
                task1.Project_id = Convert.ToInt32(table.Rows[i]["project_id"]);
                task1.Created_Date= Convert.ToDateTime(table.Rows[i]["created_date"]);
                task1.End_Date = Convert.ToDateTime(table.Rows[i]["end_date"]);

                taskList.Add(task1);
            }


            _objResponseModel.Data = taskList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Task Data Received successfully";
            return _objResponseModel;


        }


    
        
        // GET api/<TaskController>/5
        [HttpGet("{id}")]
        public ResponseModel Get(int id)
        {
            ResponseModel _objResponseModel = new ResponseModel();

            string query = @"
                            select * from
                            Task where id=@id
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("phonebookDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            List<dynamic> taskList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                Task1 task1 = new Task1();
                task1.Id = Convert.ToInt32(table.Rows[i]["id"]);
                task1.Task_title = table.Rows[i]["task_title"].ToString();
                task1.Task_description = table.Rows[i]["task_description"].ToString();
                task1.Attachment = table.Rows[i]["attachment"].ToString();
                task1.Project_id = Convert.ToInt32(table.Rows[i]["project_id"]);
                task1.Created_Date = Convert.ToDateTime(table.Rows[i]["created_date"]);
                task1.End_Date = Convert.ToDateTime(table.Rows[i]["end_date"]);

                taskList.Add(task1);
            }


            _objResponseModel.Data = taskList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Task Data Received successfully";
            return _objResponseModel;
        }

        // POST api/<TaskController>
        [Route("SaveFile")]
        [HttpPost]
       public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;



                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }



                return new JsonResult(filename);
            }
            catch (Exception)
            {



                return new JsonResult("anonymous.png");
            }
        }
        
       [HttpPost]
        public StatusResponse Post(Task1 taskdata)
        {
            StatusResponse _objResponseModel = new StatusResponse();

            string query = @"
                           insert into task
                           (task_title,task_description,attachment,project_id,created_date,end_date)
                    values (@task_title,@task_description,@attachment,@project_id,@created_date,@end_date)
                            ";

            string query1 = @"select max(id) as LastId from task";
            DataTable table = new DataTable();
            DataTable table1 = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("phonebookDB");
            SqlDataReader myReader;
            int LastId;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@task_title", taskdata.Task_title);
                    myCommand.Parameters.AddWithValue("@task_description", taskdata.Task_description);
                    myCommand.Parameters.AddWithValue("@attachment", taskdata.Attachment);
                    myCommand.Parameters.AddWithValue("@project_id", taskdata.Project_id);
                    myCommand.Parameters.AddWithValue("@created_date", taskdata.Created_Date);
                    myCommand.Parameters.AddWithValue("@end_date", taskdata.End_Date);


                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                   
                }
                using (SqlCommand mycommand1 = new SqlCommand(query1, myCon))
                {
                    myReader = mycommand1.ExecuteReader();
                    table.Load(myReader);
                    LastId =Convert.ToInt32(table.Rows[0]["LastId"]);


                }
                myReader.Close();
                myCon.Close();
            }

            
            _objResponseModel.Status = true;
            _objResponseModel.Message =LastId.ToString();
            return _objResponseModel;
        }

        // PUT api/<TaskController>/5
        [HttpPut]
        public StatusResponse Put(Task1 taskdata)
        {
            StatusResponse _objResponseModel = new StatusResponse();
            //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from Task
                              where id = @id";
            string sqlDataSource1 = _configuration.GetConnectionString("phonebookDB");
            int rowexists = 0;

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {


                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {

                    myCommand.Parameters.AddWithValue("@id", taskdata.Id);
                    rowexists = (int)myCommand.ExecuteScalar();

                    myCon1.Close();
                }
            }
            //end To check if the ID exists in Database
            if (rowexists == 1)
            {
                string query = @"
                           update Task set
                          task_title = @task_title,
                          task_description = @task_description,
                          attachment = @attachment,
                          project_id = @project_id,
                          created_date =@created_date,
                          end_date = @end_date
                          where id=@id
                            ";

                DataTable table = new DataTable();

                string sqlDataSource = _configuration.GetConnectionString("phonebookDB");
                SqlDataReader myReader;

                using (SqlConnection myCon = new SqlConnection(sqlDataSource))
                {

                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@id", taskdata.Id);
                        myCommand.Parameters.AddWithValue("@task_title", taskdata.Task_title);
                        myCommand.Parameters.AddWithValue("@task_description", taskdata.Task_description);
                        myCommand.Parameters.AddWithValue("@attachment", taskdata.Attachment);
                        myCommand.Parameters.AddWithValue("@project_id", taskdata.Project_id);
                        myCommand.Parameters.AddWithValue("@created_date", taskdata.Created_Date);
                        myCommand.Parameters.AddWithValue("@end_date", taskdata.End_Date);


                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }


                _objResponseModel.Status = true;
                _objResponseModel.Message = "Task Data Updated successfully";
                return _objResponseModel;
            }
            else
            {
                _objResponseModel.Status = false;
                _objResponseModel.Message = "Task Data does not exists";
                return _objResponseModel;
            }
        }

        // DELETE api/<TaskController>/5
        [HttpDelete("{id}")]
        public StatusResponse Delete(int id)
        { 
        StatusResponse _objResponseModel = new StatusResponse();
        //To check if the ID exists in Database
        string query1 = @"select count(*)
                              from Task
                              where id = @id";
        string sqlDataSource1 = _configuration.GetConnectionString("phonebookDB");
        int rowexists = 0;

        Task1 task1data = new Task1();

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {


                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {


                    myCommand.Parameters.AddWithValue("@id",id);
                    rowexists = (Int32) myCommand.ExecuteScalar();

                    myCon1.Close();
                }
            }
            //end To check if the ID exists in Database
            if (rowexists == 1)
            {
                string query = @"
                           delete from Task where id=@id
                            ";

                DataTable table = new DataTable();

                string sqlDataSource = _configuration.GetConnectionString("phonebookDB");
                SqlDataReader myReader;

             using (SqlConnection myCon = new SqlConnection(sqlDataSource))
             {

                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query, myCon))
                    {
                         myCommand.Parameters.AddWithValue("@id", id);
                         myReader = myCommand.ExecuteReader();
                         table.Load(myReader);
                         myReader.Close();
                         myCon.Close();
                    }
             }


                _objResponseModel.Status = true;
                _objResponseModel.Message = "Task Data Deleted successfully";
                return _objResponseModel;
             }
            else
            { 
                _objResponseModel.Status = false;
                _objResponseModel.Message = "Task Data does not exists";
                 return _objResponseModel;
            }
        }
    }
}
