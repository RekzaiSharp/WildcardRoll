using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

public class Memory
{
    private IntPtr pHandel = IntPtr.Zero;
    private Process attachedProcess = null;
    private byte[] buffer;

    public void Open_pHandel(IntPtr handle)
    {
        pHandel = handle;
    }

    #region x86
    #region Write
    public bool WriteBytes(int address, byte[] value, bool virtualProtect = false)
    {
        int countOfUsed = 0;
        bool result = false;
        uint oldProtection = 0;


        if (virtualProtect)
            Win32.x86.VirtualProtectEx(pHandel, address, (uint)value.Length, 0x40, out oldProtection);

        result = Win32.x86.WriteProcessMemory(pHandel, address, value, (uint)value.Length, out countOfUsed);

        if (virtualProtect)
            Win32.x86.VirtualProtectEx(pHandel, address, (uint)value.Length, oldProtection, out oldProtection);

        return result;
    }

    public bool WriteInt(int address, int value, bool virtualProtect = false)
    {
        buffer = BitConverter.GetBytes(value);
        return WriteBytes(address, buffer, virtualProtect);
    }

    public bool WriteByte(int address, byte value, bool virtualProtect = false)
    {
        buffer = new byte[] { value };
        return WriteBytes(address, buffer, virtualProtect);
    }

    public bool WriteString(int address, string value, bool virtualProtect = false)
    {
        buffer = Encoding.ASCII.GetBytes(value);//No unicode support atm
        return WriteBytes(address, buffer, virtualProtect);
    }

    public bool WriteFloat(int address, float value, bool virtualProtect = false)
    {
        buffer = BitConverter.GetBytes(value);
        return WriteBytes(address, buffer, virtualProtect);
    }
    #endregion
    #region Read
    public byte[] ReadBytes(int address, int length)
    {
        byte[] readBytes = new byte[length];
        int numBytesChanged = 0;

        Win32.x86.ReadProcessMemory(pHandel, address, readBytes, (uint)length, out numBytesChanged);

        return readBytes;
    }

    public int ReadInt(int address)
    {
        return BitConverter.ToInt32(ReadBytes(address, 4), 0);
    }

    public int ReadUShort(int address)
    {
        return BitConverter.ToUInt16(ReadBytes(address, 2), 0);
    }

    public byte ReadByte(int address)
    {
        return ReadBytes(address, 1)[0];
    }

    public string ReadString(int address, int length)
    {//No unicode support
        return Encoding.ASCII.GetString(ReadBytes(address, length));
    }

    public string ReadStringAdvanced(int address, int maxStringLength = 1000)
    {//No unicode support
        string result = null;
        byte currentByte = 0;

        for (int i = 0; i < maxStringLength; i++)
        {
            currentByte = ReadByte(address + i);

            if (currentByte == 0x00)
                break;
            else
                result += (char)currentByte;
        }

        return result;
    }

    public float ReadFloat(int address)
    {
        return BitConverter.ToSingle(ReadBytes(address, 4), 0);
    }
    #endregion
    #region Scans
    public int PatternScan(string pattern)
    {
        string[] splitPattern = pattern.Split(' ');
        bool[] indexValid = new bool[splitPattern.Length];
        byte[] indexValue = new byte[splitPattern.Length];

        byte tempByte = (byte)0x00;

        for (int i = 0; i < splitPattern.Length; i++)
        {
            indexValid[i] = !(splitPattern[i] == "??" || splitPattern[i] == "?");
            if (Byte.TryParse(splitPattern[i], out tempByte))
                indexValue[i] = tempByte;
            else
                indexValid[i] = false;
        }

        int startOfMemory = attachedProcess.MainModule.BaseAddress.ToInt32();
        int endOfMemory = attachedProcess.MainModule.ModuleMemorySize;

        for (int currentMemAddy = startOfMemory; currentMemAddy < endOfMemory; currentMemAddy++)
        {
            bool complete = false;
            for (int i = 0; i < splitPattern.Length; i++)
            {
                if (!indexValid[i])
                    continue;

                tempByte = ReadByte(currentMemAddy + i);

                if (tempByte != indexValue[i])
                    break;

                if (i == splitPattern.Length - 1)
                    complete = true;

                if (complete)
                    break;
            }

            if (complete)
                return currentMemAddy;
        }

        throw new Exception("Pattern not found!");
        return 0;
    }
    #endregion
    #endregion

    private static class Win32
    {
        public static class NativeMethods
        {
            #region IsWow64Process
            public static bool IsWow64Process(IntPtr handel)
            {
                bool isTarget64Bit = false;
                IsWow64Process(handel, out isTarget64Bit);
                return isTarget64Bit;
            }

            [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
            [return: MarshalAs(UnmanagedType.Bool)]
            private static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
            #endregion
        }

        public static class x64
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool VirtualProtectEx(IntPtr hProcess, long lpAddress, UInt32 dwSize, uint flNewProtect, out uint lpflOldProtect);

            [DllImport("kernel32.dll")]
            public static extern bool ReadProcessMemory(IntPtr hProcess, long lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out int lpNumberOfBytesWritten);

            [DllImport("kernel32.dll")]
            public static extern bool WriteProcessMemory(IntPtr hProcess, long lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out int lpNumberOfBytesWritten);
        }

        public static class x86
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            public static extern bool VirtualProtectEx(IntPtr hProcess, int lpAddress, UInt32 dwSize, uint flNewProtect, out uint lpflOldProtect);

            [DllImport("kernel32.dll")]
            public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out int lpNumberOfBytesWritten);

            [DllImport("kernel32.dll")]
            public static extern bool WriteProcessMemory(IntPtr hProcess, int lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out int lpNumberOfBytesWritten);
        }
    }
}