using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareLib
{
    public static class RandomNumber
    {
        [ThreadStatic]
        private static Random _instance;
        public static Random Instance
        {
            get
            {
                return _instance ?? (_instance = new Random(Guid.NewGuid().GetHashCode()));
            }
        }
    }
}
