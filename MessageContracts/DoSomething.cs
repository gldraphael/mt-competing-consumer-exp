using System;

namespace MessageContracts
{
    public class DoSomething
    {
        public string For { get; }

        public DoSomething(string @for)
        {
            For = @for;
        }
    }
}
