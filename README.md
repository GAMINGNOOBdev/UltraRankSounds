# UltraRankSounds
This is a mod that plays a sound whenever your style rank increases or decreases.\
If you have any suggestions, make an issue in the github repo.\
**Note**: The current sounds are made using Microsoft SAM.

## Current features
- Disabling all sounds (so you can have the mod loaded but disable it in a plugin preset)
- Changing announcer volume (will not be synced to master volume)
- Enabling, if sounds should be played when the rank decreases
- Multiple sounds for one style rank (for more variety in gameplay)
- Customizable sounds for **both ascension and descension** (by replacing the audio files)
- Support for `.mp3`, `.ogg`, `.wav` and `.flac`

## Planned features
- Customizable sounds for each style bonus (hopefully with support for custom style bonuses)

## Installation
There are 2 options:
1. Install using any mod manager like r2modman or thunderstore mod manager.
2. Download the zip from the releases and extract it's `plugins` folder to `ULTRAKILL/BepInEx/`.

## Customizing sounds
To customize sounds, open the `UltraRankSounds` panel in the plugin configs. \
Then click on `Open sounds folder` and just replace any sound file. \
Which file names associate to which rank should be pretty self-explanitory. \
Files that do not start with `downrank-` are the rank ascension sounds and those with the prefix are the descention sounds.
Files now do not have to have the same file extension, only the name \
of the file has to **start** with the short name of the style rank.
For example:
- `downrank-Ass.ogg` would be registered as the downrank sound for **A**narchic.
- `ULTR-gabriel-voiceline-spoken-by-gianni-himself.wav` would be registered as the **ULTR**AKILL rank up sound.
