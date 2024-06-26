using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace PlateMightsight
{
    public class Utility
    {
        public static int levenshteinDistance(string strA, string strB)
        {
            int lenA = strA.Length;
            int lenB = strB.Length;

            // Declaring a 2D array on the heap dynamically
            int[][] lev = new int[lenA + 1][];
            for (int i = 0; i < lenA + 1; i++)
                lev[i] = new int[lenB + 1];

            // Initialising first column
            for (int i = 0; i < lenA + 1; i++)
                lev[i][0] = i;

            // Initialising first row
            for (int j = 0; j < lenB + 1; j++)
                lev[0][j] = j;

            // Applying the algorithm:
            int insertion, deletion, replacement;

            for (int i = 1; i < lenA + 1; i++)
            {
                for (int j = 1; j < lenB + 1; j++)
                {
                    if (strA[i - 1] == strB[j - 1])
                    {
                        lev[i][j] = lev[i - 1][j - 1];
                    }
                    else
                    {
                        // Choosing the best option:
                        insertion = lev[i][j - 1];
                        deletion = lev[i - 1][j];
                        replacement = lev[i - 1][j - 1];

                        lev[i][j] = 1 + min(insertion, deletion, replacement);
                    }
                }
            }

            int result = lev[lenA][lenB];
            return result;
        }
        public static int min(int x, int y, int z)
        {
            if (x <= y && x <= z) return x;
            else if (y <= x && y <= z) return y;
            else return z;
        }
        public static void saveImageByFormat(Image image, string imagePath, ImageFormat imageFormat)
        {
            if (!checkExistingFolderFromFilePath(imagePath))
            {
                throw new Exception("Folder is not found at " + imagePath);
            }

            Bitmap bitmap = new Bitmap(image.Width, image.Height, image.PixelFormat);
            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.DrawImage(image, new Point(0, 0));
            graphics.Dispose();
            bitmap.Save(imagePath, imageFormat);
            bitmap.Dispose();
        }
        public static bool checkExistingFolderFromFilePath(string filePath)
        {
            string path = string.Empty;
            for (int num = filePath.Length - 1; num >= 0; num--)
            {
                if (filePath[num].Equals('\\'))
                {
                    path = filePath.Substring(0, num + 1);
                    break;
                }
            }

            return Directory.Exists(path);
        }
        public static Image fixBase64ForImage(string base64)
        {
            byte[] buffer = Convert.FromBase64String(base64);
            using (MemoryStream stream = new MemoryStream(buffer))
            return Image.FromStream(stream);
        }
        public static Bitmap fixBase64ForBitmap(string base64)
        {
            byte[] buffer = Convert.FromBase64String(base64);
            Image image;
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                image = Image.FromStream(stream);
            }

            return image as Bitmap;
        }
        public static Bitmap drawBorderOnImage(Bitmap bitmap, int x, int y, int width, int height, Color color, int thickness)
        {
            try
            {
                Graphics graphics = Graphics.FromImage(bitmap);
                Pen pen = new Pen(color, thickness);
                pen.DashStyle = DashStyle.Dash;
                graphics.DrawRectangle(pen, x, y, width, height);
                pen.Dispose();
                graphics.Dispose();
                return bitmap;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static Image getImageFromFile(string imagePath)
        {
            if (!isValidImage(imagePath))
            {
                return null;
            }

            try
            {
                Bitmap bitmap = new Bitmap(imagePath);
                Image thumbnailImage = bitmap.GetThumbnailImage(bitmap.Width, bitmap.Height, null, (IntPtr)0);
                bitmap.Dispose();
                return thumbnailImage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static void SaveBitmapToFile(Bitmap bitmap, string filePath)
        {
            if (bitmap == null || string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("Invalid Bitmap or file path.");
            }
            bitmap.Save(filePath, ImageFormat.Jpeg);
        }
        public static Image drawBorderOnImage(Image image, int x, int y, int width, int height, Color color, int thickness)
        {
            try
            {
                Graphics graphics = Graphics.FromImage(image);
                Pen pen = new Pen(color, thickness);
                pen.DashStyle = DashStyle.Dash;
                graphics.DrawRectangle(pen, x, y, width, height);
                pen.Dispose();
                graphics.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static int getInt(object obj)
        {
            try
            {
                if (obj == null)
                {
                    return -1;
                }

                if (int.TryParse(obj.ToString(), out var result))
                {
                    return result;
                }

                return -1;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static Bitmap drawImageOnImage(Bitmap backgroundImage, Bitmap foregroundImage)
        {
            try
            {
                using (Graphics graphics = Graphics.FromImage(backgroundImage))
                {

                    int newWidth = backgroundImage.Width / 4;
                    int newHeight = (foregroundImage.Height * newWidth) / foregroundImage.Width;

                    graphics.DrawImage(foregroundImage, 0, 0, newWidth+330, newHeight+300);
                }

                return backgroundImage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static Bitmap writeTextOnImage(Bitmap bitmap, Dictionary<string, string> content, Color titleColor, Color contentColor)
        {
            Graphics graphics = null;
            Font font = null;
            Font font2 = null;
            SolidBrush solidBrush = null;
            SolidBrush solidBrush2 = null;
            try
            {
                graphics = Graphics.FromImage(bitmap);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                int num = ((bitmap.Size.Width < bitmap.Size.Height) ? (bitmap.Size.Width / 40) : (bitmap.Size.Height / 40));
                font = new Font("Arial", num, FontStyle.Bold);
                font2 = new Font("Arial", num, FontStyle.Regular);
                bool flag = false;
                bool flag2 = false;
                solidBrush = new SolidBrush(titleColor);
                solidBrush2 = new SolidBrush(contentColor);
                int x = 10;
                int num2 = 10;
                int num3 = 0;
                int num4 = 0;
                foreach (KeyValuePair<string, string> item in content)
                {
                    num4 = graphics.MeasureString(item.Key, font).ToSize().Width;
                    if (num4 > num3)
                    {
                        num3 = num4;
                    }
                }

                num3 += 10;
                foreach (KeyValuePair<string, string> item2 in content)
                {
                    graphics.DrawString(item2.Key, font, solidBrush, new Point(x, num2));
                    graphics.DrawString(item2.Value, font2, solidBrush2, new Point(num3, num2));
                    num2 += 40;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                graphics?.Dispose();
                font?.Dispose();
                font2?.Dispose();
                solidBrush?.Dispose();
                solidBrush2?.Dispose();
            }

            return bitmap;
        }

        public static Bitmap drawImageOnImage(Bitmap backgroundImage, Bitmap foregroundImage, int left, int top)
        {
            try
            {
                using (Graphics graphics = Graphics.FromImage(backgroundImage))
                {
                    graphics.DrawImageUnscaled(foregroundImage, left, top);
                }

                return backgroundImage;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static Image drawImageOnImage(string backgroundImagePath, string foregroundImagePath, int left, int top)
        {
            try
            {
                if (!isValidImage(backgroundImagePath))
                {
                    throw new Exception("File " + backgroundImagePath + " is not valid!");
                }

                if (!isValidImage(foregroundImagePath))
                {
                    throw new Exception("File " + foregroundImagePath + " is not valid!");
                }

                Image image = Image.FromFile(backgroundImagePath);
                Image image2 = Image.FromFile(foregroundImagePath);
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawImageUnscaled(image2, left, top);
                image2.Dispose();
                return image;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static bool isValidImage(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    return false;
                }

                using (new Bitmap(fileName))
                {
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool isValidImage(byte[] imageData)
        {
            try
            {
                if (imageData.Length == 0)
                {
                    return false;
                }

                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    using (new Bitmap(stream))
                    {
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
