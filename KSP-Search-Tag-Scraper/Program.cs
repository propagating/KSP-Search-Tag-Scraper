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
            var dictTags = new Dictionary<string, List<string>>();
            var kerbalDirectory = @"D:\Steam\steamapps\common\Kerbal Space Program\GameData\Squad";
            var outputDirectory = @"C:\Users\Ryan\Desktop";

            var category = string.Empty;
            foreach (
                var file in
                    Directory.GetFiles(
                        path: kerbalDirectory,
                        searchPattern: "*.cfg", searchOption: SearchOption.AllDirectories))
            {
                foreach (var line in File.ReadAllLines(file))
                {

                    if (line.Contains("\tcategory ="))
                    {
                        category = line.Split('=')[1];
                    }

                    if (!line.Contains("tags =")) continue;
                    if (!dictTags.ContainsKey(category))
                    {
                        dictTags.Add(category, new List<string>());
                    }

                    var catList = dictTags.FirstOrDefault(x => x.Key == category).Value;

                    var nTags = line.Substring(7).Split(null);
                    foreach (var tag in nTags)
                    {
                        if (string.IsNullOrWhiteSpace(tag) || catList.Contains(tag)) continue;
                        if (tag.Contains(@"#auto")) continue;
                        catList.Add(tag);
                    }
                    catList.Sort(StringComparer.InvariantCultureIgnoreCase);
                }

            }

            using (var newFile = new StreamWriter(outputDirectory+ "\\KSPTags.txt"))
            {

                foreach (var outTag in dictTags)
                {
                    newFile.WriteLine(outTag.Key);

                    foreach (var uniqueTag in outTag.Value)
                    {
                        newFile.WriteLine("\t" + uniqueTag);
                    }

                    newFile.WriteLineAsync("\n");
                }
            }

        }
    }
}

