﻿namespace WorkoutWotch.Models.Parsers
{
    using Kent.Boogaart.HelperTrinity.Extensions;
    using Sprache;
    using WorkoutWotch.Models.Actions;
    using WorkoutWotch.Services.Contracts.Delay;
    using WorkoutWotch.Services.Contracts.Speech;

    internal static class PrepareActionParser
    {
        public static Parser<PrepareAction> GetParser(
            IDelayService delayService,
            ISpeechService speechService)
        {
            delayService.AssertNotNull(nameof(delayService));
            speechService.AssertNotNull(nameof(speechService));

            return
                from _ in Parse.IgnoreCase("prepare")
                from __ in HorizontalWhitespaceParser.Parser.AtLeastOnce()
                from ___ in Parse.IgnoreCase("for")
                from ____ in HorizontalWhitespaceParser.Parser.AtLeastOnce()
                from duration in TimeSpanParser.Parser
                select new PrepareAction(
                    delayService,
                    speechService,
                    duration);
        }
    }
}