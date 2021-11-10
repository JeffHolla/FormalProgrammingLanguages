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

        public void Start(IEnumerable<char> inputSeq, bool showStates = false, bool showLexems = false)
        {
            Console.WriteLine($"Input seq is [{string.Join("", inputSeq)}]");

            List<Lexem> lexems = new List<Lexem>();

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
                    string lexem = string.Join("", chars).Trim();
                    LexemEnum lexemType = GetLexemType(lexem);

                    if (showLexems)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;

                        Console.WriteLine($"Lexem is [{lexem}]");
                        Console.WriteLine($"Lexem type is [{lexemType}]");
                        Console.WriteLine($"Index - {position}");
                        position++;

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine();
                    }

                    lexems.Add(new Lexem(lexem, lexemType));

                    chars.Clear();
                }
            }

            IsForKeyWord(lexems);

            Console.WriteLine();
        }

        private bool IsForKeyWord(List<Lexem> lexems)
        {
            int i = 0;
            int skippedPositions = 0;
            if (lexems[i].Value != "for")
            {
                Console.WriteLine($"Ошибка! Ожидалось ключевое слово 'for' на позиции [{i}]");
                return false;
            }

            if (lexems[i].Value == "for")
            {
                skippedPositions = 1; // For [for]
                lexems = lexems.Skip(1).ToList();
                if (!IsAssignment(ref lexems, ref skippedPositions)) // Is Assigment - 2 symbols -> [Identifier] [=]
                {
                    return false;
                }

                if (!IsArithmeticOperation(ref lexems, ref skippedPositions))
                {
                    return false;
                }
                else
                {
                    if (IsOperator(ref lexems, ref skippedPositions))
                    {
                        Console.WriteLine("Синтаксический анализ был пройден успешно!");
                    }
                }
            }

            return true;
        }

        // <арифметическое выражение> -> <операнд>|<арифметическое выражение><арифметическая операция><операнд>
        // <операнд> -> <идентификатор>|<константа>
        // <арифметическая операция> -> +|-|/|*
        private bool IsArithmeticOperation(ref List<Lexem> lexems, ref int skippedPositions, bool isFirstCheck = true)
        {
            int i = 0;

            if (lexems[i].Type == LexemEnum.KeyWord && lexems[i].Value == "to")
            {
                // i - чтобы не пропустить 'to'
                skippedPositions += 1;
                lexems = lexems.Skip(i + 1).ToList();
                return IsToKeyWord_ArithmeticOperation(ref lexems, ref skippedPositions);
            }

            if (lexems[i].Type != LexemEnum.Constant && lexems[i].Type != LexemEnum.Identifier)
            {
                Console.WriteLine($"Ошибка! Ожидался Операнд на позиции [{i + skippedPositions}]");
                return false;
            }
            else if (lexems[i + 1].Type != LexemEnum.ArithmeticOperation)
            {
                Console.WriteLine($"Ошибка! Ожидался знак арифметической операции на позиции [{i + 1 + skippedPositions}]");
                return false;
            }
            else if (lexems[i + 2].Type != LexemEnum.Constant && lexems[i + 2].Type != LexemEnum.Identifier)
            {
                Console.WriteLine($"Ошибка! Ожидался Операнд на позиции [{i + 2 + skippedPositions}]");
                return false;
            }

            if (lexems[i + 3].Type == LexemEnum.ArithmeticOperation)
            {
                skippedPositions += 2;
                lexems = lexems.Skip(2).ToList();
            }
            else
            {
                skippedPositions += 3;
                lexems = lexems.Skip(3).ToList();
            }
            return IsArithmeticOperation(ref lexems, ref skippedPositions);
        }

        // <арифметическое выражение> -> <операнд>|<арифметическое выражение><арифметическая операция><операнд>
        // <операнд> -> <идентификатор>|<константа>
        // <арифметическая операция> -> +|-|/|*
        private bool IsToKeyWord_ArithmeticOperation(ref List<Lexem> lexems, ref int skippedPositions)
        {
            int i = 0;

            if (lexems[i].Type == LexemEnum.Identifier && lexems[i + 1].Type != LexemEnum.ArithmeticOperation)
            {
                return true;
            }
            else if (lexems[i].Type != LexemEnum.Constant && lexems[i].Type != LexemEnum.Identifier)
            {
                Console.WriteLine($"Ошибка! Ожидался Операнд на позиции [{i + skippedPositions}]");
                return false;
            }
            else if (lexems[i + 1].Type != LexemEnum.ArithmeticOperation)
            {
                Console.WriteLine($"Ошибка! Ожидался знак арифметической операции на позиции [{i + 1 + skippedPositions}]");
                return false;
            }
            else if (lexems[i + 2].Type != LexemEnum.Constant && lexems[i + 2].Type != LexemEnum.Identifier)
            {
                Console.WriteLine($"Ошибка! Ожидался Операнд на позиции [{i + 2 + skippedPositions}]");
                return false;
            }

            if (lexems[i + 3].Type == LexemEnum.ArithmeticOperation)
            {
                skippedPositions += 2;
                lexems = lexems.Skip(2).ToList();
            }
            else
            {
                skippedPositions += 3;
                lexems = lexems.Skip(3).ToList();
            }
            return IsToKeyWord_ArithmeticOperation(ref lexems, ref skippedPositions);
        }

        // [идентификатор] [=]
        private bool IsAssignment(ref List<Lexem> lexems, ref int skippedPositions)
        {
            int i = 0;

            if (lexems[i].Type != LexemEnum.Identifier)
            {
                Console.WriteLine($"Ошибка! Ожидался Операнд на позиции [{i + skippedPositions}]");
                return false;
            }

            if (lexems[i + 1].Type != LexemEnum.Assignment)
            {
                Console.WriteLine($"Ошибка! Ожидался знак '=' на позиции [{i + 1 + skippedPositions}]");
                return false;
            }

            skippedPositions += 2;
            lexems = lexems.Skip(2).ToList();

            return true;
        }

        // <арифметическое выражение> -> <операнд>|<арифметическое выражение><арифметическая операция><операнд>
        // <операнд> -> <идентификатор>|<константа>
        // <арифметическая операция> -> +|-|/|*
        private bool IsOperator_ArithmeticOperation(ref List<Lexem> lexems, ref int skippedPositions)
        {
            int i = 0;

            if (lexems.Count == 0)
            {
                Console.WriteLine($"Ошибка! Ожидалось ключевое слово [next] на позиции [{skippedPositions}]");
                return false;
            }

            if (lexems.Count == 1 && (lexems[i].Type != LexemEnum.KeyWord || lexems[i].Value != "next"))
            {
                Console.WriteLine($"Ошибка! Ожидалось ключевое слово [next] на позиции [{skippedPositions}]");
                return false;
            }

            if (lexems[i].Type == LexemEnum.KeyWord && lexems[i].Value == "next")
            {
                return true;
            }

            if (lexems[i].Type != LexemEnum.Constant && lexems[i].Type != LexemEnum.Identifier)
            {
                Console.WriteLine($"Ошибка! Ожидался Операнд на позиции [{i + skippedPositions}]");
                return false;
            }
            else if (lexems[i + 1].Type == LexemEnum.Assignment)
            {
                if (!IsAssignment(ref lexems, ref skippedPositions))
                {
                    return false;
                }
                else
                {
                    return IsOperator_ArithmeticOperation(ref lexems, ref skippedPositions);
                }
            }
            else if (lexems[i + 1].Type == LexemEnum.Identifier)
            {
                skippedPositions += 1;
                lexems = lexems.Skip(1).ToList();
                return true;
            }
            else if (lexems[i + 1].Type == LexemEnum.KeyWord && lexems[i + 1].Value == "next")
            {
                skippedPositions += 1;
                lexems = lexems.Skip(1).ToList();
                return true;
            }
            else if (lexems[i + 1].Type != LexemEnum.ArithmeticOperation)
            {
                Console.WriteLine($"Ошибка! Ожидался знак арифметической операции на позиции [{i + 1 + skippedPositions}]");
                return false;
            }
            else if (lexems[i + 2].Type != LexemEnum.Constant && lexems[i + 2].Type != LexemEnum.Identifier)
            {
                Console.WriteLine($"Ошибка! Ожидался Операнд на позиции [{i + 2 + skippedPositions}]");
                return false;
            }

            if (i + 3 < lexems.Count)
            {
                if (lexems[i + 3].Type == LexemEnum.ArithmeticOperation)
                {
                    skippedPositions += 2;
                    lexems = lexems.Skip(2).ToList();
                }
                else
                {
                    skippedPositions += 3;
                    lexems = lexems.Skip(3).ToList();
                }
            }
            else
            {
                skippedPositions += 3;
                lexems = lexems.Skip(3).ToList();
            }
            return IsOperator_ArithmeticOperation(ref lexems, ref skippedPositions);

        }

        //<операторы> -> <идентификатор> = <арифметическое выражение>
        private bool IsOperator(ref List<Lexem> lexems, ref int skippedPositions)
        {
            int i = 0;

            if (!IsOperator_ArithmeticOperation(ref lexems, ref skippedPositions))
            {
                return false;
            }
            else
            {
                if (lexems.Count == 1 && lexems[i].Type == LexemEnum.KeyWord && lexems[i].Value == "next")
                {
                    return true;
                }
                else
                {
                    if (!IsOperator(ref lexems, ref skippedPositions))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private LexemEnum GetLexemType(string lexem)
        {
            if (Lexems.ArithmeticOperation.Contains(lexem))
            {
                return LexemEnum.ArithmeticOperation;
            }
            else if (Lexems.Assignment.Contains(lexem))
            {
                return LexemEnum.Assignment;
            }
            else if (new Regex(Lexems.ConstantRegexPattern).IsMatch(lexem))
            {
                return LexemEnum.Constant;
            }
            else if (Lexems.KeyWord.Contains(lexem))
            {
                return LexemEnum.KeyWord;
            }
            else if (Lexems.LogicOperation.Contains(lexem))
            {
                return LexemEnum.LogicOperation;
            }
            else if (new Regex(Lexems.IdentifierRegexPattern).IsMatch(lexem))
            {
                return LexemEnum.Identifier;
            }
            else
            {
                return LexemEnum.Unknown;
            }
        }
    }


    public class Lexem
    {
        public string Value { get; set; }
        public LexemEnum Type { get; set; }
        public Lexem(string value, LexemEnum type)
        {
            Value = value;
            Type = type;
        }

        public override string ToString()
        {
            return $"[{Value}] [{Type}]";
        }
    }
}