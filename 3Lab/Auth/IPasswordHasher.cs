using System;
using System.Collections.Generic;
using System.Text;

namespace _3Lab.Auth
{
    public interface IPasswordHasher
    {
        byte[] GetHash(string password, string username);
    }
}
