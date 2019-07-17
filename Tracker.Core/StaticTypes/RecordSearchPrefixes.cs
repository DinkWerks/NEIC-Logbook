using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Core.StaticTypes
{
    public class RecordSearchPrefixes
    {
        public static readonly Prefix A = new Prefix("A", "Shasta County");
        public static readonly Prefix B = new Prefix("B", "Plumas County");
        public static readonly Prefix C = new Prefix("C", "Siskiyou County");
        public static readonly Prefix D = new Prefix("D", "Private/Non-Arch.");
        public static readonly Prefix E = new Prefix("E", "CFIP's & SIP's");
        public static readonly Prefix F = new Prefix("F", "VMP's");
        public static readonly Prefix G = new Prefix("G", "Feds - FMHA, US Army Corps, Etc.");
        public static readonly Prefix H = new Prefix("H", "State - CALTRANS, DWR, Etc.");
        public static readonly Prefix I = new Prefix("I", "Lassen County");
        public static readonly Prefix J = new Prefix("J", "Misc. City/County");
        public static readonly Prefix K = new Prefix("K", "THP's/CDF Project");
        public static readonly Prefix L = new Prefix("L", "Butte County");
        public static readonly Prefix M = new Prefix("M", "Trinity County");
        public static readonly Prefix N = new Prefix("N", "Sierra County");
        public static readonly Prefix O = new Prefix("O", "Town of Paradise");
        public static readonly Prefix P = new Prefix("P", "Modoc County");
        public static readonly Prefix Q = new Prefix("Q", "City of Oroville");
        public static readonly Prefix R = new Prefix("R", "City of Chico");
        public static readonly Prefix S = new Prefix("S", "City of Red Bluff");
        public static readonly Prefix T = new Prefix("T", "City of Redding");
        public static readonly Prefix U = new Prefix("U", "City of Shasta Lake");
        public static readonly Prefix V = new Prefix("V", "City of Weed");
        public static readonly Prefix W = new Prefix("W", "In-House/Confidentiality Form");
        public static readonly Prefix X = new Prefix("X", "Tehama County");
        public static readonly Prefix Y = new Prefix("Y", "Glenn County");
        public static readonly Prefix Z = new Prefix("Z", "Sutter County");

        public static IEnumerable<Prefix> Values
        {
            get
            {
                yield return A;
                yield return B;
                yield return C;
                yield return D;
                yield return E;
                yield return F;
                yield return G;
                yield return H;
                yield return I;
                yield return J;
                yield return K;
                yield return L;
                yield return M;
                yield return N;
                yield return O;
                yield return P;
                yield return Q;
                yield return R;
                yield return S;
                yield return T;
                yield return U;
                yield return W;
                yield return X;
                yield return Y;
                yield return Z;
            }
        }
    }

    public class Prefix
    {
        public string Code { get; private set; }
        public string Name { get; private set; }

        public Prefix(string code, string name)
        {
            Code = code;
            Name = name;
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
