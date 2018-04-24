using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rstlib
{
    public class Process
    {
        public static List<BSDF_> bSDF_s;

        public static void ProBSDF(string s, int count)
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

                string temp = ChangeName(new string(s_c, i, j - i + 1), num);

                bSDF_s.Add(new BSDF_(num++, new string(s_c, i, j - i + 1)));
            }
        }

        public static string ChangeName(string s,int i)
        {
            int index = s.IndexOf("name", StringComparison.Ordinal);
            int end = s.IndexOf(",", StringComparison.Ordinal);
            return new string(s.ToCharArray(),0,index+6)+"\""+i.ToString()+"\""+new string(s.ToCharArray(),end,s.Length-end);
        }
    }
}
