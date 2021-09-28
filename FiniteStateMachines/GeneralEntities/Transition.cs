namespace Formal_Languages_Task_1_KDA
{
    public class Transition
    {
        public int Value { get; set; }
        public State From { get; set; }
        public State To { get; set; }
        public int Condition { get; set; }
    }
}
