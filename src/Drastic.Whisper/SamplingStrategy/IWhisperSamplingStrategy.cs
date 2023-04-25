// Licensed under the MIT license: https://opensource.org/licenses/MIT

using Drastic.Whisper.Native;

namespace Drastic.Whisper.SamplingStrategy;

internal interface IWhisperSamplingStrategy
{
    public WhisperSamplingStrategy GetNativeStrategy();
}
