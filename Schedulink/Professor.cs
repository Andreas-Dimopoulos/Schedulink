using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedulink
{
    internal class Professor : User
    {
        String phone;


        public Professor(String name, String surname, String username, String password, String email, String phone) 
            : base(name, surname, username, password, email)
        {
            
            this.phone = phone;
        }
        
    }
}
