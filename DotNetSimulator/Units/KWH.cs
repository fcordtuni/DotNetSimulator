//Author: FCORDT
using System.Globalization;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("TestDotNetSimulator")]
namespace DotNetSimulator.Units;

/// <summary>
/// represents KWH units, i.e. <see cref="KW"/> multiplied by <see cref="TimeSpan"/>
/// </summary>
/// <remarks>
/// 
/// </remarks>
/// <param name="amount"></param>
public class KWH(double amount)
{
    /// <summary>
    /// returns the numeric representation
    /// </summary>
    internal double Amount { get; } = amount;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="kW"></param>
    /// <param name="time"></param>
    public KWH(KW kW, TimeSpan time) : this(kW.Amount * time.TotalHours)
    {
    }

    public static readonly KWH Infinity = new(double.PositiveInfinity);
    public static readonly KWH Zero = new(0);

    /// <summary>
    /// returns the maximum of the given values
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static KWH Max(KWH left, KWH right)
    {
        return left.Amount >= right.Amount ? left : right;
    }

    /// <summary>
    /// returns the minimum of the given values
    /// </summary>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns></returns>
    public static KWH Min(KWH left, KWH right)
    {
        return left.Amount <= right.Amount ? left : right;
    }

    public static KWH operator -(KWH left, KWH right)
    {
        return new KWH(left.Amount - right.Amount);
    }

    public static KWH operator +(KWH left, KWH right)
    {
        return new KWH(left.Amount + right.Amount);
    }

    public static KW operator /(KWH left, TimeStep right)
    {
        return new KW(left, right);
    }

    public static KWH operator *(KWH left, double right)
    {
        return new KWH(left.Amount * right);
    }

    public static KWH operator *(double left, KWH right)
    {
        return right * left;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        var formatter = CultureInfo.CreateSpecificCulture("de-DE");
        return Amount.ToString(formatter) + " KWH";
    }

    public override bool Equals(object? obj)
    {
        return obj is KWH kwh && Equals(kwh);
    }

    private bool Equals(KWH other)
    {
        return Amount.Equals(other.Amount);
    }

    public override int GetHashCode()
    {
        return Amount.GetHashCode();
    }
    public static bool operator ==(KWH left, KWH right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(KWH left, KWH right)
    {
        return !(left == right);
    }
}