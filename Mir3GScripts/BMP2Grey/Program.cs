using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMP2Grey
{
    class Program
    {
        struct Pixcel {
            int x;
            int y;
            public Pixcel(int x, int y) {
                this.x = x;
                this.y = y;
            }
        }
        static void ToGrey(Bitmap img1)
        {
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    //计算灰度值
                    int grey = (int)(0.299 * pixelColor.R + 0.587 * pixelColor.G + 0.114 * pixelColor.B);
                    Color newColor = Color.FromArgb(grey, grey, grey);
                    img1.SetPixel(i, j, newColor);
                }
            }
        }
        static void Thresholding(Bitmap img1)
        {
            int[] histogram = new int[256];
            int minGrayValue = 255, maxGrayValue = 0;
            int[,] bitMap = new int[img1.Width,img1.Height];
            //求取直方图
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    histogram[pixelColor.R]++;
                    if (pixelColor.R > maxGrayValue)
                        maxGrayValue = pixelColor.R;
                    if (pixelColor.R < minGrayValue)
                        minGrayValue = pixelColor.R;
                }
            }
            //迭代计算阀值
            int threshold = -1;
            int newThreshold = (minGrayValue + maxGrayValue) / 2;
            for (int iterationTimes = 0; threshold != newThreshold && iterationTimes < 100; iterationTimes++)
            {
                threshold = newThreshold;
                int lP1 = 0;
                int lP2 = 0;
                int lS1 = 0;
                int lS2 = 0;
                //求两个区域的灰度的平均值
                for (int i = minGrayValue; i < threshold; i++)
                {
                    lP1 += histogram[i] * i;
                    lS1 += histogram[i];
                }
                int mean1GrayValue = (lP1 / lS1);
                for (int i = threshold + 1; i < maxGrayValue; i++)
                {
                    lP2 += histogram[i] * i;
                    lS2 += histogram[i];
                }
                int mean2GrayValue = (lP2 / lS2);
                newThreshold = (mean1GrayValue + mean2GrayValue) / 2;
            }
            //计算二值化
            for (int i = 0; i < img1.Width; i++)
            {
                for (int j = 0; j < img1.Height; j++)
                {
                    Color pixelColor = img1.GetPixel(i, j);
                    if (pixelColor.R > threshold)
                    {
                        img1.SetPixel(i, j, Color.FromArgb(255, 255, 255));
                        bitMap[i, j] = 1;
                    }
                    else
                    {
                        img1.SetPixel(i, j, Color.FromArgb(0, 0, 0));
                        bitMap[i, j] = 0;
                    }
                }
            }
            Stack<Pixcel> path = new Stack<Pixcel>();
            path.Push(new Pixcel(0, 0));            
            for (int i = 0; i < img1.Height; i++)
            {
                for (int j = 0; j < img1.Width; j++)
                {
                    File.AppendAllText("bitmap.txt", bitMap[j, i].ToString() + (j == img1.Width - 1 ? "" : ","));
                }
                File.AppendAllText("bitmap.txt", i == img1.Height - 1 ? "" : ";");
            }
        }
        static void Main(string[] args)
        {
            try
            {
                //打开位图文件
                Bitmap img1 = new Bitmap("test.jpg", true);
                //灰度化
                ToGrey(img1);
                //二值化
                Thresholding(img1);
                //写回位图文件
                img1.Save("output.jpg");
                Console.WriteLine("Converted.");
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid usage!");
                Console.WriteLine("Usage: bmp2grey source object");
            }
        }
    }
}
