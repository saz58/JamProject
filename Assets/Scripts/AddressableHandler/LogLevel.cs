[System.Flags]
public enum LogLevel : byte
{
    None = 0,
    Info = 1,
    Warn = 2,
    Error = 4,
    All = Info | Warn | Error
}

public static class LogLevelExt
{
    public static bool Contains(this LogLevel level, LogLevel value)
    {
        return (level & value) != LogLevel.None;
    }
}