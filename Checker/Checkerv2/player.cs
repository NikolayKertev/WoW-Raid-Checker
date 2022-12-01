using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace RaidChecker
{
    public class player : IComparable<player>
    {
        private string name;
        private clas clas;
        public player(string playerName, string playerClas, string playerSpec)
        {
            name = playerName;

            clas clas = new clas(playerClas, playerSpec);

            this.clas = clas;
        }
        public string Name
        {
            get { return name; }
            private set { }
        }
        public clas Clas
        {
            get { return clas; }
            private set { }
        }

        public int CompareTo(player other)
        {

            if (name == other.name)
            {
                if (clas.ClassName == other.clas.ClassName)
                {
                    if (clas.Spec == other.Clas.Spec)
                    {
                        return 1;
                    }
                }
            }

            return 0;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Name: {name}{Environment.NewLine}");

            sb.Append($"Class: {clas.ClassName}, Specc: {clas.Spec}");

            return sb.ToString();
        }
    }
}
