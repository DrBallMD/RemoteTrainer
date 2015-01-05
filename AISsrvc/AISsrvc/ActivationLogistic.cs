using System;

namespace AISsrvc
{
	public class ActivationLogistic:ANeuralNetwork.IActivation
	{
		public double aFunction(double x){
			return 1.0/(1.0+Math.Exp(-x));
		}
	}
}

