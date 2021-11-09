namespace Formal_Languages_Task_1_KDA
{
    public class State
    {
        public string Title { get; set; }

        public override bool Equals(object obj)
        {
            return ((State)obj).Title == Title;
        }

        public override int GetHashCode()
        {
            return Title.GetHashCode();
        }

        public static bool operator ==(State state1, State state2)
        {
            return state1.Equals(state2);
        }
        public static bool operator !=(State state1, State state2)
        {
            return !state1.Equals(state2);
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
