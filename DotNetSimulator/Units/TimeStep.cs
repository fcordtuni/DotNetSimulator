namespace DotNetSimulator.Units
{
    internal readonly struct TimeStep
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public TimeSpan Duration { get; }

        public TimeStep(DateTime start, DateTime end) { Start = start; End = end; Duration = end - start; }
        public TimeStep(DateTime start, TimeSpan duration) { Start = start; End = start + duration; Duration = duration; }

        public TimeStep Next(TimeSpan duration) => new(End, duration);

        public override string ToString()
        {
            return Start + " - " + End;
        }
    }
}
