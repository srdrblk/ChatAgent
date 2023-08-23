namespace Common.Attributes
{
    public class WorkHour : Attribute
    {
        public int Start { get; set; }
        public int End { get; set; }
        public WorkHour(int Start, int End)
        {
            this.Start = Start;
            this.End = End;
        }
    }
}
