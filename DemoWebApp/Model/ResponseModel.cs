namespace DemoWebApp.Model
{
    public class ResponseModel
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
    
   
}
