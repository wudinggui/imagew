using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageWizard.FastbmpInfo
{
    public unsafe class BmpInfo
    {
        private int[] m_Red = new int[256];           //存放每个灰度值所对应的像素个数
        private int[] m_Green = new int[256];
        private int[] m_Blue= new int [256];
        private int m_PixelCount;
        public int[] Red { get { return m_Red; } }
        public int[] Green { get { return m_Green; } }
        public int[] Blue { get { return m_Blue; } }
        public int PixelCount { get { return m_PixelCount; } }

        public Boolean GetHisgram(FastBitmap bmp)
        {
            if (bmp == null) return false;
            if (bmp.Handle == IntPtr.Zero) return false;

            int Width, Height, Stride, X, Y;
            byte* Pointer, Scan0;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride;
            Scan0 = (byte*)bmp.Pointer;
            m_PixelCount = Width * Height;
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
                        Pointer = Scan0 + Y * Stride;           //行的首地址
                        for (X = 0; X < Width; X++)
                        {
                             m_Blue[*Pointer] += 1;             //统计个颜色分量直方图
                             m_Green[*(Pointer + 1)] += 1;
                             m_Red[*(Pointer + 2)] += 1;
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
