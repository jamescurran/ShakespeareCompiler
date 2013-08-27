using Irony.Parsing;
using Shakespeare.AST;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Shakespeare
{
    public class ContextData
    {
        public string CurrentAct { get; set; }
        public string CurrentScene { get; set; }
        public int NumErrors { get; set; }
        public int NumWarnings { get; set; }
        public List<BnfTerm> Characters;
        public List<CharacterNode> ActiveCharacters { get; set; }
        public TextWriter Writer { get; set; }
        public Stream UnderlyingStream { get; set; }

        public ContextData()
        {
            UnderlyingStream = new MemoryStream();
            Writer = new StreamWriter(UnderlyingStream);
            ActiveCharacters = new List<CharacterNode>();
        }
    }
}
