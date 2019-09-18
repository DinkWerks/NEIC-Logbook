using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tracker.Core.StaticTypes
{
    public class RecordSearchPrefixes
    {
        public static readonly Prefix A = new Prefix("A", "Shasta County", "808008900");
        public static readonly Prefix B = new Prefix("B", "Plumas County", "808008900");
        public static readonly Prefix C = new Prefix("C", "Siskiyou County", "808008900");
        public static readonly Prefix D = new Prefix("D", "Private/Non-Arch.", "808008900");
        public static readonly Prefix E = new Prefix("E", "CFIP's & SIP's", "808008900");
        public static readonly Prefix F = new Prefix("F", "VMP's", "808008900");
        public static readonly Prefix G = new Prefix("G", "Feds - FMHA, US Army Corps, Etc.", "808008900");//
        public static readonly Prefix H = new Prefix("H", "State - CALTRANS, DWR, Etc.", "808008900");
        public static readonly Prefix I = new Prefix("I", "Lassen County", "808008900");
        public static readonly Prefix J = new Prefix("J", "Misc. City/County", "808008900");
        public static readonly Prefix K = new Prefix("K", "THP's/CDF Project", "808008900"); //
        public static readonly Prefix L = new Prefix("L", "Butte County", "808008900");
        public static readonly Prefix M = new Prefix("M", "Trinity County", "808008900");
        public static readonly Prefix N = new Prefix("N", "Sierra County", "808008900");
        public static readonly Prefix O = new Prefix("O", "Town of Paradise", "808008900");
        public static readonly Prefix P = new Prefix("P", "Modoc County", "808008900");
        public static readonly Prefix Q = new Prefix("Q", "City of Oroville", "808008900");
        public static readonly Prefix R = new Prefix("R", "City of Chico", "808008900");
        public static readonly Prefix S = new Prefix("S", "City of Red Bluff", "808008900");
        public static readonly Prefix T = new Prefix("T", "City of Redding", "808008900");
        public static readonly Prefix U = new Prefix("U", "City of Shasta Lake", "808008900");
        public static readonly Prefix V = new Prefix("V", "City of Weed", "808008900");
        public static readonly Prefix W = new Prefix("W", "In-House/Confidentiality Form", "808008900");
        public static readonly Prefix X = new Prefix("X", "Tehama County", "808008900");
        public static readonly Prefix Y = new Prefix("Y", "Glenn County", "808008900");
        public static readonly Prefix Z = new Prefix("Z", "Sutter County", "808008900");

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

        public static Prefix GetPrefix(string prefix)
        {
            Type type = typeof(RecordSearchPrefixes);
            FieldInfo field = type.GetField(prefix.ToUpper());
            return (Prefix)field.GetValue(null);
        }
    }

    public class Prefix
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string BillingCode { get; private set; }

        public Prefix(string code, string name, string billingCode)
        {
            Code = code;
            Name = name;
            BillingCode = billingCode;
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
