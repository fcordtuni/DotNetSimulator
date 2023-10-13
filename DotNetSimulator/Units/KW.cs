namespace DotNetSimulator.Units
{
    public readonly struct KW
    {
        public double Amount { get; }
        public KW(double amount) { Amount = amount; }


        public KWH ForTimeSpan(TimeSpan time) => new(this, time);

        public static KWH operator *(KW kw, TimeSpan time) => kw.ForTimeSpan(time);

        public static KW operator *(KW left, double right) => new(left.Amount * right);
        public static KW operator *(double left, KW right) => right * left;
    }
}
