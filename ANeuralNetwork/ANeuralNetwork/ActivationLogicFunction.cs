using System;

namespace ANeuralNetwork
{
	public class ActivationLogicFunction:IActivation
	{
		public double aFunction(double x){
			return 1.0/(1.0+Math.Exp(-x));
		}
	}
}

