﻿namespace WorkoutWotch.UnitTests.Models.Actions.Builders
{
    using System.Collections.Generic;
    using WorkoutWotch.Models;
    using WorkoutWotch.Models.Actions;

    internal sealed class ParallelActionBuilder
    {
        private readonly IList<IAction> children;

        public ParallelActionBuilder()
        {
            this.children = new List<IAction>();
        }

        public ParallelActionBuilder AddChild(IAction child)
        {
            this.children.Add(child);
            return this;
        }

        public ParallelAction Build() =>
            new ParallelAction(this.children);
        

        public static implicit operator ParallelAction(ParallelActionBuilder builder) =>
            builder.Build();
    }
}