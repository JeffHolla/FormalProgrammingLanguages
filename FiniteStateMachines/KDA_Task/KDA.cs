using System;
using System.Collections.Generic;
using System.Linq;

namespace Formal_Languages_Task_1_KDA
{
    // Список состояний, алфавит, стартовое состояние,
    // список заключительных состояний(список конечных точек, типа в которых конец),
    // таблица переходов(входная последов)
    public class KDA
    {
        public IEnumerable<int> Alphabet { get; set; }
        public IEnumerable<Transition> Transitions { get; set; }

        public State StartState;
        public IEnumerable<State> FinalStates;

        private State _сurrentState;

        public KDA(State startState,
            IEnumerable<int> alphabet,
            IEnumerable<State> finalStates, IEnumerable<Transition> transitions)
        {
            Alphabet = alphabet;
            //InputSeq = inputSeq;
            Transitions = transitions;

            FinalStates = finalStates;
            StartState = startState;

            _сurrentState = startState;
        }

        public void Start(IEnumerable<int> inputSeq)
        {
            if (!IsAlphabetContainInputSeqElem(inputSeq))
            {
                Console.WriteLine("!!!!!!!!!!!!!!");
                Console.WriteLine("Входная последовательность не удовлетворяет данному алфавиту!");
                Console.WriteLine("!!!!!!!!!!!!!!");

                return;
            }

            int counter = 0;
            var inputSeqElementsCount = inputSeq.ToArray().Length;
            // По входной последовательности
            foreach (var value in inputSeq)
            {
                // Проходим по каждому переходу
                foreach (var transition in Transitions)
                {
                    if (transition.Condition == value
                        && transition.From == _сurrentState)
                    {
                        Console.WriteLine($"Old state [{transition.From}]" +
                            $" -> New state [{transition.To}] -> Value is [{transition.Value}]");
                        _сurrentState = transition.To;

                        ++counter;

                        break;
                    }
                }
            }

            Console.WriteLine($"Финальное состояние - {_сurrentState}");

            if (FinalStates.Contains(_сurrentState) && counter == inputSeqElementsCount)
            {
                Console.WriteLine("================");
                Console.WriteLine("Данная входная последовательность принадлежит языку!");
                Console.WriteLine("================");
            }
            else
            {
                Console.WriteLine("================");
                Console.WriteLine("Данная входная последовательность не принадлежит языку!");
                Console.WriteLine("================");
            }
        }

        private bool IsAlphabetContainInputSeqElem(IEnumerable<int> inputSeq)
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
    }
}
