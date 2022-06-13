using System;
public class RTSJob
{
    public string Name { get; set; }
    public int Priority { get; set; }
    public int Duration { get; set; }
    public System.Action Execute { get; set; }
    public int FramePeriod { get; set; } = 1;
}
