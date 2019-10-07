using UnityEngine;

public class StopWatch : System.IDisposable
{
    public string text { get; set; }
    private System.Diagnostics.Stopwatch m_StopWatch = new System.Diagnostics.Stopwatch();
    public System.Diagnostics.Stopwatch stopWatch { get { return m_StopWatch; } }

    public StopWatch(string _text)
    {
        text = _text;
        stopWatch.Start();
    }

    public void Dispose()
    {
        stopWatch.Stop();
        var elapsed = (float)stopWatch.Elapsed.TotalSeconds;
        Debug.LogFormat("{0} Time:{1}", text, elapsed);
    }
}
