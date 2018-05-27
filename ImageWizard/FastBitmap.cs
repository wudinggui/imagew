using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing;

namespace ImageWizard
{
    public unsafe class FastBitmap : IDisposable
    {
        private static int m_Width = 0;
        private static int m_Height = 0;
        private static int m_Stride = 0;
        private static ushort m_BitCount = 0;
        private static IntPtr m_Hdc = IntPtr.Zero;
        private static IntPtr m_Handle = IntPtr.Zero;
        private static IntPtr m_Pointer = IntPtr.Zero;
        private static IntPtr m_PaletteAddress = IntPtr.Zero;
        private static int m_PaletteSize = 0;

        public int Width { get { return m_Width; } }
        public int Height { get { return m_Height; } }
        public int Stride { get { return m_Stride; } }
        public ushort BitCount { get { return m_BitCount; } }
        public IntPtr Handle { get { return m_Handle; } }
        public IntPtr Hdc { get { return m_Hdc; } }
        public IntPtr Pointer { get { return m_Pointer; } }
        public IntPtr PaletteAddress { get { return m_PaletteAddress; } }
        public int PaletteSize { get { return m_PaletteSize; } }

        ~FastBitmap()
        {
            Dispose();
        }

        public Boolean AllocateBitmap(int BmpWidth, int BmpHeight, ushort BitCount)//在内存中创建位图
        {
            if ((BitCount != 1) & (BitCount != 4) & (BitCount != 8) & (BitCount != 16) & (BitCount != 24) & (BitCount != 32))
            {
                return false;
            }
            Dispose(false);
            Win32Structure.BITMAPINFO BmpInfo = new Win32Structure.BITMAPINFO();
            BmpInfo.Header.Size = (uint)sizeof(Win32Structure.BITMAPINFOHEADER);
            BmpInfo.Header.Width = BmpWidth;
            BmpInfo.Header.Height = -BmpHeight;
            BmpInfo.Header.BitCount = BitCount;
            BmpInfo.Header.Planes = 1;
            BmpInfo.Header.Compression = Win32Const.BI_RGB;
            BmpInfo.Header.XPelsPerMeter = 96;
            BmpInfo.Header.YPelsPerMeter = 96;
            BmpInfo.Header.ClrUsed = 0;
            BmpInfo.Header.SizeImage = 0;
            BmpInfo.Header.ClrImportant = 0;
            BmpInfo.Header.SizeImage = 0;
            IntPtr ScreecDC = Win32Api.GetDC(IntPtr.Zero);
            m_Hdc = Win32Api.CreateCompatibleDC(ScreecDC);
            int Result = Win32Api.ReleaseDC(IntPtr.Zero, ScreecDC);
            m_Handle = Win32Api.CreateDIBSection(m_Hdc, ref BmpInfo, Win32Const.DIB_RGB_COLORS, out m_Pointer, IntPtr.Zero, 0);

            if (m_Handle == IntPtr.Zero)
            {
                m_Hdc = IntPtr.Zero; m_Width = 0; m_Height = 0; m_Stride = 0; m_BitCount = BitCount;
                return false;
            }
            else
            {
                Win32Api.SelectObject(m_Hdc, m_Handle);
                m_Width = BmpWidth; m_Height = BmpHeight; m_BitCount = BitCount;
                switch (BitCount)
                {
                    case 1: m_Stride = (int)(((m_Width + 7) / 8 + 3) & 0XFFFFFFFC); break;
                    case 4: m_Stride = (int)(((m_Width + 1) / 2 + 3) & 0XFFFFFFFC); break;
                    case 8: m_Stride = (int)((m_Width + 3) & 0XFFFFFFFC); break;
                    case 16: m_Stride = (int)((m_Width * 2 + 3) & 0XFFFFFFFC); break;
                    case 24: m_Stride = (int)((m_Width * 3 + 3) & 0XFFFFFFFC); break;
                    case 32: m_Stride = m_Width * 4; break;
                }
                if (m_BitCount <= 8)
                {
                    m_PaletteAddress = Marshal.AllocHGlobal(1024);
                    m_PaletteSize = 1 << (m_BitCount);
                }
                return true;
            }
        }

