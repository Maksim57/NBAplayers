using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NBAplayers
{
    public class Menu
    {
        public int MaxYears;
        public double MinRate;
        public List<NBAplayer> list;
        public void Show()
        {
            Console.WriteLine("Enter file location:");
            string path = Console.ReadLine();

            if (File.Exists(path))
            {
                try
                {
                    string readText = File.ReadAllText(path);
                    list = JsonConvert.DeserializeObject<List<NBAplayer>>(readText);
                    if(list == null)
                    {
                        Console.WriteLine("The file is empty.");
                        Show();
                    }
                    if(list.Count > 0)
                    {
                        Console.WriteLine("{0} records were found in the file.", list.Count);
                    }
                    GetMaxYears();
                    GetMinRate();
                    FilterCriteria();

                }
                catch (Exception)
                {

                    Console.WriteLine("Invalid data format.");
                    Show();
                }

            }
            else
            {
                Console.WriteLine("File dose not exist.");
                Show();
            }
        }
        public void GetMaxYears()
        {
            Console.WriteLine("Enter the maximum number of years that player has played in the league:");
            bool IsValidNumber = int.TryParse(Console.ReadLine(), out MaxYears);
            if (!IsValidNumber || MaxYears < 0)
            {
                Console.WriteLine("Enter a valid number");
                GetMaxYears();
            }
        }
        public void GetMinRate()
        {
            Console.WriteLine("Enter minimum rating the player should have:");
            bool IsValidRate = double.TryParse(Console.ReadLine(), out MinRate);
            if (!IsValidRate || MinRate < 0)
            {
                Console.WriteLine("Enter a valid rate number");
                GetMinRate();
            }
        }
        public void FilterCriteria()
        {
            var filteredResults = list.FindAll(x => x.Rating > MinRate && x.PlayerSince < DateTime.Now.Year - MaxYears);
            if (filteredResults.Count > 0)
            {
                Console.WriteLine("There are {0} found results.", filteredResults.Count);
                SaveCSV(filteredResults);
            }
            else
            {
                Console.WriteLine("There are no found results.");
            }
           

        }
        public void SaveCSV(List<NBAplayer> listToSave)
        {
            var result = string.Empty;
            foreach (var item in listToSave)
            {
                result += item.Name + "," + item.Rating + Environment.NewLine;
            }
            Console.WriteLine("Enter the name and location of the CSV file.");
            string path = Console.ReadLine();
            File.WriteAllText(path + ".csv", result);
        }

    }
}

