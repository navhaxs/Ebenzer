using CoreAudio;

namespace Ebenezer;

public static class AudioDeviceModule
{
    public static List<AudioDevice> GetAllAudioDevices()
    {
        var enumerator = new MMDeviceEnumerator(Guid.NewGuid());
        // var defaultAudioEndpoint = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        // var x = new Device() { ID = defaultAudioEndpoint.ID, DeviceFriendlyName = defaultAudioEndpoint.DeviceFriendlyName };
        List<AudioDevice> devices = new List<AudioDevice>();
        foreach (var enumerateAudioEndPoint in enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active))
        {
            devices.Add(new AudioDevice { ID = enumerateAudioEndPoint.ID, DeviceFriendlyName = enumerateAudioEndPoint.DeviceFriendlyName });
        }
        return devices;
    }

    public static AudioDevice GetDefaultAudioDevice()
    {
        var enumerator = new MMDeviceEnumerator(Guid.NewGuid());
        var defaultAudioEndpoint = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
        return new AudioDevice { ID = defaultAudioEndpoint.ID, DeviceFriendlyName = defaultAudioEndpoint.DeviceFriendlyName };
    }
    
    public static void SetDefaultAudioDevice(string id)
    {
        MMDeviceEnumerator enumerator = new MMDeviceEnumerator(Guid.NewGuid());
        MMDevice device = enumerator.GetDevice(id);
        enumerator.SetDefaultAudioEndpoint(device);
    }

    public class AudioDevice
    {
        public string ID { get; set; }
        public string DeviceFriendlyName { get; set; }
    }
}