        public Boolean LoadImageFormFile(string FileName)
        {
            Bitmap bmp = (Bitmap)Bitmap.FromFile(FileName, false);
            BitmapData BmpData = new BitmapData();
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                    AllocateBitmap(bmp.Width, bmp.Height, 1); break;
                case PixelFormat.Format4bppIndexed:
                    AllocateBitmap(bmp.Width, bmp.Height, 4); break;
                case PixelFormat.Format8bppIndexed:
                    AllocateBitmap(bmp.Width, bmp.Height, 8); break;
                case PixelFormat.Format24bppRgb:
                    AllocateBitmap(bmp.Width, bmp.Height, 24); break;
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    AllocateBitmap(bmp.Width, bmp.Height, 32); break;
                default:
                    bmp.Dispose();
                    return false;
            }
            BmpData.Stride = m_Stride;
            BmpData.Scan0 = m_Pointer;
            bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly | ImageLockMode.UserInputBuffer, bmp.PixelFormat, BmpData);
            switch (bmp.PixelFormat)
            {
                case PixelFormat.Format1bppIndexed:
                case PixelFormat.Format4bppIndexed:
                case PixelFormat.Format8bppIndexed:
                    m_PaletteSize = bmp.Palette.Entries.Length;
                    byte* Palette = (byte*)m_PaletteAddress;
                    for (int x = 0; x < m_PaletteSize; x++)
                    {
                        *Palette = bmp.Palette.Entries[x].B;
                        *(Palette + 1) = bmp.Palette.Entries[x].G;
                        *(Palette + 2) = bmp.Palette.Entries[x].R;      //RGBQUAD和Color两者的结构是相反的
                        *(Palette + 3) = bmp.Palette.Entries[x].A;
                        Palette += 4;
                    }
                    Win32Api.SetDIBColorTable(m_Hdc, 0, m_PaletteSize, m_PaletteAddress);
                    break;
                default:
                    break;
            }
            bmp.UnlockBits(BmpData);
            bmp.Dispose();
            return false;
        }

        public Boolean CopyToClipBoard(Boolean CompatibleWithPhotoShop = true)
        {
            if (m_Handle == IntPtr.Zero) return false;

            Win32Structure.BITMAPINFOHEADER BmpInfo = new Win32Structure.BITMAPINFOHEADER();
            IntPtr MemPointer, Buffer;
            int BufferSize = 0;

            //   FastBitmap本身的数据顶点数据位于屏幕的左上角，因此设置为负值则后面的图像数据无需单独行拷贝  
            //   但是PhotoShop不支持这种格式。
            BmpInfo.Height = (CompatibleWithPhotoShop == true ? m_Height : -m_Height);

            BmpInfo.Width = m_Width;
            BmpInfo.Compression = Win32Const.BI_RGB;
            BmpInfo.Planes = 1;
            BmpInfo.Size = 40;
            BmpInfo.BitCount = m_BitCount;
            BmpInfo.ClrUsed = (uint)m_PaletteSize;                           // 增加这一句，否则从剪贴板获取数据就错位（mspaint的黏贴也不对）

            switch (m_BitCount)
            {
                case 1:
                case 4:
                case 8:
                    BufferSize = 40 + m_PaletteSize * 4 + m_Stride * m_Height;          // 文件头+调色板+图像数据
                    break;
                case 24:
                case 32:
                    BufferSize = 40 + m_Stride * m_Height;                                // 文件头+图像数据
                    break;
                default:
                    return false;
            }
            Buffer = Win32Api.GlobalAlloc(Win32Const.GMEM_MOVEABLE, BufferSize);    // 给全局内存对象分配全局内存 ，必须使用GMEM_MOVEABLE标志  http://topic.csdn.net/u/20070122/10/cde3555b-c4da-40ca-8fd9-a5c05fcf75df.html
            if (Buffer == IntPtr.Zero) return false;
            MemPointer = Win32Api.GlobalLock(Buffer);                               // 通过给全局内存对象加锁获得对全局内存块的引用

            Marshal.StructureToPtr(BmpInfo, MemPointer, true);                      // 封送结构体到非托管内存中

            switch (m_BitCount)
            {
                case 1:
                case 4:
                case 8:
                    Win32Api.CopyMemory(MemPointer + 40, m_PaletteAddress, m_PaletteSize * 4);               // 拷贝调色板数据
                    if (CompatibleWithPhotoShop == true)
                    {
                        for (int Y = 0; Y < Height; Y++)
                            Win32Api.CopyMemory(MemPointer + m_PaletteSize * 4 + 40 + Y * Stride, m_Pointer + Stride * (Height - 1 - Y), Stride);
                    }
                    else
                        Win32Api.CopyMemory(MemPointer + m_PaletteSize * 4 + 40, m_Pointer, m_Stride * m_Height);  // 拷贝图像数据
                    break;
                case 24:
                case 32:
                    if (CompatibleWithPhotoShop == true)
                    {
                        for (int Y = 0; Y < Height; Y++)
                            Win32Api.CopyMemory(MemPointer + 40 + Stride * Y, Pointer + Stride * (Height - 1 - Y), Stride);
                    }
                    else
                        Win32Api.CopyMemory(MemPointer + 40, m_Pointer, m_Stride * m_Height);   // 拷贝图像数据
                    break;
                default:
                    return false;
            }
            Win32Api.GlobalUnlock(Buffer);                                          // 使用完全局内存块后需要对全局内存块解锁
            if (Win32Api.OpenClipboard(IntPtr.Zero) == true)                        // 以当前的任务或者说是进程来打开剪贴板。返回值不为0则表示打开成功
            {
                Win32Api.EmptyClipboard();                                          // 这个函数将清空剪贴板，并释放剪贴板中数据的句柄，然后将剪贴板的所有权分配给当前打开剪贴板的窗口
                Win32Api.SetClipboardData(Win32Const.CF_DIB, Buffer);
                Win32Api.CloseClipboard();                                           //GlobalFree Buffer 这个释放就交给系统去释放了，千万不要在这里释放，否则会造成一些莫名其妙的错误
                return true;
            }
            else
            {
                Win32Api.CloseClipboard();
                return false;
            }
        }

        public Boolean LoadPictureFromClipBoard()
        {
            if (Win32Api.OpenClipboard(IntPtr.Zero) == true)                        // 以当前的任务或者说是进程来打开剪贴板。返回值不为0则表示打开成功
            {
                Win32Structure.BITMAPINFOHEADER BmpInfo = new Win32Structure.BITMAPINFOHEADER();
                IntPtr MemPointer, Buffer;
                if (Win32Api.IsClipboardFormatAvailable(Win32Const.CF_DIB) != 0)    // 剪贴板中有DIB图像数据
                {
                    Buffer = Win32Api.GetClipboardData(Win32Const.CF_DIB);          // 得到DIB数据的起始地址
                    MemPointer = Win32Api.GlobalLock(Buffer);
                    BmpInfo = (Win32Structure.BITMAPINFOHEADER)Marshal.PtrToStructure(MemPointer, typeof(Win32Structure.BITMAPINFOHEADER));       // 复制数据带结构体
                    AllocateBitmap(BmpInfo.Width, Math.Abs(BmpInfo.Height), (ushort)BmpInfo.BitCount);
                    if (BmpInfo.BitCount <= 8)
                    {
                        m_PaletteSize = BmpInfo.ClrUsed == 0 ? (1 << (BmpInfo.BitCount)) : (int)BmpInfo.ClrUsed;    // 得到实际的调色板的数量
                        Win32Api.CopyMemory(m_PaletteAddress, MemPointer + 40, m_PaletteSize * 4);                      // 复制剪贴板数据
                        if (BmpInfo.Height < 0)                                                                         // FastBitmap的图像复制容许这个值为负值
                            Win32Api.CopyMemory(m_Pointer, MemPointer + 40 + m_PaletteSize * 4, m_Stride * m_Height);   // 复制图像数据
                        else
                        {
                            for (int Y = 0; Y < m_Height; Y++)                                                          // 调整行序
                                Win32Api.CopyMemory(m_Pointer + Y * m_Stride, MemPointer + 40 + m_PaletteSize * 4 + (m_Height - 1 - Y) * m_Stride, m_Stride);
                        }
                        Win32Api.SetDIBColorTable(m_Hdc, 0, m_PaletteSize, m_PaletteAddress);                           // 设置DC的调色板
                    }
                    else
                    {
                        if (BmpInfo.Height < 0)
                            Win32Api.CopyMemory(m_Pointer, MemPointer + 40, m_Stride * m_Height);                       // 24/32位图像无调色板，直接复制数据
                        else
                        {
                            for (int Y = 0; Y < m_Height; Y++)
                                Win32Api.CopyMemory(m_Pointer + Y * m_Stride, MemPointer + 40 + (m_Height - 1 - Y) * m_Stride, m_Stride);
                        }
                    }
                    Win32Api.GlobalUnlock(Buffer);
                }
                Win32Api.CloseClipboard();
            }
            return true;
        }

        public int GetImageUesdColorAmount()
        {
            if (m_Handle == IntPtr.Zero) return 0;
            int X, Y, Count=0;
            byte* Scan0, FlagPos, HistPtr;
            switch (m_BitCount)
            {
                case 1:
                    Count = 2;
                    break;
                case 4:
                    HistPtr = (byte*)Marshal.AllocHGlobal(16);
                    for (Y = 0; Y < m_Height; Y++)
                    {
                        Scan0 = (byte*)m_Pointer + m_Stride * Y;
                        for (X = 0; X < m_Width; X++)
                        {
                            if ((X & 1) == 0)
                            {
                                (*(HistPtr + (*Scan0 & 15)))=1;             //通过计算调色板获取不通的像素数
                                Scan0++;
                            }
                            else
                                (*(HistPtr + (*Scan0 >> 4)))=1;
                        }
                    }
                    for (Y=0;Y<16;Y++)
                        if(*(HistPtr +Y)==1) Count++;
                    Marshal.FreeHGlobal((IntPtr)HistPtr);       
                    break;
                case 8:
                    HistPtr = (byte*)Marshal.AllocHGlobal(256);
                    for (Y = 0; Y < m_Height; Y++)
                    {
                        Scan0 = (byte*)m_Pointer + m_Stride * Y;
                        for (X = 0; X < m_Width; X++)
                        {
                            (*(HistPtr + *Scan0))=1;
                            Scan0++;
                        }
                    }
                    for (Y=0;Y<256;Y++)
                        if(*(HistPtr +Y)==1) Count++;
                    Marshal.FreeHGlobal((IntPtr)HistPtr);
                    break;
                case 24:
                case 32:
                    int Index,BitValue;
                    int BytePerPixel = m_BitCount >> 3;
                    byte* FlagPtr = (byte*)Marshal.AllocHGlobal(2097152);
                    for (Y=0;Y<m_Height;Y++)
                    {
                        Scan0 = (byte *) m_Pointer + m_Stride * Y;      
                        for (X = 0; X < m_Width; X++)
                        {
                            Index = *(Scan0) + (*(Scan0 + 1) << 8) + (*(Scan0 + 2) << 16);      //未考虑ALPHA
                            FlagPos = FlagPtr+ (Index >> 3);
                            BitValue = 128>>(Index & 7);
                            if ((*FlagPos & BitValue) == 0)                                     //通过每个位的信息来确定那个索引是否存在了
                            {
                                *(FlagPos) ^= (byte)BitValue;
                                Count++;
                            }
                            Scan0 += BytePerPixel;
                        }
                    }
                    Marshal.FreeHGlobal((IntPtr) FlagPtr); 
                    break;
                default:
                    break;
            }
            return Count;
        }

        public Boolean FlipHorizontal()
        {
            if (m_Handle == IntPtr.Zero) return false ;
            int X, Y, HalfWidth = m_Width / 2;
            byte Blue,Green,Red;
            byte* RowLeft,RowRight;
            switch (m_BitCount)
            { 
                case 1:
                case 4:
                    BitmapData BmpData = new BitmapData();
                    Bitmap GdiPbmp = new Bitmap(m_Width, m_Height, m_Stride, (m_BitCount==1 ?  PixelFormat.Format1bppIndexed:PixelFormat.Format4bppIndexed), m_Pointer);
                    GdiPbmp.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    BmpData.Stride = m_Stride;
                    BmpData.Scan0 = m_Pointer;
                    GdiPbmp.LockBits(new Rectangle(0, 0, m_Width, m_Height), ImageLockMode.ReadOnly | ImageLockMode.UserInputBuffer, (m_BitCount == 1 ? PixelFormat.Format1bppIndexed : PixelFormat.Format4bppIndexed), BmpData);
                    GdiPbmp.UnlockBits(BmpData);
                    GdiPbmp.Dispose();                  //这里用GDI+的速度要比StretchBlt快很多，不知道为什么
                    //Win32Api.StretchBlt( m_Hdc, m_Width, 0, -m_Width, m_Height, m_Hdc, 0, 0, m_Width, m_Height, Win32Const.SRCCOPY);
                    break;
                case 8:
                    for (Y = 0; Y < m_Height; Y++)
                    {
                        RowLeft = (byte*)m_Pointer + m_Stride * Y;
                        RowRight = RowLeft + (m_Width - 1);
                        for (X = 0; X < HalfWidth; X++)
                        {
                            Blue = *RowLeft;
                            *(RowLeft) = *(RowRight);
                            *(RowRight) = Blue;
                            RowLeft ++;
                            RowRight --;
                        }
                    }
                    break;
                case 24:
                    for (Y=0;Y<m_Height;Y++)
                    {
                        RowLeft = (byte *) m_Pointer + m_Stride * Y;
                        RowRight = RowLeft + (m_Width - 1) * 3;
                        for (X = 0; X < HalfWidth; X++)
                        {
                            Blue            =   *RowLeft        ;
                            Green           =   *(RowLeft+1)    ;
                            Red             =   *(RowLeft+2)    ;
                            *(RowLeft)      =   *(RowRight)     ;
                            *(RowLeft+1)    =   *(RowRight+1)   ;
                            *(RowLeft+2)    =   *(RowRight+2)   ;
                            *(RowRight)     =   Blue            ;
                            *(RowRight+1)   =   Green           ;
                            *(RowRight+2)   =   Red             ;
                            RowLeft         +=  3               ;
                            RowRight        -=  3               ;  
                        }
                    }
                    break;
                case 32:
                    int* RowLeftInt, RowRightInt;
                    int Pixel;
                    for (Y = 0; Y < m_Height; Y++)
                    {
                        RowLeftInt = (int*)m_Pointer + m_Width * Y;
                        RowRightInt = RowLeftInt + (m_Width - 1);
                        for (X = 0; X < HalfWidth; X++)
                        {
                            Pixel = *RowLeftInt;
                            *(RowLeftInt) = *(RowRightInt);
                            *(RowRightInt) = Pixel;
                            RowLeftInt ++;
                            RowRightInt --;
                        }
                    }

                    break ;
                default:
                    break;
            }
            return true;
        }


        public Boolean FlipVertical()
        {
            int X, Y;
            IntPtr RowData = Marshal.AllocHGlobal (m_Stride);
            for (Y=0;Y<m_Height/2;Y++)
            {  
                Win32Api.CopyMemory (RowData, m_Pointer+Y*m_Stride,m_Stride );
                Win32Api.CopyMemory ( m_Pointer+Y*m_Stride,m_Pointer + (m_Height - 1 - Y) * m_Stride,m_Stride );
                Win32Api.CopyMemory ( m_Pointer + (m_Height - 1 - Y) * m_Stride,RowData,m_Stride );
            }
            return true ;

        }

        public Boolean IsGrayBitmap()
        {
            Boolean Gray;
            if (m_BitCount == 4 || m_BitCount == 8)
            {
                Gray = true;
                byte * Palette = (byte*) m_PaletteAddress;
                for (int X = 0; X < m_PaletteSize; X++)
                {
                    if (*(Palette) != *(Palette + 1) || *(Palette) != *(Palette + 1) || *(Palette + 1) != *(Palette + 2))
                    {
                        Gray = false;
                        break;
                    }
                }
            }
            else
            {
                Gray = false;
            }
            return Gray;
        }

        public void DrawImage(IntPtr DestDC, int DestX, int DestY, int DestWidth, int DestHeight, int SrcX, int SrcY, int SrcWidth, int SrcHeight)
        {
            Win32Api.StretchBlt(DestDC, DestX, DestY, DestWidth, DestHeight, m_Hdc, SrcX, SrcY, SrcWidth, SrcHeight, Win32Const.SRCCOPY);
        }

        public void FreeBitmap(IntPtr Handle)
        {
            Boolean result;
            if (m_Hdc != IntPtr.Zero) result = Win32Api.DeleteDC(m_Hdc);
            if (m_Handle != IntPtr.Zero) result = Win32Api.DeleteObject(m_Handle);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public void Dispose(bool Suppress = true)
        {
            if (m_Hdc != IntPtr.Zero) Win32Api.DeleteDC(m_Hdc); m_Hdc = IntPtr.Zero;
            if (m_Handle != IntPtr.Zero) Win32Api.DeleteObject(m_Handle); m_Handle = IntPtr.Zero;
            if (m_PaletteAddress != IntPtr.Zero) Marshal.FreeHGlobal(m_PaletteAddress); m_PaletteAddress = IntPtr.Zero;
            if (Suppress == true) GC.SuppressFinalize(this);
        }

    }
}

  