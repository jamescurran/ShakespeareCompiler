using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MakeScannerCS
{
    class Program
    {
        static void Main(string[] args)
        {
            var ms = new MakeScanner(args.Any() ? args[0] : ".");

            ms.InsertFile("user_code_top.metaflex");
            ms.InsertFile("roman_numbers.metaflex");
            ms.WriteLine();
            ms.WriteLine("%%");
            ms.RulesFromFile("article.wordlist", "ARTICLE");

            ms.RulesFromFile("be.wordlist", "BE");
            ms.RulesFromFile("character.wordlist","CHARACTER");
            ms.RulesFromFile("first_person.wordlist","FIRST_PERSON");
            ms.RulesFromFile("first_person_possessive.wordlist","FIRST_PERSON_POSSESSIVE");
            ms.RulesFromFile("first_person_reflexive.wordlist","FIRST_PERSON_REFLEXIVE");
            ms.RulesFromFile("negative_adjective.wordlist","NEGATIVE_ADJECTIVE");
            ms.RulesFromFile("negative_comparative.wordlist","NEGATIVE_COMPARATIVE");
            ms.RulesFromFile("negative_noun.wordlist","NEGATIVE_NOUN");
            ms.RulesFromFile("neutral_adjective.wordlist","NEUTRAL_ADJECTIVE");
            ms.RulesFromFile("neutral_noun.wordlist","NEUTRAL_NOUN");
            ms.RulesFromFile("nothing.wordlist","NOTHING");
            ms.RulesFromFile("positive_adjective.wordlist","POSITIVE_ADJECTIVE");
            ms.RulesFromFile("positive_comparative.wordlist","POSITIVE_COMPARATIVE");
            ms.RulesFromFile("positive_noun.wordlist","POSITIVE_NOUN");
            ms.RulesFromFile("second_person.wordlist","SECOND_PERSON");
            ms.RulesFromFile("second_person_possessive.wordlist","SECOND_PERSON_POSSESSIVE");
            ms.RulesFromFile("second_person_reflexive.wordlist","SECOND_PERSON_REFLEXIVE");
            ms.RulesFromFile("third_person_possessive.wordlist","THIRD_PERSON_POSSESSIVE");

            ms.WriteLine();
            ms.WriteLine(" /* single word rules */\n");

            ms.RuleForWord("and", "AND");
            ms.RuleForWord("as", "AS");
            ms.RuleForWord("enter", "ENTER");
            ms.RuleForWord("exeunt", "EXEUNT");
            ms.RuleForWord("exit", "EXIT");
            ms.RuleForWord("heart", "HEART");
            ms.RuleForWord("if not", "IF_NOT");
            ms.RuleForWord("if so", "IF_SO");
            ms.RuleForWord("less", "LESS");
            ms.RuleForWord("let us", "LET_US");
            ms.RuleForWord("listen to", "LISTEN_TO");
            ms.RuleForWord("mind", "MIND");
            ms.RuleForWord("more", "MORE");
            ms.RuleForWord("not", "NOT");
            ms.RuleForWord("open", "OPEN");
            ms.RuleForWord("proceed to", "PROCEED_TO");
            ms.RuleForWord("recall", "RECALL");
            ms.RuleForWord("remember", "REMEMBER");
            ms.RuleForWord("return to", "RETURN_TO");
            ms.RuleForWord("speak", "SPEAK");
            ms.RuleForWord("than", "THAN");
            ms.RuleForWord("the cube of", "THE_CUBE_OF");
            ms.RuleForWord("the difference between", "THE_DIFFERENCE_BETWEEN");
            ms.RuleForWord("the factorial of", "THE_FACTORIAL_OF");
            ms.RuleForWord("the product of", "THE_PRODUCT_OF");
            ms.RuleForWord("the quotient between", "THE_QUOTIENT_BETWEEN");
            ms.RuleForWord("the remainder of the quotient between", "THE_REMAINDER_OF_THE_QUOTIENT_BETWEEN");
            ms.RuleForWord("the square of", "THE_SQUARE_OF");
            ms.RuleForWord("the square root of", "THE_SQUARE_ROOT_OF");
            ms.RuleForWord("the sum of", "THE_SUM_OF");
            ms.RuleForWord("twice", "TWICE");
            ms.RuleForWord("we must", "WE_MUST");
            ms.RuleForWord("we shall", "WE_SHALL");

            /* - Other rules */
            ms.WriteLine();
            ms.WriteLine(" /* rules for terminals from file terminals.metaflex */");
            ms.InsertFile("terminals.metaflex");

            /* Separator */
            ms.WriteLine();
            ms.WriteLine("%%");

            /* User code */
            ms.InsertFile("user_code_bottom.metaflex");

            ms.WriteTo(Console.Out);

        }

    }

    class MakeScanner
    {
        string include_path;
        StringWriter sw;

        public MakeScanner(string inc_path)
        {
            include_path = inc_path;
            sw = new StringWriter();
        }

        public void InsertFile(string filename)
        {
            var pathname = Path.Combine(include_path, filename);
            if (!File.Exists(pathname))
                throw new FileNotFoundException("Could not find file.", pathname);
            else
            {
                var text = File.ReadAllText(pathname);
                sw.WriteLine();
                sw.Write(text);
            }
        }

        public void WriteLine(string text)
        {
            sw.WriteLine(text);
        }
        public void WriteLine()
        {
            sw.WriteLine();
        }

        public void RulesFromFile(string filename, string token)
        {
            var pathname = Path.Combine(include_path, filename);
            if (!File.Exists(pathname))
                throw new FileNotFoundException("Could not find file.", pathname);
            else
            {
                sw.WriteLine();
                sw.WriteLine(" /* rules from file {0} */", pathname);
                foreach (var line in File.ReadAllLines(pathname))
                {
                    RuleForWord(line, token);
                }

                var text = File.ReadAllText(pathname);
            }

        }

#if YACC
        public void RuleForWord(string line, string token)
        {
            sw.WriteLine("{0} {{\n\tyylval.str = newstr(yytext); return {1};\n }}",
                MakeRegExp(line), token);
        }
#else
        public void RuleForWord(string line, string token)
        {
            //       var BAR = ToTerm("|", "bar");

            sw.WriteLine("\t var {0} = ToTerm({1}, \"{2}\");", token, MakeRegExp(line), token.ToLowerInvariant());
        }

#endif

        private string MakeRegExp(string line)
        {
            const int STRING_LENGTH = 4096;
            const string space_string = "\"[[:space:]]+\"";

            var TempStr = new StringBuilder(STRING_LENGTH);
            line = line.Trim();

            bool isPrevSpace = true;
            TempStr.Append('"');
            foreach (char c in line)
            {
                if (Char.IsWhiteSpace(c))
                {
                    if (!isPrevSpace)
                    {
                        TempStr.Append(space_string);
                    }
                }
                else
                {
                    isPrevSpace = false;
                    TempStr.Append(c);
                }
            }
            TempStr.Append('"');
            return TempStr.ToString();
        }

        internal void WriteTo(TextWriter textWriter)
        {
            textWriter.Write(sw.ToString());
        }
    }

}
