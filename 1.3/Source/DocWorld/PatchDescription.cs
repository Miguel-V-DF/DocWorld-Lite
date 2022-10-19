namespace DocWorldLite;

public struct PatchDescription
{
    public string file;
    public string label;
    public string tooltip;

    public PatchDescription(string file, string label, string tooltip = null)
    {
        this.file = file;
        this.label = label;
        this.tooltip = tooltip;
    }
}