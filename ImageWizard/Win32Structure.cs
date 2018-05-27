using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ImageWizard
{ 
    internal static class Win32Structure
    {
        [StructLayout(LayoutKind.Sequential)]
        internal struct RGBQUAD
        {
            internal byte Blue;
            internal byte Green;
            internal byte Red;
            internal byte Reserved;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct BITMAPINFOHEADER
        {
            internal uint Size;
            internal int Width;
            internal int Height;
            internal ushort Planes;
            internal ushort BitCount;
            internal uint Compression;
            internal uint SizeImage;
            internal int XPelsPerMeter;
            internal int YPelsPerMeter;
            internal uint ClrUsed;
            internal uint ClrImportant;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct BITMAPINFO
        {
            internal BITMAPINFOHEADER Header;
            internal RGBQUAD Colors;
        }
    }
}
