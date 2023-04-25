using Drastic.Whisper.Models;

namespace Drastic.Whisper.Services
{
    public class DefaultWhisperService : IWhisperService, IDisposable
    {
        private bool disposedValue;
        private WhisperFactory? factory;
        private WhisperProcessor? processor;

        public event EventHandler<OnNewSegmentEventArgs>? OnNewWhisperSegment;

        public bool IsInitialized => this.processor is not null;

        public bool IsIndeterminate => true;

        public Action<double>? OnProgress { get; set; }

        void IDisposable.Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private void OnNewSegment(SegmentData e)
        {
            this.OnNewWhisperSegment?.Invoke(this, new OnNewSegmentEventArgs(new Models.WhisperSegmentData(e.Text, e.Start, e.End, e.MinProbability, e.MaxProbability, e.Probability, e.Language)));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.factory?.Dispose();
                    this.processor?.Dispose();
                }

                this.disposedValue = true;
            }
        }

        public void InitModel(string path)
        {
            this.factory = WhisperFactory.FromPath(path);
            this.processor = this.factory.CreateBuilder()
                    .WithLanguage("auto")
                    .WithSegmentEventHandler(this.OnNewSegment)
                    .Build();
        }

        public void InitModel(byte[] buffer)
        {
            int max_threads = Math.Min(8, Environment.ProcessorCount);
            this.factory = WhisperFactory.FromBuffer(buffer);
            this.processor = this.factory.CreateBuilder().WithGreedySamplingStrategy()
                    .ParentBuilder
                    .WithPrintProgress()
                    .WithPrintResults()
                    .WithPrintSpecialTokens()
                    .WithPrintTimestamps()
                    .WithLanguage("auto")
                    .WithThreads(max_threads)
                    .WithNoContext()
                    .WithSingleSegment()
                    .WithSegmentEventHandler(this.OnNewSegment)
                    .Build();
        }

        public Task ProcessAsync(string filePath, WhisperLanguage language, CancellationToken? cancellationToken = null)
        {
            ArgumentNullException.ThrowIfNull(this.processor);

            return Task.Run(
                () =>
                {
                    using var fileStream = File.OpenRead(filePath);
                    this.processor.Process(fileStream);
                },
                cancellationToken ?? CancellationToken.None);
        }


        public Task ProcessAsync(byte[] buffer, WhisperLanguage lang, CancellationToken? cancellationToken = null)
        {
            return this.ProcessAsync(new MemoryStream(buffer), lang, cancellationToken);
        }

        public Task ProcessAsync(Stream stream, WhisperLanguage lang, CancellationToken? cancellationToken = null)
        {
            ArgumentNullException.ThrowIfNull(this.processor);

            return Task.Run(
                () =>
                {
                    this.processor.Process(stream);
                },
                cancellationToken ?? CancellationToken.None);
        }
    }
}

