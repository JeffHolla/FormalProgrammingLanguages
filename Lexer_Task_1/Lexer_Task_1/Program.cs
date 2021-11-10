﻿using Formal_Languages_Task_1_KDA;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lexer_Task_1
{
    // Вариант 13
    // Конструкция 5 - оператор цикла со счётчиком
    // for <идентификатор> = <арифметическое выражение> to
    //                      <арифметическое выражение>
    //      <операторы>
    // next

    // Уровень 2

    // <лог. выражение> -> <выражение сравнения>|<логическое выражение><логическая операция><выражение сравнения>
    // <выражение сравнения> -> <операнд>|<операнд><операция сравнения><операнд>
    // <операция сравнения> -> <|>|=|<>
    // <логическая операция> -> and|or
    // <операнд> -> <идентификатор>|<константа>
    // <операторы> -> <идентификатор> = <арифметическое выражение>
    // <арифметическое выражение> -> <операнд>|<арифметическое выражение><арифметическая операция><операнд>
    // <арифметическая операция> -> +|-|/|*
    // Чувствительность к регистру - Да
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            Console.ForegroundColor = ConsoleColor.White;

            KDA_CreateJson();
            KDA_Start();
        }
        static void KDA_Start()
        {
            string jsonReaded;
            using (StreamReader reader = new StreamReader(@"../../DigitsAndSymbolsWithSpaces.json"))
            {
                jsonReaded = reader.ReadToEnd();
            }

            List<Transition> transitions = JsonConvert.DeserializeObject<List<Transition>>(jsonReaded);

            var states = transitions.Select(x => x.From).Distinct();

            var startState = new State()
            {
                Title = States.Symbol
            };

            // for <идентификатор> = <арифметическое выражение> to <арифметическое выражение> <операторы>
            var input = "for iter = 5 * 2 to 5 + 20 it = 50 - 72 next"; 
            var input_2 = "iter = 5 * 2 to 5 + 20 it = 50 - 72 next"; 
            var input_3 = "for = 5 * 2 to 5 + 20 it = 50 - 72 next"; 
            var input_4 = "for iter 5 * 2 to 5 + 20 it = 50 - 72 next"; 
            var input_5 = "for iter = * 2 to 5 + 20 it = 50 - 72 next"; 
            var input_6 = "for iter = 5 2 to 5 + 20 it = 50 - 72 next"; 
            var input_7 = "for iter = 5 * to 5 + 20 it = 50 - 72 next"; 
            var input_9 = "for iter = 5 * 2 to + 20 it = 50 - 72 next"; 
            var input_10 = "for iter = 5 * 2 to 5 20 it = 50 - 72 next"; 
            var input_11 = "for iter = 5 * 2 to 5 + it = 50 - 72 next"; 
            var input_12 = "for iter = 5 * 2 to 5 + 20 = 50 - 72 next"; 
            var input_13 = "for iter = 5 * 2 to 5 + 20 it 50 - 72 next"; 
            var input_14 = "for iter = 5 * 2 to 5 + 20 it = - 72 next"; 
            var input_15 = "for iter = 5 * 2 to 5 + 20 it = 50 72 next"; 
            var input_16 = "for iter = 5 * 2 to 5 + 20 it = 50 - next"; 
            var input_17 = "for iter = 5 * 2 to 5 + 20 it = 50 - 72"; 

            var input_extented_arithm_1 = "for iter = 5 * 2 + 205 - 2 / 5 to 5 + 20 it = 50 - 72 next"; 
            var input_extented_arithm_2 = "for iter = 5 * 2 to 5 + 20 - 15 / 2 it = 50 - 72 next"; 
            var input_extented_arithm_3 = "for iter = 5 * 2 to 5 + 20 it = 50 - 72 + 5 * 2 / 2 next"; 

            var input_extented_operator = "for iter = 5 * 2 to 5 + 20 - 15 / 2 it = 50 - 72 ite = 15 another = 125 next"; 
            //Console.WriteLine($"Input seq is [{input}]");
            Console.WriteLine();

            KDA kda = new KDA(startState, transitions);

            kda.Start(input, showStates: false);
            kda.Start(input_2, showStates: false);
            kda.Start(input_3, showStates: false);
            kda.Start(input_4, showStates: false);
            kda.Start(input_5, showStates: false);
            kda.Start(input_6, showStates: false);
            kda.Start(input_7, showStates: false);
            kda.Start(input_9, showStates: false);
            kda.Start(input_10, showStates: false);
            kda.Start(input_11, showStates: false);
            kda.Start(input_12, showStates: false);
            kda.Start(input_13, showStates: false);
            kda.Start(input_14, showStates: false);
            kda.Start(input_15, showStates: false);
            kda.Start(input_16, showStates: false);
            kda.Start(input_17, showStates: false);


            kda.Start(input_extented_arithm_1, showStates: false);
            kda.Start(input_extented_arithm_2, showStates: false);
            kda.Start(input_extented_arithm_3, showStates: false);
            kda.Start(input_extented_operator, showStates: false);

            Console.ReadKey();
        }

        static void KDA_CreateJson()
        {
            var transitions = new List<Transition>()
            {
                //Symbols To Symbols
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Symbol
                    },
                    To = new State()
                    {
                        Title = States.Symbol
                    },
                    ConditionRegexPattern = "[A-Za-z]",
                },
                //Symbols To Digits
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Symbol
                    },
                    To = new State()
                    {
                        Title = States.Digit
                    },
                    ConditionRegexPattern = "[0-9]",
                },
                //Symbols To Space
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Symbol
                    },
                    To = new State()
                    {
                        Title = States.Space
                    },
                    ConditionRegexPattern = " ",
                },

                //Digits To Digits
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Digit
                    },
                    To = new State()
                    {
                        Title = States.Digit
                    },
                    ConditionRegexPattern = "[0-9]",
                },
                //Digits To Symbols
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Digit
                    },
                    To = new State()
                    {
                        Title = States.Symbol
                    },
                    ConditionRegexPattern = "[A-Za-z]",
                },
                //Digits To Space
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Digit
                    },
                    To = new State()
                    {
                        Title = States.Space
                    },
                    ConditionRegexPattern = " ",
                },

                //Space To Symbols
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Space
                    },
                    To = new State()
                    {
                        Title = States.Symbol
                    },
                    ConditionRegexPattern = "[A-Za-z]",
                },
                //Space To Digits
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Space
                    },
                    To = new State()
                    {
                        Title = States.Digit
                    },
                    ConditionRegexPattern = "[1-9]",
                },

                //LowerSymbol To EqualSymbol
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.LowerSymbol
                    },
                    To = new State()
                    {
                        Title = States.EqualSymbol
                    },
                    ConditionRegexPattern = "=",
                },
                //LowerSymbol To GreaterSymbol -- NonEquality
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.LowerSymbol
                    },
                    To = new State()
                    {
                        Title = States.GreaterSymbol
                    },
                    ConditionRegexPattern = ">",
                },
                
                //GreaterSymbol To Space
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.GreaterSymbol
                    },
                    To = new State()
                    {
                        Title = States.Space
                    },
                    ConditionRegexPattern = " ",
                },
                //GreaterSymbol To EqualSymbol
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.GreaterSymbol
                    },
                    To = new State()
                    {
                        Title = States.EqualSymbol
                    },
                    ConditionRegexPattern = "=",
                },
                //Space To GreaterSymbol
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Space
                    },
                    To = new State()
                    {
                        Title = States.GreaterSymbol
                    },
                    ConditionRegexPattern = ">",
                },


                //LowerSymbol To Space
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.LowerSymbol
                    },
                    To = new State()
                    {
                        Title = States.Space
                    },
                    ConditionRegexPattern = " ",
                },
                //Space To LowerSymbol
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Space
                    },
                    To = new State()
                    {
                        Title = States.LowerSymbol
                    },
                    ConditionRegexPattern = "<",
                },
                //EqualSymbol To Space
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.EqualSymbol
                    },
                    To = new State()
                    {
                        Title = States.Space
                    },
                    ConditionRegexPattern = " ",
                },
                //Space To EqualSymbol
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Space
                    },
                    To = new State()
                    {
                        Title = States.EqualSymbol
                    },
                    ConditionRegexPattern = "=",
                },

                //Space To Minus
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Space
                    },
                    To = new State()
                    {
                        Title = States.Minus
                    },
                    ConditionRegexPattern = "-",
                },
                //Space To Plus
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Space
                    },
                    To = new State()
                    {
                        Title = States.Plus
                    },
                    ConditionRegexPattern = @"\+",
                },
                //Space To Divide
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Space
                    },
                    To = new State()
                    {
                        Title = States.Divide
                    },
                    ConditionRegexPattern = "/",
                },
                //Space To Multiply
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Space
                    },
                    To = new State()
                    {
                        Title = States.Multiply
                    },
                    ConditionRegexPattern = @"\*",
                },

                //Minus To Space
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Minus
                    },
                    To = new State()
                    {
                        Title = States.Space
                    },
                    ConditionRegexPattern = " ",
                },
                //Plus To Space
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Plus
                    },
                    To = new State()
                    {
                        Title = States.Space
                    },
                    ConditionRegexPattern = " ",
                },
                //Divide To Space
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Divide
                    },
                    To = new State()
                    {
                        Title = States.Space
                    },
                    ConditionRegexPattern = " ",
                },
                //Multiply To Space
                new Transition()
                {
                    From = new State()
                    {
                        Title = States.Multiply
                    },
                    To = new State()
                    {
                        Title = States.Space
                    },
                    ConditionRegexPattern = " ",
                },
            };

            var tmp = JsonConvert.SerializeObject(transitions);

            using (StreamWriter writer = new StreamWriter(@"../../DigitsAndSymbolsWithSpaces.json"))
            {
                writer.WriteLine(tmp);
            }
        }
    }

    public enum LexemEnum
    {
        KeyWord, // { for | to | next }
        Identifier, // Vars
        Assignment, // = 
        ArithmeticOperation, // { +|-|/|* }
        Constant, // Nums
        LogicOperation, // { And | Or }
        Unknown
    }

    public static class Lexems
    {
        public static string[] KeyWord => new string[] { "for", "to", "next" };
        public static string IdentifierRegexPattern => @"[A-Za-z]+[0-9]*";
        public static string[] Assignment => new string[] { "=" };
        public static string[] ArithmeticOperation => new string[] { "+", "-", "/", "*" };
        public static string ConstantRegexPattern => @"[1-9]+[0-9]*";
        public static string[] LogicOperation => new string[] { "and", "or" };
    }

    public enum StatesEnum
    {
        Symbol,
        Digit,
        Space,

        LowerSymbol,
        GreaterSymbol,
        EqualSymbol,

        Plus,
        Minus,
        Divide,
        Multiply
    }

    public static class States
    {
        public static string Symbol => StatesEnum.Symbol.ToString();
        public static string Digit = StatesEnum.Digit.ToString();
        public static string Space = StatesEnum.Space.ToString();

        public static string LowerSymbol = StatesEnum.LowerSymbol.ToString();
        public static string GreaterSymbol = StatesEnum.GreaterSymbol.ToString();
        public static string EqualSymbol = StatesEnum.EqualSymbol.ToString();

        public static string Plus = StatesEnum.Plus.ToString();
        public static string Minus = StatesEnum.Minus.ToString();
        public static string Divide = StatesEnum.Divide.ToString();
        public static string Multiply = StatesEnum.Multiply.ToString();
    }
}
