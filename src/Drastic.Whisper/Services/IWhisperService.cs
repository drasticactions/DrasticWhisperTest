using System;
using Drastic.Whisper.Models;

namespace Drastic.Whisper.Services
{
    public interface IWhisperService
    {
        event EventHandler<OnNewSegmentEventArgs>? OnNewWhisperSegment;

        bool IsInitialized { get; }

        Task ProcessAsync(string filePath, WhisperLanguage lang, CancellationToken? cancellationToken = default);

        Task ProcessAsync(byte[] buffer, WhisperLanguage lang, CancellationToken? cancellationToken = default);

        Task ProcessAsync(Stream stream, WhisperLanguage lang, CancellationToken? cancellationToken = default);

        void InitModel(string path);

        void InitModel(byte[] buffer);

        bool IsIndeterminate { get; }

        public Action<double>? OnProgress { get; set; }
    }

    public class OnNewSegmentEventArgs : EventArgs
    {
        public OnNewSegmentEventArgs(WhisperSegmentData segmentData)
        {
            this.Segment = segmentData;
        }

        public WhisperSegmentData Segment { get; }
    }

    public class OnCaptureStatusChangedEventArgs : EventArgs
    {
        public OnCaptureStatusChangedEventArgs(CaptureStatus status)
        {
            this.CaptureStatus = status;
        }

        public CaptureStatus CaptureStatus { get; }
    }
}

