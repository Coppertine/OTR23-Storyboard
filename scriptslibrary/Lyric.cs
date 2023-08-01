using System.Collections.Generic;
using System.Linq;
using OpenTK;
using OpenTK.Graphics;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;

namespace StorybrewScripts {
    public class Lyric {
        public List<OsbSprite> Letters {get; set;}
        public string line;
        public FontGenerator font;
        private StoryboardObjectGenerator gen;
        public double scale {get; set;}
        private Vector2 startPosition;
        private Vector2 endPosition;

        public Lyric(StoryboardLayer layer, StoryboardObjectGenerator gen, FontGenerator font, string line, OsbOrigin origin, double scale, Vector2 initialPosition)
        {
            this.scale = scale;
            this.line = line;
            this.font = font;
            this.gen = gen;
            this.startPosition = initialPosition;
            this.endPosition = initialPosition;

            Letters = new List<OsbSprite>();

            var lineWidth = GetLineWidth(this.scale);

            var letterX = initialPosition.X - lineWidth * 0.5f;
            foreach (char letter in line)
            {
                var letterTexture = font.GetTexture(letter.ToString());
                // "Hello World!"
                if(!letterTexture.IsEmpty)
                {
                    var letterPossition = new Vector2((float)letterX, initialPosition.Y) + letterTexture.OffsetFor(origin) * (float)scale;
                    var letterSprite = layer.CreateSprite(letterTexture.Path, origin, letterPossition);
                    Letters.Add(letterSprite);
                } else {
                    Letters.Add(new OsbSprite());
                }
                letterX += letterTexture.BaseWidth * scale;
            }
        }

        private double GetLineWidth(double scale)
        {
            var lineWidth = 0.0;
            //  "Test" => 'T' 'e' 's' 't'
            foreach(char letter in line)
            {
                var texture = font.GetTexture(letter.ToString());
                lineWidth += texture.BaseWidth * scale;
            }
            return lineWidth;
        }

        public void Fade(double startTime, double endTime, double startOpacity, double endOpacity)
        {
            Fade(OsbEasing.None, startTime, endTime, startOpacity, endOpacity);
        }
        public void Fade(OsbEasing easing, double startTime, double endTime, double startOpacity, double endOpacity)
        {
            foreach(var letter in Letters)
            {
                if(letter.CommandCount == 0)
                {
                    letter.Scale(startTime, scale);
                }
                letter.Fade(easing, startTime, endTime, startOpacity, endOpacity);
            }
        }

        public void Fade(double time, double opacity)
        {
            foreach(var letter in Letters)
            {
                if(letter.CommandCount == 0)
                {
                    letter.Scale(time, scale);
                }
                letter.Fade(time, opacity);
            }
        }

        public void Color(OsbEasing easing, double startTime, double endTime, Color4 startColor, Color4 endColor)
        {
            foreach(var letter in Letters)
            {
                if(letter.CommandCount == 0)
                {
                    letter.Scale(startTime, scale);
                }
                letter.Color(easing, startTime, endTime, startColor, endColor);
            }
        }

        public void Color(double time, Color4 color)
        {
            foreach(var letter in Letters)
            {
                if(letter.CommandCount == 0)
                {
                    letter.Scale(time, scale);
                }
                letter.Color(time, color);
            }
        }

