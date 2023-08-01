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
using System.IO;
using System.Linq;
using System.Text;

namespace StorybrewScripts
{
    public class StaffCredits : StoryboardObjectGenerator
    {
        public override void Generate()
        {

            var mainFont = LoadFont("sb/f/s", new FontDescription
            {
                FontPath = "fonts/Nexa-Heavy.ttf",
                Color = Color4.White
            },
            new FontShadow()
            {
                Thickness = 10,
                Color = Color4.Black
            });

            var title = GetLayer("").CreateLyric(this, mainFont, "天使の帰郷", OsbOrigin.Centre, 0.2, new Vector2(320, 240));
            var artist = GetLayer("").CreateLyric(this, mainFont, "Asatsumei feat. L4hee", OsbOrigin.Centre, 0.2, new Vector2(320, 240));
            var lineSeperator = GetLayer("").CreateSprite("sb/p.png");

            lineSeperator.ScaleVec(OsbEasing.OutExpo, 273882, 274680, new Vector2(1, 1), new Vector2(200, 1));
            title.MoveY(OsbEasing.OutExpo, 273882, 274680, 240, 225);
            artist.MoveY(OsbEasing.OutExpo, 273882, 274680, 240, 255);
            title.Fade(OsbEasing.OutExpo, 273882, 274680, 0, 1);
            artist.Fade(OsbEasing.OutExpo, 273882, 274680, 0, 1);

            title.MoveY(OsbEasing.OutExpo, 279096, 279346, 225, 240);
            artist.MoveY(OsbEasing.OutExpo, 279096, 279346, 255, 240);
            lineSeperator.ScaleVec(OsbEasing.OutExpo, 279096, 279346, new Vector2(200, 1), new Vector2(1, 1));


            title.Fade(OsbEasing.OutExpo, 279096, 279346, 1,0);
            artist.Fade(OsbEasing.OutExpo, 279096, 279346,  1,0);

            // 274096
            var HostGroup = new StaffGroup("HOST");
            var GFXGroup = new StaffGroup("GFX/DEVELOPER");
            var MappersGroup = new StaffGroup("MAPPERS");
            var MappoolTeam = new StaffGroup("POOLERS");
            var PlaytestTeam = new StaffGroup("PLAYTESTING");
            var RefereeTeam = new StaffGroup("REFEREES");
            var StreamerTeam = new StaffGroup("STREAMERS");
            var CasterTeam = new StaffGroup("CASTERS");
            var Storyboard = new StaffGroup("STORYBOARD");
            var Hitsound = new StaffGroup("HITSOUND");

            string line;
            using (StreamReader reader = new StreamReader(OpenProjectFile("stafflist.txt"), Encoding.UTF8))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] staff = line.Split(',');
                    switch (staff[0])
                    {
                        case "HOST":
                            HostGroup.AddStaff(staff[1]);
                            break;
                        case "GFX/DEVELOPER":
                            GFXGroup.AddStaff(staff[1]);
                            break;
                        case "MAPPER":
                            MappersGroup.AddStaff(staff[1]);
                            break;
                        case "POOLING":
                            MappoolTeam.AddStaff(staff[1]);
                            break;
                        case "PLAYTESTING":
                            PlaytestTeam.AddStaff(staff[1]);
                            break;
                        case "REFEREE":
                            RefereeTeam.AddStaff(staff[1]);
                            break;
                        case "STREAMER":
                            StreamerTeam.AddStaff(staff[1]);
                            break;
                        case "CASTER":
                            CasterTeam.AddStaff(staff[1]);
                            break;
                        case "HITSOUNDER":
                            Hitsound.AddStaff(staff[1]);
                            break;
                        case "STORYBOARD":
                            Storyboard.AddStaff(staff[1]);
                            break;
                    }
                }
                reader.Dispose();
            }
            float spacing = 20;
            HostGroup.displayStaff(GetLayer("Staff"), mainFont, 279430, 284763, new Vector2(320, 70));
            // AdminGroup.displayStaff(GetLayer("Staff"), mainFont, 154880, 284763, new Vector2(50, 90));
            GFXGroup.displayStaff(GetLayer("Staff"), mainFont, 279430, 284763, new Vector2((320 / 2) - spacing, 90));
            MappersGroup.displayStaff(GetLayer("Staff"), mainFont, 279430, 284763, new Vector2((320 / 2) - spacing, 180));
            MappoolTeam.displayStaff(GetLayer("Staff"), mainFont, 279430, 284763, new Vector2((320 / 2) - spacing, 300));
            PlaytestTeam.displayStaff(GetLayer("Staff"), mainFont, 279430, 282096, new Vector2(320 + (320 / 2) + spacing, 250));
            RefereeTeam.displayStaff(GetLayer("Staff"), mainFont, 279430, 282096, new Vector2(320 + (320 / 2) + spacing, 90));
            StreamerTeam.displayStaff(GetLayer("Staff"), mainFont, 282096, 284763, new Vector2(320 + (320 / 2) + spacing, 90));
            CasterTeam.displayStaff(GetLayer("Staff"), mainFont, 282096, 284763, new Vector2(320 + (320 / 2) + spacing, 170));
            Hitsound.displayStaff(GetLayer("Staff"), mainFont, 282096, 284763, new Vector2(320 + (320 / 2) - 50, 300));
            Storyboard.displayStaff(GetLayer("Staff"), mainFont, 282096, 284763, new Vector2(320 + (320 / 2) + 100, 300));
        }

        public class StaffGroup
        {
            public string name;
            public List<string> staffs = new List<string>();

            public StaffGroup(string name)
            {
                this.name = name;
            }

            public void AddStaff(string staff)
            {
                staffs.Add(staff);
            }

            public void displayStaff(StoryboardLayer layer, FontGenerator font, double startTime, double endTime, Vector2 position)
            {
                var groupNameSprite = layer.CreateLyric(StaffCredits.Current, font, name, OsbOrigin.Centre, 0.2, position);
                groupNameSprite.Fade(startTime, startTime + 200, 0, 1);
                groupNameSprite.MoveY(OsbEasing.InOutSine, startTime, startTime + 200, position.Y + 10, position.Y);
                groupNameSprite.Fade(endTime - 200, endTime, 1, 0);
                float y = position.Y;
                double time = startTime;
                foreach (var staff in staffs)
                {
                    y += 20;
                    time += 20;
                    float x = position.X;
                    if (name == "GFX/DEVELOPER" || name == "REFEREES" || name == "PLAYTESTING" || name == "CASTERS" || name == "STREAMERS" || name == "MAPPERS" || name == "POOLERS")
                    {
                        x = staffs.IndexOf(staff) % 2 == 0 ? position.X + 70 : position.X - 70;
                        y = staffs.IndexOf(staff) % 2 == 0 ? y : y - 20;
                    }
                    var staffSprite = layer.CreateLyric(StaffCredits.Current, font, staff, OsbOrigin.Centre, 0.2, new Vector2(x, y));
                    staffSprite.Fade(time, time + 200, 0, 1);
                    staffSprite.MoveY(OsbEasing.InOutSine, time, time + 200, y + 10, y);
                    staffSprite.Fade(endTime - 200, endTime, 1, 0);

                }
            }
        }
    }
}
