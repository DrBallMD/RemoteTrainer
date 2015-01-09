using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticLibrary
{
    public class Chromosome
    {
        public Chromosome()
        {
        }
        public Chromosome(char[] bitarray)
        {
            this.bits = new char[bitarray.Length];
            bitarray.CopyTo(this.bits, 0);
        }
        public Chromosome(Chromosome oldChromo)
        {
            this.bits = new char[oldChromo.GetBits().Length];
            oldChromo.GetBits().CopyTo(this.bits, 0);
        }
        protected char[] bits;
        public int Length
        {
            get
            {
                return bits.Length;
            }
        }
        public char[] GetBits()
        {
            return this.bits;
        }
        public char this[int index]
        {
            get
            {
                return bits[index];
            }
            set
            {
                bits[index] = value;
            }
        }
        public override string ToString()
        {
            return new String(this.bits);
        }
        public override bool Equals(object obj)
        {
            Chromosome chromoObj = obj as Chromosome;
            if (chromoObj != null)
            {
                if (Enumerable.SequenceEqual(this.bits, chromoObj.GetBits()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new Exception();
            }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
