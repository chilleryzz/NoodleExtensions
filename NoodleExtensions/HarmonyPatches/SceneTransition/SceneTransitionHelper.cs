﻿namespace NoodleExtensions.HarmonyPatches
{
    using System.Collections.Generic;
    using System.Linq;
    using CustomJSONData;
    using CustomJSONData.CustomBeatmap;
    using NoodleExtensions.Animation;
    using static NoodleExtensions.Plugin;

    internal static class SceneTransitionHelper
    {
        internal static void Patch(IDifficultyBeatmap difficultyBeatmap, PlayerSpecificSettings playerSpecificSettings)
        {
            if (difficultyBeatmap.beatmapData is CustomBeatmapData customBeatmapData)
            {
                IEnumerable<string> requirements = ((List<object>)Trees.at(customBeatmapData.beatmapCustomData, "_requirements"))?.Cast<string>();
                bool noodleRequirement = requirements?.Contains(CAPABILITY) ?? false;
                NoodleController.ToggleNoodlePatches(noodleRequirement);

                // Reset tracks when entering game scene
                Dictionary<string, Track> tracks = Trees.at(customBeatmapData.customData, "tracks");
                if (tracks != null)
                {
                    foreach (KeyValuePair<string, Track> track in tracks)
                    {
                        track.Value.ResetVariables();
                    }
                }
            }

            NoodleController.LeftHandedMode = playerSpecificSettings.leftHanded;
        }
    }
}
