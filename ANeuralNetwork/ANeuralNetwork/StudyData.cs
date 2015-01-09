using System;
using System.Collections.Generic;
namespace ANeuralNetwork
{
	public class StudyData
	{
		public List<double> input;
		public List<double> output;
		public StudyData ()
		{
            this.input = new List<double>();
            this.output = new List<double>();
		}
	}
}

