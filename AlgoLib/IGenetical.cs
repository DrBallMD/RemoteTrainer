using System;
using System.Collections.Generic;
using System.Text;

namespace AlgoLib
{
    public interface IGenetical
    {
        void NextGeneration();
        string GetBest();
        //bool IsAcceptableDeviation();
    }
}
