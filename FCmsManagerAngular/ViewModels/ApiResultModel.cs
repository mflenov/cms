namespace FCmsManagerAngular.ViewModels
{
    public class ApiResultModel {
        public const int FAIL = 0;
        public const int SUCCESS = 1;
        public const int NOT_FOUND = 2;



        public ApiResultModel(int status) {
            this.Status = status;
        }

        public ApiResultModel(int status, string description) {
            this.Status = status;
            this.Description = description;
        }

        public int Status { get; set; }

        public string Description { get; set; } 

        public object Data { get; set; } 
    }
}