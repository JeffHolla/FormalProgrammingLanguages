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

        private List<PostfixEntry> Entries = new List<PostfixEntry>();

        private List<PostfixEntry> AwaitsList = new List<PostfixEntry>();

        private Stack<PostfixEntry> _stack = new Stack<PostfixEntry>();

        public KDA(State startState, IEnumerable<Transition> transitions)
        {
            Transitions = transitions;

            StartState = startState;

            _сurrentState = startState;
        }

        public void Start(IEnumerable<char> inputSeq, bool showStates = false, bool showLexems = false,
            bool showIntepreterSteps = false, bool showSETSteps = false, bool showPOLISSteps = false)
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

            var result = CodeChecker.Check(string.Join("", inputSeq), out List<PostfixEntry> codeEntries);
            Console.WriteLine();
            Console.WriteLine("Final entryList :");
            if (result == null)
            {
                PrintEntryList(true);
            }
            else
            {
                Entries = codeEntries;
                CodeChecker.PrintEntryList(result);
            }

            PrintEntryList(codeEntries);

            Console.WriteLine();

            EnterVariableValues();

            MyInterpret(showIntepreterSteps: showIntepreterSteps, showSETSteps: showSETSteps);

            Console.WriteLine();
            Console.WriteLine("Final vars :");
            PrintAllVariablesAndValues();

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
        private bool IsArithmeticOperation(ref List<Lexem> lexems, ref int skippedPositions)
        {
            int i = 0;

            if (lexems[i].Type == LexemEnum.KeyWord && lexems[i].Value == "to")
            {
                WriteCmd(Cmd.SET);
                PrintEntryList();

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
                switch (lexems[i].Type)
                {
                    case LexemEnum.Identifier:
                        WriteVar(i, lexems);
                        break;
                    case LexemEnum.Constant:
                        WriteConst(i, lexems);
                        break;
                }

                switch (lexems[i + 2].Type)
                {
                    case LexemEnum.Identifier:
                        WriteVar(i + 2, lexems);
                        break;
                    case LexemEnum.Constant:
                        WriteConst(i + 2, lexems);
                        break;
                }

                Cmd cmd = GetArithmeticCmd(lexems[i + 1].Value);

                WriteCmd(cmd);
                PrintEntryList();

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
                // Write Var Iter
                Entries.Add(new PostfixEntry
                {
                    EntryType = EntryType.Var,
                    Value = Entries[0].Value
                });


                switch (lexems[i].Type)
                {
                    case LexemEnum.Identifier:
                        WriteVar(i, lexems);
                        break;
                    case LexemEnum.Constant:
                        WriteConst(i, lexems);
                        break;
                }

                switch (lexems[i + 2].Type)
                {
                    case LexemEnum.Identifier:
                        WriteVar(i + 2, lexems);
                        break;
                    case LexemEnum.Constant:
                        WriteConst(i + 2, lexems);
                        break;
                }

                Cmd cmd = GetArithmeticCmd(lexems[i + 1].Value);

                WriteCmd(cmd);
                PrintEntryList();


                WriteCmd(Cmd.CMPE);

                WriteCmdPtr(-1);
                WriteCmd(Cmd.JZ);

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

            WriteVar(i, lexems);
            PrintEntryList();

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
                    switch (lexems[i].Type)
                    {
                        case LexemEnum.Identifier:
                            WriteVar(i, lexems);
                            break;
                        case LexemEnum.Constant:
                            WriteConst(i, lexems);
                            break;
                    }

                    switch (lexems[i + 2].Type)
                    {
                        case LexemEnum.Identifier:
                            WriteVar(i + 2, lexems);
                            break;
                        case LexemEnum.Constant:
                            WriteConst(i + 2, lexems);
                            break;
                    }

                    Cmd cmd = GetArithmeticCmd(lexems[i + 1].Value);

                    WriteCmd(cmd);
                    PrintEntryList();

                    WriteCmd(Cmd.SET);

                    skippedPositions += 3;
                    lexems = lexems.Skip(3).ToList();
                }
            }
            else
            {
                switch (lexems[i].Type)
                {
                    case LexemEnum.Identifier:
                        WriteVar(i, lexems);
                        break;
                    case LexemEnum.Constant:
                        WriteConst(i, lexems);
                        break;
                }

                switch (lexems[i + 2].Type)
                {
                    case LexemEnum.Identifier:
                        WriteVar(i + 2, lexems);
                        break;
                    case LexemEnum.Constant:
                        WriteConst(i + 2, lexems);
                        break;
                }

                Cmd cmd = GetArithmeticCmd(lexems[i + 1].Value);

                WriteCmd(cmd);
                PrintEntryList();

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
                    // Write Var Iter
                    Entries.Add(new PostfixEntry
                    {
                        EntryType = EntryType.Var,
                        Value = Entries[0].Value
                    });

                    // Write Var Iter += 1
                    Entries.Add(new PostfixEntry
                    {
                        EntryType = EntryType.Var,
                        Value = Entries[0].Value
                    });

                    // Write Const 1
                    Entries.Add(new PostfixEntry
                    {
                        EntryType = EntryType.Const,
                        Value = "1"
                    });

                    WriteCmd(Cmd.ADD);

                    WriteCmd(Cmd.SET);

                    WriteCmdPtr(5); // Start for

                    WriteCmd(Cmd.JMP);

                    SetCmdPtr(-1, Entries.Count);

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

        private int WriteCmd(Cmd cmd)
        {
            Entries.Add(new PostfixEntry
            {
                Cmd = cmd,
                EntryType = EntryType.Cmd
            });

            return Entries.Count - 1;
        }

        private int WriteVar(int index, List<Lexem> lexems)
        {
            Entries.Add(new PostfixEntry
            {
                EntryType = EntryType.Var,
                Value = lexems[index].Value
            });

            return Entries.Count - 1;
        }

        private int WriteConst(int index, List<Lexem> lexems)
        {
            Entries.Add(new PostfixEntry
            {
                EntryType = EntryType.Const,
                Value = lexems[index].Value
            });

            return Entries.Count - 1;
        }

        private int WriteCmdPtr(int ptr)
        {
            Entries.Add(new PostfixEntry
            {
                EntryType = EntryType.CmdPtr,
                CmdPtr = ptr,
            });

            return Entries.Count - 1;
        }

        private void SetCmdPtr(int indexToChange, int newPtr)
        {
            var index = Entries.IndexOf(Entries.First(x => x.CmdPtr == indexToChange));
            Entries[index].CmdPtr = newPtr;
        }

        //Interp Section
        private void EnterVariableValues()
        {
            try
            {
                Console.WriteLine("Enter variable values:");

                var variables = Entries
                                    .Where(e => e.EntryType == EntryType.Var)
                                    .Select(v => v.Value).Distinct();
                foreach (var variable in variables)
                {
                    Console.Write($"{variable} = ");
                    var value = int.Parse(Console.ReadLine());
                    Entries
                        .Where(e => e.EntryType == EntryType.Var)
                        .Where(v => v.Value == variable)
                        .ToList()
                        .ForEach(v => v.VariableValue = value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void MyInterpret(bool showIntepreterSteps = false, bool showSETSteps = false)
        {
            int iter = 0;
            while (iter < Entries.Count)
            {
                if (Entries[iter].EntryType == EntryType.Cmd)
                {
                    var cmd = Entries[iter].Cmd.Value;

                    int? beforeJmpOrJz = null;
                    if (cmd == Cmd.JZ || cmd == Cmd.JMP)
                    {
                        beforeJmpOrJz = iter;
                    }

                    switch (cmd)
                    {
                        //iter 5 2 MUL SET iter 5 20 ADD CMPE 24 JZ it 50 72 SUB SET iter iter 1 ADD SET 5 JMP
                        case Cmd.JMP:
                            iter = PopVal();
                            break;
                        case Cmd.JZ:
                            var temp = PopVal();
                            if (PopVal() == 0)
                            {
                                iter++;
                            }
                            else
                            {
                                iter = temp;
                            }
                            break;
                        case Cmd.SET:
                            if (showSETSteps)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("~~~~~~~~~~~~~~~~");
                                Console.WriteLine("Before SET");
                                PrintAllVariablesAndValues();
                                Console.WriteLine("~~~~~~~~~~~~~~~~");
                            }

                            SetVarAndPop(PopVal());

                            if (showSETSteps)
                            {
                                Console.WriteLine("~~~~~~~~~~~~~~~~");
                                Console.WriteLine("After SET");
                                PrintAllVariablesAndValues();
                                Console.WriteLine("~~~~~~~~~~~~~~~~");
                                Console.ForegroundColor = ConsoleColor.White;
                            }
                            iter++;
                            break;

                        case Cmd.ADD:
                            PushValue(PopVal() + PopVal());
                            iter++;
                            break;
                        case Cmd.SUB:
                            PushValue(-PopVal() + PopVal());
                            iter++;
                            break;
                        case Cmd.MUL:
                            PushValue(PopVal() * PopVal());
                            iter++;
                            break;
                        case Cmd.DIV:
                            PushValue((int)(1.0 / PopVal() * PopVal()));
                            iter++;
                            break;

                        case Cmd.CMPE:
                            PushValue((PopVal() == PopVal()) ? 1 : 0);
                            iter++;
                            break;
                        case Cmd.CMPNE:
                            PushValue((PopVal() != PopVal()) ? 1 : 0);
                            iter++;
                            break;
                        case Cmd.CMPL:
                            PushValue((PopVal() > PopVal()) ? 1 : 0);
                            iter++;
                            break;
                        case Cmd.CMPLE:
                            PushValue((PopVal() >= PopVal()) ? 1 : 0);
                            iter++;
                            break;
                    }

                    if (showIntepreterSteps)
                    {
                        var toShowIter = iter == 0 ? 0 : iter - 1;

                        toShowIter = beforeJmpOrJz.HasValue ? beforeJmpOrJz.Value : toShowIter;
                        
                        PrintInterpreter(toShowIter);
                    }
                }
                else
                {
                    _stack.Push(Entries[iter]);

                    ++iter;

                    if (showIntepreterSteps)
                    {
                        PrintInterpreter(iter - 1);
                    }
                }
            }
        }

        private int PopVal()
        {
            if (_stack.Count != 0)
            {
                var obj = _stack.Pop();
                switch (obj.EntryType)
                {
                    case EntryType.Var:
                        return obj.VariableValue.Value;
                    case EntryType.Const:
                        return Convert.ToInt32(obj.Value);
                    case EntryType.CmdPtr:
                        return obj.CmdPtr.Value;
                    default:
                        throw new ArgumentException("obj.EntryType");
                }
            }
            else
            {
                return 0;
            }
        }

        private void PushValue(int val)
        {
            var entry = new PostfixEntry
            {
                EntryType = EntryType.Const,
                Value = val.ToString()
            };
            _stack.Push(entry);
        }

        private void SetVarAndPop(int val)
        {
            var variable = _stack.Pop();
            if (variable.EntryType != EntryType.Var)
            {
                throw new ArgumentException("EntryType");
            }

            Entries
                .Where(e => e.EntryType == EntryType.Var)
                .Where(v => v.Value == variable.Value)
                .ToList()
                .ForEach(v => v.VariableValue = val);
        }

        private void PrintAllVariablesAndValues()
        {
            var listOfAllVars = Entries
                .Where(entry => entry.EntryType == EntryType.Var)
                .GroupBy(entry => entry.Value)
                .Select(group => group.First());

            foreach (var varVar in listOfAllVars)
            {
                Console.WriteLine($"Variable = [{varVar.Value}], Value = [{varVar.VariableValue}]");
            }
        }

        private string GetAllVariablesAndValues()
        {
            var listOfAllVars = Entries
                .Where(entry => entry.EntryType == EntryType.Var)
                .GroupBy(entry => entry.Value)
                .Select(group => group.First());

            string result = "";
            foreach (var varVar in listOfAllVars)
            {
                result += $"Variable = [{varVar.Value}], Value = [{varVar.VariableValue}] \n";
            }

            return result;
        }

        private string GetAllVariablesAndValuesShorter()
        {
            var listOfAllVars = Entries
                .Where(entry => entry.EntryType == EntryType.Var)
                .GroupBy(entry => entry.Value)
                .Select(group => group.First());

            string result = "Vars : [";
            foreach (var varVar in listOfAllVars)
            {
                result += $"{varVar.Value} = {varVar.VariableValue} | ";
            }

            result = result.Substring(0, result.LastIndexOf("|") - 1) + "]";

            return result;
        }


        // Common
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

        public void PrintEntryList(bool showPOLISSteps = false)
        {
            if (showPOLISSteps)
            {
                Console.WriteLine();
                foreach (var entry in Entries)
                {
                    Console.Write($"{GetEntryString(entry)} ");
                }
                Console.WriteLine();
            }
        }

        public void PrintEntryList(List<PostfixEntry> entries)
        {
            Console.WriteLine();
            foreach (var entry in entries)
            {
                Console.Write($"{GetEntryString(entry)} ");
            }
            Console.WriteLine();
        }

        public void PrintInterpreter(int entryIndex)
        {
            //Console.WriteLine("----------------");
            Console.Write($"Index : [{entryIndex}] // ");
            Console.Write($"Entry : [{GetEntryString(Entries[entryIndex])}] // ");
            Console.Write($"Stack : [{GetStackString()}] // ");
            //Console.Write($"Stack : [{string.Join(" -> ", _stack)}] // ");
            //Console.Write(GetAllVariablesAndValues());
            Console.WriteLine(GetAllVariablesAndValuesShorter());
            //Console.WriteLine("----------------");
        }

        public string GetEntryString(PostfixEntry entry)
        {
            if (entry.EntryType == EntryType.Var) return entry.Value;
            else if (entry.EntryType == EntryType.Const) return entry.Value;
            else if (entry.EntryType == EntryType.Cmd) return entry.Cmd.ToString();
            else if (entry.EntryType == EntryType.CmdPtr) return entry.CmdPtr.ToString();
            throw new ArgumentException("PostfixEntry");
        }

        public string GetStackString()
        {
            string result = "";
            foreach (var element in _stack)
            {
                if (element.EntryType == EntryType.Var) result += $"{EntryType.Var} | {element.Value} -> ";
                else if (element.EntryType == EntryType.Const) result += $"{EntryType.Const} | {element.Value} -> ";
                else if (element.EntryType == EntryType.Cmd) result += $"Cmd | {element.Cmd} -> ";
                else if (element.EntryType == EntryType.CmdPtr) result += $"CmdPtr | {element.CmdPtr.Value} -> ";
            }

            if (result.Length != 0)
            {
                result = result.Substring(0, result.LastIndexOf(" ->"));
            }

            return result;
        }

        public Cmd GetArithmeticCmd(string arithmeticOp)
        {
            switch (arithmeticOp)
            {
                case "+":
                    return Cmd.ADD;
                case "-":
                    return Cmd.SUB;
                case "*":
                    return Cmd.MUL;
                case "/":
                    return Cmd.DIV;
            }

            throw new Exception($"Not found Arithmetic Operation in [{arithmeticOp}]");
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

    public class PostfixEntry
    {
        public int Index { get; set; }
        public EntryType EntryType { get; set; }
        public Cmd? Cmd { get; set; }
        public string Value { get; set; }
        public int? CmdPtr { get; set; }
        public int? VariableValue { get; set; }

        public override string ToString()
        {
            return $"{EntryType} | {Value}";
        }
    }


    public enum EntryType
    {
        Cmd,
        Var,
        Const,
        CmdPtr
    }

    public enum Cmd
    {
        JMP,
        JZ,
        SET,
        ADD,
        SUB,
        MUL,
        DIV,
        AND,
        OR,
        CMPE,
        CMPNE,
        CMPL,
        CMPLE,

        NotFound
    }
}