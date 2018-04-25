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
            FileStream stream = new FileStream("./bathroom-1/scene.json", FileMode.Open);// ofd.FileName;
            StreamReader streamReader = new StreamReader(stream);
            string json = streamReader.ReadToEnd();

            //var temp = JsonConvert.DeserializeObject(json);
            JsonFile temp = JsonConvert.DeserializeObject<JsonFile>(json);
            Process.Path = "bathroom-1/";
            Process.bSDF_s = new List<BSDF_>();
            Process.Primitive_s = new List<Primitive_>();
            Process.ProcessBSDF(json,temp.bsdfs.Count);
            Process.ProcessPRIMI(json,temp.primitives.Count);
           // Type t = temp.GetType();
        }
    }
}
