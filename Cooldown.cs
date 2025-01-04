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
    public class Cooldown : StoryboardObjectGenerator
    {
        public override void Generate()
        {


            int startTime = 218977;
            int endTime = 295413;
            var FadeDuration = 200;
            var MaxOpacity = 0.1;
            var Amount = 45;
            var PerOne = 6;
            Vector2 Scale = new Vector2(2,2);
            Vector2 BorderMin = new Vector2(-107, 0);
            Vector2 BorderMax = new Vector2(747, 480);

            OsbSprite bg = GetLayer("").CreateSprite("sb/p.png");
            bg.ScaleVec(startTime - FadeDuration, 854, 480);
            bg.Color(startTime - FadeDuration, Color4.DarkMagenta);
            bg.Fade(startTime - FadeDuration, startTime, 0, 0.4);
            bg.Fade(endTime - FadeDuration, endTime, 0.4, 0);

            using (var pool = new OsbSpritePool(GetLayer(""), "sb/smoke.png", OsbOrigin.Centre, true))
                for (var s = 0; s < Amount; s++)
                {
                    var RandomPos = new Vector2(Random(BorderMin.X, BorderMax.X), Random(BorderMin.Y, BorderMax.Y));
                    for (var c = 0; c < PerOne; c++)
                    {
                        var sprite = pool.Get(startTime - FadeDuration, endTime);
                        var RandomOffset = new Vector2(Random(-25, 25), Random(-25, -25));
                        sprite.Move(startTime - FadeDuration, endTime, RandomPos + RandomOffset, (RandomPos + RandomOffset - new Vector2(320, 240)) * Random(1.25f, 2f) + new Vector2(320, 240));

                        // var RandomWhite = Random(190, 255) / 255f;
                        Color4[] colors = { Color4.Red,Color4.DarkGray, Color4.Purple, Color4.DarkGreen, Color4.Coral, Color4.DarkGoldenrod };

                        sprite.Color(startTime - FadeDuration, colors[Random(0, colors.Count())]);
                        var opacity = Random(0.05f, MaxOpacity);
                        if (startTime != 85) sprite.Fade(startTime - FadeDuration, startTime, 0, opacity);
                        else
                        {
                            sprite.Fade(0, 0);
                            sprite.Fade(startTime, opacity);

                        }
                        var ranScale = new Vector2(Random(Scale.X * 0.25f, Scale.X), Random(Scale.Y * 0.25f, Scale.Y));
                        sprite.ScaleVec(startTime - FadeDuration, endTime, ranScale, ranScale * Random(1.25f, 2f));
                        sprite.Rotate(startTime - FadeDuration, Random(0, 2 * Math.PI));
                        sprite.Fade(endTime - FadeDuration, endTime, opacity, 0);
                    }
                }

            OsbSprite fadeOutOverlay = GetLayer("Foreground").CreateSprite("sb/p.png");
            fadeOutOverlay.ScaleVec(endTime - FadeDuration, 853,480);
            fadeOutOverlay.Color(endTime - FadeDuration, Color4.Black);
            fadeOutOverlay.Fade(endTime - FadeDuration, endTime, 0, 1);
            fadeOutOverlay.Fade(295430, 0);

            // todo: lyrics, make them vertical with fading in characters
        }
    }
}
