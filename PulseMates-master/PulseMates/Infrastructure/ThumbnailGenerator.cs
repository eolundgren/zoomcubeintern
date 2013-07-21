namespace PulseMates.Infrastructure
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;


    public enum ThumbnailSize
    {
        Tiny = 128,
        Small = 256,
        Medium = 512,
        Large = 1024
    }

    public static class ThumbnailGenerator
    {
        //90 is the magic setting - really. It has excellent quality and file size.
        public const long THUMBNAIL_QUALITY = 80;

        /// <summary>
        /// Generate a thumbnails where the lagest size will be controlled by the
        /// specificed size.
        /// </summary>
        public static byte[] Resize(Image image, ThumbnailSize size)
        {
            var newSize = GetOptimalSizeForResize(image.Size, (int)size, (int)size);
            
            using (var newImage = new Bitmap(newSize.Width, newSize.Height))
            using (var g = Graphics.FromImage(newImage))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                g.DrawImage(image, 0, 0, newSize.Width, newSize.Height);

                return ToStream(newImage, image.RawFormat);
            }
        }

        public static byte[] Crop(byte[] imageArr, ThumbnailSize size)
        {
            try
            {
                using (var ms = new MemoryStream(imageArr))
                using (var image = Image.FromStream(ms))
                    return Crop(image, size);
            }
            catch { return new byte[0]; }
        }

        /// <summary>
        /// Generate a new thumbnail that will crop the largest size to the specificed size.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] Crop(Image image, ThumbnailSize size)
        {
            using (var newImage = FixedSize(image, (int)size, (int)size))
                return ToStream(newImage, image.RawFormat);

            //using (var newImage = new Bitmap((int)size, (int)size))
            //using (var g = Graphics.FromImage(image))
            //{
            //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //    g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            //    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            //    var cropRect = GetCropRectangle(image.Size, (int)size);

            //    g.DrawImage(image,
            //        cropRect, // cropped rect
            //        new Rectangle(0, 0, (int)size, (int)size), // newSize
            //        GraphicsUnit.Pixel
            //    );

            //    return ToStream(newImage, image.RawFormat);
            //}
        }

        private static byte[] ToStream(Image img, ImageFormat orgFormat)
        {
            using (var ms = new MemoryStream())
            {
                var encoderParameters = new System.Drawing.Imaging.EncoderParameters(1);
                encoderParameters.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, THUMBNAIL_QUALITY);

                var codecInfo = ImageCodecInfo.GetImageEncoders()
                    .FirstOrDefault(x => x.FormatID == orgFormat.Guid);

                img.Save(ms, codecInfo, encoderParameters);
                return ms.ToArray();
            }
        }
        
        private static Rectangle GetCropRectangle(Size actualSize, int size)
        {
            double width = actualSize.Width
                 , height = actualSize.Height
                 , ratio = width > height ? height / width : width / height;

            if (width > height)
            {
                var x = ((height - width) / 2) * ratio;
                var newWidth = (int)Math.Round((size -
                        (width * ratio)) / 2);

                return new Rectangle((int)x, 0, (int)newWidth, (int)size);
            }

                 //, ratio = width > height ? height / width : width / height;

            //var optimalSize = GetOptimalSizeForResize(actualSize, size, size);

            return new Rectangle();
        }

        private static Size GetOptimalSizeForResize(Size actualSize, int maxWidth, int maxHeight)
        {
            int width = actualSize.Width, height = actualSize.Height;
            double ratio;

            if (width > height)
            {
                ratio = (double)height / (double)width;

                return new Size()
                {
                    Width = maxWidth,
                    Height = Convert.ToInt32(Math.Round(maxHeight * ratio)),
                };
            }
            else
            {
                ratio = (double)width / (double)height;

                return new Size()
                {
                    Width = Convert.ToInt32(Math.Round(maxWidth * ratio)),
                    Height = maxHeight,
                };
            }
        }


        private static Image FixedSize(Image imgPhoto, int Width, int Height, bool needToFill = true)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (!needToFill)
            {
                if (nPercentH < nPercentW)
                {
                    nPercent = nPercentH;
                }
                else
                {
                    nPercent = nPercentW;
                }
            }
            else
            {
                if (nPercentH > nPercentW)
                {
                    nPercent = nPercentH;
                    destX = (int)Math.Round((Width -
                        (sourceWidth * nPercent)) / 2);
                }
                else
                {
                    nPercent = nPercentW;
                    destY = (int)Math.Round((Height -
                        (sourceHeight * nPercent)) / 2);
                }
            }

            if (nPercent > 1)
                nPercent = 1;

            int destWidth = (int)Math.Round(sourceWidth * nPercent);
            int destHeight = (int)Math.Round(sourceHeight * nPercent);

            System.Drawing.Bitmap bmPhoto = new System.Drawing.Bitmap(
                destWidth <= Width ? destWidth : Width,
                destHeight < Height ? destHeight : Height,
                              PixelFormat.Format32bppRgb);

            System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto);
            grPhoto.Clear(System.Drawing.Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            grPhoto.SmoothingMode = SmoothingMode.HighQuality;
            grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
            grPhoto.CompositingQuality = CompositingQuality.HighQuality;

            grPhoto.DrawImage(imgPhoto,
                new System.Drawing.Rectangle(destX, destY, destWidth, destHeight),
                new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                System.Drawing.GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    }
}