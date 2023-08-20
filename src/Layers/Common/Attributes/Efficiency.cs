namespace Common.Attributes
{
    public class Efficiency :Attribute
    {
        public double Value { get; set; }
        public Efficiency(double Value)
        {
            this.Value = Value;
        }
    }
}
