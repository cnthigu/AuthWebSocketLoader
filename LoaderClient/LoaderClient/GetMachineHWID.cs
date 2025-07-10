using System;
using System.Management;

namespace LoaderClient
{
    public static class HardwareHelper
    {
        public static string GetMachineHWID()
        {
            try
            {
                var searcher = new ManagementObjectSearcher("SELECT VolumeSerialNumber FROM Win32_LogicalDisk WHERE DeviceID='C:'");
                foreach (ManagementObject disk in searcher.Get())
                {
                    return disk["VolumeSerialNumber"]?.ToString() ?? "UNKNOWN";
                }
            }
            catch
            {

            }
            return "UNKNOWN";
        }
    }
}
