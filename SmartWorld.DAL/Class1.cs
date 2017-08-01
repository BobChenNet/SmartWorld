using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartWorld.DAL
{
    public class Class1
    {
        public int Save(Type className, object obj)
        {
            using (var conn = new SmartWorldEntities())
            {
                return conn.SaveChanges();
            }
        }
    }
}
