using System;
using System.Xml;
using System.Xml.Serialization;
namespace ANeuralNetwork
{
	public class ALink
	{
		[XmlAttribute]
		public int layer;
		[XmlAttribute]
		public int neuron;
		[XmlAttribute]
		public double weight;
		public ALink(){
			this.layer = 0;
			this.neuron = 0;
			this.weight = 1.0;
		}
	}
}

