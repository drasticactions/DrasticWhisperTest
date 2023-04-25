// Licensed under the MIT license: https://opensource.org/licenses/MIT

namespace Drastic.Whisper;

public interface IWhisperSamplingStrategyBuilder
{
    /// <summary>
    /// Returns the parent <seealso cref="WhisperProcessorBuilder"/>.
    /// </summary>
    WhisperProcessorBuilder ParentBuilder { get; }
}
