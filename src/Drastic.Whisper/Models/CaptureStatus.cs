using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Drastic.Whisper.Models
{
    public enum CaptureStatus
    {
        Listening = 1,
        Voice = 2,
        Transcribing = 4,
        Stalled = 0x80,
    }

    public static class CaptureStatusExtensions
    {
        public static string ToTranslatedString(this CaptureStatus status)
        {
            switch (status)
            {
                case CaptureStatus.Listening:
                    return Translations.Common.Listening;
                case CaptureStatus.Voice:
                    return Translations.Common.Voice;
                case CaptureStatus.Transcribing:
                    return Translations.Common.Transcribing;
                case CaptureStatus.Stalled:
                    return Translations.Common.Stalled;
                default:
                    return string.Empty;
            }
        }
    }
}
