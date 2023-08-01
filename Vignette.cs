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
            vignette.Scale(0, 480.0f / 1080);
            vignette.Fade(0,1);
            vignette.Fade(407062, 417971, 1,0); 
        }
    }
}
