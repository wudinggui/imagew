using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImageWizard
{
    internal static class Win32Const
    {
        
        internal const uint STRETCH_ANDSCANS = 1;
        internal const uint STRETCH_ORSCANS = 2;
        internal const uint STRETCH_DELETESCANS = 3;
        internal const uint STRETCH_HALFTONE = 4; 

        internal const uint DIB_RGB_COLORS = 0; /* color table in RGBs */
        internal const uint DIB_PAL_COLORS = 1; /* color table in palette indices */

        internal const uint BI_RGB = 0;
        internal const uint BI_RLE8 = 1;
        internal const uint BI_RLE4 = 2;
        internal const uint BI_BITFIELDS = 3;
        internal const uint BI_JPEG = 4;
        internal const uint BI_PNG = 5;

        internal const uint GMEM_FIXED   = 0X0;
        internal const uint GMEM_ZEROINIT   = 0X40;
        internal const uint GPTR   = (GMEM_FIXED | GMEM_ZEROINIT);
        internal const uint PAGE_EXECUTE_READWRITE   = 0X40;
        internal const uint GMEM_MOVEABLE   = 0X2;

        internal const uint CF_DIB =8; 

        internal const uint SRCCOPY = 0XCC0020;

    }
}
