using Formal_Languages_Task_1_KDA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KNA_With_E_Task
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.White;

            KNA_With_E_Start();
        }

        static void KNA_With_E_Start()
        {
            string jsonReaded;
            using (StreamReader reader = new StreamReader(@"../../jsonTrans_KNA_WithE.json"))
            {
                jsonReaded = reader.ReadToEnd();
            }

            List<Transition_KNA> transitions = JsonConvert.DeserializeObject<List<Transition_KNA>>(jsonReaded);

            var states = transitions.Select(x => x.From).Distinct();

            var startState = new State()
            {
                Title = "q0"
            };

            var finishState1 = new State()
            {
                Title = "q1"
            };
            var finishState2 = new State()
            {
                Title = "q2"
            };
            var finishState3 = new State()
            {
                Title = "q3"
            };
            var finishState6 = new State()
            {
                Title = "q6"
            };

            var finishStates = new[] { /*finishState1,*/ finishState2,
                finishState3, finishState6 };

            var alphabet = new[] { 1, 0 };

            var input = Console.ReadLine().Select(x => int.Parse($"{x}")).ToList();

            KNA_With_E start_kna = new KNA_With_E(startState, alphabet, finishStates, transitions, 0);

            KNA_Manager.Add(start_kna);

            //KNA_Manager.KNAs[0].Start(input);
            var KNA_First = KNA_Manager.KNAs[0];
            ((KNA_With_E)KNA_First).Start(input);

            KNA_Manager.StartAll_KNAs_With_E(input);

            Console.ReadKey();
        }
    }
}
