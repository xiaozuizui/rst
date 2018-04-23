using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rstlib
{
    public class BSDF
    {
        //public string name;
        //public List<float> albedo;
        //public string type;
        //public float exponent;
        //public float diffuse_ratio;
        //public string distribution;
        //public float roughness;
        //public string material;
        //public float ior;
        //public float thickness;
        //public float sigma_a;
        //public string slpha;
        //public bool enable_refraction;
    }
    public class BSDF_
    {
        public BSDF_(int num_, string content_)
        {
            num = num_;
            content = content_;
        }
        public int num;
        public string content;
    }
}
