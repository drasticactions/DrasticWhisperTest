// Licensed under the MIT license: https://opensource.org/licenses/MIT

namespace Drastic.Whisper.Internals.ModelLoader;

internal interface IWhisperProcessorModelLoader : IDisposable
{
    public IntPtr LoadNativeContext();
}
