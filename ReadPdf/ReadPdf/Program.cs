using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReadPdf.model;

namespace ReadPdf
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a route argument.");
                return 1;
            }

            string route = args[0];
            //string route = "407547";
            //string path = @"\\DESKTOP-RJP17DV\Users\Public\Documents\" + route;
            string path = @"\\Siacetelynetq\pads_plataforma\" + route;
            var directory = new DirectoryInfo(path);

            /* Get the latest available file */
            /*var file = (from f in directory.GetFiles()
                    orderby f.LastWriteTime descending
                    select f).First();*/

            var files = (from f in directory.GetFiles()
                        orderby f.LastWriteTime descending
                        select f);

            List<Filemodel> ListFilemo = new List<Filemodel>();
            List<Filepdfmodel> ListFilepdf = new List<Filepdfmodel>();

            foreach (var file in files)
            {
                Filemodel filemo = new Filemodel();
                filemo.file = file.Name.Replace(".pdf", "");
                filemo.route = route;
                filemo.filedes= "File " + file.Name.Replace(".pdf", "");
                ListFilemo.Add(filemo);
                Console.WriteLine(file.Name.Replace(".pdf", ""));
                Byte[] bytes = File.ReadAllBytes(file.FullName);
                String s = Convert.ToBase64String(bytes);
                //Console.Write(s);
                Filepdfmodel filepdf = new Filepdfmodel();
                filepdf.name= file.Name.Replace(".pdf", "");
                filepdf.route = route;
                filepdf.mime = "application/pdf";
                filepdf.data = s;
                ListFilepdf.Add(filepdf);
            }
            string jsonFilePdf = JsonConvert.SerializeObject(ListFilepdf);
            string jsonFile= JsonConvert.SerializeObject(ListFilemo);

            File.WriteAllText("files.json", jsonFile);
            File.WriteAllText("filespdf.json", jsonFilePdf);
            Console.WriteLine(" Se generó files.json y  filespdf.json");
            Console.ReadLine();

            return 0;

        }
    }
}
