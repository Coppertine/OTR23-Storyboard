using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Animations;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Subtitles;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{

    /**
        TODO:
        Collecting Square build up from Beggars
        Rings and Particles on all finishes in the kiai sections.
        Add some basic hitlighting

    **/
    public class Buildup : StoryboardObjectGenerator
    {
        public override void Generate()
        {
            OsbSprite blurBackground = GetLayer("Background").CreateSprite("sb/ultrablur.jpg");

            /* Particles */
            OsbSpritePool particlePool;
            var particleCount = 200;

            using (particlePool = new OsbSpritePool(GetLayer("Background"), "sb/dot.png", OsbOrigin.Centre, (sprite, start, end) =>
            {
                sprite.Scale(start, Random(0.5, 1.2));
                sprite.Fade(start, start + 200, 0, 1);
            }))
            {
                var pIncrement = (100332 - 92665) / particleCount;
                for (int time = 92665; time <= 100332; time += pIncrement)
                {
                    var duration = time < 97999 ? Random(1200, 2200) : Random(400, 700);
                    var sprite = particlePool.Get(time, time + duration);
                    var startRadius = Random(20, 50);
                    var endRadius = Random(400, 500);
                    var angle = Random(0, Math.PI * 2);
                    var startPosition = new Vector2(
                        320 + (float)Math.Cos(angle) * startRadius,
                        240 + (float)Math.Sin(angle) * startRadius
                    );
                    var endPosition = new Vector2(
                        320 + (float)Math.Cos(angle) * endRadius,
                        240 + (float)Math.Sin(angle) * endRadius
                    );
                    sprite.Move(time, time + duration, startPosition, endPosition);
                }
            }

            OsbSprite topPanel = GetLayer("Foreground").CreateSprite("sb/p.png", OsbOrigin.TopCentre, new Vector2(320, 0));
            OsbSprite bottomPanel = GetLayer("Foreground").CreateSprite("sb/p.png", OsbOrigin.BottomCentre, new Vector2(320, 480));


            blurBackground.Fade(92665, 95332, 0, 0.8);
            blurBackground.Scale(97799, (480.0f / 720.0f) * 1.25);
            topPanel.Fade(97999, 1);
            bottomPanel.Fade(97999, 1);
            topPanel.Color(97999, "#111320");
            bottomPanel.Color(97999, "#111320");

            topPanel.ScaleVec(97999, 100665, new Vector2(853, 0), new Vector2(853, 240));
            bottomPanel.ScaleVec(97999, 100665, new Vector2(853, 0), new Vector2(853, 240));
            topPanel.Fade(100665, 0);
            bottomPanel.Fade(100665, 0);
            blurBackground.Fade(100665, 0);


            // ** Build up 2 **//
            // Vector2 center = new Vector2(320, 240);
            // Vector2 bottomStart = new Vector2(320, 480);
            // Vector2 topStart = new Vector2(320, 0);

            // int barCount = 5;
            // double offset = 200;
            // double barDuration = 1000;
            // for (int barIndex = 0; barIndex <= barCount; barIndex++)
            // {
            //     OsbSprite bar = GetLayer("Buildup 2").CreateSprite("sb/p.png", OsbOrigin.Centre, bottomStart);
            //     bar.StartLoopGroup(295430 + (offset * barIndex), (int)((300763) - 295430 + (offset * barIndex)) / (int)barDuration);
            //         bar.ScaleVec(OsbEasing.InOutSine, 0, barDuration, new Vector2(1200, 3), new Vector2(700, 1));
            //         bar.Move(OsbEasing.OutSine, 0, barDuration, bottomStart, center + new Vector2(0,20));
            //     bar.EndGroup();

            //     OsbSprite barTop = GetLayer("Buildup 2").CreateSprite("sb/p.png", OsbOrigin.Centre, topStart);
            //     barTop.StartLoopGroup(295430 + (offset * barIndex), (int)((300763) - 295430 + (offset * barIndex)) / (int)barDuration);
            //         barTop.ScaleVec(OsbEasing.InOutSine, 0, barDuration, new Vector2(1200, 3), new Vector2(700, 1));
            //         barTop.Move(OsbEasing.OutSine, 0, barDuration, topStart, center - new Vector2(0,20));
            //     barTop.EndGroup();
            // }




            // We gotta use Storybrew 3d... yaaaaaaay...
            GenerateBarTunnel();
            GenerateCircleTunnel();

        }



        void GenerateBarTunnel()
        {
            Scene3d scene = new Scene3d();
            PerspectiveCamera camera = new PerspectiveCamera();
            camera.FarFade.Add(295413, 200);
            camera.FarClip.Add(295413, 350);
            scene.Root.PositionX.Add(295413, 0);
            scene.Root.PositionY.Add(295413, 0);
            scene.Root.PositionZ.Add(295413, 0);

            camera.PositionX.Add(295413, 0);
            camera.PositionY.Add(295413, 0);
            camera.PositionZ.Add(295413, -100);

            var startTime = 295413;
            var ReserseTime = 300746;
            var stopReverse = 304746;
            var endTime = 306079;
            var offset = 10;
            var length = 1000;
            var barWidth = 700;
            var tunnelHeight = 200;

            int i = 0;
            for (float z = 1; z < length; z += offset)
            {
                Sprite3d barTop = new Sprite3d
                {
                    SpritePath = "sb/p.png",
                    UseDistanceFade = true
                };
                Sprite3d barBottom = new Sprite3d
                {
                    SpritePath = "sb/p.png",
                    UseDistanceFade = true
                };
                // Log(z);
                barTop.PositionX.Add(startTime, 0);
                barTop.PositionY.Add(startTime, -(tunnelHeight / 2));
                barTop.PositionZ.Add(startTime, z);
                barTop.ScaleX.Add(startTime, barWidth);
                barTop.ScaleY.Add(startTime, 1);

                barBottom.PositionX.Add(startTime, 0);
                barBottom.PositionY.Add(startTime, (tunnelHeight / 2));
                barBottom.PositionZ.Add(startTime, z);
                barBottom.ScaleX.Add(startTime, barWidth);
                barBottom.ScaleY.Add(startTime, 1);

                scene.Add(barTop);
                scene.Add(barBottom);

                if (i % 6 == 0)
                {
                    Sprite3d wallLeft = new Sprite3d
                    {
                        SpritePath = "sb/p.png",
                        UseDistanceFade = true
                    };
                    Sprite3d wallRight = new Sprite3d
                    {
                        SpritePath = "sb/p.png",
                        UseDistanceFade = true,

                    };

                    wallLeft.PositionX.Add(startTime, (barWidth / 2));
                    wallLeft.PositionY.Add(startTime, 0);
                    wallLeft.PositionZ.Add(startTime, z);
                    wallLeft.ScaleX.Add(startTime, 1);
                    wallLeft.ScaleY.Add(startTime, tunnelHeight);

                    wallRight.PositionX.Add(startTime, -(barWidth / 2));
                    wallRight.PositionY.Add(startTime, 0);
                    wallRight.PositionZ.Add(startTime, z);
                    wallRight.ScaleX.Add(startTime, 1);
                    wallRight.ScaleY.Add(startTime, tunnelHeight);
                    scene.Add(wallLeft);
                    scene.Add(wallRight);
                }

                if (i % 3 == 0 && i % 6 != 0)
                {
                    Node3d windowLeft = new Node3d();
                    Node3d windowRight = new Node3d();

                    windowLeft.PositionX.Add(startTime, (barWidth / 2));
                    windowLeft.PositionY.Add(startTime, 0);
                    windowLeft.PositionZ.Add(startTime, z);

                    // windowLeft.ScaleX.Add(startTime, 0.5f);
                    // windowLeft.ScaleY.Add(startTime, 0.5f);
                    // windowLeft.ScaleZ.Add(startTime, 0.5f);

                    windowRight.PositionX.Add(startTime, -(barWidth / 2));
                    windowRight.PositionY.Add(startTime, 0);
                    windowRight.PositionZ.Add(startTime, z);

                    // windowRight.ScaleX.Add(startTime, 0.5f);
                    // windowRight.ScaleY.Add(startTime, 0.5f);
                    // windowRight.ScaleZ.Add(startTime, 0.5f);
                    // window.Rotation.Add(startTime, new Quaternion(new Vector3(0.0f,  0.383f, 0.0f),  0.924f));
                    AddTriangles(windowLeft, startTime);
                    AddTriangles(windowRight, startTime);

                    scene.Add(windowLeft);
                    scene.Add(windowRight);
                }
                i++;
            }
            scene.Root.PositionZ.Add(startTime, -600);
            scene.Root.PositionZ.Add(ReserseTime, -800);

            scene.Root.Rotation.Add(ReserseTime, new Quaternion(new Vector3(0, 0, 0), 1));
            scene.Root.Rotation.Add(stopReverse, new Quaternion(new Vector3(0, 0, 1), 0));

            scene.Root.PositionZ.Add(303430, -400);
            scene.Root.PositionZ.Add(stopReverse, -50);
            scene.Root.PositionZ.Add(endTime, -800, EasingFunctions.ExpoIn);

            scene.Generate(camera, GetLayer("Tunnel"), startTime, endTime, Beatmap.GetTimingPointAt(startTime).BeatDuration / 2);
        }

        void AddTriangles(Node3d node, double time)
        {
            Line3d lineLeft = new Line3d
            {
                SpritePath = "sb/p.png"
            };

            Line3d lineRight = new Line3d
            {
                SpritePath = "sb/p.png"
            };

            Line3d lineDown = new Line3d
            {
                SpritePath = "sb/p.png"
            };

            lineLeft.StartPosition.Add(time, new Vector3(0, -60, 0));
            lineLeft.EndPosition.Add(time, new Vector3(0, 20, 10));

            lineRight.StartPosition.Add(time, new Vector3(0, -60, 0));
            lineRight.EndPosition.Add(time, new Vector3(0, 20, -10));

            lineDown.StartPosition.Add(time, new Vector3(0, 20, -10));
            lineDown.EndPosition.Add(time, new Vector3(0, 20, 10));

            node.Add(lineLeft);
            node.Add(lineRight);
            node.Add(lineDown);
        }

        void GenerateCircleTunnel()
        {
            int startTime = 343430;
            int endTime = 348763;
            Scene3d scene = new Scene3d();
            // I don't want to fuck around with cameras... sooo camera stays there during s̵e̵x̵  scene
            PerspectiveCamera camera = new PerspectiveCamera();
            camera.FarClip.Add(startTime, 350);
            camera.FarFade.Add(startTime, 200);
            camera.PositionX.Add(startTime, 0);
            camera.PositionY.Add(startTime, 0);
            camera.PositionZ.Add(startTime, -100);

            int circleCount = 400;
            int circleOffset = 20;
            int startZ = 400;
            int endZ = -1030;
            for (int i = 0; i < circleCount; i++)
            {
                Sprite3d square = new Sprite3d
                {
                    SpritePath = "sb/ring.png",
                    UseDistanceFade = true
                };
                square.ConfigureGenerators((s) =>
                {
                    s.ScaleDecimals = 4;
                });

                square.PositionZ.Add(startTime, (i * circleCount) + startZ);
                square.PositionZ.Add(endTime, (i * circleOffset) + endZ);
                // square.SpriteScale.Add(startTime, new Vector2(0.5));
                scene.Add(square);
            }

            Sprite3d finalSquare = new Sprite3d
            {
                SpritePath = "sb/circle.png",
                UseDistanceFade = true
            };

            finalSquare.PositionZ.Add(endTime - 2000, startZ);
            finalSquare.PositionZ.Add(endTime, -90);
            scene.Add(finalSquare);

            scene.Generate(camera, GetLayer(""), startTime, endTime, Beatmap.GetTimingPointAt(startTime).BeatDuration / 4);
        }
    }


}
