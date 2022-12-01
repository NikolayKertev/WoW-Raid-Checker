using System;
using System.Collections.Generic;
using System.Text;

namespace RaidChecker
{
    public class clas
    {
        private string className;
        private string spec;
        public clas(string clas, string spec)
        {
            className = clas;
            this.spec = spec;
        }

        public string ClassName
        {
            get { return className; }
            private set { }
        }
        public string Spec
        {
            get { return spec; }
            private set { }
        }

    }
}
