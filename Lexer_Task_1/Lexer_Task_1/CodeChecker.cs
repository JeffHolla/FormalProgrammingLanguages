using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer_Task_1
{
    public static class CodeChecker
    {
        public static string Check(string code, out List<PostfixEntry> entries)
        {
            string input_extented_arithm_1 = "iter 5 2 MUL 205 ADD 2 SUB SET iter 5 210 ADD CMPE 28 JZ it 50 72 SUB SET iter iter 1 ADD SET 9 JMP";

            string input_extented_arithm_2 = "iter 5 2 MUL SET iter 6 10 MUL 20 DIV 10 ADD CMPE 28 JZ it 50 72 SUB SET iter iter 1 ADD SET 5 JMP";

            string input_extented_arithm_3 = "iter 5 2 MUL SET iter 5 8 ADD CMPE 28 JZ it 50 72 MUL 5 MUL 2 DIV SET iter iter 1 ADD SET 5 JMP";
            

            string input_extented_operator = "iter 5 2 MUL SET iter 5 8 ADD CMPE 30 JZ it 50 72 SUB SET ite 15 SET another 125 SET iter iter 1 ADD SET 5 JMP";
            
            switch (code)
            {
                case "for iter = 5 * 2 + 205 - 2 to 5 + 210 it = 50 - 72 next":
                    entries = new List<PostfixEntry>()
                {
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Value = "2",
                        EntryType = EntryType.Const
                    }, // 2
                    new PostfixEntry()
                    {
                        Cmd = Cmd.MUL,
                        EntryType = EntryType.Cmd
                    }, // MUL
                    new PostfixEntry()
                    {
                        Value = "205",
                        EntryType = EntryType.Const
                    }, // 205
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Value = "2",
                        EntryType = EntryType.Const
                    }, // 2
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SUB,
                        EntryType = EntryType.Cmd
                    }, // SUB
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Value = "210",
                        EntryType = EntryType.Const
                    }, // 210
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.CMPE,
                        EntryType = EntryType.Cmd
                    }, // CMPE
                    new PostfixEntry()
                    {
                        CmdPtr = 28,
                        EntryType = EntryType.CmdPtr
                    }, // 28
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JZ,
                        EntryType = EntryType.Cmd
                    }, // JZ
                    new PostfixEntry()
                    {
                        Value = "it",
                        EntryType = EntryType.Var
                    }, // it
                    new PostfixEntry()
                    {
                        Value = "50",
                        EntryType = EntryType.Const
                    }, // 50
                    new PostfixEntry()
                    {
                        Value = "72",
                        EntryType = EntryType.Const
                    }, // 72
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SUB,
                        EntryType = EntryType.Cmd
                    }, // SUB
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "1",
                        EntryType = EntryType.Const
                    }, // 1
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        CmdPtr = 9,
                        EntryType = EntryType.CmdPtr
                    }, // 9
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JMP,
                        EntryType = EntryType.Cmd
                    } // JMP
                };
                    return input_extented_arithm_1;

                case "for iter = 5 * 2 to 6 * 10 / 20 + 10 it = 50 - 72 next":
                    entries = new List<PostfixEntry>()
                {
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Value = "2",
                        EntryType = EntryType.Const
                    }, // 2
                    new PostfixEntry()
                    {
                        Cmd = Cmd.MUL,
                        EntryType = EntryType.Cmd
                    }, // MUL
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "6",
                        EntryType = EntryType.Const
                    }, // 6
                    new PostfixEntry()
                    {
                        Value = "10",
                        EntryType = EntryType.Const
                    }, // 10
                    new PostfixEntry()
                    {
                        Cmd = Cmd.MUL,
                        EntryType = EntryType.Cmd
                    }, // MUL
                    new PostfixEntry()
                    {
                        Value = "20",
                        EntryType = EntryType.Const
                    }, // 20
                    new PostfixEntry()
                    {
                        Cmd = Cmd.DIV,
                        EntryType = EntryType.Cmd
                    }, // DIV
                    new PostfixEntry()
                    {
                        Value = "10",
                        EntryType = EntryType.Const
                    }, // 10
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.CMPE,
                        EntryType = EntryType.Cmd
                    }, // CMPE
                    new PostfixEntry()
                    {
                        CmdPtr = 28,
                        EntryType = EntryType.CmdPtr
                    }, // 28
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JZ,
                        EntryType = EntryType.Cmd
                    }, // JZ
                    new PostfixEntry()
                    {
                        Value = "it",
                        EntryType = EntryType.Var
                    }, // it
                    new PostfixEntry()
                    {
                        Value = "50",
                        EntryType = EntryType.Const
                    }, // 50
                    new PostfixEntry()
                    {
                        Value = "72",
                        EntryType = EntryType.Const
                    }, // 72
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SUB,
                        EntryType = EntryType.Cmd
                    }, // SUB
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "1",
                        EntryType = EntryType.Const
                    }, // 1
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        CmdPtr = 5,
                        EntryType = EntryType.CmdPtr
                    }, // 5
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JMP,
                        EntryType = EntryType.Cmd
                    } // JMP
                };
                    return input_extented_arithm_2;

                case "for iter = 5 * 2 to 5 + 8 it = 50 * 72 * 5 / 2 next":
                    entries = new List<PostfixEntry>()
                {
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Value = "2",
                        EntryType = EntryType.Const
                    }, // 2
                    new PostfixEntry()
                    {
                        Cmd = Cmd.MUL,
                        EntryType = EntryType.Cmd
                    }, // MUL
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Value = "8",
                        EntryType = EntryType.Const
                    }, // 8
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.CMPE,
                        EntryType = EntryType.Cmd
                    }, // CMPE
                    new PostfixEntry()
                    {
                        CmdPtr = 28,
                        EntryType = EntryType.CmdPtr
                    }, // 28
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JZ,
                        EntryType = EntryType.Cmd
                    }, // JZ
                    new PostfixEntry()
                    {
                        Value = "it",
                        EntryType = EntryType.Var
                    }, // it
                    new PostfixEntry()
                    {
                        Value = "50",
                        EntryType = EntryType.Const
                    }, // 50
                    new PostfixEntry()
                    {
                        Value = "72",
                        EntryType = EntryType.Const
                    }, // 72
                    new PostfixEntry()
                    {
                        Cmd = Cmd.MUL,
                        EntryType = EntryType.Cmd
                    }, // MUL
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Cmd = Cmd.MUL,
                        EntryType = EntryType.Cmd
                    }, // MUL
                    new PostfixEntry()
                    {
                        Value = "2",
                        EntryType = EntryType.Const
                    }, // 2
                    new PostfixEntry()
                    {
                        Cmd = Cmd.DIV,
                        EntryType = EntryType.Cmd
                    }, // DIV
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "1",
                        EntryType = EntryType.Const
                    }, // 1
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        CmdPtr = 5,
                        EntryType = EntryType.CmdPtr
                    }, // 5
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JMP,
                        EntryType = EntryType.Cmd
                    } // JMP
                };
                    return input_extented_arithm_3;
                
                case "for iter = 5 * 2 to 5 + 8 it = 50 - 72 ite = 15 another = 125 next":
                    entries = new List<PostfixEntry>()
                {
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Value = "2",
                        EntryType = EntryType.Const
                    }, // 2
                    new PostfixEntry()
                    {
                        Cmd = Cmd.MUL,
                        EntryType = EntryType.Cmd
                    }, // MUL
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Value = "8",
                        EntryType = EntryType.Const
                    }, // 8
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.CMPE,
                        EntryType = EntryType.Cmd
                    }, // CMPE
                    new PostfixEntry()
                    {
                        CmdPtr = 30,
                        EntryType = EntryType.CmdPtr
                    }, // 30
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JZ,
                        EntryType = EntryType.Cmd
                    }, // JZ
                    new PostfixEntry()
                    {
                        Value = "it",
                        EntryType = EntryType.Var
                    }, // it
                    new PostfixEntry()
                    {
                        Value = "50",
                        EntryType = EntryType.Const
                    }, // 50
                    new PostfixEntry()
                    {
                        Value = "72",
                        EntryType = EntryType.Const
                    }, // 72
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SUB,
                        EntryType = EntryType.Cmd
                    }, // SUB
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "ite",
                        EntryType = EntryType.Var
                    }, // ite
                    new PostfixEntry()
                    {
                        Value = "15",
                        EntryType = EntryType.Const
                    }, // 15
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "another",
                        EntryType = EntryType.Var
                    }, // another
                    new PostfixEntry()
                    {
                        Value = "125",
                        EntryType = EntryType.Const
                    }, // 125
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "1",
                        EntryType = EntryType.Const
                    }, // 1
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        CmdPtr = 5,
                        EntryType = EntryType.CmdPtr
                    }, // 5
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JMP,
                        EntryType = EntryType.Cmd
                    } // JMP
                };
                    return input_extented_operator;

                default:
                    entries = new List<PostfixEntry>();
                    return null;
            }
        }
        public static void PrintEntryList(string code)
        {
            Console.WriteLine();
            Console.WriteLine(code);
            Console.WriteLine();
        }
    }
}


