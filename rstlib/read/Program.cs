using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using rstlib;
namespace read
{
    class Program
    {
        static void Main(string[] args)
        {
            FileStream stream = new FileStream("./scene.json", FileMode.Open);// ofd.FileName;
            StreamReader streamReader = new StreamReader(stream);
            string json = streamReader.ReadToEnd();

            //var temp = JsonConvert.DeserializeObject(json);
            JsonFile temp = JsonConvert.DeserializeObject<JsonFile>(json);
            Process.bSDF_s = new List<BSDF_>();
            Process.ProBSDF(json,temp.bsdfs.Count);
           // Type t = temp.GetType();
        }
    }
}
