using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiAPITranslatorExample {

    /// <summary>
    /// extracted from MiAPI.h - hopefully this works?
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
#pragma warning disable CS0649, CS0169
    internal unsafe class MiAPI {
        /// <summary>
        /// Gets the meaning of a <see cref="RETURN_CODE"/> in plain english.
        /// </summary>
        /// <param name="code">The <see cref="RETURN_CODE"/></param>
        /// <returns></returns>
        public static string GetCodeMeaning(RETURN_CODE code) {

            switch (code) {
                case RETURN_CODE.RESP_OK:
                    return "Success";
                case RETURN_CODE.RESP_INIT_FAIL:
                    return "Driver or library initialization fail";
                case RETURN_CODE.RESP_NOT_SUPPORT:
                    return "This board doesn’t support MiAPI.";
                case RETURN_CODE.RESP_UNLOAD_FAIL:
                    return "Unload fail";
                case RETURN_CODE.RESP_READ_FAIL:
                    return "Read Fail";
                case RETURN_CODE.RESP_OLD_VERSION:
                    return "Motherboard does not (fully) support MiAPI Version";
                case RETURN_CODE.PCI_READ_FAIL:
                    return "PCI Read Fail";
                case RETURN_CODE.PCI_WRITE_FAIL:
                    return "PCI Write Fail";
                case RETURN_CODE.IO_READ_FAIL:
                    return "IO Read Fail";
                case RETURN_CODE.IO_WRITE_FAIL:
                    return "IO Write Fail";
                case RETURN_CODE.MEMORY_MAP_FAIL:
                    return "Memory Map Fail";
                case RETURN_CODE.MEMORY_READ_FAIL:
                    return "Memory Read Fail";
                case RETURN_CODE.MEMORY_WRITE_FAIL:
                    return "Memory Write Fail";
                case RETURN_CODE.WDT_GET_FAIL:
                    return "WDT Get Fail";
                case RETURN_CODE.WDT_SET_FAIL:
                    return "WDT Set Fail";
                case RETURN_CODE.GPIO_QUERY_FAIL:
                    return "GPIO Query Fail";
                case RETURN_CODE.GPIO_MUX_FAIL:
                    return "GPIO MUX Fail";
                case RETURN_CODE.GPIO_SETDIR_FAIL:
                    return "GPIO SetDir Fail";
                case RETURN_CODE.GPIO_GETSTATUS_FAIL:
                    return "GPIO GetStatus Fail";
                case RETURN_CODE.GPIO_SETSTATUS_FAIL:
                    return "GPIO SetStatus Fail";
                case RETURN_CODE.SMBUS_DEVICE_UNAVAILABLE:
                    return "SMBus Device Timeout";
                case RETURN_CODE.SMBUS_READBYTE_FAIL:
                    return "SMBus Invalid Parameter";
                case RETURN_CODE.SMBUS_WRITEBYTE_FAIL:
                    return "SMBus Unsupported";
                case RETURN_CODE.SMBUS_READBLOCK_FAIL:
                    return "SMBus Buffer too small";
                case RETURN_CODE.SMBUS_WRITEBLOCK_FAIL:
                    return "SMBus CRC Error";
                case RETURN_CODE.VGA_INVALID_RANGE:
                    return "";
                case RETURN_CODE.VGA_INIT_FAIL:
                    return "";
                case RETURN_CODE.VGA_QUERY_FAIL:
                    return "";
                case RETURN_CODE.VGA_GET_AMOUNT_OF_MONITORS_FAIL:
                    return "";
                case RETURN_CODE.VGA_GET_BRIGHTNESS_FAIL:
                    return "";
                case RETURN_CODE.VGA_SET_BRIGHTNESS_FAIL:
                    return "";
                case RETURN_CODE.VGA_GET_CONTRAST_FAIL:
                    return "";
                case RETURN_CODE.VGA_SET_CONTRAST_FAIL:
                    return "";
                case RETURN_CODE.VGA_SET_ORIENTATION_FAIL:
                    return "";
                case RETURN_CODE.VGA_DDCCI_UNSUPPORTED:
                    return "";
                case RETURN_CODE.FANSPEED_GET_FAIL:
                    return "";
                case RETURN_CODE.FANSPEED_SET_FAIL:
                    return "";
                case RETURN_CODE.VOLTAGE_GET_FAIL:
                    return "";
                case RETURN_CODE.TEMPERATURE_GET_FAIL:
                    return "";
                case RETURN_CODE.WIRELESS_INIT_FAIL:
                    return "";
                case RETURN_CODE.WIRELESS_QUERY_FAIL:
                    return "";
                case RETURN_CODE.WIRELESS_GET_AMOUNT_OF_ACCESS_POINT_FAIL:
                    return "";
                default:
                    return "UNKOWN";
            }
        }

        public enum RETURN_CODE : byte {

            RESP_OK = 0x00,
            RESP_INIT_FAIL,
            RESP_NOT_SUPPORT,
            RESP_UNLOAD_FAIL,
            RESP_READ_FAIL,
            RESP_OLD_VERSION,

            PCI_READ_FAIL = 0x11,
            PCI_WRITE_FAIL,
            IO_READ_FAIL,
            IO_WRITE_FAIL,
            MEMORY_MAP_FAIL,
            MEMORY_READ_FAIL,
            MEMORY_WRITE_FAIL,

            WDT_GET_FAIL = 0x21,
            WDT_SET_FAIL,

            GPIO_QUERY_FAIL = 0x31,
            GPIO_MUX_FAIL,
            GPIO_SETDIR_FAIL,
            GPIO_GETSTATUS_FAIL,
            GPIO_SETSTATUS_FAIL,

            SMBUS_DEVICE_UNAVAILABLE = 0x41,
            SMBUS_READBYTE_FAIL,
            SMBUS_WRITEBYTE_FAIL,
            SMBUS_READBLOCK_FAIL,
            SMBUS_WRITEBLOCK_FAIL,

            VGA_INVALID_RANGE = 0x51,
            VGA_INIT_FAIL,
            VGA_QUERY_FAIL,
            VGA_GET_AMOUNT_OF_MONITORS_FAIL,
            VGA_GET_BRIGHTNESS_FAIL,
            VGA_SET_BRIGHTNESS_FAIL,
            VGA_GET_CONTRAST_FAIL,
            VGA_SET_CONTRAST_FAIL,
            VGA_SET_ORIENTATION_FAIL,
            VGA_DDCCI_UNSUPPORTED,

            FANSPEED_GET_FAIL = 0x61,
            FANSPEED_SET_FAIL,
            VOLTAGE_GET_FAIL,
            TEMPERATURE_GET_FAIL,

            WIRELESS_INIT_FAIL = 0x71,
            WIRELESS_QUERY_FAIL,
            WIRELESS_GET_AMOUNT_OF_ACCESS_POINT_FAIL,
        }

        public enum FAN_TYPE : ushort {
            NONEFAN = 0,
            CPUFAN,
            SYSFAN,
            AUXFAN
        }

        /// <summary>
        /// Structure for GPIO status including its direction and voltage level.
        /// </summary>
        public struct GPIO_STATUS {
            /// <summary>
            /// GPIO status member to indicate input or output direction. 1 = Input ; 0 = Output.
            /// </summary>
            public byte Direction;
            /// <summary>
            /// GPIO status member to indicate pin high or low voltage level. 1 = High ; 0 = Low.
            /// </summary>
            public byte VoltageLevel;
        }

        public struct MONITOR_INFO {
            /// <summary>
            /// Display orientation degrees to set: <list type="bullet">
            /// <item>000: natural orientation of the display device.</item>
            /// <item>090: rotated 90 degrees in clockwise.</item>
            /// <item>180: rotated 180 degrees in clockwise.</item>
            /// <item>270: rotated 270 degrees in clockwise.</item>
            /// </list>
            /// </summary>
            public ushort Orientation;
            public uint DeviceIndex;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
            public string FriendlyDeviceName;
            public uint WMITotalBrightnessLevel;
        }

        public struct WIFI_INFO {
            public byte DeviceIndex;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string SSID;
            public uint SSIDLength;
            public byte RSSI_dBm;
            public byte RSSI_Percentage;
        }

        public struct MONITOR_BRIGHTNESS {
            public uint MinimumBrightness;
            public uint MaximumBrightness;
            public uint CurrentBrightness;
        }

        public struct MONITOR_CONTRAST {
            public uint MinimumContrast;
            public uint MaximumContrast;
            public uint CurrentContrast;
        }


        /// <summary>
        /// Initializes the MiAPI Library.
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        ///     <item><see cref="RETURN_CODE.RESP_OK"/> (0x00) - Success</item>
        ///     <item><see cref="RETURN_CODE.RESP_INIT_FAIL"/> (0x01) - Driver or library initialization fail</item>
        ///     <item><see cref="RETURN_CODE.RESP_NOT_SUPPORT"/> (0x02) - This board doesn’t support MiAPI.</item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Start")]
        static extern public RETURN_CODE Start();

        /// <summary>
        /// Exit the MiAPI Library.
        /// </summary>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Exit")]
        static extern public void Exit();

        /// <summary>
        /// Get MiAPI Version
        /// </summary>
        /// <param name="Major">Pointer to a variable containing the major version</param>
        /// <param name="Minor">Pointer to a variable containing the minor version</param>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_NOT_SUPPORT" /> (0x02) - This board doesn’t support MiAPI.</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OLD_VERSION" /> (0x05) - Motherboard support only limited or old features.</item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_GetMiAPIVersion")]
        static extern public RETURN_CODE GetMiAPIVersion([In, Out] ref ushort Major, [In, Out] ref ushort Minor);

        /// <summary>
        /// Get MiAPI Version
        /// </summary>
        /// <returns>
        /// A string of the version or an error description
        /// </returns>
        static public string GetMiAPIVersion() {
            ushort Major = 0, Minor = 0;
            RETURN_CODE code = GetMiAPIVersion(ref Major, ref Minor);
            switch (code) {
                case RETURN_CODE.RESP_OK:
                    return string.Format("{0}.{1}", Major, Minor);
                default:
                    return GetCodeMeaning(code);
            }
        }

        /// <summary>
        /// Get mother board BIOS version.
        /// </summary>
        /// <param name="BIOSVersion">Pointer to a string in which the BIOS version is returned.</param>
        /// <param name="Size">Pointer to a variable that specifies the length of the BIOSVersion string</param>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_READ_FAIL" /> (0x04) - Fail</item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_GetBIOSVersion")]
        static extern public RETURN_CODE GetBIOSVersion([In, Out] ref byte[] BIOSVersion, ref ushort Size);

        /// <summary>
        /// Get mother board product name.
        /// </summary>
        /// <param name="ProductName">Pointer to a string in which the product name is returned.</param>
        /// <param name="Size">Pointer to a variable that specifies the size of string to ProductName</param>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_READ_FAIL" /> (0x04) - Fail</item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_GetProductName")]
        static extern public RETURN_CODE GetProductName([In, Out] ref byte[] ProductName, ref ushort Size);
        /// <summary>
        /// Get mother board product name.
        /// </summary>
        /// <returns>
        /// A string of the product name or an error description
        /// </returns>
        static public string GetProductName() {
            byte[] PProductName = new byte[80];
            ushort Size = 0;
            RETURN_CODE code = GetProductName(ref PProductName, ref Size);
            switch (code) {
                case RETURN_CODE.RESP_OK:
                    return BitConverter.ToString(PProductName, 0, Size); // Marshal.PtrToStringAuto(PProductName, Size);
                default:
                    return GetCodeMeaning(code);
            }



            /*
            Hello, this is Bing. I found some possible solutions for your question.😊

            One way to retrieve a string from a char array in C# when calling a DLL is to use the **MarshalAs** attribute with the **UnmanagedType.LPStr** or **UnmanagedType.LPArray** enumeration¹³. For example:

    [DllImport("mydll.dll", CharSet = CharSet.Ansi)]
    public static extern int MiAPI_GetBIOSVersion([MarshalAs(UnmanagedType.LPStr)] out string BIOSVersion, ref uint Size);

            Another way is to use a** StringBuilder**object as the parameter type and pass it by reference². For example:

    [DllImport("mydll.dll", CharSet = CharSet.Ansi)]
            public static extern int MiAPI_GetProductName(ref StringBuilder ProductName, ref uint Size);

            I hope this helps.Do you have any other questions?

Quelle:
            Unterhaltung mit Bing, 4.8.2023
(1) Passing a char array from c# to c++ dll - Stack Overflow. https://stackoverflow.com/questions/38146181/passing-a-char-array-from-c-sharp-to-c-dll.
(2) pinvoke - char array marshaling in C# - Stack Overflow. https://stackoverflow.com/questions/17480193/char-array-marshaling-in-c-sharp.
(3) c# - DllImport and char* - Stack Overflow. https://stackoverflow.com/questions/2568436/dllimport-and-char.
            */

        }

        /// <summary>
        /// Start the watchdog timer.
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.WDT_SET_FAIL" /> (0x22) - Fail</item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Watchdog_Start")]
        static extern public RETURN_CODE Watchdog_Start();

        /// <summary>
        /// Get the minimum, maximum and current values of the watchdog timer.
        /// </summary>
        /// <param name="Minimum">Pointer to a variable containing the minimum timeout value in seconds.</param>
        /// <param name="Maximum">Pointer to a variable containing the maximum timeout value in seconds.</param>
        /// <param name="Current">Pointer to a variable containing the current count of the timer in seconds.</param>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_NOT_SUPPORT"/> (0x02) - Watchdog doesn’t support.</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.WDT_GET_FAIL"/> (0x21) - Fail </item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Watchdog_GetRange")]
        static extern public RETURN_CODE Watchdog_GetRange(ushort* Minimum, ushort* Maximum, ushort* Current);

        /// <summary>
        /// Set watchdog timer with specified timeout value and define the action to reboot or trigger a WD_TIME pin when expired.
        /// </summary>
        /// <param name="Timeout">Specifies a value in seconds for the watchdog timeout.</param>
        /// <param name="Reboot">True to reboot system when expired; False to trigger a low pulse on MiAPI WD_TIME pin.</param>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.WDT_SET_FAIL" /> (0x22) - Fail</item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Watchdog_SetConfig")]
        static extern public RETURN_CODE Watchdog_SetConfig(ushort Timeout, bool Reboot);

        /// <summary>
        /// Reset the watchdog timer to the timeout value set by MiAPI_Watchdog_SetConfig. It is always inserted in application main loop to prevent watchdog expires.
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.WDT_SET_FAIL" /> (0x22) - Fail</item>
        /// </list>
        /// Remarks:
        /// It is better for users to set a longer 1.5~2 times timeout than user’s service loop. Once system busy causes user service delays, it will be a safe tolerance for application refreshing the timer before watchdog expires.
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Watchdog_Refresh")]
        static extern public RETURN_CODE Watchdog_Refresh();

        /// <summary>
        /// Disable the watchdog timer.
        /// </summary>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.WDT_SET_FAIL" /> (0x04) - Fail</item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Watchdog_Disable")]
        static extern public RETURN_CODE Watchdog_Disable();

        /// <summary>
        /// A general-purpose input/output (GPIO) is an uncommitted digital signal pin on an integrated circuit or electronic circuit board whose behavior—including whether it acts as input or output—is controllable by the user at run time.
        /// </summary>
        /// <param name="PinNum">Pin index to get the status from</param>
        /// <param name="GPIOStatus">Pointer to a structure for <see cref="GPIO_STATUS">GPIO status</see> including its direction and voltage level.</param>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.GPIO_GETSTATUS_FAIL" /> (0x34) - Fail</item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_GPIO_GetStatus")]
        static extern public RETURN_CODE GPIO_GetStatus(byte PinNum, GPIO_STATUS* GPIOStatus);

        /// <summary>
        /// Set one GPIO output pin as status high or low.
        /// </summary>
        /// <param name="PinNum">Pin index to set</param>
        /// <param name="GPIOStatus">Pointer to a structure for <see cref="GPIO_STATUS">GPIO status</see> including its direction and voltage level.</param>
        /// <returns>
        /// <list type="bullet">
        ///   <item>
        ///     <see cref="RETURN_CODE.RESP_OK" /> (0x00) - Success</item>
        ///   <item>
        ///     <see cref="RETURN_CODE.GPIO_SETSTATUS_FAIL" /> (0x35) - Fail</item>
        /// </list>
        /// </returns>
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SetStatus")]
        static extern public RETURN_CODE GPIO_SetStatus(byte PinNum, GPIO_STATUS GPIOStatus);


        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SMBusReadQuick")]
        static extern public RETURN_CODE SMBusReadQuick(byte SlaveAddress);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SMBusWriteQuick")]
        static extern public RETURN_CODE SMBusWriteQuick(byte SlaveAddress);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_ReceiveByte")]
        static extern public RETURN_CODE SMBusReceiveByte(byte SlaveAddress, byte* Result);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SMBusSendByte")]
        static extern public RETURN_CODE SMBusSendbyte(byte SlaveAddress, byte Result);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SMBusReadByte")]
        static extern public RETURN_CODE SMBusReadByte(byte SlaveAddress, byte RegisterOffset, byte* Result);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SMBusWriteByte")]
        static extern public RETURN_CODE SMBusWriteByte(byte SlaveAddress, byte RegisterOffset, byte Result);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SMBusReadWord")]
        static extern public RETURN_CODE SMBusReadWord(byte SlaveAddress, byte RegisterOffset, ushort* Result);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SMBusWriteWord")]
        static extern public RETURN_CODE SMBusWriteWord(byte SlaveAddress, byte RegisterOffset, ushort Result);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SMBusReadBlock")]
        static extern public RETURN_CODE SMBusReadBlock(byte SlaveAddress, byte RegisterOffset, byte* Result, uint* Count);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SMBusWriteBlock")]
        static extern public RETURN_CODE SMBusWriteBlock(byte SlaveAddress, byte RegisterOffset, byte* Result, uint Count);

        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Display_GetAmountOfMonitors")]
        static extern public RETURN_CODE Display_GetAmountOfMonitors(ushort* AmountOfMonitors);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Display_GetMonitorInfo")]
        static extern public RETURN_CODE Display_GetMonitorInfo(MONITOR_INFO* MonitorInfo, ushort Index);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Display_GetMonitorCap")]
        static extern public RETURN_CODE Display_GetMonitorCapabilites(ushort* MonitorCapabilities, uint Index);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Display_Rescan")]
        static extern public void Display_Rescan();
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Display_GetBrightness")]
        static extern public RETURN_CODE Display_GetBrightness(MONITOR_BRIGHTNESS* Brightness, ushort Index);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Display_SetBrightness")]
        static extern public RETURN_CODE Display_SetBrightness(ushort NewBrightness, ushort Index);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_DisplaySetBrightness")]
        static extern public RETURN_CODE Display_GetContrast(MONITOR_CONTRAST* Contrast, ushort Index);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_DisplaySetContrast")]
        static extern public RETURN_CODE Display_SetContrast(ushort NewContrast, ushort Index);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SetOrientation")]
        static extern public RETURN_CODE Display_SetOrientation(ushort Orientation, ushort Index);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Display_On")]
        static extern public void Display_On();
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Display_Off")]
        static extern public void Display_Off();

        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_GetFanSpeed")]
        static extern public RETURN_CODE GetFanSpeed(FAN_TYPE FanType, ushort* Retval, ushort* TypeSupport);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_SetFanSpeed")]
        static extern public RETURN_CODE SetFanSpeed(ushort FanType, ushort SetVal, ushort* TypeSupport);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_GetTemperature")]
        static extern public RETURN_CODE GetTemperature(ushort TempType, ushort* RetVal, ushort* TypeSupport);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_GetVoltage")]
        static extern public RETURN_CODE GetVoltage(uint VoltType, ushort* RetVal, ushort* TypeSupport);

        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Wireless_GetAmountOfAccessPoint")]
        static extern public RETURN_CODE Wireless_GetAmountOfAccessPoints(byte* AmountOfAccessPoint);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Wireless_GetWiFiStatus")]
        static extern public RETURN_CODE Wireless_GetWiFiStatus(WIFI_INFO* WiFiInfo, uint Index);
        [DllImport("MiAPI.dll", EntryPoint = "MiAPI_Wireless_RequeryWiFiStatus")]
        static extern public RETURN_CODE Wireless_RequeryWiFiStatus();
    }
}
