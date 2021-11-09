using Formal_Languages_Task_1_KDA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lexer_Task_1
{
    public class KDA
    {
        public IEnumerable<Transition> Transitions { get; set; }

        public State StartState;

        private State _сurrentState;

        public KDA(State startState, IEnumerable<Transition> transitions)
        {
            Transitions = transitions;

            StartState = startState;

            _сurrentState = startState;
        }

        public void Start(IEnumerable<char> inputSeq, bool showStates = false)
        {
            int position = 1;
            int lengthCounter = 0;
            var inputSeqElementsCount = inputSeq.Count();
            List<char> chars = new List<char>();
            // По входной последовательности
            foreach (var value in inputSeq)
            {
                // Проходим по каждому переходу
                foreach (var transition in Transitions)
                {
                    if (transition.From == _сurrentState)
                    {
                        if (new Regex(transition.ConditionRegexPattern).IsMatch(value.ToString()))
                        {
                            if (showStates)
                            {
                                Console.WriteLine($"Old state [{transition.From}]" +
                                    $" -> New state [{transition.To}] -> Cur char is [{value}]");
                            }

                            _сurrentState = transition.To;

                            chars.Add(value);

                            ++lengthCounter;

                            break;
                        }
                    }
                }

                if (value == ' ' || lengthCounter == inputSeqElementsCount)
                {
                    //Console.WriteLine();

                    string lexem = string.Join("", chars).Trim();

                    Console.ForegroundColor = ConsoleColor.Green;

                    Console.WriteLine($"Lexem is [{lexem}]");
                    if (Program.Lexems.ArithmeticOperation.Contains(lexem))
                    {
                        Console.WriteLine($"Lexem type is [{Program.LexemEnum.ArithmeticOperation}]");
                    }
                    else if (Program.Lexems.Assignment.Contains(lexem))
                    {
                        Console.WriteLine($"Lexem type is [{Program.LexemEnum.Assignment}]");
                    }
                    else if (new Regex(Program.Lexems.ConstantRegexPattern).IsMatch(lexem))
                    {
                        Console.WriteLine($"Lexem type is [{Program.LexemEnum.Constant}]");
                    }
                    else if (Program.Lexems.KeyWord.Contains(lexem))
                    {
                        Console.WriteLine($"Lexem type is [{Program.LexemEnum.KeyWord}]");
                    }
                    else if (Program.Lexems.LogicOperation.Contains(lexem))
                    {
                        Console.WriteLine($"Lexem type is [{Program.LexemEnum.LogicOperation}]");
                    }
                    else if (new Regex(Program.Lexems.IdentifierRegexPattern).IsMatch(lexem))
                    {
                        Console.WriteLine($"Lexem type is [{Program.LexemEnum.Identifier}]");
                    }
                    else
                    {
                        Console.WriteLine($"Lexem type is [{Program.LexemEnum.Unknown}]");
                    }

                    Console.WriteLine($"Index - {position}");
                    position++;

                    Console.ForegroundColor = ConsoleColor.White;

                    Console.WriteLine();

                    chars.Clear();
                }
            }
        }
    }
}
