using Formal_Languages_Task_1_KDA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KDA_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.White;

            KDA_Start();
        }

        static void KDA_Start()
        {
            string jsonReaded;
            using (StreamReader reader = new StreamReader(@"../../jsonTrans_KDA.json"))
            {
                jsonReaded = reader.ReadToEnd();
            }

            List<Transition> transitions = JsonConvert.DeserializeObject<List<Transition>>(jsonReaded);

            var states = transitions.Select(x => x.From).Distinct();

            var startState = new State()
            {
                Title = "State1"
            };

            var finishState = new State()
            {
                Title = "State3"
            };

            var finishStates = new[] { finishState };

            var alphabet = new[] { 1, 0 };

            var input = Console.ReadLine().Select(x => int.Parse($"{x}"));

            KDA kda = new KDA(startState, alphabet, finishStates, transitions);

            //11010
            kda.Start(input);

            Console.ReadKey();
        }


        static void KDA_CreateJson()
        {
            Transition transition1 = new Transition()
            {
                From = new State()
                {
                    Title = "State1"
                },

                To = new State()
                {
                    Title = "State2"
                },

                Condition = 1,

                Value = 1
            };

            Transition transition2 = new Transition()
            {
                From = new State()
                {
                    Title = "State2"
                },

                To = new State()
                {
                    Title = "State1"
                },

                Condition = 1,

                Value = 0
            };

            Transition transition3 = new Transition()
            {
                From = new State()
                {
                    Title = "State1"
                },

                To = new State()
                {
                    Title = "State1"
                },

                Condition = 0,

                Value = 1
            };

            Transition transition4 = new Transition()
            {
                From = new State()
                {
                    Title = "State2"
                },

                To = new State()
                {
                    Title = "State3"
                },

                Condition = 0,

                Value = 1
            };

            List<Transition> transitions = new List<Transition>()
            {
                transition1, transition2, transition3, transition4
            };

            var tmp = JsonConvert.SerializeObject(transitions);

            using (StreamWriter writer = new StreamWriter(@"../../jsonTrans.json"))
            {
                writer.WriteLine(tmp);
            }
        }

    }
}
