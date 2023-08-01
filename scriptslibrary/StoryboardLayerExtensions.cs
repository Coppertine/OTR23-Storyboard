using OpenTK;
using StorybrewCommon.Scripting;
using StorybrewCommon.Storyboarding;
using StorybrewCommon.Subtitles;

namespace StorybrewScripts {
    public static class StoryboardLayerExtensions {
        public static Lyric CreateLyric(this StoryboardLayer layer, StoryboardObjectGenerator gen, FontGenerator font, string line, OsbOrigin origin, double scale, Vector2 initialPosition)
        {
            return new Lyric(layer, gen, font, line, origin, scale, initialPosition);
        }
    }
}