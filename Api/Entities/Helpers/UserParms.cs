namespace Api.Entities.Helpers
{
    public class UserParms
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 10;
        
        public int PageSize
        {
            get => _pageSize; 
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
        

        // this param is used to exclude current user name from the members list
        public string CurrentUsername { get; set; }
        // this param is used get only opposite gender of the current user
        public string Gender { get; set; }

        // this to sit min age
        public int MaxAge { get; set; } = 100;
        public int MinAge { get; set; } = 18;
        public string OrderBy { get; set; } = "LastActive";
    }
}