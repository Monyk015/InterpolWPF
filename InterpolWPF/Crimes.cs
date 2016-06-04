using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InterpolWPF
{
    public class Crimes
    {
        public static ObservableCollection<Crimes> AllCrimes = new ObservableCollection<Crimes>
            {
                new Crimes
                {
                    Name = "Acts leading to death or intending to cause death",
                    Code = "01",
                    Children = new ObservableCollection<Crimes>
                    {
                        new Crimes
                        {
                            Name = "Intentional homicide",
                            Code = "0101"
                        },
                        new Crimes
                        {
                            Name = "Attempted intentional homicide",
                            Code = "0102"
                        },
                        new Crimes
                        {
                            Name = "Non-intentional homicide",
                            Code = "0103",
                            Children = new ObservableCollection<Crimes>
                            {
                                new Crimes
                                {
                                    Name = "Non-negligent manslaughter",
                                    Code = "01031"
                                },
                                new Crimes
                                {
                                    Name = "Negligent manslaughter",
                                    Code = "01032",
                                    Children = new ObservableCollection<Crimes>
                                    {
                                        new Crimes
                                        {
                                            Name = "Vehicular homicide",
                                            Code = "010321"
                                        },
                                        new Crimes
                                        {
                                            Name = "Non-vehicular homicide",
                                            Code = "010322"
                                        }
                                    }
                                }
                            }
                        },
                        new Crimes
                        {
                            Name = "Assisting or instigating suicide",
                            Code = "0104",
                            Children = new ObservableCollection<Crimes>
                            {
                                new Crimes
                                {
                                    Name = "Assisting suicide",
                                    Code = "01041"
                                },
                                new Crimes
                                {
                                    Name = "Other acts of assisting or instigating suicide ",
                                    Code = "01049"
                                }
                            }
                        },
                        new Crimes
                        {
                            Name = "Euthanasia",
                            Code = "0105"
                        },
                        new Crimes
                        {
                            Name = "Illegal feticide",
                            Code = "0106"
                        },
                        new Crimes
                        {
                            Name = "Unlawful killing associated with armed conflict",
                            Code = "0107"
                        },
                        new Crimes
                        {
                            Name = "Other acts leading to death or intending to cause death",
                            Code = "0108"
                        }
                    }
                }
        };

        public string Name { get; set; }
        public string Code { get; set; }
        [JsonIgnore]
        public ObservableCollection<Crimes> Children { get; set; }

        public bool isSuccessor(Crimes s)
        {
            if (s.Code.IndexOf(this.Code) == 0)
                return true;
            return false;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}