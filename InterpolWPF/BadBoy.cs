using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

namespace InterpolWPF
{
    public class BadBoy
    {
        public DateTime _birthDate;
        public List<string> _distinguishingTraits;
        public List<string> _languages;
        public string _name, _surname, _nickname,_eyeColor,_hairColor, _citizenship;
        public DateTime _nameUpdateTime;
        public string _birthPlace;
        public DateTime _birthPlaceUpdateTime;
        public DateTime _surnameUpdateTime;
        public int _height;
        public DateTime _heightUpdateTime;
        public DateTime _eyeColorUpdateTime;
        public DateTime _hairColorUpdateTime;
        public string _lastLivingPlace;
        public DateTime _lastLivingPlaceUpdateTime;
        public string _lastCase;
        public DateTime _lastCaseUpdateTime;
        public string _profession;
        public DateTime _professionUpdateTime;
        public DateTime _citizenshipUpdateTime;
        public DateTime _distinguishingTraitsUpdateTime;
        public DateTime _languagesUpdateTime;
        public DateTime _birthDateUpdateTime;
        public DateTime _nicknameUpdateTime;
        public List<int> _accomplices;
        public int id;
        public DateTime _accomplicesUpdateTime;
        public List<Crimes> _Crimes;
        private DateTime _CrimesUpdateTime;

        [JsonIgnore]
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _nameUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                _surnameUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string Nickname
        {
            get { return _nickname; }
            set
            {
                _nickname = value;
                _nicknameUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string Height
        {
            get { return _height.ToString(); }
            set
            {
                try
                {
                    _height = Int32.Parse(value);
                }
                catch
                {
                    _height = 0;
                }
                _heightUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string EyeColor
        {
            get { return _eyeColor; }
            set
            {
                _eyeColor = value;
                _eyeColorUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string HairColor
        {
            get { return _hairColor; }
            set
            {
                _hairColor = value;
                _hairColorUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string Citizenship
        {
            get { return _citizenship; }
            set
            {
                _citizenship = value;
                _citizenshipUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string BirthPlace
        {
            get { return _birthPlace; }
            set
            {
                _birthPlace = value;
                _birthPlaceUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string LastLivingPlace
        {
            get { return _lastLivingPlace; }
            set
            {
                _lastLivingPlace = value;
                _lastLivingPlaceUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string LastCase
        {
            get { return _lastCase; }
            set
            {
                _lastCase = value;
                _lastCaseUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string Profession
        {
            get { return _profession;}
            set
            {
                _profession = value;
                _professionUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string BirthDate
        {
            get { return _birthDate == DateTime.Now ? "" : _birthDate.Date.ToString("d"); }
            set
            {
                try
                {
                    _birthDate = DateTime.Parse(value);
                }
                catch
                {

                }
                _birthDateUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string DistinguishingTraits
        {
            get
            {
                if (_distinguishingTraits != null)
                    return String.Join(",", _distinguishingTraits.ToArray());
                return null;
            }
            set
            {
                _distinguishingTraits = value.Split(',').ToList();
                _distinguishingTraitsUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public List<BadBoy> Accomplices
        {
            get
            {
                var toGet = new List<BadBoy>();
                if(_accomplices != null)
                    foreach (var i in _accomplices)
                    {
                        toGet.Add(G.BadBoys[G.BadBoys.FindIndex(x => x.id == i)]);
                    }
                return toGet;
            }
            set
            {
                var toSet = new List<int>();
                foreach (var i in value)
                {
                    toSet.Add(i.id);
                }
                _accomplices = toSet;
                _accomplicesUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public string Languages
        {
            get
            {
                if (_languages != null)
                    return String.Join(",", _languages.ToArray());
                return null;
            }
            set
            {
                _languages = value.Split(',').ToList();
                _languagesUpdateTime = DateTime.Now;
            }
        }
        [JsonIgnore]
        public List<Crimes> Crimes
        {
            get
            {
                return _Crimes;
            }
            set
            {
                _Crimes = value;
                _CrimesUpdateTime = DateTime.Now;
            }
        }
        //Properties and fields

        public BadBoy()
        {
            id = G.BadBoys.Count > 0? G.BadBoys.Last().id + 1 : 0;
        }


        public bool IsFound(string s)
        {
            PropertyInfo[] properties = typeof(BadBoy).GetProperties();
            foreach (var i in properties)
            {
                if (i.GetValue(this) != null && i.GetValue(this).ToString().ToLower().Contains(s.ToLower()))
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Name} \"{Nickname}\" {Surname}";
        }
    }
}
