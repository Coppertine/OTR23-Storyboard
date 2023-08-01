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
    public class Transitions : StoryboardObjectGenerator
    {
        StoryboardLayer layer;
        OsbSprite flashPixel;
        public override void Generate()
        {
		    layer = GetLayer("");
            Flash(OsbEasing.OutExpo, 87333, 1000);
            Flash(OsbEasing.OutSine, 89999, 90415);
            Flash(OsbEasing.OutSine, 90499, 90999);
            Flash(OsbEasing.OutSine, 90999, 91665);

            Flash(OsbEasing.OutExpo, 103332, 200);
            Flash(OsbEasing.OutExpo, 108665, 500);
            


            Flash(OsbEasing.InSine, 71333,72833);
            Flash(OsbEasing.InExpo, 92665, 93499);


            Flash(OsbEasing.OutExpo, 183332,3000);
            Flash(OsbEasing.OutExpo, 195665,2000);
            Flash(OsbEasing.OutExpo, 199998,2000);
            Flash(OsbEasing.OutExpo, 203332,203593);
            Flash(OsbEasing.OutExpo, 203679,203979);
            Flash(OsbEasing.OutExpo, 204080,2000);

            Flash(OsbEasing.OutExpo, 121999,2000);
            Flash(OsbEasing.OutExpo, 113999,2000);
            Flash(OsbEasing.OutExpo, 124665,10000);
            Flash(OsbEasing.OutExpo, 177998,10000);



            Flash(OsbEasing.None, 295430, 1000);
            Flash(OsbEasing.OutExpo, 300763, 1000);
            Flash(OsbEasing.OutExpo, 303430, 1000);

            Flash(OsbEasing.OutExpo, 306096,3000);
            Flash(OsbEasing.OutExpo, 348763,3000);
            Flash(OsbEasing.OutExpo, 393130,10000);


        }

        void Flash(OsbEasing easing, double startTime, double duration)
        {
            double endTime = duration < startTime ? startTime + duration : duration;
            if(flashPixel == null)
            {
                flashPixel = layer.CreateSprite("sb/p.png");
                flashPixel.ScaleVec(startTime, 853, 480);
            }
            flashPixel.Fade(easing, startTime, endTime, 1, 0);
        }
    }
}
