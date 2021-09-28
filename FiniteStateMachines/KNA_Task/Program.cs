using Formal_Languages_Task_1_KDA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KNA_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.White;

            KNA_Start();
        }

        static void KNA_CreateJson()
        {
            List<Transition_KNA> transitions = new List<Transition_KNA>()
            {
                // ===================
                new Transition_KNA(){
                    From = new State()
                    {
                        Title = "q0"
                    },

                    To = new State()
                    {
                        Title = "q0"
                    },

                    Condition = new List<int>()
                    {
                        0, 1
                    },

                    Value = 0
                },

                // ===================
                new Transition_KNA(){
                    From = new State()
                    {
                        Title = "q0"
                    },

                    To = new State()
                    {
                        Title = "q1"
                    },

                    Condition = new List<int>()
                    {
                        0
                    },

                    Value = 0
                },

                // ===================
                new Transition_KNA(){
                    From = new State()
                    {
                        Title = "q1"
                    },

                    To = new State()
                    {
                        Title = "q2"
                    },

                    Condition = new List<int>()
                    {
                        0
                    },

                    Value = 0
                },
            };

            var tmp = JsonConvert.SerializeObject(transitions);

            using (StreamWriter writer = new StreamWriter(@"../../jsonTrans_KNA.json"))
            {
                writer.WriteLine(tmp);
            }
        }

        static void KNA_Start()
        {
            string jsonReaded;
            using (StreamReader reader = new StreamReader(@"../../jsonTrans_KNA.json"))
            {
                jsonReaded = reader.ReadToEnd();
            }

            List<Transition_KNA> transitions = JsonConvert.DeserializeObject<List<Transition_KNA>>(jsonReaded);

            var states = transitions.Select(x => x.From).Distinct();

            var startState = new State()
            {
                Title = "q0"
            };

            var finishState = new State()
            {
                Title = "q2"
            };
            var finishState2 = new State()
            {
                Title = "q3"
            };

            var finishStates = new[] { finishState, finishState2 };

            var alphabet = new[] { 1, 0 };

            var input = Console.ReadLine().Select(x => int.Parse($"{x}")).ToList();

            KNA start_kna = new KNA(startState, alphabet, finishStates, transitions, 0);

            KNA_Manager.Add(start_kna);

            KNA_Manager.KNAs[0].Start(input);

            KNA_Manager.StartAll(input);

            Console.ReadKey();
        }
    }
}
