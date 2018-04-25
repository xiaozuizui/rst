using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rstlib
{
    public class Process
    {
        public static string Path;
        public static List<BSDF_> bSDF_s;
        public static List<Primitive_> Primitive_s;
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

                bSDF_s.Add(new BSDF_(num, ChangeName(new string(s_c, i, j - i + 1), num++)));
            }
        }

        public static void ProcessPRIMI(string s, int count)
        {
            int num = 0;

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
                Primitive_s.Add(new Primitive_(num, ChangeModelAndFile(new string(s_c, i, j - i + 1), num++)));
                // Primitive_s.Add(new Primitive_(num, ChangeName(new string(s_c, i, j - i + 1), num++)));
            }
        }
        public static string ChangeName(string s,int i)
        {
            int index = s.IndexOf("name", StringComparison.Ordinal);
            int end = s.IndexOf(",",index, StringComparison.Ordinal);
            return new string(s.ToCharArray(),0,index+6)+"\""+i.ToString()+"\""+new string(s.ToCharArray(),end,s.Length-end);
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
                temp = new string(temp_s, 0, index + 6) + "\"" + i.ToString() + "\"";
            }
           
            index = s.IndexOf("file", StringComparison.Ordinal);

            temp+=new string(temp_s,end,index-end+7);
            end = s.IndexOf(",", index, StringComparison.Ordinal);
            string filename = new string(temp_s,index+8,end-index-9);
            ChangeFileName(Path+filename,Path+i.ToString());
            temp += " \"" + i.ToString() + "\"" + new string(temp_s, end, s.Length - end);
            return temp;
            // end =
        }

        public static void ChangeFileName(string old_file, string new_file)
        {
            System.IO.Directory.Move(old_file,new_file);
        }
    }
}
