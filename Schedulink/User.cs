using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedulink
{
    internal class User
    {
        String name;
        String surname;
        String username;
        String password;
        String email;


        public User() 
        {
        }


        public User(String name, String surname, String username, String password, String email) 
        {
            this.name = name;
            this.surname = surname;
            this.username = username;
            this.password = password;
            this.email = email;
        }

        public String getName()
        {
            return name;
        }
        public String getSurname()
        {
            return surname;
        }
        public String getFullName() 
        {
            return name + " " + surname;
        }
        public String getUsername()
        {
            return username;
        }

        public String getPassword()
        {
            return password;
        }

        public String getEmail()
        {
            return email;
        }
        
    }
}
