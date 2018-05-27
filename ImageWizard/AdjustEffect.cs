using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace ImageWizard.Effect
{
    unsafe static class AdjustEffect
    {
        public static bool Invert(FastBitmap bmp)
        {
            if (bmp == null)  return false;
            if (bmp.Handle == IntPtr.Zero) return false;

            int Width, Height, Stride, Y;
            uint* Pointer;

            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride; Pointer =(uint *) bmp.Pointer;
            switch (bmp.BitCount)
            {
                case 4:
                case 8:
                    if (bmp.IsGrayBitmap() == false)                    //索引色图像
                    {
                        byte* Palette = (byte*)bmp.PaletteAddress;      //获取调色板
                        for (Y = 0; Y < bmp.PaletteSize; Y++)
                        {
                            *Palette ^= 255;                            //调色板反转
                            *(Palette + 1) ^= 255;
                            *(Palette + 2) ^= 255;
                            Palette += 4; 
                        }
                        Win32Api.SetDIBColorTable(bmp.Hdc, 0, bmp.PaletteSize, bmp.PaletteAddress);
                    }
                    else
                    {
                        for (Y = 0; Y < (Stride * Height >> 2); Y++)    //充分利用扫描行4字节对齐的属性
                        {
                            *Pointer ^= 0XFFFFFFFF;
                            Pointer++;
                        }
                    }
                    break;
                case 24:
                    for (Y = 0; Y < (Stride * Height >> 2); Y++)
                    {
                        *Pointer ^= 0XFFFFFFFF;
                        Pointer++;
                    }
                    break;
                case 32:
                   for (Y = 0; Y < (Stride * Height >> 2); Y++)     //32位是不用处理ALPHA的
                    {
                        *Pointer ^= 0X00FFFFFF;
                        Pointer++;
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        public static bool Desaturate(FastBitmap bmp)
        {
            if (bmp == null) return false;
            if (bmp.Handle == IntPtr.Zero) return false;

            int Width, Height, Stride, X, Y;
            byte* Pointer,Scan0;
            byte Max, Min;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride; Scan0 = (byte*)bmp.Pointer;

            switch (bmp.BitCount)
            {
                case 4:
                case 8:
                    if (bmp.IsGrayBitmap() == false)                    //索引色图像
                    {
                        byte* Palette = (byte*)bmp.PaletteAddress;      //获取调色板
                        for (Y = 0; Y < bmp.PaletteSize; Y++)
                        {
                            if (*Palette >= *(Palette + 1))
                            {
                                Max = *Palette;
                                Min = *(Palette + 1);
                            }
                            else
                            {
                                Max = *(Palette + 1);
                                Min = *Palette;
                            }
                            if (*(Palette + 2) > Max)
                                Max = *(Palette + 2);
                            else if (*(Palette + 2) < Min)
                                Min = *(Palette + 2);
                            *Palette = *(Palette + 1) = *(Palette + 2) = (byte)((Max + Min) >> 1);
                            Palette += 4;
                        }
                        Win32Api.SetDIBColorTable(bmp.Hdc, 0, bmp.PaletteSize, bmp.PaletteAddress);
                    }
                    else
                        return false;
                    break;
                case 24:
                case 32:
                    int PixelBitCount = bmp.BitCount>>3;       //每个像素占用的字节数 右移3 即除以8
                    for (Y = 0; Y < Height; Y++)
                    {
                        Pointer = Scan0 + Y * Stride;           //行的首地址
                        for (X = 0; X < Width; X++)
                        {
                            if (*Pointer >= *(Pointer + 1))     //取R/G/B各分量的最大和最小值的平均值
                            {
                                Max = *Pointer;
                                Min = *(Pointer + 1);
                            }
                            else
                            {
                                Max = *(Pointer + 1);
                                Min = *Pointer;
                            }
                            if (*(Pointer + 2) > Max)
                                Max = *(Pointer + 2);
                            else if (*(Pointer + 2) < Min)
                                Min = *(Pointer + 2);
                            *Pointer = *(Pointer + 1) = *(Pointer + 2) = (byte)((Max + Min) >> 1);
                            Pointer += PixelBitCount;
                        }
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }

        public static bool Bin(FastBitmap bmp, int ThrValue)
        {
            if (bmp == null) return false;
            if (bmp.Handle == IntPtr.Zero) return false;

            int Width, Height, Stride,X,Y;
            byte* Pointer, Scan0;
            byte tempValue;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride; 
            Scan0 = (byte*)bmp.Pointer;
            switch (bmp.BitCount)
            {
                case 4:
                case 8:
                    break;
                case 24:
                case 32:

                    byte[] Map =new byte[256] ;               //得到每个像素的映射值
                    int i;
                    for (i = 0; i < 256; i++)
                       Map[i] = (i > ThrValue) ? (byte)0 : (byte)255;

                    int PixelBitCount = bmp.BitCount>>3;       //每个像素占用的字节数 右移3 即除以8
                    for (Y = 0; Y < Height; Y++)
                    {
                        Pointer = Scan0 + Y * Stride;           //行的首地址
                        for (X = 0; X < Width; X++)
                        {
                            tempValue = (byte)(*Pointer * 0.114 + *(Pointer + 1) * 0.587 + *(Pointer + 2) * 0.299);
                            *Pointer = *(Pointer + 1) = *(Pointer + 2) = (byte)Map[tempValue];
                            Pointer += PixelBitCount;
                        }
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }
        public static bool Gama(FastBitmap bmp, int ThrValue)
        {
            //功能描述 ：    Gama颜色调整(指数增强)   公式   NewValue=lev*OldValue^r
            if (bmp.Handle == IntPtr.Zero) return false;

            int Width, Height, Stride, X, Y;
            byte* Pointer, Scan0;
            byte tempValue;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride;
            Scan0 = (byte*)bmp.Pointer;
            switch (bmp.BitCount)
            {
                case 4:
                case 8:
                    break;
                case 24:
                case 32:

                    byte[] Map = new byte[256];               //得到每个像素的映射值
                    int i;
                    double GamaLevel =(double)ThrValue / 50;
                    for (i = 0; i < 256; i++)
                    {
                        Map[i] = (byte)(Math .Pow (255 ,(1 - GamaLevel)) *  Math .Pow ( i , GamaLevel));
                    }

                    int PixelBitCount = bmp.BitCount >> 3;       //每个像素占用的字节数 右移3 即除以8
                    for (Y = 0; Y < Height; Y++)
                    {
                        Pointer = Scan0 + Y * Stride;           //行的首地址
                        for (X = 0; X < Width; X++)
                        {
                            tempValue = (byte)(*Pointer);
                            *Pointer = (byte)Map[tempValue];
                            tempValue = (byte)*(Pointer+1);
                            *(Pointer+1) = (byte)Map[tempValue];
                            tempValue = (byte)*(Pointer+2);
                            *(Pointer+2) = (byte)Map[tempValue];
                            Pointer += PixelBitCount;
                        }
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }
        public static bool OldStyle(FastBitmap bmp)
        {
            if (bmp == null) return false;
            if (bmp.Handle == IntPtr.Zero) return false;

            int Width, Height, Stride, X, Y;
            byte* Pointer, Scan0, PointerC;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride; Scan0 = (byte*)bmp.Pointer;
            byte* DataCopy = (byte*)Marshal.AllocHGlobal(Stride * Height);
            Win32Api.CopyMemory((IntPtr)DataCopy, (IntPtr)Scan0, Stride * Height);

            switch (bmp.BitCount)
            {
                case 4:
                case 8:
                    break;
                case 24:
                case 32:
                    int PixelBitCount = bmp.BitCount >> 3;       //每个像素占用的字节数 右移3 即除以8
                    for (Y = 0; Y < Height; Y++)
                    {
                        PointerC = DataCopy  + Y * Stride;           //行的首地址
                        Pointer = Scan0 + Y * Stride;                //行的首地址
                        for (X = 0; X < Width; X++)
                        {
                            *(Pointer + 2) = (byte)Math.Max(0, Math.Min(255, 0.393 * (*PointerC + 2) + 0.769 * (*PointerC + 1) + 0.189 * (*PointerC)));
                            *(Pointer + 1) = (byte)Math.Max(0, Math.Min(255, 0.349 * (*PointerC + 2) + 0.686 * (*PointerC + 1) + 0.168 * (*PointerC)));
                            *Pointer = (byte)Math.Max(0, Math.Min(255, 0.272 * (*PointerC + 2) + 0.534 * (*PointerC + 1) + 0.131 * (*PointerC)));
                            PointerC += PixelBitCount;
                            Pointer += PixelBitCount;
                        }
                    }
                    break;
                default:
                    return false;
            }
            Marshal.FreeHGlobal((IntPtr)DataCopy);
            return true;
        }
        public static bool FilmStyle(FastBitmap bmp, int ThrValue)
        {
            if (bmp == null) return false;
            if (bmp.Handle == IntPtr.Zero) return false;

            int Width, Height, Stride, X, Y;
            byte* Pointer, Scan0;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride; Scan0 = (byte*)bmp.Pointer;
            byte* DataCopy = (byte*)Marshal.AllocHGlobal(Stride * Height);
            Win32Api.CopyMemory((IntPtr)DataCopy, (IntPtr)Scan0, Stride * Height);

            switch (bmp.BitCount)
            {
                case 4:
                case 8:
                    break;
                case 24:
                case 32:
                    int PixelBitCount = bmp.BitCount >> 3;       //每个像素占用的字节数 右移3 即除以8
                    byte[] Map = new byte[256];                  //建立映射关系
                    int i;
                    for (i = 0; i < 256; i++)
                    {
                        Map[i] = (byte)(i > ThrValue ? 192 : 64);
                    }

                    for (Y = 0; Y < Height; Y++)
                    {
                        Pointer = Scan0 + Y * Stride;                //行的首地址
                        for (X = 0; X < Width; X++)
                        {
                            *(Pointer + 2) = (byte)Map[*(Pointer + 2)];
                            *(Pointer + 1) = (byte)Map[*(Pointer + 1)];
                            *Pointer = (byte)Map[*(Pointer)];
                            Pointer += PixelBitCount;
                        }
                    }
                    break;
                default:
                    return false;
            }
            Marshal.FreeHGlobal((IntPtr)DataCopy);
            return true;
        }
        public static bool CrossPhoto(FastBitmap bmp)
        {
            if (bmp.Handle == IntPtr.Zero) return false;

            int Width, Height, Stride, X, Y;
            byte* Pointer, Scan0;
            byte tempValue;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride;
            Scan0 = (byte*)bmp.Pointer;
            switch (bmp.BitCount)
            {
                case 4:
                case 8:
                    break;
                case 24:
                case 32:

                    byte[] MapR = new byte[256];               //得到每个像素的映射值
                    byte[] MapG = new byte[256];
                    byte[] MapB = new byte[256]; 
                    int i;
                    for (i = 0; i < 256; i++)
                    {
                       int value = i < 128 ? i : 256 - i;
                       double r = (Math .Pow(value, 3) / 64.0 / 256.0);
                       MapR[i] = (byte)(i < 128 ? r : 256 - r);

                       double g = (Math .Pow(value, 2) / 128.0);
                       MapG[i] = (byte)(i < 128 ? g : 256 - g);
                       MapB[i]= (byte)(i/2 + 0X25);

                    }

                    int PixelBitCount = bmp.BitCount >> 3;       //每个像素占用的字节数 右移3 即除以8
                    for (Y = 0; Y < Height; Y++)
                    {
                        Pointer = Scan0 + Y * Stride;           //行的首地址
                        for (X = 0; X < Width; X++)
                        {
                            tempValue = (byte)(*Pointer);
                            *Pointer = (byte)MapB[tempValue];
                            tempValue = (byte)*(Pointer + 1);
                            *(Pointer + 1) = (byte)MapG[tempValue];
                            tempValue = (byte)*(Pointer + 2);
                            *(Pointer + 2) = (byte)MapR[tempValue];
                            Pointer += PixelBitCount;
                        }
                    }
                    break;
                default:
                    return false;
            }
            return true;
        }
        public static bool LomoStyle(FastBitmap bmp)
        {
            if (bmp.Handle == IntPtr.Zero) return false;

            int Width, Height, Stride, X, Y;
            byte* Pointer, Scan0;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride;
            Scan0 = (byte*)bmp.Pointer;
            switch (bmp.BitCount)
            {
                case 4:
                case 8:
                    break;
                case 24:
                case 32:

                double cx = Width>>1;
                double cy = Height>>1; 
                double maxDist = 1.0/Math.Sqrt(cx*cx+cy*cy);
                double dist,lumen=0.0;

                int PixelBitCount = bmp.BitCount >> 3;       //每个像素占用的字节数 右移3 即除以8
                for (Y = 0; Y < Height; Y++)
                {
                    Pointer = Scan0 + Y * Stride;           //行的首地址
                    for (X = 0; X < Width; X++)
                    {
                        dist = (double)Math.Sqrt((X - cx) * (X - cx) + (Y - cy) * (Y - cy));
                        lumen = (double)0.75 / (1.0 + Math.Exp((dist * maxDist - 0.73) * 20.0)) + 0.25;   //lumen关于半径的一个e指数衰减函数，最后收敛于1，即半径越小流明越大，范围【0.25-1.0】
                        *Pointer = (byte)(*(Pointer) * lumen);
                        *(Pointer + 1) = (byte)(*(Pointer + 1) * lumen);
                        *(Pointer + 2) = (byte)(*(Pointer + 2) * lumen);
                        Pointer += PixelBitCount;
                    }
                }
                break;
            default:
                return false;
            }
            return true;
        }
    }
  
}
