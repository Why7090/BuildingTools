﻿
using System;
using BrilliantSkies.Core.Timing;
using BrilliantSkies.Core.Unity;
using BrilliantSkies.PlayerProfiles;
using UnityEngine;

namespace BuildingTools
{
    public static class Extensions
    {
        public static void SetIfNull<T>(this KeyMap<T> self, T input, KeyCode code, bool overrideExisting) =>
            self.SetIfNull(input, new KeyDef(code, KeyMod.None), overrideExisting);

        public static void SetIfNull<T>(this KeyMap<T> self, T input, KeyDef code, bool overrideExisting)
        {
            if (self.MapDictionary.TryGetValue(input, out var keyDef))
                if (overrideExisting || !keyDef.IsAssigned)
                    self.MapDictionary[input] = code;
            self.DefaultDictionary[input] = code;
        }

        public static GameEvents.DRegularEvent ToDRegularEvent(this KeyPressEvent self) => (ts) => self.CheckAndCallEvents();
    }
}
