
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
namespace ImageWizard
{
    internal unsafe static class Win32Api
    {

        [DllImport("Gdi32.dll", SetLastError = true)]
        internal static extern IntPtr CreateDIBSection(IntPtr Hdc, ref Win32Structure.BITMAPINFO BmpInfo, uint iUsage, out IntPtr ppvBits, IntPtr hSection, uint dwOffset);

        [DllImport("Gdi32.dll", SetLastError = true)]
        public static extern unsafe uint SetDIBColorTable(IntPtr Hdc, int un1, int un2, IntPtr Palette);

        //[DllImport("Gdi32.dll", SetLastError = true)]
        //public static extern uint SetDIBColorTable(IntPtr Hdc, int un1, int un2, RGBQUAD[] pcRGBQUAD);

        [DllImport("Gdi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr Object);
        
        [DllImport("Gdi32.dll", SetLastError = true)]
        internal extern static IntPtr SelectObject(IntPtr Hdc,IntPtr Object);

        [DllImport("Gdi32.dll", SetLastError = true)]
        internal extern static IntPtr CreateCompatibleDC(IntPtr Hdc);

        [DllImport("Gdi32.dll",SetLastError=true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        internal extern static Boolean DeleteDC(IntPtr Hdc);

        [DllImport ("User32.dll",SetLastError=true)]
        internal extern static IntPtr GetDC(IntPtr Hwnd);

        [DllImport ("User32.dll",SetLastError =true)]
        internal extern static int ReleaseDC(IntPtr Hwnd,IntPtr Hdc);

        [DllImport ("Gdi32.dll",SetLastError=true)]
        internal extern static uint BitBlt(IntPtr DestDC, int DestX, int DestY, int DestWidth, int DestHeight, IntPtr SrcDC, int SrcX, int SrcY, uint Rop);

        [DllImport ("Gdi32.dll",SetLastError=true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        internal extern static Boolean StretchBlt(IntPtr Hdc,int DestX,int DestY,int DestWidth,int DestHeight,IntPtr SrcDC,int SrcX,int SrcY,int SrcWidth,int SrcHeight,uint Rop);
        
        [DllImport("Gdi32.dll", SetLastError = true)]
        public static extern int SetStretchBltMode(IntPtr hDC, uint StrechMode);

        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = true)]
        internal static extern void CopyMemory(IntPtr Dest, IntPtr src, int Length);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern void ZeroMemory(IntPtr handle, uint length);

        [DllImport ("Kernel32.dll",EntryPoint="RtlFillMemory",SetLastError=true)]
        internal static extern void FillMemory(IntPtr Dest,int Length,byte Value);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GlobalAlloc(uint Flags, int Length);

        [DllImport ("User32.dll",SetLastError =true)]
        internal extern static IntPtr GlobalFree(IntPtr Handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal extern static IntPtr GlobalLock(IntPtr Handle);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal extern static IntPtr GlobalUnlock(IntPtr Handle);

        [DllImport ("User32.dll",SetLastError =true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        internal extern static Boolean OpenClipboard(IntPtr hWnd);

        [DllImport ("User32.dll",SetLastError =true)]
        internal extern static IntPtr SetClipboardData(uint Format, IntPtr Mem);

        [DllImport("user32", SetLastError = true)]
        public static extern IntPtr GetClipboardData(uint Format);

        [DllImport("user32", SetLastError = true)]
        public static extern int IsClipboardFormatAvailable(uint Format);

        [DllImport ("User32.dll",SetLastError =true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        internal extern static Boolean EmptyClipboard();

        [DllImport ("User32.dll",SetLastError =true)]
        [return:MarshalAs(UnmanagedType.Bool)]
        internal extern static Boolean CloseClipboard();

    }

}


