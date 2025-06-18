using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Diagnostics;
using CSCore.CoreAudioAPI;

namespace AudioEngine
{
    // Audio device info for JSON output
    public class AudioDevice
    {
        public string Id { get; set; } = "";        // Device ID for routing
        public string Name { get; set; } = "";      // Display name
        public string State { get; set; } = "";     // Active/Disabled
        public bool IsDefault { get; set; } = false; // Windows default device
    }

    // Audio session info for JSON output
    public class AudioSession
    {
        public string ProcessName { get; set; } = "";   // App name (e.g., "Spotify")
        public int ProcessId { get; set; } = 0;         // Windows process ID
        public string DeviceName { get; set; } = "";    // Current audio device
        public string DeviceId { get; set; } = "";      // Device ID for routing
        public bool IsPlaying { get; set; } = false;    // Currently making sound
        public float Volume { get; set; } = 0.0f;       // Session volume level
    }

    class Program 
    {
        // Main entry point - handles commands from StreamDeck plugin
        static void Main(string[] args) 
        {
            try
            {
                if (args.Length == 0)
                {
                    ShowHelp();
                    return;
                }

                // Route commands to appropriate functions
                switch (args[0].ToLower())
                {
                    case "list-devices":
                        ListAudioDevices();
                        break;
                    case "list-sessions":
                        ListAudioSessions();
                        break;
                    case "route":
                        if (args.Length >= 3)
                        {
                            RouteApplication(args[1], args[2]);
                        }
                        else
                        {
                            Console.WriteLine("Usage: AudioEngine.exe route [appName] [deviceName]");
                        }
                        break;
                    default:
                        ShowHelp();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        // Show available commands
        static void ShowHelp()
        {
            Console.WriteLine("HyperRoute Audio Engine v1.0");
            Console.WriteLine("Available commands:");
            Console.WriteLine("  list-devices    - List all audio playback devices");
            Console.WriteLine("  list-sessions   - List all active audio sessions");
            Console.WriteLine("  route [app] [device] - Route application to device");
        }

        // Get all audio devices and output as JSON
        static void ListAudioDevices()
        {
            var devices = new List<AudioDevice>();
            
            // Access Windows Audio API
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                // Get all active playback devices
                var deviceCollection = deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
                var defaultDevice = deviceEnumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
                
                // Convert to our model
                foreach (var device in deviceCollection)
                {
                    devices.Add(new AudioDevice
                    {
                        Id = device.DeviceID,
                        Name = device.FriendlyName,
                        State = device.DeviceState.ToString(),
                        IsDefault = defaultDevice.DeviceID == device.DeviceID
                    });
                }
            }

            // Output JSON for StreamDeck plugin
            var json = JsonSerializer.Serialize(devices, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);
        }

        // Find all apps currently using audio
        static void ListAudioSessions()
        {
            var sessions = new List<AudioSession>();
            
            // Access Windows Audio API
            using (var deviceEnumerator = new MMDeviceEnumerator())
            {
                // Check each audio device for active sessions
                var deviceCollection = deviceEnumerator.EnumAudioEndpoints(DataFlow.Render, DeviceState.Active);
                
                foreach (var device in deviceCollection)
                {
                    try
                    {
                        // Get the session manager for this device
                        var sessionManager = AudioSessionManager2.FromMMDevice(device);
                        var sessionEnumerator = sessionManager.GetSessionEnumerator();
                        
                        // Check each audio session on this device
                        foreach (var session in sessionEnumerator)
                        {
                            try
                            {
                                // Cast to AudioSessionControl2 to get process info
                                var sessionControl2 = session.QueryInterface<AudioSessionControl2>();
                                
                                // Skip system sessions (no process ID)
                                var processId = sessionControl2.ProcessID;
                                if (processId == 0) 
                                {
                                    sessionControl2.Dispose();
                                    continue;
                                }
                                
                                // Get process info
                                var process = Process.GetProcessById((int)processId);
                                if (process == null) 
                                {
                                    sessionControl2.Dispose();
                                    continue;
                                }
                                
                                // Get volume control interface
                                var volumeControl = session.QueryInterface<SimpleAudioVolume>();
                                
                                // Create session info
                                var audioSession = new AudioSession
                                {
                                    ProcessName = process.ProcessName,
                                    ProcessId = (int)processId,
                                    DeviceName = device.FriendlyName,
                                    DeviceId = device.DeviceID,
                                    IsPlaying = session.SessionState == AudioSessionState.AudioSessionStateActive,
                                    Volume = volumeControl.MasterVolume
                                };
                                
                                sessions.Add(audioSession);
                                
                                // Clean up interfaces
                                sessionControl2.Dispose();
                                volumeControl.Dispose();
                            }
                            catch
                            {
                                // Skip sessions we can't access
                                continue;
                            }
                        }
                    }
                    catch
                    {
                        // Skip devices we can't access
                        continue;
                    }
                }
            }

            // Output JSON for StreamDeck plugin
            var json = JsonSerializer.Serialize(sessions, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);
        }

        // Route app to device - coming soon
        static void RouteApplication(string appName, string deviceName)
        {
            Console.WriteLine($"Routing functionality coming soon: {appName} -> {deviceName}");
        }
    }
}