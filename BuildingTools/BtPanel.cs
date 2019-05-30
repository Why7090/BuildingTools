﻿using BrilliantSkies.Core.SteamworksIntegration;
using BrilliantSkies.PlayerProfiles;
using BrilliantSkies.Ui.Consoles;
using BrilliantSkies.Ui.Consoles.Getters;
using BrilliantSkies.Ui.Consoles.Interpretters.Simple;
using BrilliantSkies.Ui.Consoles.Interpretters.Subjective.Choices;
using BrilliantSkies.Ui.Consoles.Styles;
using BrilliantSkies.Ui.Tips;
using Harmony;
using Ui.Consoles.Examples;
using UnityEngine;

namespace BuildingTools
{
    public class BtPanel : KeyMappingUi<KeyInputsBt>
    {
        public override Content Name => new Content("BT Settings", "Change the behaviour of the BuildingTools mod");

        public BtPanel(ConsoleWindow window) : base(window, ProfileManager.Instance.GetModule<BtKeyMap>(), 3, x => (KeyInputsBt)x) { }

        public override void Build()
        {
            CreateHeader("Settings", new ToolTip("BuildingTools mod settings"));
            var data = BtSettings.Data;
            var seg = CreateTableSegment(2, 2);

            seg.AddInterpretter(SubjectiveToggle<BtSettings.InternalData>.Quick(data,
                "Disable CapsLock",
                new ToolTip("Automagically revert CapsLock when you press it, so you won't accidentally type cAPITALIZED lETTERS"),
                (x, val) => x.DisableCapsLock = val,
                x => x.DisableCapsLock));
            seg.AddInterpretter(SubjectiveToggle<BtSettings.InternalData>.Quick(data,
                "Enable changelog report",
                new ToolTip("After each update, show a list of new changes made to FtD at start, if any"),
                (x, val) => x.EnableNewFeaturesReport = val,
                x => x.EnableNewFeaturesReport));
            seg.AddInterpretter(SubjectiveToggle<BtSettings.InternalData>.Quick(data,
                "Disable skills in designer",
                new ToolTip("Ignore all skill levels in designer mode, to stop you from thinking that you're building overpowered crafts when they are garbage in reality", 330),
                (x, val) => {
                    x.DisableSkillsInDesigner = val;
                    BuildingToolsPlugin.RefreshSkills();
                },
                x => x.DisableSkillsInDesigner));
            seg.AddInterpretter(SubjectiveToggle<BtSettings.InternalData>.Quick(data,
                "Invincible character in designer",
                new ToolTip("Prevent the rambot from receiving any damage", 210),
                (x, val) => {
                    x.CharacterInvincibilityInDesigner = val;
                    BuildingToolsPlugin.RefreshSkills();
                },
                x => x.CharacterInvincibilityInDesigner));

            CreateHeader("Key Bindings", new ToolTip("Configure key bindings for BuildingTools"));
            base.Build();

            CreateSpace();
            WriteLink("BuildingTools GitHub repository", "https://github.com/Why7090/BuildingTools", "Introduction and latest update of the mod");
            WriteLink("Official FtD Discord server", "https://discord.gg/8DS4P8V", "Ping me (WengH) if you have questions or issues with my mod");
        }

        protected void WriteLink(string text, string url, string description = null)
        {
            CreateStandardSegment(InsertPosition.OnCursor).AddInterpretter(Button.Quick(
                string.Format("<i><b>{0}</b> ({1})</i>", text, url),
                description == null ? null : new ToolTip(description),
                () => new SteamInterface().__OpenUrl(url)))
                .Style = M.m<StylePlus>(ConsoleStyles.Instance.Styles.Display.DisplayText);
        }
    }
}
