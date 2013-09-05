using Irony.Parsing;
using Shakespeare.Ast;
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
        public List<CharacterNode> ActiveCharacters { get; set; }

        public ContextData()
        {
            ActiveCharacters = new List<CharacterNode>();
        }
    }
}