        public void Move(OsbEasing easing, double startTime, double endTime, Vector2 startPosition, Vector2 endPosition)
        {
            var lineWidth = GetLineWidth(this.scale);
            var startLetterX = startPosition.X - lineWidth * 0.5f;
            var endLetterX = endPosition.X - lineWidth * 0.5f;

            var i = 0;
            foreach(var letter in Letters)
            {
                if(letter.CommandCount == 0)
                {
                    letter.Scale(startTime, scale);
                }
                if(!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                {
                    var letterStartPosition = new Vector2((float)startLetterX, startPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                    var letterEndPosition = new Vector2((float)endLetterX, endPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                    letter.Move(easing, startTime, endTime, letterStartPosition, letterEndPosition);                    
                }
                startLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                i++;
            }
        }

        public void MoveX(OsbEasing easing, double startTime, double endTime, double startX, double endX)
        {
            var lineWidth = GetLineWidth(this.scale);
            var startLetterX = startPosition.X - lineWidth * 0.5f;
            var endLetterX = endPosition.X - lineWidth * 0.5f;
            this.startPosition.X = (float)startX;
            this.endPosition.X = (float)endX;
            
    
            var i = 0;
            foreach(var letter in Letters)
            {
                if(letter.CommandCount == 0)
                {
                    letter.Scale(startTime, scale);
                }
                if(!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                {
                    var letterStartX = new Vector2((float)startLetterX, 0) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                    var letterEndX = new Vector2((float)endLetterX, 0) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                    letter.MoveX(easing, startTime, endTime, letterStartX.X, letterEndX.X);                    
                }
                startLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                i++;
            }
        }
        public void MoveY(OsbEasing easing, double startTime, double endTime, double startY, double endY)
        {
            foreach(var letter in Letters)
            {
                if(letter.CommandCount == 0)
                {
                    letter.Scale(startTime, scale);
                }
                letter.MoveY(easing, startTime, endTime, startY, endY);
            }
        }
        public void MoveY( double endTime, double endY)
        {
            foreach(var letter in Letters)
            {
                if(letter.CommandCount == 0)
                {
                    letter.Scale(endTime, scale);
                }
                letter.MoveY(endTime,  endY);
            }
        }

        public void MoveX(double time, double x)
        {
            var lineWidth = GetLineWidth(this.scale);
            
            var endLetterX = endPosition.X - lineWidth * 0.5f;
            this.startPosition.X = (float)x;
            this.endPosition.X = (float)x;
            
            var i = 0;
            foreach(var letter in Letters)
            {
                if(letter.CommandCount == 0)
                {
                    letter.Scale(time, scale);
                }
                if(!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                {
                    
                    var letterEndX = new Vector2((float)endLetterX, 0) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                    letter.MoveX(time, letterEndX.X);                    
                }
                endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * scale;
                i++;
            }
        }

        /// <summary>
        /// Scales the lyric to a specified size with the option to not allow the scaling of individual letters.
        /// Note: This uses OsbSprite.MoveX() to move the letters to the spesified spacing, this will not work if Move() has been used.
        /// </summary>
        /// <param name="startTime">The time to start scaling.</param>
        /// <param name="endTime">The time to end scaling.</param>
        /// <param name="startScale">The starting scale.</param>
        /// <param name="endScale">The ending scale.</param>
        /// <param name="scaleLetters">Whether or not to allow individual letter scaling.</param>
        public void Scale(OsbEasing easing, double startTime, double endTime, double startScale, double endScale, bool scaleLetters = true)
        {
            var startLineWidth = GetLineWidth(startScale);
            var endLineWidth = GetLineWidth(endScale); 
            var startLetterX = startPosition.X - startLineWidth * 0.5f;
            var endLetterX = endPosition.X - endLineWidth * 0.5f;

            var i = 0;
            foreach (var letter in Letters)
            {
                if(!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                {
                    var letterStartPosition = new Vector2((float)startLetterX, startPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                    var letterEndPosition = new Vector2((float)endLetterX, endPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)scale;
                    letter.MoveX(easing, startTime, endTime, letterStartPosition.X, letterEndPosition.X);
                    if(scaleLetters)
                    {
                        letter.Scale(easing, startTime, endTime, startScale, endScale);
                    }
                }
                startLetterX += font.GetTexture(line[i].ToString()).BaseWidth * startScale;
                endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * endScale;
                i++;
            }
        }

        /// <summary>
        /// Scales the lyric to a specified size with the option to not allow the scaling of individual letters.
        /// Note: This uses OsbSprite.MoveX() to move the letters to the spesified spacing, this will not work if Move() has been used.
        /// </summary>
        /// <param name="startTime">The time to start scaling.</param>
        /// <param name="endTime">The time to end scaling.</param>
        /// <param name="startScale">The starting scale.</param>
        /// <param name="endScale">The ending scale.</param>
        /// <param name="scaleLetters">Whether or not to allow individual letter scaling.</param>
        public void Scale(double startTime, double endScale, bool scaleLetters = true)
        {
            var endLineWidth = GetLineWidth(endScale);
            var endLetterX = endPosition.X - endLineWidth * 0.5f;

            var i = 0;
            foreach (var letter in Letters)
            {
                if (!font.GetTexture(line[i].ToString()).IsEmpty && letter != new OsbSprite())
                {
                    var letterEndScale = endScale;
                    var letterEndPosition = new Vector2((float)endLetterX, endPosition.Y) + font.GetTexture(line[i].ToString()).OffsetFor(letter.Origin) * (float)endScale;
                    letter.MoveX(startTime, letterEndPosition.X);
                    if (scaleLetters)
                    {
                        letter.Scale(startTime, letterEndScale);
                    }
                }
                endLetterX += font.GetTexture(line[i].ToString()).BaseWidth * endScale;
                i++;
            }
        }
    }
}