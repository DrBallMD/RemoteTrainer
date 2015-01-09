using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AlgoLib;
namespace GeneticLibrary
{
    public class Maximization: IGenetical
    {
        protected internal double mutationRate;

        int start;
        int end;

        protected internal int POPULATION_SIZE;
        protected internal int chromolength = 1; //для цифр от 0 до 31, сделать параметром
        List<int> population;
        List<double> fitnessValues;
        protected internal List<Chromosome> chromosomes;
        Random random;
        public Maximization(string[] parametrs)
        {
            POPULATION_SIZE = Convert.ToInt32(parametrs[0]);
            this.mutationRate = Convert.ToDouble(parametrs[1]);
            this.start = Convert.ToInt32(parametrs[2]);
            this.end = Convert.ToInt32(parametrs[3]);
        }
        public Maximization()
        {
            POPULATION_SIZE = 1000;
            random = new Random();
            population = new List<int>();
            fitnessValues = new List<double>();
            chromosomes = new List<Chromosome>();
            this.mutationRate = 0.04;
            start = -50;
            end = 50;
            this.FormBasicPopulation();       
        }
        public Maximization(int _start, int _end, int populationSize, double mutationrate)
        {
            POPULATION_SIZE = populationSize;
            random = new Random();
            population = new List<int>();
            fitnessValues = new List<double>();
            chromosomes = new List<Chromosome>();
            this.mutationRate = mutationrate;
            start = _start;
            end = _end;
            this.FormBasicPopulation();       
        }
        private double func(double x)
        {
            return 
                //Math.Pow(15 * x * 5 * (1 - x) * (1 - 5) * Math.Sin(9 * Math.PI * x) * Math.Sin(9 * Math.PI * 5), 2);
                Math.Exp(-x) * Math.Sin(x) / x;
        }
        //нормально
        private void FormBasicPopulation()
        {
            int st = Convert.ToString(Math.Abs(start), 2).Length;
            int en = Convert.ToString(Math.Abs(end), 2).Length;
            if (st > en) chromolength += st; else chromolength += en;
            while (population.Count < POPULATION_SIZE)
            {
                population.Add(random.Next(start, end));
            }
            CreateChromosomes();
            ConvertBitToInt();
        }

        //нормально
        private void CreateChromosomes()
        {
            this.chromosomes.Clear();//переписать лист на массив
            
            foreach (int vls in population)
            {
                
                chromosomes.Add(new Chromosome(IntToBit(vls).ToCharArray()));
            }
        }
        

        //более менее
        private void Selection() {
            List<Chromosome> offsprings = new List<Chromosome>();
            while (offsprings.Count<this.POPULATION_SIZE){
                int index1 = random.Next(0,POPULATION_SIZE-1);
                int index2 = random.Next(0, POPULATION_SIZE-1);

                int fr1 = (int)func(population[index1]);
                int fr2 = (int)func(population[index2]);

                offsprings.Add(fr1 < fr2 ? new Chromosome(this.chromosomes[index1]) : new Chromosome(this.chromosomes[index2]));
            }
            chromosomes = offsprings;
            ConvertBitToInt();
        }
        //нормально
        private void Crossover()
        {
            List<Chromosome> newPopulation = new List<Chromosome>();
                while (newPopulation.Count<POPULATION_SIZE)
                {
                    int index1 = random.Next(0, POPULATION_SIZE-1);
                    int index2 = random.Next(0, POPULATION_SIZE-1);
                    Chromosome chromo1 = new Chromosome(chromosomes[index1]);
                    Chromosome chromo2 = new Chromosome(chromosomes[index2]);
                    if (chromo1.Equals(chromo2))
                    {
                        Mutate(chromo2);
                    }                    
                    
                    Crossing(chromo1, chromo2);
                    newPopulation.Add(chromo1);
                    newPopulation.Add(chromo2);
                }
                chromosomes = newPopulation;
                ConvertBitToInt();
        }

        //правильно
        private void Crossing(Chromosome chromo1, Chromosome chromo2)
        {
            int position = random.Next(1, chromo1.Length-2);
            for (int i = position; i < chromo1.Length; i++)
            {
                char tmp = chromo2[i];
                chromo2[i] = chromo1[i];
                chromo1[i] = tmp;
            }
            
        }
        //правильно
        private void Mutation()
        {
            foreach (Chromosome chromo in chromosomes)
            {
                if (mutationRate > random.NextDouble())
                {
                    Mutate(chromo);
                }
            }
            ConvertBitToInt();
        }
        //правильно
        private void Mutate(Chromosome chromo)
        {
            int position = random.Next(0, chromo.Length - 1);
            char bit = chromo[position];
            switch (bit)
            {
                case '0':
                    bit = '1';
                    break;
                case '1':
                    bit = '0';
                    break;
            }
            chromo[position] = bit;
        }
        public void NextGeneration()
        {
            Selection();
            Crossover();
            Mutation();
        }
        public string GetBest()
        {
            ConvertBitToInt();
            double max = fitnessValues.Max();
            int id = fitnessValues.IndexOf(max);
            return population[id] + "\t" + max;
                //population[id];
        }
        public bool IsBestGeneration()
        {
            if (population.Max() == 31)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void ConvertBitToInt()
        {
            population.Clear();
            fitnessValues.Clear();
            foreach (Chromosome chromo in chromosomes)
            {
                char symb = chromo[0];
                char[] str = new char[chromo.GetBits().Length-1];

                Array.Copy(chromo.GetBits(), 1, str, 0, str.Length); 
                int value = Convert.ToInt32(new string(str), 2);
                if (symb.Equals('1')) value *= -1;
                population.Add(value);
                fitnessValues.Add(func(value));
            }
        }
        public string IntToBit(int value)
        {
            int module = Math.Abs(value);
                string cur = Convert.ToString(module, 2);
                while (cur.Length < chromolength-1)
                {
                    cur = "0" + cur;
                }
                if (value < 0) cur = "1" + cur; else cur = "0" + cur;
                return cur;
        }
    }
}
