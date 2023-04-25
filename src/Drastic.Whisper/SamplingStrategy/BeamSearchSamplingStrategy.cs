// Licensed under the MIT license: https://opensource.org/licenses/MIT

using Drastic.Whisper.Native;

namespace Drastic.Whisper.SamplingStrategy;

internal class BeamSearchSamplingStrategy : IWhisperSamplingStrategy
{
    public WhisperSamplingStrategy GetNativeStrategy()
    {
        return WhisperSamplingStrategy.StrategyBeamSearch;
    }

    public int? BeamSize { get; set; }

    public float? Patience { get; set; }
}
