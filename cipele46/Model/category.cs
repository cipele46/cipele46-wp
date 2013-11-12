
using System.Collections.Generic;
namespace cipele46.Model
{
    public class category
    {
        public string created_at { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string updated_at { get; set; }
        public List<city> cities { get; set; }
    }
}
