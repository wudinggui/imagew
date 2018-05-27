using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace ImageWizard.Effect
{
    unsafe static class BlurEffect
    {  
        public static Boolean ZoomBlur(FastBitmap bmp, uint SampleRadius =50,uint Amount =100,int CenterX=256,int CenterY =256)
        {
            int Red,Green,Blue;
            int Fcx,Fcy,TempFy,Fx,Fy;
            int U,V;
            int Width, Height, Stride;
            int X,Y,I;
            byte * Scan0,Pointer,PointerC;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride; Scan0 =(byte *) bmp.Pointer;
            byte * DataCopy = (byte *)Marshal.AllocHGlobal(Stride*Height );
            Win32Api.CopyMemory((IntPtr)DataCopy,(IntPtr)Scan0,Stride*Height );

            Fcx = CenterX<<16;
            Fcy = CenterY<<16;

            for (Y=0;Y<Height;Y++)
            {
                Pointer = Scan0 +Stride * Y;
                TempFy = (Y << 16) - Fcy;
                for (X=0;X<Width;X++)
                {
                    Fx = (X <<16) - Fcx;
                    Fy = TempFy;
                    Red = 0;
                    Green = 0;
                    Blue = 0;
                    for (I=0;I<SampleRadius;I++)
                    {
                        Fx -=(int) ((Fx >>4) * Amount) >>10;
                        Fy -= (int)((Fy >> 4) * Amount) >> 10;
                        U = (int)(Fx + Fcx + 32768) >>16;
                        V = (int)(Fy + Fcy + 32768) >> 16;
                        PointerC = DataCopy + Stride * V + U * 3 ;          // U*3如果优化为(U<<1)+U速度反倒还慢了一些，暂时不解中
                        Blue+= *(PointerC);
                        Green += *(PointerC + 1);
                        Red += *(PointerC + 2);
                    }
                    *(Pointer) = (byte)(Blue / SampleRadius);
                    *(Pointer+1) =(byte) (Green/SampleRadius);
                    *(Pointer+2) = (byte)(Red/SampleRadius);
                    Pointer += 3;
                }
            }
            Marshal.FreeHGlobal((IntPtr)DataCopy);
            return true;
        }
        public static Boolean BoxBlur(FastBitmap bmp, int SampleRadius)
        {
            int Width, Height, Stride;
            int X, Y,Speed;
            byte* Scan0, Pointer, PointerC;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride; Scan0 = (byte*)bmp.Pointer;
            int PixelBitCount = bmp.BitCount>>3;       //每个像素占用的字节数 右移3 即除以8

            byte* DataCopy = (byte*)Marshal.AllocHGlobal(Stride * Height);
            Win32Api.CopyMemory((IntPtr)DataCopy, (IntPtr)Scan0, Stride * Height);

            if (SampleRadius < 2) SampleRadius = 2;
            int[] BoxR = new int[Width * Height];  //每个方框RGB灰度总值
            int[] BoxG = new int[Width * Height];
            int[] BoxB = new int[Width * Height];
            for (Y = 0; Y < Height; Y++)
            {
                int SumR = 0; int SumG = 0; int SumB = 0;   //统计每行的GRB灰度总值
                //Pointer = Scan0 + Stride * Y;
                PointerC = DataCopy + Stride * Y;
                Speed = Width * Y;
                for (X = 0; X < Width; X++)
                {
                    SumB += *(PointerC);
                    SumG += *(PointerC+1);
                    SumR += *(PointerC+2);
                    if (Y == 0)
                    {
                        BoxB[Speed] = SumB;
                        BoxB[Speed] = SumR;
                        BoxR[Speed] = SumR;
                    }
                    else
                    {
                        BoxB[Speed] = BoxB[Speed - Width] + SumB;
                        BoxG[Speed] = BoxG[Speed - Width] + SumG;
                        BoxR[Speed] = BoxR[Speed - Width] + SumR; 
                    }
                    PointerC += PixelBitCount;
                    Speed += 1;
                }
            }
          for (Y = 0; Y < Height; Y++)
          {
             Pointer = Scan0 +Stride * Y;
             int Y1 = Y - SampleRadius;
             int Y2 = Y + SampleRadius;
             if (Y1 < 0)  Y1 = 0;
             if (Y2 >=Height)  Y2 = Height-1;           //防止越界     
             for (X = 0; X < Width; X++)
             {
                int X1 = X -SampleRadius;
                int X2 = X +SampleRadius;
                if (X1 < 0)  X1 = 0;
                if (X2 >=Width)  X2 = Width-1;           //防止越界 
                int BoxCount = (X2 - X1) * (Y2 - Y1);
                *(Pointer)     =(byte)((BoxB[Y2 * Width + X2] - BoxB[Y1 * Width + X2] -BoxB[Y2 * Width + X1] + BoxB[Y1 * Width + X1])/ BoxCount);
                *(Pointer + 1) = (byte)((BoxG[Y2 * Width + X2] - BoxG[Y1 * Width + X2] -BoxG[Y2 * Width + X1] + BoxG[Y1 * Width + X1])/ BoxCount);
                *(Pointer + 2) = (byte)((BoxR[Y2 * Width + X2] - BoxR[Y1 * Width + X2] - BoxR[Y2 * Width + X1] + BoxR[Y1 * Width + X1]) / BoxCount);
                Pointer += PixelBitCount;
             }
          }
            Marshal.FreeHGlobal((IntPtr)DataCopy);
            return true;
        }

        public static Boolean Mosaic(FastBitmap bmp, int SampleBlock)
        {
            int Width, Height, Stride;
            int X, Y;
            byte* Scan0, Pointer, PointerC;
            Width = bmp.Width; Height = bmp.Height; Stride = bmp.Stride; Scan0 = (byte*)bmp.Pointer;
            int PixelBitCount = bmp.BitCount >> 3;       //每个像素占用的字节数 右移3 即除以8
            byte* DataCopy = (byte*)Marshal.AllocHGlobal(Stride * Height);
  
            int  maxH = (Height / SampleBlock) *SampleBlock;        //为越界处理准备，获得越界部分的坐标
            int  maxW = (Width / SampleBlock) * SampleBlock;       
            int BX,BY;                //block中的x和y
            for (Y=0;Y<Height;Y++)
            {
                int BlockEdgeY = (Y / SampleBlock) * SampleBlock;
                for (X = 0; X < Width; X++)
                {
                    int BlockEdgeX = (X / SampleBlock) * SampleBlock;
                    if ((Y%SampleBlock==0)&&(X%SampleBlock==0))
                    {
                        int BlockLeft = BlockEdgeX;
                        int BlockTop = BlockEdgeY;
                        int BlockBottom = (maxH == Y ? Height : (BlockEdgeY + SampleBlock));
                        int BlockRight = (maxW == X ? Width  : (BlockEdgeX+ SampleBlock));

                        int  Blue = 0;             //每次循环都要初始化这几个参数
                        int  Green = 0;
                        int  Red = 0; 
                        int  BlockPixel = 0;

                        for (BY = BlockTop; BY < BlockBottom; BY++)
                        {
                            for (BX = BlockLeft; BX < BlockRight; BX++)
                            {
                                PointerC = Scan0 + BY * Stride + PixelBitCount * BX;
                                Blue += *(PointerC);                //求block中RGB灰度值和
                                Green += *(PointerC+1);
                                Red += *(PointerC+2);
                                BlockPixel += 1;
                            }
                        }

                        if (BlockPixel > 0)
                        {
                            Blue = Blue / BlockPixel;               //求block中RGB灰度平均值
                            Green = Green / BlockPixel;
                            Red = Red / BlockPixel;
                        }

                        for (BY = BlockTop; BY < BlockBottom; BY++)
                        {
                            for (BX = BlockLeft; BX < BlockRight; BX++)
                            {
                                Pointer= Scan0 + BY * Stride + PixelBitCount * BX;
                               *(Pointer)=(byte)Blue;
                               *(Pointer + 1) = (byte)Green;
                               *(Pointer + 2) = (byte)Red;
                            }
                        }
                    }
                }
             }
          
           
            Win32Api.CopyMemory((IntPtr)DataCopy, (IntPtr)Scan0, Stride * Height);

            Marshal.FreeHGlobal((IntPtr)DataCopy);
            return true;
        }
    
   
    }
}


