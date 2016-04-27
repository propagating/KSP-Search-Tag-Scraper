using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSP_Search_Tag_Scraper
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, HashSet<string>> dictTags = new Dictionary<string, HashSet<string>>();
            var kerbalDirectory = string.Empty;
            var outputDirectory = string.Empty;

            foreach (
                string f in
                    Directory.GetFiles(
                        path: kerbalDirectory,
                        searchPattern: "*.cfg", searchOption: SearchOption.AllDirectories))
            {

                string category = string.Empty;

                foreach (var line in File.ReadAllLines(f))
                {

                    if (line.Contains("\tcategory ="))
                    {
                        category = line.Split('=')[1];
                    }

                    if (line.Contains("tags ="))
                    {
                        if (!dictTags.ContainsKey(category))
                        {
                            dictTags.Add(category, new HashSet<string>());
                        }

                        var catList = dictTags.FirstOrDefault(x => x.Key == category).Value;

                        string[] nTags = line.Substring(7).Split(null);
                        foreach (var tag in nTags)
                        {
                            if (!string.IsNullOrWhiteSpace(tag))
                            {
                                catList.Add(tag);

                            }
                        }

                    }
                }

            }

            using (StreamWriter newFile = new StreamWriter(outputDirectory+ "\\KSPTags.txt"))
            {

                foreach (var outTag in dictTags)
                {
                    newFile.WriteLine(outTag.Key);

                    foreach (var uniqueTag in outTag.Value)
                    {
                        newFile.WriteLine("\t" + uniqueTag);
                    }
                }
            }

        }
    }
}

