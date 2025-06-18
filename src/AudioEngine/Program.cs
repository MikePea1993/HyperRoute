using System;
using System.Collections.Generic;
using System.Text.Json;
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

        // List apps using audio - coming soon
        static void ListAudioSessions()
        {
            Console.WriteLine("Audio sessions functionality coming soon...");
        }

        // Route app to device - coming soon
        static void RouteApplication(string appName, string deviceName)
        {
            Console.WriteLine($"Routing functionality coming soon: {appName} -> {deviceName}");
        }
    }
}