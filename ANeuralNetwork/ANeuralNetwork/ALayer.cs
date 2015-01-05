using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
namespace ANeuralNetwork
{
	[Serializable]
	[XmlRoot("Layer")]
	public class ALayer
	{	
		[XmlArray("Neurons"),XmlArrayItem("neuron")]	
		public List<ANeuron> neurons;
		public ALayer(){
			this.neurons = new List<ANeuron>();
		}
		public ALayer (int numberOfNeurons, int act_type, int numberOfWeights)
		{
			this.neurons = new List<ANeuron> ();
			for (int i=0; i<numberOfNeurons; i++) {
				this.neurons.Add(new ANeuron(act_type,numberOfWeights));
			}

		}

		public ANeuron getNeuron(int i){
			return this.neurons [i];
		}

		public int getNeuronsCount(){
			return this.neurons.Count;
		}

		public List<double> getResult(){
			List<double> results = new List<double> ();
			foreach (ANeuron n in neurons) {
				results.Add (n.getOutput ());
			}
			return results;
		}

		public List<double> calcResult(List<double> input){
			List<double> results = new List<double> ();
			foreach (ANeuron n in neurons) {
				results.Add(n.calcOutput (input));
			}
			return results;
		}


		public List<List<ALink> > getLinks(){
			List<List<ALink> > result = new List< List<ALink> > ();
			foreach (ANeuron n in this.neurons) {
				List<ALink> lst = new List<ALink> ();
				foreach (ALink lnk in n.links) {
					lst.Add (lnk);
				}
				result.Add (lst);
			}
			return result;
		}
	}
}

