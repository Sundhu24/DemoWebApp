using Microsoft.AspNetCore.Mvc;
using DemoWebApp.Model;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoWebApp.Controllers
{
    public class ResponseModel4
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public List<dynamic> Data { get; set; }

    }
    public class StatusResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }

    }
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IConfiguration _configuration;

        public StudentController(IConfiguration config)
        {
            _configuration = config;
        }
        // GET: api/<StudentController>
        [HttpGet]
        public ResponseModel4 Get()
        {
            ResponseModel4 objresponse = new ResponseModel4();

            string query = @"
                           select * from
                           student
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

            List<dynamic> studentList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                Student student = new Student();
               student.Id = Convert.ToInt32(table.Rows[i]["id"]);
                student.Name = table.Rows[i]["name"].ToString();
                student.Age = Convert.ToInt32(table.Rows[i]["age"]);
                student.Email = table.Rows[i]["email"].ToString();
                student.Address = table.Rows[i]["address"].ToString();
                studentList.Add(student);
            }


            objresponse.Data = studentList;
            objresponse.Status = true;
            objresponse.Message = "Student Data Received successfully";
            return objresponse;
        }

        // GET api/<StudentController>/5
        [HttpGet("{id}")]
        public ResponseModel4 Get(int id)
        {
            ResponseModel4 _objResponseModel = new ResponseModel4();

            string query = @"
                            select * from
                            student where id=@id
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

            List<dynamic> stdList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                Student st = new Student();

                st.Id = Convert.ToInt32(table.Rows[i]["id"]);
                st.Name = table.Rows[i]["name"].ToString();
                st.Age = Convert.ToInt32(table.Rows[i]["age"]);
                st.Email = table.Rows[i]["email"].ToString();
                st.Address = table.Rows[i]["address"].ToString();
                stdList.Add(st);
            }


            _objResponseModel.Data = stdList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Student Data Received successfully";
            return _objResponseModel;
        }

        // POST api/<StudentController>
        [HttpPost]
        public StatusResponse Post(Student studentdata)
        {
            StatusResponse objResponse = new StatusResponse();

            string query = @"
                           insert into student
                           (name,age,email,address)
                    values (@name,@age,@email,@address)
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("phonebookDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@name", studentdata.Name);
                    myCommand.Parameters.AddWithValue("@age", studentdata.Age);
                    myCommand.Parameters.AddWithValue("@email", studentdata.Email);
                    myCommand.Parameters.AddWithValue("@address", studentdata.Address);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            objResponse.Status = true;
            objResponse.Message = "Student Data Inserted successfully";
            return objResponse;
        }

        // PUT api/<StudentController>/5
        [HttpPut]
        public StatusResponse Put(Student studentdata)
        {
            StatusResponse objResponse = new StatusResponse();

            //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from student
                              where id = @id";
            string sqlDataSource1 = _configuration.GetConnectionString("phonebookDB");
            int rowexists = 0;

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {


                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {

                    myCommand.Parameters.AddWithValue("@id", studentdata.Id);
                    rowexists = (int)myCommand.ExecuteScalar();

                    myCon1.Close();
                }
            }
            //end To check if the ID exists in Database
            if (rowexists == 1)
            {
                string query = @"
                           update student set
                           name = @name,
                           age = @age,
                           email = @email,
                           address = @address
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
                        myCommand.Parameters.AddWithValue("@id", studentdata.Id);
                        myCommand.Parameters.AddWithValue("@name", studentdata.Name);
                        myCommand.Parameters.AddWithValue("@age", studentdata.Age);
                        myCommand.Parameters.AddWithValue("@email", studentdata.Email);
                        myCommand.Parameters.AddWithValue("@address", studentdata.Address);


                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }


                objResponse.Status = true;
                objResponse.Message = "Student Data Updated successfully";
                return objResponse;
            }
            else
            {
                objResponse.Status = false;
                objResponse.Message = "Student Data does not exists";
                return objResponse;
            }
        }

        // DELETE api/<StudentController>/5
        [HttpDelete("{id}")]
        public StatusResponse Delete(int id)
        {
            StatusResponse objResponse = new StatusResponse();
            //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from student
                              where id = @id";
            string sqlDataSource1 = _configuration.GetConnectionString("phonebookDB");
            int rowexists = 0;

            Task1 task1data = new Task1();

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {


                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {


                    myCommand.Parameters.AddWithValue("@id", id);
                    rowexists = (Int32)myCommand.ExecuteScalar();

                    myCon1.Close();
                }
            }
            //end To check if the ID exists in Database
            if (rowexists == 1)
            {
                string query = @"
                           delete from
                            student where id=@id
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





                objResponse.Status = true;
                objResponse.Message = "Student Data Deleted successfully";
                return objResponse;
            }
            else
            {
                objResponse.Status = false;
                objResponse.Message = "Student Data does not exists";
                return objResponse;
            }
        }
    }
}
