using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersStore.Dal
{
     public static class Salt
    {
         static Salt()
        {
            SALT = Encoding.UTF8.GetBytes("Mysalt");

        }
        public static byte[] SALT { get; set; }
     }
}

