using System;
using System.Collections.Generic;
using System.Text;

namespace Bank
{
    public class Triangle
    {
        public int a;
        public int b;
        public int c;
        public Triangle(int a ,int b ,int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
        public bool isValid()
        {
            if (a <= 0 || b <= 0 || c <= 0)
            {
                return false;
            }
            return true;
        }
    }
}
