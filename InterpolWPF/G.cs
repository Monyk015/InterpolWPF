using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace InterpolWPF
{
    public class G
    {
        public static List<BadBoy> BadBoys = new List<BadBoy>();

        public static List<int> Deleted = new List<int>();

        public static void SaveToFile()
        {
            string json = JsonConvert.SerializeObject(new Package { Array = G.BadBoys, Deleted = G.Deleted });
            using (StreamWriter file = new StreamWriter("data.txt", false))
            {
                file.Write(json);
            }

        }

        public static void ReadFromFile()
        {
            try
            {
                StreamReader file = new StreamReader("data.txt");
            }
            catch(FileNotFoundException)
            {
                StreamWriter file = new StreamWriter("data.txt", false);
            }
            using (StreamReader file = new StreamReader("data.txt"))
            {
                string json = file.ReadToEnd();
                BadBoys = JsonConvert.DeserializeObject<Package>(json).Array;
                if(BadBoys == null)
                    BadBoys = new List<BadBoy>();
            }
        }
    }

    public class Package
    {
        public List<BadBoy> Array;
        public List<int> Deleted;
    }
}