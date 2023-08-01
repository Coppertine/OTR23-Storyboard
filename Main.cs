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
    public class Main : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            RemoveBackground();
            GenerateIntro();

        }

        public void RemoveBackground() => GetLayer("").CreateSprite(Beatmap.BackgroundPath).Fade(0, 0);
        /**

        **/
        public void GenerateIntro()
        {
            StoryboardLayer layer = GetLayer("Intro");
            OsbSprite background = layer.CreateSprite(Beatmap.BackgroundPath);
            background.Scale(71333, (480.0f / 1423) * 1.25);
            background.Fade(71333, 71833, 0, 1);
            background.Fade(87333, 0);
            float currRot = 0;
            Vector2 currPos = new Vector2(320, 240);
            double currTime = 71333;
            Beatmap.ForEachTick(71333, 89999, 1, (point, time, beat, tick) =>
            {
                if (beat % 8 == 0)
                {
                    Vector2 pos = new Vector2(
                        320 + Random(-10, 10),
                        240 + Random(-10, 10)
                    );
                    background.Move(currTime, time, currPos, pos);
                    float rot = (float)Random(MathHelper.DegreesToRadians(-5), MathHelper.DegreesToRadians(5));
                    background.Rotate(currTime, time, currRot, rot);
                    currPos = pos;
                    currTime = time;
                    currRot = rot;
                }
            });


            OsbSprite glow = layer.CreateSprite("sb/g.png", OsbOrigin.CentreLeft, new Vector2(320, 480));
            glow.Rotate(48, -Math.PI / 2);
            glow.Fade(OsbEasing.InOutQuad, 48, 1000, 0, 0.8);
            glow.Additive(48);
            glow.Color(48, new Color4(255, 216, 117, 255));
            Vector2 glowStartScale = new Vector2(0.5f, 9);
            Vector2 glowEndScale = new Vector2(0.65f, 9);
            glow.StartLoopGroup(48, (int)((87333 - 48) / (int)Beatmap.GetTimingPointAt(48).BeatDuration / 8) + 1);
            glow.ScaleVec(OsbEasing.InOutQuad, 0, Beatmap.GetTimingPointAt(48).BeatDuration * 4, glowStartScale, glowEndScale);
            glow.ScaleVec(OsbEasing.InOutQuad, Beatmap.GetTimingPointAt(48).BeatDuration * 4, Beatmap.GetTimingPointAt(48).BeatDuration * 8, glowEndScale, glowStartScale);
            glow.EndGroup();
            glow.Fade(87333, 0);


            // Black White bg
            OsbSprite bw = layer.CreateSprite("sb/bw.jpg");
            bw.Fade(87333, 0.5);
            bw.Scale(87333, 480.0f / 1423);
            bw.Fade(89999,0);

            OsbSprite noise = layer.CreateAnimation("sb/noise/.jpg", 3, 50, OsbLoopType.LoopForever);
            noise.Fade(87333, 1);
            noise.Additive(87333);
            noise.Fade(89999,0);

            OsbSprite backgroundCloseups = layer.CreateSprite(Beatmap.BackgroundPath, OsbOrigin.Centre, new Vector2(0,0));
            backgroundCloseups.Scale(89999, (480.0f / 1423) * 2);
            backgroundCloseups.Fade(OsbEasing.OutExpo, 89999, 90499, 1, 0.8);
            backgroundCloseups.Move(OsbEasing.OutExpo, 89999, 90499, new Vector2(580, 250), new Vector2(550, 270));
            backgroundCloseups.Move(OsbEasing.OutExpo, 90499, 90999, new Vector2(100, 200), new Vector2(50, 270));
            backgroundCloseups.Scale(OsbEasing.OutCubic, 90999, 92665, (480.0f / 1423) * 2,(480.0f / 1423));
            backgroundCloseups.Move(90999, new Vector2(320, 240));

            backgroundCloseups.Fade(92665, 0);
        }
    }
}
