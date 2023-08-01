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
    public class Hitobjects : StoryboardObjectGenerator
    {
        OsbSpritePool ringPool, particlePool, highlightPool;
        public override void Generate()
        {
            particlePool = new OsbSpritePool(GetLayer("Particles"), "sb/dot.png", OsbOrigin.Centre);
            particlePool.MaxPoolDuration = 5000;
            ringPool = new OsbSpritePool(GetLayer("Rings"), "sb/ring.png", OsbOrigin.Centre);
            ringPool.MaxPoolDuration = 5000;

            highlightPool = new OsbSpritePool(GetLayer("Highlight"), "sb/light.png", OsbOrigin.Centre);
            highlightPool.MaxPoolDuration = 5000;


            foreach (OsuHitObject hitObject in Beatmap.HitObjects)
            {
                // Fuck spinners //
                if (hitObject is OsuSpinner)
                    continue;

                // I wish there was a good way to see if an object is in a range or smth..
                // Edit, there is.. it's just shit.. let's go old fasioned..

                // Drop 1
                if (103332 <= hitObject.StartTime && hitObject.StartTime <= 123332)
                {
                    // Let's do Finishes for now
                    if (hitObject.Additions.HasFlag(HitSoundAddition.Finish))
                    {
                        Ring(OsbEasing.OutExpo, hitObject, 1000);
                        Particles(OsbEasing.OutExpo, hitObject, 1000);

                    }
                    if (hitObject is OsuSlider)
                    {
                        OsuSlider slider = (OsuSlider)hitObject;
                        if (slider.Nodes.First().Additions.HasFlag(HitSoundAddition.Finish))
                        {
                            Ring(OsbEasing.OutExpo, hitObject, 1000);
                            Particles(OsbEasing.OutExpo, hitObject, 1000);
                        }
                        Highlight(hitObject, 1000, false);
                    }
                }

                if (183332 <= hitObject.StartTime && hitObject.StartTime <= 193332)
                {
                    if (hitObject is OsuSlider)
                    {
                        Ring(OsbEasing.OutExpo, hitObject, 1000);
                        Particles(OsbEasing.OutExpo, hitObject, 1000);
                        Highlight(hitObject, 1000, true);
                    }
                }

                if (195332 <= hitObject.StartTime && hitObject.StartTime <= 198998)
                {
                    if (hitObject is OsuSlider)
                        Highlight(hitObject, 1000, true);
                }

                if (137665 <= hitObject.StartTime && hitObject.StartTime <= 138165 ||
                    140332 <= hitObject.StartTime && hitObject.StartTime <= 140832 ||
                    142998 <= hitObject.StartTime && hitObject.StartTime <= 143582 ||
                    144665 <= hitObject.StartTime && hitObject.StartTime <= 146498 ||
                    147498 <= hitObject.StartTime && hitObject.StartTime <= 147582 ||
                    148165 <= hitObject.StartTime && hitObject.StartTime <= 148665 ||
                    149665 <= hitObject.StartTime && hitObject.StartTime <= 151248 ||
                    153998 <= hitObject.StartTime && hitObject.StartTime <= 161998

                    || 169998 <= hitObject.StartTime && hitObject.StartTime <= 171998 ||
                    163165 <= hitObject.StartTime && hitObject.StartTime <=167332 ||
                    172665 <= hitObject.StartTime && hitObject.StartTime <= 177998 ||
                    203332 <= hitObject.StartTime && hitObject.StartTime <= 204080)
                {
                    Ring(OsbEasing.OutExpo, hitObject, 1000);
                    Particles(OsbEasing.OutExpo, hitObject, 1000);
                    Highlight(hitObject, 1000, true);
                }

                if (370096 <= hitObject.StartTime && hitObject.StartTime <= 388957 ||
                167332 <=hitObject.StartTime && hitObject.StartTime <=169332)
                {
                    Highlight(hitObject, 1000, true);
                }

                if (212154 <= hitObject.StartTime && hitObject.StartTime <= 218598 ||
                212154 <= hitObject.EndTime && hitObject.EndTime <= 218598)
                {
                    Ring(OsbEasing.OutExpo, hitObject, 1000);
                }

                if (216947 <= hitObject.StartTime && hitObject.StartTime <= 218598)
                {
                    Particles(OsbEasing.OutExpo, hitObject, 1000);
                }
            }

        }

        void Highlight(OsuHitObject hitObject, double duration, bool stayVisible)
        {
            double endTime = stayVisible ? hitObject.EndTime + duration : hitObject.StartTime + duration;
            if (hitObject is OsuCircle)
            {
                OsbSprite light = highlightPool.Get(hitObject.StartTime, endTime);
                light.Move(hitObject.StartTime, hitObject.Position);
                light.Color(hitObject.StartTime, hitObject.Color);
                light.Additive(hitObject.StartTime);
                light.Scale(hitObject.StartTime, Random(0.3, 0.4));
                light.Fade(hitObject.StartTime, hitObject.StartTime + 200, 0, 1);
                light.Fade(hitObject.EndTime, hitObject.EndTime + duration, 1, 0);
                light.CommandSplitThreshold = 300;
                return;
            }

            for (double time = hitObject.StartTime; time <= hitObject.EndTime; time += Beatmap.GetTimingPointAt((int)hitObject.StartTime).BeatDuration / 8)
            {
                OsbSprite light = highlightPool.Get(time, hitObject.EndTime + duration);
                light.Additive(time);
                light.Move(time, hitObject.PositionAtTime(time));
                light.Color(time, hitObject.Color);
                light.Scale(time, Random(0.3, 0.4));
                light.Fade(time, time <= hitObject.EndTime - 200 && hitObject.EndTime <= time ? hitObject.EndTime : time + 200, 0, 1);
                light.Fade(hitObject.EndTime, hitObject.EndTime + duration, 1, 0);
                light.CommandSplitThreshold = 300;
            }
        }

        void Ring(OsbEasing easing, OsuHitObject hitObject, double duration)
        {
            double endTime = hitObject.StartTime + duration;
            using (ringPool)
            {
                OsbSprite ring = ringPool.Get(hitObject.StartTime, endTime);
                ring.Move(hitObject.StartTime, hitObject.Position);
                ring.Scale(easing, hitObject.StartTime, endTime, 0.1f, 0.3f);
                ring.Fade(easing, hitObject.StartTime, endTime, 1, 0);
                if (212154 <= hitObject.EndTime && hitObject.EndTime <= 218598 && (int)hitObject.EndTime != 216871)
                {
                    Log(hitObject.EndTime);
                    endTime = hitObject.EndTime + duration;
                    ring = ringPool.Get(hitObject.EndTime, endTime);
                    ring.Move(hitObject.EndTime, hitObject.EndPosition);
                    ring.Scale(easing, hitObject.EndTime, endTime, 0.1f, 0.3f);
                    ring.Fade(easing, hitObject.EndTime, endTime, 1, 0);
                }
            }
        }

        void Particles(OsbEasing easing, OsuHitObject hitObject, double duration)
        {
            int particleCount = 100;
            double maxRadius = 100;
            double endTime = hitObject.StartTime + duration;
            using (particlePool)
            {
                for (int i = 0; i <= particleCount; i++)
                {
                    OsbSprite particle = particlePool.Get(hitObject.StartTime, endTime);
                    double angle = Random(0, Math.PI * 2);
                    double radius = Random(0, maxRadius);
                    Vector2 position = new Vector2(
                        (float)(hitObject.Position.X + Math.Cos(angle) * radius),
                        (float)(hitObject.Position.Y + Math.Sin(angle) * radius)
                    );
                    particle.Move(easing, hitObject.StartTime, endTime, hitObject.Position, position);
                    particle.Fade(hitObject.StartTime, endTime, 1, 0);
                    particle.Scale(hitObject.StartTime, Random(0.1, 0.5));
                }
            }
        }

    }
}
