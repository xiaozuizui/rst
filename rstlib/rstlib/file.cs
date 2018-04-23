using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rstlib
{
    public class JsonFile
    {
        //public Media media;
        public List<BSDF> bsdfs;
        public List<Primitives> primitives;
        public Camera camera;
        public Integrator integrator;
        public Renderer renderer;

        public List<BSDF_> bSDF_s;

        public static string ProAlbedo(string s )
        {
            int i = 0;
            int j = 0;
            int strindex = 0;

            string restr = null;
            char[] s_c = s.ToCharArray();

            i = s.IndexOf("albedo", i);
           
            while (i != -1)
            {
              

                restr += new string(s_c, strindex, i + 8 - strindex);

                // char temp = s[i + 8];
                if (s[i + 8] == ' ')
                    i++;
                if (s[i + 8] != '[')
                {
                    j = s.IndexOf(",", i);
                    string num = new string(s_c, i + 8, j - i - 8);
                    restr += "[" + num + "," + num + "," + num + "]";
                    strindex = j;
                }
                else
                {
                    //j = s.IndexOf(",", i);
                    //j = s.IndexOf(",", j+1);
                    //j = s.IndexOf(",", j+1);
                    strindex = i+8;
                }
                i = s.IndexOf("albedo", i+1);

                if (i == -1)
                {
                    restr += new string(s_c, strindex, s_c.Length - strindex);
                    break;
                }
                    //break;
                j = s.IndexOf(",", i);
            }
          //  if(s[i+7]!='[')

            return restr;
        }

        public  void ProBSDF(string s)
        {
            int num = 0;

            int i = 0;
            int j = 0;
            int strindex = 0;

            string restr = null;
            char[] s_c = s.ToCharArray();

            i = s.IndexOf("bsdfs", 0);

            while(num<bsdfs.Count)
            {
                i = s.IndexOf("{", i + 1);
                
                j = s.IndexOf("}", i);

                
                bSDF_s.Add( new BSDF_(num++, new string(s_c, i, j - i+1)));
            }
           
        }

        public static void ReName(string path,string name)
        {
            System.IO.Directory.Move(path, name);
        }
    }
}
