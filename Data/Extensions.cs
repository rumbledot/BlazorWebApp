using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppBlazor.Data
{
    public static class Extensions
    {
        public static float ToDPI(this float centimeter)
        {
            var inch = centimeter / 2.54f;
            return (float)(inch * 72);
        }
    }
}
