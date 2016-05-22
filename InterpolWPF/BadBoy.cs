using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InterpolWPF
{
    public class BadBoy
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Nickname { get; set; }
        public int Height { get; set; }
        public string EyeColor { get; set; }
        public string HairColor { get; set; }
        public string Citizenship { get; set; }
        public List<string> DistinguishingTraits { get; set; }
        public List<string> Languages { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string LastLivingPlace { get; set; }
        public string LastCase { get; set; }
        public string Profession { get; set; }

        public bool isFound(string s)
        {
            PropertyInfo[] properties = typeof(BadBoy).GetProperties();
            foreach (var i in properties)
            {
                if (i.GetValue(this) != null && i.GetValue(this).ToString().ToLower().Contains(s.ToLower()))
                    return true;
            }
            return false;
        }
    }
}
