//Author: FCORDT
using System.Globalization;

namespace DotNetSimulator.Units;

/// <summary>
/// represents KWH units, i.e. <see cref="KW"/> multiplied by <see cref="TimeSpan"/>
/// </summary>
public class KWH
{
    /// <summary>
    /// returns the numeric representation
    /// </summary>
    private double Amount { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="amount"></param>
    public KWH(double amount) { Amount = amount; }


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

    /// <inheritdoc/>
    public override string ToString()
    {
        var formatter = CultureInfo.CreateSpecificCulture("de-DE");
        return Amount.ToString(formatter) + " KWH";
    }
}