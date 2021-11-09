namespace Formal_Languages_Task_1_KDA
{
    public class Transition
    {
        public string Value { get; set; }
        public State From { get; set; }
        public State To { get; set; }
        public string ConditionRegexPattern { get; set; }
    }
}
