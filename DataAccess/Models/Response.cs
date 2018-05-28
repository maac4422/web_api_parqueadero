namespace DataAccess.Models
{
    public class Response
    {
        public int StatusCode { get; set; }

        public string Message { get; set; }

        public void BadRequest(string message = null)
        {
            this.Message = string.IsNullOrEmpty(message) ? "Error de negocio" : message;
            this.StatusCode = 400;
        }
    }
}
