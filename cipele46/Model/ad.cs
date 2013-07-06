
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

    public class ad_mage
    {
        public string url { get; set; }
        public ad_mage large { get; set; }
        public ad_mage mob_large { get; set; }
        public ad_mage medium { get; set; }
        public ad_mage small { get; set; }
    }

    public class ad
    {
        public int ad_type { get; set; }
        public int category_id { get; set; }
        public int city_id { get; set; }
        public string created_at { get; set; }
        public string description { get; set; }
        public string email { get; set; }
        public int id { get; set; }
        public ad_mage image { get; set; }
        public string phone { get; set; }
        public int status { get; set; }
        public string title { get; set; }
        public string updated_at { get; set; }
        public int? user_id { get; set; }
    }
}
