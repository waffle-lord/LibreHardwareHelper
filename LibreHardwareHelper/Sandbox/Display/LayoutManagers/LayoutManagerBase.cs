using Spectre.Console;

namespace Sandbox.Display.LayoutManagers;

internal enum ColorRange
{
    None,
    Low,
    Medium,
    High
}

internal class LayoutManagerBase
{
    private ColorRange GetPercentRangeValue(float percent)
    {
        switch (percent)
        {
            case float p when p > 100:
                return ColorRange.None;
            case float p when p > 80:
                return ColorRange.High;
            case float p when p > 50:
                return ColorRange.Medium;
            default:
                return ColorRange.Low;
        }
    }

    public string GetPercentColorString(float percent)
    {
        var valueRange = GetPercentRangeValue(percent);

        switch (valueRange)
        {
            case ColorRange.Low:
                return $"[blue]{percent.ToString("0.0").EscapeMarkup()}[/]";

            case ColorRange.Medium:
                return $"[yellow]{percent.ToString("0.0").EscapeMarkup()}[/]";

            case ColorRange.High:
                return $"[red]{percent.ToString("0.0").EscapeMarkup()}[/]";

            case ColorRange.None:
            default:
                return $"[grey]{percent.ToString("0.0").EscapeMarkup()}[/]";
        }
    }

    public Color GetPercentColor(float percent)
    {
        var valueRange = GetPercentRangeValue(percent);

        switch (valueRange)
        {
            case ColorRange.Low:
                return Color.Blue;

            case ColorRange.Medium:
                return Color.Yellow;

            case ColorRange.High:
                return Color.Red;

            case ColorRange.None:
            default:
                return Color.Grey;
        }
    }

    public string GetTempColorString(float temp)
    {
        switch (temp)
        {
            case float t when t > 80:
                return $"[red]{temp.ToString("0.0").EscapeMarkup()}[/]";
            case float t when t > 50:
                return $"[yellow]{temp.ToString("0.0").EscapeMarkup()}[/]";
            default:
                return $"[blue]{temp.ToString("0.0").EscapeMarkup()}[/]";
        }
    }
}