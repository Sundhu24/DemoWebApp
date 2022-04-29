using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using DemoWebApp.Model;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoWebApp.Controllers
{
    public class ResponseModel2
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }
    [Route("api/[controller]")]
    [ApiController]
    
    public class CityController : ControllerBase
    {
        private IConfiguration _configuration; 

        public CityController(IConfiguration config)
        {
            _configuration = config;
        }

        // GET: api/<CityController>
        [HttpGet]
        public ResponseModel2 Get()
        {
            ResponseModel2 _objResponseModel = new ResponseModel2();

            string query = @"
                            select * from
                            city
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

            List<dynamic> cityList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                City ctry = new City();
                ctry.Id = Convert.ToInt32(table.Rows[i]["id"]);
                ctry.Cityname = table.Rows[i]["cityname"].ToString();
                ctry.CountryId = Convert.ToInt32(table.Rows[i]["country_id"]);
                ctry.Population = Convert.ToInt32(table.Rows[i]["population"]);
                ctry.Rating = Convert.ToInt32(table.Rows[i]["rating"]);
                //ctry.Points = Convert.ToInt32(table.Rows[i]["points"]);
                cityList.Add(ctry);
            }


            _objResponseModel.Data = cityList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "City Data Received successfully";
            return _objResponseModel;

        }

        // GET api/<CityController>/5
        [HttpGet("{id}")]
        public ResponseModel2 Get(int id)
        {
            ResponseModel2 _objResponseModel = new ResponseModel2();

            string query = @"
                            select * from
                            city where id = @id
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

            List<dynamic> cityList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                City ctry = new City();
                ctry.Id = Convert.ToInt32(table.Rows[i]["id"]);
                ctry.Cityname = table.Rows[i]["cityname"].ToString();
                ctry.CountryId = Convert.ToInt32(table.Rows[i]["country_id"]);
                ctry.Population = Convert.ToInt32(table.Rows[i]["population"]);
                ctry.Rating = Convert.ToInt32(table.Rows[i]["rating"]);
                //ctry.Points = Convert.ToInt32(table.Rows[i]["points"]);
                cityList.Add(ctry);
            }


            _objResponseModel.Data = cityList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "City Data Received successfully";
            return _objResponseModel;
        }

        // POST api/<CityController>
        [HttpPost]
        public ResponseModel2 Post(City citydata)
        {
            ResponseModel2 _objResponseModel = new ResponseModel2();

            string query = @"
                           insert into city
                           (cityname,country_id,population,rating)
                    values (@cityname,@country_id,@population,@rating)
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("phonebookDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@cityname", citydata.Cityname);
                    myCommand.Parameters.AddWithValue("@country_id", citydata.CountryId);
                    myCommand.Parameters.AddWithValue("@population", citydata.Population);
                    myCommand.Parameters.AddWithValue("@rating", citydata.Rating);
                   // myCommand.Parameters.AddWithValue("@points", citydata.Points);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "City Data Inserted successfully";
            return _objResponseModel;
        }

        // PUT api/<CityController>/5
        [HttpPut]
        public ResponseModel2 Put(City citydata)
        {
            ResponseModel2 _objResponseModel = new ResponseModel2();
            //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from city
                              where id = @id";
            string sqlDataSource1 = _configuration.GetConnectionString("phonebookDB");
            int rowexists = 0;

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {


                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {

                    myCommand.Parameters.AddWithValue("@id", citydata.Id);
                    rowexists = (int)myCommand.ExecuteScalar();

                    myCon1.Close();
                }
            }
            //end To check if the ID exists in Database
            if (rowexists == 1)
            {
                string query = @"
                           update city set
                           cityname = @cityname ,
                           country_id = @country_id,
                           population = @population,rating = @rating 
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
                        myCommand.Parameters.AddWithValue("@id", citydata.Id);
                        myCommand.Parameters.AddWithValue("@cityname", citydata.Cityname);
                        myCommand.Parameters.AddWithValue("@country_id", citydata.CountryId);
                        myCommand.Parameters.AddWithValue("@population", citydata.Population);
                        myCommand.Parameters.AddWithValue("@rating", citydata.Rating);
                        //myCommand.Parameters.AddWithValue("@points", citydata.Points);

                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }


                _objResponseModel.Status = true;
                _objResponseModel.Message = "City Data Updated successfully";
                return _objResponseModel;
            }
            else
            {
                _objResponseModel.Status = false;
                _objResponseModel.Message = "City Data does not exists";
                return _objResponseModel;
            }

        }

        // DELETE api/<CityController>/5
        [HttpDelete("{id}")]
        public StatusResponse Delete(int id)
        {
            StatusResponse _objResponseModel = new StatusResponse();

            //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from city
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
                            city where id = @id
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
                _objResponseModel.Message = "City Data Deleted successfully";
                return _objResponseModel;
            }

            else
            {
                _objResponseModel.Status = false;
                _objResponseModel.Message = "City Data does not exists";
                return _objResponseModel;
            }


        }
    }
}
