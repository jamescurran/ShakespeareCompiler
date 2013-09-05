using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shakespeare.Support.Utilities
{
    public interface IKeySearchable<T>
    {
        T Key { get; }
    }
}
