using DemoWebApp.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DemoWebApp.Controllers
{

    public class ResponseModel3
    {
        public string Message { set; get; }
        public bool Status { set; get; }
        public List<dynamic> Data { set; get; }
    }

    public class StatusResponseModel2
    {
        public string Message { set; get; }
        public bool Status { set; get; }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {

        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;



       

        public CountryController(IConfiguration config,IWebHostEnvironment env)
        {
            _configuration = config;
            _env = env;
        }

        // GET: api/<CityController>
        [HttpGet]
        public ResponseModel3 Get()
        {
            ResponseModel3 _objResponseModel = new ResponseModel3();

            string query = @"
                            select * from
                            country
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

                Country ctry = new Country();
                ctry.id = Convert.ToInt32(table.Rows[i]["id"]);
                ctry.cname = table.Rows[i]["cname"].ToString();
                ctry.population = Convert.ToInt32(table.Rows[i]["population"]);
                ctry.area = Convert.ToInt32(table.Rows[i]["area"]);
                cityList.Add(ctry);
            }


            _objResponseModel.Data = cityList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Country Data Received successfully";
            return _objResponseModel;

        }
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
        [Route("Updatefilename")]
        [HttpPut]
        public StatusResponseModel2 Updatefilename(int id,string filename)
        {

            StatusResponseModel2 _objResponseModel = new StatusResponseModel2();
           /* //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from country
                              where id = @id";
            string sqlDataSource1 = _configuration.GetConnectionString("phonebookDB");
            int rowexists = 0;

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {


                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {

                    myCommand.Parameters.AddWithValue("@id", id);
                    rowexists = (int)myCommand.ExecuteScalar();

                    myCon1.Close();
                }
            }
            //end To check if the ID exists in Database
            if (rowexists == 1)
            {
                string query = @"
                           update country set
                          filename =@filename 
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
                        myCommand.Parameters.AddWithValue("@id", countrydata.id);
                        myCommand.Parameters.AddWithValue("@cname", countrydata.cname);
                        myCommand.Parameters.AddWithValue("@population", countrydata.population);
                        myCommand.Parameters.AddWithValue("@area", countrydata.area);


                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }


                _objResponseModel.Status = true;
                _objResponseModel.Message = "Country Data Updated successfully";
                return _objResponseModel;
            }
            else
            {
                _objResponseModel.Status = false;
                _objResponseModel.Message = "Country Data does not exists";
                return _objResponseModel;
            }*/
            return _objResponseModel;

        }

        // GET api/<CityController>/5
        [HttpGet("{id}")]
        public ResponseModel3 Get(int id)
        {
            ResponseModel3 _objResponseModel = new ResponseModel3();

            string query = @"
                            select * from
                            country where id=@id
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

            List<dynamic> countryList = new List<dynamic>();
            for (int i = 0; i < table.Rows.Count; i++)
            {

                Country ctry = new Country();
                ctry.id = Convert.ToInt32(table.Rows[i]["id"]);
                ctry.cname = table.Rows[i]["cname"].ToString();
                ctry.population = Convert.ToInt32(table.Rows[i]["population"]);
                ctry.area = Convert.ToInt32(table.Rows[i]["area"]);
                countryList.Add(ctry);
            }


            _objResponseModel.Data = countryList;
            _objResponseModel.Status = true;
            _objResponseModel.Message = "Country Data Received successfully";
            return _objResponseModel;
        }

        // POST api/<CityController>
        [HttpPost]
        public StatusResponseModel2 Post(Country countrydata)
        {

            StatusResponseModel2 _objResponseModel = new StatusResponseModel2();

            string query = @"
                           insert into country
                           (cname,population,area)
                    values (@cname,@population,@area)
                            ";

            DataTable table = new DataTable();

            string sqlDataSource = _configuration.GetConnectionString("phonebookDB");
            SqlDataReader myReader;

            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {

                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@cname", countrydata.cname);
                    myCommand.Parameters.AddWithValue("@population", countrydata.population);
                    myCommand.Parameters.AddWithValue("@area", countrydata.area);
                   
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }


            _objResponseModel.Status = true;
            _objResponseModel.Message = "Country Data Inserted successfully";
            return _objResponseModel;

        }

        // PUT api/<CityController>/5
        [HttpPut]
        public StatusResponseModel2 Put(Country countrydata)
        {

            StatusResponseModel2 _objResponseModel = new StatusResponseModel2();
            //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from country
                              where id = @id";
            string sqlDataSource1 = _configuration.GetConnectionString("phonebookDB");
            int rowexists = 0;

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {
              

                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {
                                                         
                    myCommand.Parameters.AddWithValue("@id", countrydata.id);
                    rowexists = (int)myCommand.ExecuteScalar();
                                    
                    myCon1.Close();
                }
            }
            //end To check if the ID exists in Database
            if (rowexists == 1)
            {
                string query = @"
                           update country set
                           cname = @cname,
                           population = @population,
                           area = @area 
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
                        myCommand.Parameters.AddWithValue("@id", countrydata.id);
                        myCommand.Parameters.AddWithValue("@cname", countrydata.cname);
                        myCommand.Parameters.AddWithValue("@population", countrydata.population);
                        myCommand.Parameters.AddWithValue("@area", countrydata.area);


                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
                    }
                }


                _objResponseModel.Status = true;
                _objResponseModel.Message = "Country Data Updated successfully";
                return _objResponseModel;
            }
            else
            {
                _objResponseModel.Status = false;
                _objResponseModel.Message = "Country Data does not exists";
                return _objResponseModel;
            }

        }

        // DELETE api/<CityController>/5
        [HttpDelete("{id}")]
        public StatusResponseModel2 Delete(int id)
        {
            StatusResponseModel2 _objResponseModel = new StatusResponseModel2();
            //To check if the ID exists in Database
            string query1 = @"select count(*)
                              from country
                              where id = @id";
            string sqlDataSource1 = _configuration.GetConnectionString("phonebookDB");
            int rowexists = 0;
            
            Country countrydata =new Country();

            using (SqlConnection myCon1 = new SqlConnection(sqlDataSource1))
            {


                myCon1.Open();
                using (SqlCommand myCommand = new SqlCommand(query1, myCon1))
                {

                   // myCommand.Parameters.Add("@id",SqlDbType.Int);
                    myCommand.Parameters.AddWithValue("@id",id);
                    rowexists = (int)myCommand.ExecuteScalar();

                    myCon1.Close();
                }
            }
            //end To check if the ID exists in Database
            if (rowexists == 1)
            {
                string query = @"
                           delete from country where id=@id
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
                _objResponseModel.Message = "Country Data Deleted successfully";
                return _objResponseModel;
            }
            else
            {
                _objResponseModel.Status = false;
                _objResponseModel.Message = "Country Data does not exists";
                return _objResponseModel;
            }
        }


    }
}