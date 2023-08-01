# OTR23-Storyboard
Collab storyboard made with salihefree (did Lyrics and CollabNames), made using Storybrew (uses Storybrew3d, therefore requiring dev build of storybrew)
https://osu.ppy.sh/beatmapsets/2036657#osu/4247350

## Script Credits
- Particle3D, Cooldown: Peter (I'm sorry for the lag this causes)
- Drop2: Darky1 (yes... I copied the code from Skyshifter VIP)
## Script Inspiration
- Cowbell: Storyboarder (Inspired from `Sweet Dreams (11t dnb mix)`, `00:37:387 -`)
- CircleInvasion, Line Tunnel (Buildup.cs), Circle Tunnel (Buildup): Tommy Phelps (Inspired from `true DJ MAG top ranker's song 前編 (かたぎり Remix)`, he does it better than me...)
- CircleInvasion & Drop2: OLD idea of my Skyshifter VIP.. where squares marched into the land of circles
- StaffCredits: My own unfinished storyboard for BQT4 Tiebreaker

## Considerations
I have attempted my best to make this as low osb file size and optimised as possible. However the 3d particles at 219026 is EXTREMELY taxing (even the particles itself are 2mb.. using loops) and I don't think I could fix it for now..
Cool thing is that all the spectrums in each drop are essentially less than the 3d particles in terms of file size.

## wtf am i writing..
This storyboard, although extremely large in scale, took me around 2 weeks. Had to rush through this as they initially requested a storyboard in the style of SAMString - NUMA (one of my earlier storyboards which includes extremely simple spectrum and background movement). It was until I first heard this track that I knew it wasn't a song for a simple storyboard. So I put my head down and ploped in as much ideas as I had on this song.
During the development of the storyboard, I realised that Tommy Phelps used Storybrew3d (which I had some experience with before) to generate the tunnel effects on true DJ MAG (which happened to be one of the most scuffed things I wrote)

## Script Times
- 00:00:015 - 01:32:665 - Intro.cs
- 01:32:665 - 01:40:665 - Buildup.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Buildup.cs#L26-L75))
- 01:40:665 - 01:43:332 - Drop1.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Drop1.cs#L27))
- 01:43:332 - 02:04:665 - Drop1.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Drop1.cs#L174-L269), yes.. each chromatic movement is done line by line...)
- 02:04:665 - 02:47:332 - Drop1.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Drop1.cs#L271-L482), I was too lazy to finish off the section)
- 02:49:998 - 02:57:998 - Drop1.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Drop1.cs#L483-L661), the most tedious section.. so many background switches PER 1/4 tick)
- 02:57:998 - 03:03:332 - CircleInvasion.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/CircleInvasion.cs#L27))
- 03:03:332 - 03:13:998 - Drop2.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Drop2.cs#L19))
- 03:13:998 - 03:27:609 - Cowbell.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Cowbell.cs#L29))
- 03:39:011 - 04:52:263 - Particle3D
- 03:39:011 - 04:52:263 - Cooldown ([Clouds](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Cooldown.cs#L17))
- 04:34:097 - 04:44:763 - StaffCredits ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/StaffCredits.cs#L19))
- 04:52:763 - 04:55:430 - Logo.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Logo.cs#L17))
- 04:55:430 - 05:04:763 - Buildup.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Buildup.cs#L112-L261))
- 05:06:096 - 05:43:430 - Drop3.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Drop3.cs#L20))
- 05:44:096 - 05:48:763 - Buildup.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Buildup.cs#L263-L310))
- 05:48:763 - 06:33:130 - Drop4.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Drop4.cs#L19))
- 06:33:130 - 06:57:971 - Outro.cs ([Lines](https://github.com/Coppertine/OTR23-Storyboard/blob/04851144d11cccf6e41c7cfcfc2584b7fe147802/Outro.cs#L17))

### General Scripts
- Hitobjects: Hitobjects.cs
- Lyrics: Lyrics.cs
- Flashes: Transitions.cs
- Vignette: Vignette.cs

