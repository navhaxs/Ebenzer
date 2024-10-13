using System.Drawing;
using Microsoft.Win32;
using WindowsDisplayAPI;

namespace Ebenezer;

public class PowerPointDisplayMonitorModule
{

    static internal RegistryKey GetPowerPointPath()
    {
        // Attempt to match
        string path = null;
        string[] try_paths = {
            @"Software\Microsoft\Office\16.0\PowerPoint\Options",
            @"Software\Microsoft\Office\15.0\PowerPoint\Options",
            @"Software\Microsoft\Office\14.0\PowerPoint\Options",
            @"Software\Microsoft\Office\12.0\PowerPoint\Options"
        };

        foreach (string i in try_paths)
        {
            if (Registry.CurrentUser.OpenSubKey(i) != null)
            {
                path = i;
            }
        }

        if (path == null)
        {
            // MessageBox.Show("This tool requires PowerPoint to be installed and run before for this user.");
            throw new Exception(@"Did not detect PowerPoint registry key in HKCU\Software\Microsoft\Office\");
        }

        // Write it to powerpoint's registry
        return Registry.CurrentUser.OpenSubKey(path, true);
    }
    
    /*
     * Logic for writing PowerPoint display configuration
     */
    static internal DisplayDevice? GetConfig()
    {
        var oldValue = GetPowerPointPath().GetValue("DisplayMonitor", null);
 
        if (oldValue is string oldValueAsString)
        {
            return ToDisplay(GetDisplay(oldValueAsString));
        }

        return null;
    }

    static internal DisplayDevice? ToDisplay(Display? display)
    {
        if (display == null)
        {
            return null;
        }
        return new DisplayDevice(display.DisplayName, display.DeviceName, display.CurrentSetting.Position,
            display.CurrentSetting.Resolution);
    }

    static internal List<DisplayDevice> GetAllDisplayDevices()
    {
        // Get displays
        IEnumerable<Display> displays = Display.GetDisplays();
        return displays.Select(d => ToDisplay(d)).ToList();
    }

    static Display? GetDisplay(string id)
    {
        // Get displays
        IEnumerable<Display> displays = Display.GetDisplays();

        // Attempt to match
        return displays.FirstOrDefault(d => d.DisplayName == id);
    }
    
    /*
     * Logic for writing PowerPoint display configuration
     */
    static internal bool SetConfig(string id)
    {
        // Display m = GetDisplay(id);

        // Write it to powerpoint's registry
        RegistryKey key = GetPowerPointPath();

        var oldValue = key.GetValue("DisplayMonitor", null);
        if (oldValue != null && oldValue.ToString() == id)
        {
            return false;
        }

        key.SetValue("DisplayMonitor", id);

        return true;
    }

    public record DisplayDevice(string ID, string DeviceFriendlyName, Point Position, Size Resolution);
}