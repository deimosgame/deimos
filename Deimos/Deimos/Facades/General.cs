using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deimos
{
    static class General
    {
        public static string Uniqid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
