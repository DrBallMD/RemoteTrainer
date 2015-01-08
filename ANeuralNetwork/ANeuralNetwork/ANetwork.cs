using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading;
namespace ANeuralNetwork
{[Serializable]
	public class ANetwork
	{
		public ANetwork(){
		}
		protected internal List<ALayer> layers;

		public ANetwork (int inputNeurons, int hiddenLayers, int hiddenNeurons, int outputNeurons, int activation_func)
		{

			Random rnd = new Random ();
			this.layers = new List<ALayer> ();
			#region initvalues
			layers.Add(new ALayer(inputNeurons,activation_func,inputNeurons));
			for(int i=0;i<hiddenLayers;i++){
				ALayer l = new ALayer (hiddenNeurons, activation_func, layers [i].getNeuronsCount ());
				foreach (ANeuron n in l.neurons) {
					for (int j = 0; j<layers[i].getNeuronsCount()	;j++) {
						ALink lnk = new ALink ();
						lnk.layer = i;
						lnk.neuron = j;
						lnk.weight = rnd.NextDouble () - 0.5;
						n.setLink (j, lnk);
					}
				}
				layers.Add (l);
			}
			ALayer last = new ALayer (outputNeurons, activation_func, hiddenNeurons);
			foreach (ANeuron n in last.neurons) {
				for (int j = 0; j<layers[hiddenLayers].getNeuronsCount();j++) {
					ALink lnk = new ALink ();
					lnk.layer = hiddenLayers;
					lnk.neuron = j;
					lnk.weight = rnd.NextDouble () - 0.5;
					n.setLink (j, lnk);
				}
			}
			layers.Add (last);
			#endregion
		}
		protected internal void setInput(List<double> input){
			for (int i=0; i<layers[0].getNeuronsCount(); i++) {
				layers[0].neurons[i].setOutput(input[i]);
			}				                             
		}
		protected internal List<double> getResults(){
			List<double> result = new List<double> ();
			foreach (ANeuron n in layers[layers.Count-1].neurons) {
				result.Add (n.getOutput ());
			}
			return result;
		}
		protected internal void calcResult(){
			for (int i=1; i<layers.Count; i++) {
				foreach (ANeuron n in layers[i].neurons) {
					List<double> inp = new List<double> ();
					foreach (ALink lnk in n.links) {
						inp.Add (layers [lnk.layer].neurons [lnk.neuron].getOutput ());
					}
					n.calcOutput (inp);
				}
			}
		}
		protected internal double calcStdError(List<double> correctAnwser){
			double sum = 0;
			List<double> res = getResults ();
			for (int i = 0; i< correctAnwser.Count; i ++) {
				sum += Math.Pow(res [i]- correctAnwser[i],2.0);
			}
			//Console.WriteLine ("Err:" + sum * 0.5);
			return sum * 0.5;
		}
		public double study(List<List<double>> inputs, List<List<double>> anwsers, double accectableStdError, int maxIterations){
			double oldError = 0;
			int layer;
			int neuron;
			int link;
			int iter = 0;
			ALink oldLink;
			ALink newLink;
			double oldWeight;
			//double newWeight;
			Random rnd = new Random ();
			double newError = 0;
			double sum;
			#region calc_error
			sum = 0;
			for (int i = 0; i<inputs.Count; i++) {
				setInput (inputs [i]);
				calcResult ();
				sum += calcStdError (anwsers [i]);
			}
			oldError = sum / inputs.Count;
			#endregion

			do {
				#region choose_random_link
				layer = rnd.Next(1,layers.Count);
				neuron = rnd.Next(layers[layer].getNeuronsCount());
				link = rnd.Next(layers[layer].getNeuron(neuron).links.Count);
				//Console.WriteLine(layer.ToString()+"::"+neuron.ToString()+"::"+link.ToString());
				#endregion
				#region create_new_link
				oldLink = layers[layer].getNeuron(neuron).getLink(link);
				newLink = oldLink;

				//newLink.weight = rnd.NextDouble()*2.0-1.0;
				#endregion
				//layers[layer].neurons[neuron].setLink(link,newLink);
				oldWeight = layers[layer].neurons[neuron].links[link].weight;
				layers[layer].neurons[neuron].links[link].weight = rnd.NextDouble()*2.0-1.0;
				//Console.WriteLine("new = "+layers[layer].neurons[neuron].links[link].weight.ToString()+" old= "+oldWeight.ToString());
				//Thread.Sleep(1000);
				#region calc_error
				sum = 0;
				for (int i = 0; i<inputs.Count; i++) {
					setInput (inputs [i]);
					calcResult ();
					sum += calcStdError (anwsers [i]);
				}
				newError = sum / inputs.Count;
				#endregion
				if(oldError<=newError){
					//layers[layer].neurons[neuron].setLink(link,oldLink);
					//Console.WriteLine(oldError.ToString()+" "+newError.ToString());
					layers[layer].neurons[neuron].links[link].weight = oldWeight;
				}else{
					oldError = newError;
					Console.WriteLine("we are the champions, my friend"+ oldError.ToString() + ">>"+newError.ToString());
				}
				iter++;
			} while(oldError>accectableStdError && iter< maxIterations);
            return newError;

		}
	}
}

