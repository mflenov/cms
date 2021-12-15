namespace FCmsManagerAngular.ViewModels
{
    public class ApiRequestModel {
        const int FAIL = 0;
        const int SUCCESS = 1;



        public ApiRequestModel(int status) {
            this.Status = status;
        }

        public ApiRequestModel(int status, string description) {
            this.Status = status;
            this.Description = description;
        }

        public int Status { get; set; }

        public string Description { get; set; } 
    }
}