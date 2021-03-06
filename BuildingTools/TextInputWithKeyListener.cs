﻿using System;
using System.Collections.Generic;
using BrilliantSkies.Core;
using BrilliantSkies.Core.Timing;
using BrilliantSkies.Core.Unity;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Texts;
using BrilliantSkies.Ui.Consoles.Styles;
using BrilliantSkies.Ui.Tips;
using UnityEngine;

namespace BuildingTools
{
    public class TextInputWithKeyListener<T> : TextInput<T>
    {
        private IEnumerable<Action<ITimeStep>> events;

        public TextInputWithKeyListener(T subject, IVS<string, T> fnGetStringCurrently, IVS<string, T> displayString,
            IVS<IToolTip, T> toolTip, Action<T, string> actionToDo, IEnumerable<Action<ITimeStep>> events, Func<T, string, string> effectOfAction,
            Func<string, string> stringCleaner, Func<T, string, string> stringChecker, params string[] keys) :
            base(subject, fnGetStringCurrently, displayString, toolTip, actionToDo, effectOfAction, stringCleaner, stringChecker, keys)
        {
            this.events = events;
        }

        public static TextInputWithKeyListener<T> Quick(T subject, IVS<string, T> getString, string label, ToolTip tip,
            Action<T, string> changeAction, params Action<ITimeStep>[] events)
        {
            return new(subject, getString, M.m<T>(label), M.m<T>(tip), changeAction, events, null, s => s, (x, s) => null);
        }

        public override void Draw(SO_BuiltUi styles)
        {
            foreach (var ev in events)
                ev(new TimeStep(Time.deltaTime));

            base.Draw(styles);
        }
    }
}
