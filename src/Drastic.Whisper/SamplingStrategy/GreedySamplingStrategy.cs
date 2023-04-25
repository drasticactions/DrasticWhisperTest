// Licensed under the MIT license: https://opensource.org/licenses/MIT

using Drastic.Whisper.Native;

namespace Drastic.Whisper.SamplingStrategy;

internal class GreedySamplingStrategy : IWhisperSamplingStrategy
{
    public WhisperSamplingStrategy GetNativeStrategy()
    {
        return WhisperSamplingStrategy.StrategyGreedy;
    }

    public int? BestOf { get; set; }
}
