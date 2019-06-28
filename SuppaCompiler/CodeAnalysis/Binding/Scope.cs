using System.Collections.Generic;

namespace SuppaCompiler.CodeAnalysis.Binding
{
    public class Scope
    {
        readonly IDictionary<string, Symbol> symbols = new Dictionary<string, Symbol>();

        public Scope()
        {
        }

        public Scope(Scope parent)
        {
            Parent = parent;
        }

        public Scope Parent { get; }

        public void Declare(string name, Symbol symbol)
        {
            symbols[name] = symbol;
        }

        public Symbol this[string name]
        {
            get
            {
                if (TryGet(name, out var v))
                {
                    return v;
                }

                throw new KeyNotFoundException(name);
            }
        }

        public bool TryGet(string name, out Symbol symbol)
        {
            var tryGetValue = symbols.TryGetValue(name, out symbol);

            if (!tryGetValue)
            {
                return Parent?.TryGet(name, out symbol) ?? false;
            }

            return true;
        }
    }
}