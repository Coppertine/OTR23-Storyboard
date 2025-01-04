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
    public class Outro : StoryboardObjectGenerator
    {
        public override void Generate()
        {
		    OsbSprite introbg = GetLayer("").CreateSprite("sb/p.png");
            introbg.ScaleVec(393130,854,480);
            introbg.Fade(393130, 1); 
            introbg.Color(393130, "#111320");
            introbg.Fade(407062, 417971,1, 0);
            // introbg.Fade(71316,0);
            OsbSprite glow = GetLayer("").CreateSprite("sb/g.png", OsbOrigin.CentreLeft, new Vector2(320, 480));
            glow.Rotate(393130, -Math.PI / 2);
            glow.Fade(OsbEasing.InOutQuad, 393130, 396308, 0, 0.4);
            glow.Fade(OsbEasing.InOutQuad, 396308, 407062, 0.4, 0.2);
            glow.Additive(393130);
            Vector2 glowStartScale = new Vector2(0.5f, 9);
            Vector2 glowEndScale = new Vector2(0.65f, 9);
            glow.StartLoopGroup(393130, (int)((417971 - 393130) / (int)Beatmap.GetTimingPointAt(393130).BeatDuration / 8) + 1);
            glow.ScaleVec(OsbEasing.InOutQuad, 0, Beatmap.GetTimingPointAt(393130).BeatDuration * 4, glowStartScale, glowEndScale);
            glow.ScaleVec(OsbEasing.InOutQuad, Beatmap.GetTimingPointAt(393130).BeatDuration * 4, Beatmap.GetTimingPointAt(393130).BeatDuration * 8, glowEndScale, glowStartScale);
            glow.EndGroup();
            glow.Fade(407062, 417971,0.2, 0);
        }
    }
}
