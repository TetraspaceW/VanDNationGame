public class ScaleUtil
{
    public int scale;
    public ScaleUtil(int scale)
    {
        this.scale = scale;
    }

    string[] numberNames = { "", " thousand", " million", " billion", " trillion", " quadrillion", " pentillion", " sextillion", " septillion", " octillion", " nonillion", };

    public string TextForScale()
    {
        try
        {
            if (scale < -40)
            {
                // let's get a little silly
                return NumberStringForScale(scale + 51) + " lₚ";
            }
            else if (scale < -26)
            {
                // 10^40 Å = 1 light year
                return NumberStringForScale(scale + 40) + " ym";
            }
            else if (scale < -13)
            {
                // 10^26 Å = 1 light year
                return NumberStringForScale(scale + 26) + " Å";
            }
            else if (scale < 0)
            {
                // 10^13 km = 1 light year
                return NumberStringForScale(scale + 13) + " km";
            }
            else
            {
                return NumberStringForScale(scale) + " ly";
            }
        }
        catch
        {
            return "10^" + scale + " ly";
        }
    }

    private string NumberStringForScale(int value)
    {
        return "1" + new string('0', value % 3) + numberNames[(value / 3)];
    }
}