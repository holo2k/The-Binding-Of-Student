using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Binding_Of_Student.Code
{
    public class DoorTypeComparer : IEqualityComparer<Door>
    {
        public bool Equals(Door d1, Door d2)
        {
            return d1.Dir == d2.Dir;
        }
        public int GetHashCode(Door type)
        {
            return 42;
        }
    }
}
