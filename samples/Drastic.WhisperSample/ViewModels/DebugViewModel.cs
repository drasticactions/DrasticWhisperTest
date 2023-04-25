using System;
using System.Collections.ObjectModel;
using Drastic.Tools;
using Drastic.ViewModels;
using Drastic.Whisper.Models;
using Drastic.Whisper.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Drastic.WhisperSample.ViewModels
{
    public class DebugViewModel : BaseViewModel
    {
        private Whisper.WhisperFactory factory;
        private WhisperLanguage selectedLanguage;
        private IWhisperService whisper;
        private double progress;

        public DebugViewModel(IServiceProvider services)
            : base(services)
        {
            this.whisper = this.Services.GetRequiredService<IWhisperService>()!;
            this.whisper.OnProgress = this.OnProgress;
            this.whisper.OnNewWhisperSegment += this.Whisper_OnNewWhisperSegment;
            this.TranscribeCommand = new AsyncCommand(this.TranscribeAsync, null, this.Dispatcher, this.ErrorHandler);

            this.WhisperLanguages = WhisperLanguage.GenerateWhisperLangauages();
            this.selectedLanguage = this.WhisperLanguages[0];
        }

        public double Progress
        {
            get
            {
                return this.progress;
            }

            set
            {
                this.SetProperty(ref this.progress, value);
            }
        }

        /// <summary>
        /// Gets the subtitles.
        /// </summary>
        public ObservableCollection<ISubtitleLine> Subtitles { get; } = new ObservableCollection<ISubtitleLine>();

        public IReadOnlyList<WhisperLanguage> WhisperLanguages { get; }

        public AsyncCommand TranscribeCommand { get; }

        public void OnProgress(double progress) => this.Progress = progress * 100;


        public WhisperLanguage SelectedLanguage
        {
            get
            {
                return this.selectedLanguage;
            }

            set
            {
                this.SetProperty(ref this.selectedLanguage, value);
                this.RaiseCanExecuteChanged();
            }
        }

        private async Task TranscribeAsync()
        {
            this.whisper.InitModel(EmbeddedModels.LoadTinyModel());
            var testing = Samples.LoadJFK();
            await this.whisper.ProcessAsync(testing, this.SelectedLanguage, CancellationToken.None);
        }

        private void Whisper_OnNewWhisperSegment(object? sender, OnNewSegmentEventArgs segment)
        {
            var e = segment.Segment;
            System.Diagnostics.Debug.WriteLine($"CSSS {e.Start} ==> {e.End} : {e.Text}");
            var item = new SrtSubtitleLine() { Start = e.Start, End = e.End, Text = e.Text, LineNumber = this.Subtitles.Count() + 1 };
            this.Dispatcher.Dispatch(() => this.Subtitles.Add(item));
        }
    }
}

