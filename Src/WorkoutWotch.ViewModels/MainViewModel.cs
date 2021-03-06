﻿namespace WorkoutWotch.ViewModels
{
    using System;
    using Kent.Boogaart.HelperTrinity.Extensions;
    using ReactiveUI;

    public sealed class MainViewModel : ReactiveObject, IScreen
    {
        private readonly Func<ExerciseProgramsViewModel> exerciseProgramsViewModelFactory;
        private readonly RoutingState router;

        public MainViewModel(
            Func<ExerciseProgramsViewModel> exerciseProgramsViewModelFactory)
        {
            exerciseProgramsViewModelFactory.AssertNotNull(nameof(exerciseProgramsViewModelFactory));

            this.router = new RoutingState();
            this.exerciseProgramsViewModelFactory = exerciseProgramsViewModelFactory;
        }

        public RoutingState Router => this.router;

        public void Initialize() =>
            // TODO: navigating here ends up showing the view twice. Probably some whackiness in the iOS RoutedViewHost implementation
            //this.router.NavigateAndReset.Execute(exerciseProgramsViewModelFactory());
            this.router.NavigationStack.Add(exerciseProgramsViewModelFactory());
    }
}