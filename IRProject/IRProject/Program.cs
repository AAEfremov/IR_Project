using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;

namespace IRProject
{
    class Program
    {
        static void Main(string[] args)
        {
            List<DocStructure> mylist = new List<DocStructure>();
            string place = @"C:\Users\Элона\Documents\Visual Studio 2012\Projects\ConsoleApplication5\ConsoleApplication5\bin\Debug\text";
            parser(mylist, place);
            var l = new LuceneWork();
            l.BuildIndex(mylist);
            
            Console.WriteLine("write");
            var a = Console.ReadLine();
            var res = l.Search(a);
            Console.WriteLine(res.Count());



            Console.ReadLine();

        }

        static void parser(List<DocStructure> mylist, string place)
        {
            string help;
            string name;
            
            DirectoryInfo dir = new DirectoryInfo(place);
            foreach (var item in dir.GetFiles())
            {
                name = item.FullName;

                string text = System.IO.File.ReadAllText(name);

                //reviews
                string pattern = @"(?<=<TEXT>)(.*)(?=</TEXT>)";
                Regex rgx = new Regex(pattern);

                foreach (Match match in rgx.Matches(text))
                {
                    DocStructure doc1 = new DocStructure();
                    doc1.review = match.Value;
                    mylist.Add(doc1);
                }
                //fav
                pattern = @"(?<=<FAVORITE>)(.*)(?=</FAVORITE>)";
                rgx = new Regex(pattern);
                int i = 0;
                foreach (Match match in rgx.Matches(text))
                {
                    mylist[i].fav = match.Value;
                    i++;

                }
                help = Regex.Match(text, @"(?<=<DOCNO>)(.*)(?=</DOCNO>)").ToString();
                help = help.Remove(0, 5);
                help = help.Replace("_", " ");
                //Console.WriteLine(help);
                for (i = 0; i < mylist.Count; i++)
                {
                   
                    mylist[i].name = help;

                }

            }
        }
    }
}
