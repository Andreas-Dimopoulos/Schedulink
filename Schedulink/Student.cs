using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedulink
{
    internal class Student : User
    { 
        public Student() { }

        public Student(String name, String surname, String username, String password, String email) 
            : base(name, surname, username, password, email) 
        {
            
        }

        
        
    }
}
