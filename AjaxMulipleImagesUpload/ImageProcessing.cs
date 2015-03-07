using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;


namespace CommonClasses
{
    public class ImageProcessing
    {
        public string FileReadFrom { get; set; }
        public string FileSaveTo { get; set; }
        public int ThumbWidth { get; set; }
        public bool keepOriginal { get; set; }
        public string WaterMarkText { get; set; }
        public string WaterMarkFontName { get; set; }
        public string WaterMarkFontSize { get; set; }
        public FontStyle WaterMarkFontStyle { get; set; }
        public string WaterMarkX { get; set; }
        public string WaterMarkY { get; set; }
        public ImageFormat ImageFormat { get; set; }


        public bool SaveThumbnailImage(ImageProcessing objImg)
        {
            bool result = false;
            try
            {
                int imageHeight = 0;
                int imageWidth = 0;
                int CurrentimgHeight = 0;
                int CurrentimgWidth = 0;
                System.Drawing.Image fullSizeImg;
                fullSizeImg = System.Drawing.Image.FromFile(objImg.FileReadFrom);
                CurrentimgHeight = fullSizeImg.Height;
                CurrentimgWidth = fullSizeImg.Width;
                if (!objImg.keepOriginal)
                {
                    imageHeight = (CurrentimgHeight * ThumbWidth) / CurrentimgWidth;
                    imageWidth = (CurrentimgWidth * ThumbWidth) / CurrentimgHeight;

                    if (imageHeight > ThumbWidth)
                    {
                        imageHeight = ThumbWidth;
                    }
                    else
                    {
                        imageWidth = ThumbWidth;
                    }
                }
                else
                {
                    imageHeight = CurrentimgHeight;
                    imageWidth = CurrentimgWidth;
                }

                //This will only work for jpeg images
                // Response.ContentType = "image/jpeg";
                if ((imageHeight > 0) && (imageWidth > 0))
                {
                    System.Drawing.Image.GetThumbnailImageAbort dummyCallBack;
                    dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
                    System.Drawing.Image thumbNailImg;
                    thumbNailImg = fullSizeImg.GetThumbnailImage(imageWidth, imageHeight, dummyCallBack, IntPtr.Zero);
                    SizeF StringSizeF;
                    Single DesiredWidth;
                    Font wmFont;
                    Single RequiredFontSize;
                    Single Ratio;

                    System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(thumbNailImg);
                    string strWatermark = objImg.WaterMarkText;
                    if (string.IsNullOrEmpty(WaterMarkText))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(fullSizeImg, 0, 0, imageWidth, imageHeight);
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        thumbNailImg.Save(objImg.FileSaveTo, objImg.ImageFormat);
                        result = true;
                    }
                    else
                    {    //Set the watermark font	
                        string pFont = !string.IsNullOrEmpty(objImg.WaterMarkFontName) ? objImg.WaterMarkFontName : "Verdana";
                        int pFontSize = Convert.ToInt32(!string.IsNullOrEmpty(objImg.WaterMarkFontSize) ? objImg.WaterMarkFontSize : "16");
                        FontStyle pFontStyle = WaterMarkFontStyle;

                        wmFont = new Font(pFont, pFontSize, pFontStyle);
                        // DesiredWidth = imageWidth * Convert.ToInt32(0.5);
                        DesiredWidth = imageWidth / 2;
                        //use the MeasureString method to position the watermark in the centre of the image

                        StringSizeF = g.MeasureString(strWatermark.Trim(), wmFont);
                        Ratio = StringSizeF.Width / wmFont.SizeInPoints;
                        RequiredFontSize = DesiredWidth / Ratio;

                        wmFont = new Font(pFont, RequiredFontSize, pFontStyle);
                        // 'Sets the interpolation mode for a high quality image  
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.DrawImage(fullSizeImg, 0, 0, imageWidth, imageHeight);
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        SolidBrush letterBrush = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
                        SolidBrush shadowBrush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
                        // 'Enter the watermark text 
                        int XPos = Convert.ToInt32(!string.IsNullOrEmpty(objImg.WaterMarkX) ? objImg.WaterMarkFontSize : (imageWidth / 3).ToString());
                        int YPos = Convert.ToInt32(!string.IsNullOrEmpty(objImg.WaterMarkY) ? objImg.WaterMarkFontSize : (imageHeight / 2).ToString());
                        g.DrawString(WaterMarkText, wmFont, shadowBrush, XPos, YPos);
                        g.DrawString(WaterMarkText, wmFont, letterBrush, XPos, YPos);
                        // thumbNailImg.Save(Response.OutputStream, ImageFormat.Jpeg);
                        thumbNailImg.Save(objImg.FileSaveTo, objImg.ImageFormat);
                        result = true;
                    }
                    thumbNailImg.Dispose();
                    g.Dispose();
                }
                else
                {
                    // fullSizeImg.Save(Response.OutputStream, ImageFormat.Jpeg);
                    // fullSizeImg.Save("C:\test_water.jpg", ImageFormat.Jpeg)
                    result = false;
                }
                //'Important, dispose of the image  – otherwise the image file will be locked by the server for several minutes
                fullSizeImg.Dispose();
            } 
            catch(Exception)
            {
                result = false;
            }
            return result;
        }

        private Boolean ThumbnailCallback()
        {
            return false;
        }
    }
}
