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
    public class Logo : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            double startScale = 1;
            double scale = 0.5;
            double endTime = 295430;

		    OsbSprite backLayerSprite = GetLayer("Background").CreateSprite("sb/logo/open-2023.png");
            backLayerSprite.Scale(OsbEasing.OutExpo, 292763, 293180, startScale, scale);
            backLayerSprite.Fade(292763, 1);
            backLayerSprite.Fade(endTime, 0);

            OsbSprite unlit = GetLayer("Background").CreateSprite("sb/logo/turkiye-unlit.png");
            unlit.Scale(294430, scale);
            unlit.Fade(294430, 1);
            unlit.Fade(endTime, 0);

            OsbSprite turkiye = GetLayer("Background").CreateSprite("sb/logo/turkiye.png");
            turkiye.Scale(OsbEasing.OutExpo, 293263, 293680, startScale, scale);
            turkiye.Fade(293263, 1);
            var flashingDivisor = 4;
            turkiye.StartLoopGroup(294430, 12);
            turkiye.Fade(0, Beatmap.GetTimingPointAt(294430).BeatDuration / (flashingDivisor * 2), 1, 0);
            turkiye.Fade(Beatmap.GetTimingPointAt(294430).BeatDuration / (flashingDivisor * 2), Beatmap.GetTimingPointAt(294430).BeatDuration / flashingDivisor, 0, 1);
            turkiye.EndGroup();

            OsbSprite osu = GetLayer("Background").CreateSprite("sb/logo/osu.png");
            osu.Scale(OsbEasing.OutExpo, 293763, 294013, startScale, scale);
            osu.Fade(293763, 1);
            osu.Fade(endTime, 0);
            
            backLayerSprite.Scale(OsbEasing.InExpo, endTime - 500, endTime, scale, startScale * 2);
            unlit.Scale(OsbEasing.InExpo, endTime - 500, endTime, scale, startScale * 2);
            turkiye.Scale(OsbEasing.InExpo, endTime - 500, endTime, scale, startScale * 2);
            osu.Scale(OsbEasing.InExpo, endTime - 500, endTime, scale, startScale * 2);
            
        }
    }
}
