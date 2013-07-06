
namespace cipele46.Model
{
    public enum types
    {
        supply = 1,
        demand = 2
    }

    public enum status
    {
        pending = 1,
        active = 2,
        closed = 3
    }

    public class ad
    {
        public int status { get; set; }
        public string description { get; set; }
        public int districtID { get; set; }
        public string phone { get; set; }
        public int categoryID { get; set; }
        public string title { get; set; }
        public string imageUrl { get; set; }
        public int id { get; set; }
        public int cityID { get; set; }
        public int type { get; set; }
        public string email { get; set; }
    }
}
