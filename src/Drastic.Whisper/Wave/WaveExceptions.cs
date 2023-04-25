// Licensed under the MIT license: https://opensource.org/licenses/MIT

namespace Drastic.Whisper.Wave;

public class CorruptedWaveException : Exception
{
    public CorruptedWaveException(string? message) : base(message)
    {
    }
}

public class NotSupportedWaveException : Exception
{
    public NotSupportedWaveException(string? message) : base(message)
    {
    }
}
