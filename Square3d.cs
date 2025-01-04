using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Mapset;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Storyboarding.Util;
using StorybrewCommon.Storyboarding3d;
using StorybrewCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StorybrewScripts
{
    public class Square3d : StoryboardObjectGenerator {
        [Configurable]
        public double StartTime = 0;
        [Configurable]
        public double EndTime = 10000;

        [Configurable]
        public double DurationPerLoop = 1000;

        [Configurable]
        public double Size = 100;

        [Configurable]
        public Vector2 CubePosition = new(0,0);
        [Configurable]
        public double CubeDepth = 100;

        [Configurable]
        public int Divisor = 4;
        public override void Generate()
        {
            Scene3d scene = new();
            PerspectiveCamera camera = new();
            camera.FarFade.Add(StartTime, 200);
            camera.FarClip.Add(StartTime, 350);

            scene.Root.PositionX.Add(StartTime, 0);
            scene.Root.PositionY.Add(StartTime, 0);
            scene.Root.PositionZ.Add(StartTime, 0);

            camera.PositionX.Add(StartTime, 0);
            camera.PositionY.Add(StartTime, 0);
            camera.PositionZ.Add(StartTime, -100);
            Node3d squareParent = new();
            squareParent.PositionX.Add(StartTime, CubePosition.X);
            squareParent.PositionY.Add(StartTime, CubePosition.Y);
            squareParent.PositionZ.Add(StartTime, (float)CubeDepth);

            var cubeVertices = new Vector3[] {
                new((float)(-Size/2), (float)(-Size/2), (float)(-Size/2)),
                new((float)(Size/2), (float)(-Size/2), (float)(-Size/2)),
                new((float)(Size/2), (float)(Size/2), (float)(-Size/2)),
                new((float)(-Size/2), (float)(Size/2), (float)(-Size/2)),
                new((float)(-Size/2), (float)(-Size/2), (float)(Size/2)),
                new((float)(Size/2), (float)(-Size/2), (float)(Size/2)),
                new((float)(Size/2), (float)(Size/2), (float)(Size/2)),
                new((float)(-Size/2), (float)(Size/2), (float)(Size/2))
            };

            var edges = new int[][] {
                [0, 1], [1, 2], [2, 3], [3, 0],
                [4, 5], [5, 6], [6, 7], [7, 4],
                [0, 4], [1, 5], [2, 6], [3, 7]
            };

            foreach (var edge in edges)
            {
                CreateEdge(squareParent, "sb/p.png", cubeVertices[edge[0]], cubeVertices[edge[1]], StartTime);
            }

            // Do position and rotation shenanigans here.
            squareParent.Rotation.Add(StartTime, new Quaternion(new Vector3(0, 0, 0), 1));
            squareParent.Rotation.Add(DurationPerLoop, new Quaternion(new Vector3(1, 0, 1), 0));
            scene.Add(squareParent);
            scene.Generate(camera, GetLayer(""), StartTime, DurationPerLoop, Beatmap.GetTimingPointAt((int)StartTime).BeatDuration / Divisor, (int)((EndTime - StartTime) / DurationPerLoop) + 1);
        }

        public void CreateEdge(Node3d parent, string spritePath, Vector3 start, Vector3 end, double time)
        {
            Line3d line = new()
            {
                SpritePath = spritePath
            };

            line.StartPosition.Add(time, start);
            line.EndPosition.Add(time, end);
            parent.Add(line);
        }
    } 
}