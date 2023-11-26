//Author: FCORDT
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestDotNetSimulator")]
namespace DotNetSimulator.Units;

/// <summary>
/// A struct representing KW units
/// </summary>
public readonly struct KW
{
    /// <summary>
    /// gets the numeric representation of the KW
    /// </summary>
    public double Amount { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public KW(double amount) { Amount = amount; }

    /// <summary>
    /// returns the <see cref="KWH"/> that this unit would provide for the given <paramref name="time"/>
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    internal KWH ForTimeSpan(TimeSpan time) => new(this, time);

    public static KWH operator *(KW kw, TimeSpan time) => kw.ForTimeSpan(time);

    public static KW operator *(KW left, double right) => new(left.Amount * right);
    public static KW operator *(double left, KW right) => right * left;

    public static readonly KW Infinity = new(double.PositiveInfinity);
    public static readonly KW Zero = new(0);

    /// <inheritdoc/>
    public override string ToString()
    {
        var formatter = CultureInfo.CreateSpecificCulture("de-DE");
        return Amount.ToString(formatter) + " KW";
    }

    public override bool Equals(object? obj)
    {
        return obj is KW kw && Equals(kw);
    }

    private bool Equals(KW other)
    {
        return Amount.Equals(other.Amount);
    }

    public override int GetHashCode()
    {
        return Amount.GetHashCode();
    }

    public static bool operator ==(KW left, KW right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(KW left, KW right)
    {
        return !(left == right);
    }
}
