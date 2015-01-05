using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
namespace ANeuralNetwork
{
	public class ANeuron
	{

		[XmlArray("Links"),XmlArrayItem("l")]
		public List<ALink> links;
		public int actType;
		private double output;
		public ANeuron(){
			this.links = new List<ALink> ();
		}
		public ANeuron (int act_type, int numOfWeights)
		{
			this.links = new List<ALink> ();
			for (int i=0; i<numOfWeights; i++) {
				this.links.Add (new ALink());
			}
			this.actType = act_type;
		}
		public ALink getLink(int i){
			return this.links [i];
		}
		public void setLink(int i, ALink newValue){
			this.links [i] = newValue;
		}
		public void setOutput(double output){
			this.output = output;
		}
		public double getOutput(){
			return this.output;
		}
		public double calcOutput(List<double> input){
			double sum = 0;
			int i = 0;
			foreach (double x in input) {
				sum += links [i].weight * x;
				i++;
			}
			IActivation act = null;
			if (this.actType == 0) {
				act = new ActivationLogicFunction ();
			}
			this.output = act.aFunction (sum);
			return this.output;
		}
	}
}