/*
 entries = new List<PostfixEntry>()
                {
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Value = "2",
                        EntryType = EntryType.Const
                    }, // 2
                    new PostfixEntry()
                    {
                        Cmd = Cmd.MUL,
                        EntryType = EntryType.Cmd
                    }, // MUL
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "5",
                        EntryType = EntryType.Const
                    }, // 5
                    new PostfixEntry()
                    {
                        Value = "20",
                        EntryType = EntryType.Const
                    }, // 20
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.CMPE,
                        EntryType = EntryType.Cmd
                    }, // CMPE
                    new PostfixEntry()
                    {
                        CmdPtr = 30,
                        EntryType = EntryType.CmdPtr
                    }, // 30
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JZ,
                        EntryType = EntryType.Cmd
                    }, // JZ
                    new PostfixEntry()
                    {
                        Value = "it",
                        EntryType = EntryType.Var
                    }, // it
                    new PostfixEntry()
                    {
                        Value = "50",
                        EntryType = EntryType.Const
                    }, // 50
                    new PostfixEntry()
                    {
                        Value = "72",
                        EntryType = EntryType.Const
                    }, // 72
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SUB,
                        EntryType = EntryType.Cmd
                    }, // SUB
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "ite",
                        EntryType = EntryType.Var
                    }, // ite
                    new PostfixEntry()
                    {
                        Value = "15",
                        EntryType = EntryType.Const
                    }, // 15
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "another",
                        EntryType = EntryType.Var
                    }, // another
                    new PostfixEntry()
                    {
                        Value = "125",
                        EntryType = EntryType.Const
                    }, // 125
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "iter",
                        EntryType = EntryType.Var
                    }, // iter
                    new PostfixEntry()
                    {
                        Value = "1",
                        EntryType = EntryType.Const
                    }, // 1
                    new PostfixEntry()
                    {
                        Cmd = Cmd.ADD,
                        EntryType = EntryType.Cmd
                    }, // ADD
                    new PostfixEntry()
                    {
                        Cmd = Cmd.SET,
                        EntryType = EntryType.Cmd
                    }, // SET
                    new PostfixEntry()
                    {
                        CmdPtr = 5,
                        EntryType = EntryType.CmdPtr
                    }, // 5
                    new PostfixEntry()
                    {
                        Cmd = Cmd.JMP,
                        EntryType = EntryType.Cmd
                    } // JMP
                };

 */