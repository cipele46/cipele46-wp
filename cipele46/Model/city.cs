
namespace cipele46.Model
{
    public class city
    {
        public string created_at { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int region_id { get; set; }
        public string updated_at { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is city)
            {
                return ((city)obj).id.Equals(this.id);
            }
            return base.Equals(obj);
        }
    }
}
