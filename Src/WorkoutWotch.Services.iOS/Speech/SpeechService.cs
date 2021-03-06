namespace WorkoutWotch.Services.iOS.Speech
{
    using System;
    using System.Reactive;
    using System.Reactive.Linq;
    using System.Threading;
    using AVFoundation;
    using Kent.Boogaart.HelperTrinity.Extensions;
    using WorkoutWotch.Services.Contracts.Speech;

    public sealed class SpeechService : ISpeechService
    {
        private static readonly AVSpeechSynthesisVoice voice = AVSpeechSynthesisVoice.FromLanguage("en-AU");

        public IObservable<Unit> SpeakAsync(string speechString, CancellationToken cancellationToken = default(CancellationToken))
        {
            speechString.AssertNotNull(nameof(speechString));

            var utterance = new AVSpeechUtterance(speechString)
            {
                Voice = voice,
                Rate = 0.55f
            };
            var synthesizer = new AVSpeechSynthesizer();
            var finishedUtterance = Observable
                .FromEventPattern<AVSpeechSynthesizerUteranceEventArgs>(x => synthesizer.DidFinishSpeechUtterance += x, x => synthesizer.DidFinishSpeechUtterance -= x)
                .Select(_ => Unit.Default)
                .Publish();

            finishedUtterance
                .Subscribe(
                    _ =>
                    {
                        utterance.Dispose();
                        synthesizer.Dispose();
                    });

            if (cancellationToken.CanBeCanceled)
            {
                cancellationToken.Register(() => synthesizer.StopSpeaking(AVSpeechBoundary.Immediate));

                Observable
                    .FromEventPattern<AVSpeechSynthesizerUteranceEventArgs>(x => synthesizer.DidCancelSpeechUtterance += x, x => synthesizer.DidCancelSpeechUtterance -= x)
                    .Select(_ => Unit.Default)
                    .Subscribe(
                        _ =>
                        {
                            utterance.Dispose();
                            synthesizer.Dispose();
                        });
            }

            synthesizer.SpeakUtterance(utterance);
            finishedUtterance.Connect();

            return finishedUtterance
                .FirstAsync()
                .RunAsync(cancellationToken);
        }
    }
}