using System;
using System.Collections.Generic;
using System.Linq;

namespace Formal_Languages_Task_1_KDA
{
    public class KNA_With_E : KNA
    {
        public KNA_With_E(State startState,
            IEnumerable<int> alphabet,
            IEnumerable<State> finalStates,
            IEnumerable<Transition_KNA> transitions, int counter) :
            base(startState, alphabet, finalStates, transitions, counter)
        { }

        public new void Start(List<int> inputSeq)
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

                    if (transition.Condition.Contains(-1))
                    {
                        // Чтобы откатить проход по заданной последовательности
                        // т.к. в for i++ стоит
                        --i;
                    }
                    else
                    {
                        ++Counter;
                    }

                    _сurrentState = transition.To;

                    break;
                }
                else if (nearbyPathsCount > 1)
                {
                    var transAround = Transitions
                        .Where(tr => tr.From == _сurrentState)
                        .Where(cond => cond.Condition.Contains(inputSeq[i])
                            || cond.Condition.Contains(-1))
                        .ToList();

                    Console.WriteLine("------------");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("FOUND MULTIPATH!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Symbol is [{inputSeq[i]}], id is [{i + 1}]");
                    Console.WriteLine();
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
                        KNA kna;
                        // Эпсилон переход
                        if (transAround[j].Condition.Contains(-1))
                        {
                            kna = new KNA_With_E(transAround[j].To, Alphabet, FinalStates, Transitions, Counter);
                        }
                        else
                        {
                            kna = new KNA_With_E(transAround[j].To, Alphabet, FinalStates, Transitions, Counter + 1);
                        }
                        KNA_Manager.Add(kna);
                    }

                    if (transAround[0].Condition.Contains(-1))
                    {
                        // Чтобы откатить проход по заданной последовательности
                        // т.к. в for i++ стоит
                        --i;
                    }
                    else
                    {
                        ++Counter;
                    }
                    _сurrentState = transAround[0].To;
                }
            }


            int countAllStates = Transitions.Select(x => x).Distinct().Count();
            int counterToEnd = 0;
            // Прыжки без надежды и денег по испилон переходам
            if (Counter == 0)
            {
                while (IsHasNearbyPathsWith_E(_сurrentState))
                {
                    if (counterToEnd > countAllStates)
                    {
                        break;
                    }

                    var transAround = Transitions
                            .Where(tr => tr.From == _сurrentState)
                            .Where(cond => cond.Condition.Contains(-1))
                            .ToList();

                    Console.WriteLine("------------");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("FOUND MULTIPATH!");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Symbol is [NULL], id is [{Counter}]");
                    Console.WriteLine();
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
                        KNA kna;
                        // Эпсилон переход
                        if (transAround[j].Condition.Contains(-1))
                        {
                            kna = new KNA_With_E(transAround[j].To, Alphabet,
                                FinalStates, Transitions, Counter);
                        }
                        else
                        {
                            kna = new KNA_With_E(transAround[j].To, Alphabet, FinalStates,
                                Transitions, Counter + 1);
                        }

                        KNA_Manager.Add(kna);
                    }

                    _сurrentState = transAround[0].To;

                    ++counterToEnd;
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

        public bool IsHasNearbyPathsWith_E(State state)
        {
            var transAround = Transitions
                        .Where(tr => tr.From == state)
                        .Where(cond => cond.Condition.Contains(-1))
                        .ToList();

            if (transAround.Count != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
