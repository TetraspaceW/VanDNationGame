public class ScaleUtil
{
    public int scale;
    public ScaleUtil(int scale)
    {
        this.scale = scale;
    }

    string[] numbersDict = { "", " thousand", " million", " billion", " trillion", " quadrillion" };

    public string TextForScale()
    {
        try
        {
            if (scale < 0)
            {
                return NumberStringForScale(scale + 13) + " km";
            }
            else
            {
                return NumberStringForScale(scale) + " light years";
            }
        }
        catch
        {
            return "10^" + scale + " light years";
        }
    }

    private string NumberStringForScale(int value)
    {
        return "1" + new string('0', value % 3) + numbersDict[(value / 3)];
    }
}