using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Vignette : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    
            var vignette = GetLayer("").CreateSprite("sb/vignette.png");
            vignette.ScaleVec(0, 480.0f / 1080, 480.0f / 1080);
            vignette.ScaleVec(159648, 787.0f / 1920, 480.0f / 1080);
            vignette.ScaleVec(159981, 480.0f / 1080, 480.0f / 1080);
            vignette.ScaleVec(160315, 787.0f / 1920, 480.0f / 1080);
            vignette.ScaleVec(160648, 480.0f / 1080, 480.0f / 1080);

            vignette.Fade(0,0.6);
            vignette.Fade(407062, 417971, 0.6,0);
        }
    }
}
