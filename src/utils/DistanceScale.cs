
public partial class DistanceScale
{
    private static string[] numberNames = { "", " thousand", " million", " billion", " trillion", " quadrillion", " pentillion", " sextillion", " septillion", " octillion", " nonillion", };
    private static (int, string)[] units = {
            (-51, "lp"),
            (-40, "ym"), (-37, "zm"), (-34, "am"), (-31, "fm"), (-26, "Å"), (-22, "um"), (-19, "mm"), (-16, "m"),
            (-13, "km"), (-5, "AU"), (0, "ly"),
            (int.MaxValue, null)
    };

    public static string TextForScale(int scale)
    {
        string scaleString = "10^" + scale + " ly";
        for (int i = 0; i < units.Length - 1; i++)
        {
            if (scale >= units[i].Item1 && scale < units[i + 1].Item1)
            {
                string numberString = NumberStringForScale(scale - units[i].Item1);
                if (numberString != null) { scaleString = numberString + " " + units[i].Item2; }
            }
        }
        return scaleString;
    }

    private static string NumberStringForScale(int value)
    {
        return (value / 3) < numberNames.Length ? "1" + new string('0', value % 3) + numberNames[(value / 3)] : null;
    }
}