using System;
using System.Collections.Generic;
using System.Linq;

namespace Formal_Languages_Task_1_KDA
{
    // Список состояний, алфавит, стартовое состояние,
    // список заключительных состояний(список конечных точек, типа в которых конец),
    // таблица переходов(входная последов)
    public class KNA
    {
        public IEnumerable<int> Alphabet { get; set; }
        public IEnumerable<Transition_KNA> Transitions { get; set; }

        public State StartState;
        public IEnumerable<State> FinalStates;

        public State _сurrentState;

        public int Counter { get; set; }

        public KNA(State startState,
            IEnumerable<int> alphabet,
            IEnumerable<State> finalStates, IEnumerable<Transition_KNA> transitions, int counter)
        {
            Alphabet = alphabet;
            Transitions = transitions;

            FinalStates = finalStates;
            StartState = startState;

            _сurrentState = startState;

            Counter = counter;
        }

        public void Start(List<int> inputSeq)
        {
            if (!IsAlphabetContainInputSeqElem(inputSeq))
            {
                Console.WriteLine("!!!!!!!!!!!!!!");
                Console.WriteLine("Входная последовательность не удовлетворяет данному алфавиту!");
                Console.WriteLine("!!!!!!!!!!!!!!");

                return;
            }
            if (Counter > inputSeq.Count)
            {
                Console.WriteLine("!!!!!!!!!!!!!!");
                Console.WriteLine("Входная последовательность не удовлетворяет данному алфавиту!");
                Console.WriteLine("!!!!!!!!!!!!!!");

                return;
            }


            ShowNearbyPaths(_сurrentState);

            var inputSeqElementsCount = inputSeq.ToArray().Length;
            // По входной последовательности
            for (int i = Counter; i < inputSeq.Count; i++)
            {
                int nearbyPathsCount = Transitions.Count(x => x.From == _сurrentState);

                if (nearbyPathsCount == 1)
                {
                    var transition = Transitions.First(x => x.From == _сurrentState);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Old state [{_сurrentState}]" +
                        $" -> New state [{transition.To}] -> Value is [{transition.Value}]");
                    Console.ForegroundColor = ConsoleColor.White;

                    _сurrentState = transition.To;

                    ++Counter;

                    break;
                }
                else if (nearbyPathsCount > 1)
                {
                    var transAround = Transitions
                        .Where(tr => tr.From == _сurrentState)
                        .Where(cond => cond.Condition.Contains(inputSeq[i]))
                        .ToList();

                    Console.WriteLine("------------");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("FOUND MULTIPATH!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Symbol is [{inputSeq[i]}], id is [{i + 1}]");
                    Console.WriteLine($"");
                    Console.Write($"From [{_сurrentState}] -> ");
                    foreach (var path in transAround)
                    {
                        Console.Write($" / [{path.To}]");
                    }

                    Console.WriteLine();
                    Console.WriteLine("Choosen first: ");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Old state [{transAround[0].From}]" +
                        $" -> New state [{transAround[0].To}]" +
                        $" -> Value is [{transAround[0].Value}]");
                    Console.ForegroundColor = ConsoleColor.White;


                    for (int j = 1; j < transAround.Count; j++)
                    {
                        KNA kna = new KNA(transAround[j].To, Alphabet, FinalStates, Transitions, Counter + 1);
                        KNA_Manager.Add(kna);
                    }

                    ++Counter;
                    _сurrentState = transAround[0].To;
                }
            }


            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Финальное состояние - {_сurrentState}");
            Console.ForegroundColor = ConsoleColor.White;

            if (FinalStates.Contains(_сurrentState) && Counter == inputSeqElementsCount)
            {
                Console.WriteLine("================");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Данная входная последовательность принадлежит языку!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("================");
            }
            else
            {
                Console.WriteLine("================");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Данная входная последовательность не принадлежит языку!");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("================");
            }
        }

        protected bool IsAlphabetContainInputSeqElem(IEnumerable<int> inputSeq)
        {
            var uniqInputSeqElems = inputSeq.Distinct();
            var uniqAlphabetElems = Alphabet.Distinct();

            foreach (var elem in uniqInputSeqElems)
            {
                if (!uniqAlphabetElems.Contains(elem))
                {
                    return false;
                }
            }

            return true;
        }

        protected void ShowNearbyPaths(State state)
        {
            Console.WriteLine("Nearby Paths :");
            foreach (var transition in Transitions.Where(x => x.From == state))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"Old state [{transition.From}]" +
                    $" -> New state [{transition.To}]" +
                    $" -> Value is [{transition.Value}]");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
