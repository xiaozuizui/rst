using System;
using System.CodeDom;
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

            Process.bsdfnum = 0;
            Process.priminum = 0;

            FileStream stream = new FileStream("./bathroom-1/scene.json", FileMode.Open);// ofd.FileName;
            StreamReader streamReader = new StreamReader(stream);
            string json = streamReader.ReadToEnd();

            //var temp = JsonConvert.DeserializeObject(json);
            JsonFile temp = JsonConvert.DeserializeObject<JsonFile>(json);

            CameraStruct c = new CameraStruct();
            c.fov = 55.0f;
            c.resolution = new[] {1024, 1204};
            c.look_at = new float[]
            {
                (float) -2.787562608718872,
                0.9699121117591858f,
                -2.6775901317596436f
            };
            c.position = new float[]{ 0.0072405338287353516f,
                0.9124049544334412f,
                -0.2275838851928711f};
            c.up = new[]
            {
                0.0f,
                1.0f,
                0.0f
            };

            Process.Path = "bathroom-1/";
            Process.bSDF_s = new List<BSDF_>();
            Process.Primitive_s = new List<Primitive_>();
            Process.ReadFile("bsdf.json");
            for (int i = 0; i < 10; i++)
            {
                Process.MakeFile("./test"+i.ToString()+".json", 400, "test"+i.ToString()+".exr", c);
            }
           
            //Process.ProcessBSDF(json,temp.bsdfs.Count);
            //Process.ProcessPRIMI(json,temp.primitives.Count);
            //Process.SaveBsdf("bsdf.json");
           // Type t = temp.GetType();
        }
    }
}
