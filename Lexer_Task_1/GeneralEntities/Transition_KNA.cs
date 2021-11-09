using System.Collections.Generic;

namespace Formal_Languages_Task_1_KDA
{
    public class Transition_KNA
    {
        public int Value { get; set; }
        public State From { get; set; }
        public State To { get; set; }
        public IEnumerable<int> Condition { get; set; }
    }
}
