# HyperRoute 🎵

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=flat-square&logo=csharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![JavaScript](https://img.shields.io/badge/JavaScript-F7DF1E?style=flat-square&logo=javascript&logoColor=black)](https://developer.mozilla.org/en-US/docs/Web/JavaScript)
[![StreamDeck](https://img.shields.io/badge/StreamDeck-Plugin-00B4D8?style=flat-square)](https://www.elgato.com/stream-deck)

**StreamDeck plugin for instant audio application routing**

Toggle multiple applications between any audio devices with a single button press. Perfect for quickly switching apps from your main audio interface to speakers (or any other device) and back.

---

## What it does

**The Problem:**
Need to route Discord, Spotify, and your game from your audio interface to speakers? You have to manually change each application's audio output one by one in Windows settings, then remember to change them all back later.

**HyperRoute Solution:**

- Press one StreamDeck button → All configured apps route to speakers
- Press same button again → All apps restore to their original devices
- Works with any applications and any audio devices

---

## Key Features

- **Multi-app toggle** - Control multiple applications with one button
- **Smart restoration** - Remembers where each app was originally routed
- **Universal compatibility** - Works with any Windows audio devices
- **Easy configuration** - Add/remove apps through StreamDeck interface

## For GoXLR Users

HyperRoute extends beyond the official GoXLR StreamDeck plugin by allowing routing to **external devices** like speakers. While the GoXLR plugin can only route between GoXLR channels, HyperRoute can route your applications anywhere - including outside the GoXLR ecosystem.

---

## Tech Stack

- **C# Audio Engine** - Windows Audio Session API integration
- **StreamDeck Plugin** - JavaScript with configuration interface
- **CSCore Library** - Windows audio API wrapper

---

## Development Status

**Week 1 of 8** - Basic project structure complete

- ✅ C# console application setup
- 🚧 Windows Audio API integration
- ⏳ StreamDeck plugin development
- ⏳ Multi-app toggle system

---

## Build (Development)

---

**MIT License** | Built for streamers and content creators who need better audio control or for people like myself who just like to have a good time without there headphones on!
