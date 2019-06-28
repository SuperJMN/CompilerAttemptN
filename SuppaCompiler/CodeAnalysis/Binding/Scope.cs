using System.Collections.Generic;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    public class Scope
    {
        readonly IDictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public void Declare(string name, Symbol symbol)
        {
            symbols[name] = symbol;
        }

        public Symbol this[string name]
        {
            get { return symbols[name]; }
        }

        public bool TryGet(string name, out Symbol symbol)
        {
            return symbols.TryGetValue(name, out symbol);
        }
    }
}