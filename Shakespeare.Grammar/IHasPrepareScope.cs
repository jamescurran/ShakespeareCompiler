using Irony.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shakespeare
{

    public interface IHasPrepareScope
    {
        void PrepareScope(ScriptThread thead, object compilerParams);
    }
}
