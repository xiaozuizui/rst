using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace rstlib
{
    public struct CameraStruct
    {
        public int[] resolution;
        public float[] position;
        public float[] look_at;
        public float[] up;
        public float fov;
    }
    public class Process
    {
        public static string Path;
        public static List<BSDF_> bSDF_s;
        public static List<Primitive_> Primitive_s;
        public static int bsdfnum;
        public static int priminum;
        public static void ProcessBSDF(string s, int count)
        {
            int num = 0;

            int i = 0;
            int j = 0;
            int strindex = 0;

            string restr = null;
            char[] s_c = s.ToCharArray();

            i = s.IndexOf("bsdfs", 0, StringComparison.Ordinal);

            while (num < count)
            {
                i = s.IndexOf("{", i + 1, StringComparison.Ordinal);

                j = s.IndexOf("}", i, StringComparison.Ordinal);

                bSDF_s.Add(new BSDF_(num+bsdfnum, ChangeName(new string(s_c, i, j - i + 1), num+bsdfnum)));
                num++;
            }

            bsdfnum += num;
        }

        public static void ProcessPRIMI(string s, int count)
        {
            int num =0;

            int i = 0;
            int j = 0;
            int strindex = 0;

            string restr = null;
            char[] s_c = s.ToCharArray();

            j = s.IndexOf("primitives", 0, StringComparison.Ordinal);

            while (num < count)
            {
                i = s.IndexOf("{", j, StringComparison.Ordinal);

                j = s.IndexOf("}", i, StringComparison.Ordinal);
                j = s.IndexOf("}", j + 1, StringComparison.Ordinal);
                
                Primitive_s.Add(new Primitive_(num+priminum, ChangeModelAndFile(new string(s_c, i, j - i + 1), num+priminum)));
                num++;
                // Primitive_s.Add(new Primitive_(num, ChangeName(new string(s_c, i, j - i + 1), num++)));
            }
            priminum += num;
        }
        public static string ChangeName(string s,int i)
        {
            int index = s.IndexOf("name", StringComparison.Ordinal);
            int end = s.IndexOf(",",index, StringComparison.Ordinal);
            return new string(s.ToCharArray(),0,index+6)+"\""+i+"\""+new string(s.ToCharArray(),end,s.Length-end);
        }

        public static string ChangeModelAndFile(string s,int i)
        {
            char [] temp_s = s.ToCharArray();
            int index = s.IndexOf("name", StringComparison.Ordinal);
            string temp;
            int end;

            if (index == -1)
            {
                temp = "";
                end = 0;
            }
            else
            {
                end = s.IndexOf(",", index, StringComparison.Ordinal);
                temp = new string(temp_s, 0, index + 6) + "\"" + i + "\"";
            }
           
            index = s.IndexOf("file", StringComparison.Ordinal);

            temp+=new string(temp_s,end,index-end+7);
            end = s.IndexOf(",", index, StringComparison.Ordinal);
            string filename = new string(temp_s,index+8,end-index-9);
            ChangeFileName(Path+filename,Path+i+".wo3");
            temp += " \"" + i+".wo3" + "\"" + new string(temp_s, end, s.Length - end);
            return temp;
            // end =
        }

        public static void ChangeFileName(string old_file, string new_file)
        {
            try
            {
                Directory.Move(old_file, new_file);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
           
        }
        public static void ReadFile(string Path)
        {
            FileStream file = new FileStream(Path, FileMode.OpenOrCreate);
            StreamReader sw = new StreamReader(file);
            string s = sw.ReadToEnd();
            char[] s_c = s.ToCharArray();

            int i = 0;
            int num_b = 0;
            int num_p = 0;

            int start = 0;
            int end = 0;

            int bs_end = s.IndexOf("!") - 1;

            while (end < bs_end)
            {
                end = s.IndexOf("#", start, StringComparison.Ordinal);
                bSDF_s.Add(new BSDF_(num_b, new string(s_c, start, end - start)));
                start = end + 1;
            }

            while (true)
            {
                end = s.IndexOf("#", start, StringComparison.Ordinal);
                if (end == -1)
                {
                    break;
                }
                Primitive_s.Add(new Primitive_(num_b, new string(s_c, start, end - start)));
                start = end + 1;
            }

        }
        public static void SaveBsdf(string path)
        {
            string re = "";
            for(int i=0;i<bsdfnum;i++)
            {
                re += bSDF_s[i].content + "#";
            }

            re += "!";
            for (int j = 0; j < priminum; j++)
            {
                re += Primitive_s[j].content + "#";
            }
            FileStream file = new FileStream(path,FileMode.Create);
            StreamWriter sw = new StreamWriter(file);
            sw.Write(re);
            sw.Close();
        }

        private static int GetRandomSeed()
        {
            byte[] bytes = new byte[4];
            RNGCryptoServiceProvider csp = new RNGCryptoServiceProvider();
            csp.GetBytes(bytes);
            return BitConverter.ToInt32(bytes,0);
        }

        

        public static void MakeFile(string path,int p_num,string name,CameraStruct c)
        {
            string json = "{\n\r\"media\":[],\n\r\"primitives\":[ ";

            List<int> bsdfs = new List<int>();
            List<int> primitives = new List<int>();
            
            Random p_n = new Random();
          
            for (int i = 0; i < p_num; i++)
            {
                string temp_p = "";
               
                p_n= new Random(GetRandomSeed());
                int num = p_n.Next(0, Primitive_s.Count);
                primitives.Add(num);
                var indexOf = Primitive_s[num].content.IndexOf("bsdf", StringComparison.Ordinal);
                char[] sc = Primitive_s[num].content.ToCharArray();

                temp_p = new string(sc,0,indexOf+6);
                p_n = new Random(GetRandomSeed());
                int num_d = p_n.Next(0, bSDF_s.Count);
                bsdfs.Add(num_d);

                temp_p +="\""+ num_d.ToString()+"\"";

                int start = Primitive_s[num].content.IndexOf("\"", indexOf+6, StringComparison.Ordinal);
                start = Primitive_s[num].content.IndexOf("\"", start + 1, StringComparison.Ordinal)+1;
                temp_p += new string(sc, start, Primitive_s[num].content.Length - start);
                json += temp_p + ",\n\r";
            }

            //bsdfs =  (List<int>) System.Linq.Enumerable.Distinct(bsdfs);

            json += "],\r\n\"bsdfs\":[";

            for (int i = 0; i < bsdfs.Count; i++)
            {
                json += bSDF_s[bsdfs[i]].content + ",\r\n";
            }

            json += "],\r\n";


            json +=
                "\"integrator\": {\r\n        \"min_bounces\": 0,\r\n        \"max_bounces\": 64,\r\n        \"enable_consistency_checks\": false,\r\n        \"enable_two_sided_shading\": true,\r\n        \"type\": \"path_tracer\",\r\n        \"enable_light_sampling\": true,\r\n        \"enable_volume_light_sampling\": true\r\n    },\r\n    \"renderer\": {\r\n        \"overwrite_output_files\": true,\r\n        \"adaptive_sampling\": true,\r\n        \"enable_resume_render\": false,\r\n        \"stratified_sampler\": true,\r\n        \"scene_bvh\": true,\r\n        \"spp\": 16,\r\n        \"spp_step\": 32768,\r\n        \"checkpoint_interval\": \"0\",\r\n        \"timeout\": \"0\",\r\n        \"output_file\": \"TungstenRender.png\",\r\n\t\t\r\n        \"resume_render_file\": \"TungstenRenderState.dat\",\r\n    ";
            json += "\"hdr_output_file\":\"" +name +  "\" },\r\n";
            json +=
                " \"camera\": {\"tonemap\": \"filmic\",\r\n       \"reconstruction_filter\": \"tent\",\r\n        \"transform\": {\r\n            \"position\": [" +
                c.position[0].ToString() + "," + c.position[1].ToString() + "," + c.position[2].ToString() +
                "], \"look_at\": [" + c.look_at[0].ToString() + "," + c.look_at[1].ToString() + "," +
                c.look_at[2].ToString() + "],\"up\": [" + c.up[0].ToString() + "," + c.up[1].ToString() + "," +
                c.up[2].ToString() + "]\r\n }," + "\"resolution\": [" + c.resolution[0] + "," + c.resolution[1] + "]," +
                "\"type\": \"pinhole\",\r\n        \"fov\":" + c.fov.ToString() + "},";

                json += "}";

            FileStream file = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(file);
            sw.Write(json);
            sw.Close();

            
           
        }
    }

    class FileJson
    {
        public FileJson(List<BSDF_> b, List<Primitive_> p)
        {
            bSDF_s = b;
            Primitive_s = p;
        }
        public  List<BSDF_> bSDF_s;
        public  List<Primitive_> Primitive_s;
    }


}
