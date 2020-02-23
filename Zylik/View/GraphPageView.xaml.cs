using System;
using System.Drawing;
using System.Windows.Forms;

namespace Zylik.View
{
    /// <summary>
    /// Логика взаимодействия для GraphPageView.xaml
    /// </summary>
    public partial class GraphPageView : System.Windows.Controls.UserControl
    {
        public GraphPageView()
        {                           
        InitializeComponent();
        Graphik();
        }
        public void Graphik()
        {
            PictureBox picture = new PictureBox();
            picture.Height = 1000;
            picture.Width = 3000;
            Bitmap bmm = new Bitmap(3000, 900);//размерность полотна
            Graphics g = Graphics.FromImage(bmm);// g - графический компонент
            System.Drawing.Pen pen1 = new System.Drawing.Pen(System.Drawing.Color.Black, 5);
            System.Drawing.Pen pen0 = new System.Drawing.Pen(System.Drawing.Color.Black, 1);// перо черного цвета, размер = 1
            System.Drawing.FontFamily ff = new System.Drawing.FontFamily("Calibri");//выбрали шрифт Calibri
            Font font = new Font(ff, 20);// размер шрифта 20
            Font font1 = new Font(ff, 15);// размер шрифта 15
            Font font2 = new Font(ff, 12);
            SolidBrush brush = new SolidBrush(System.Drawing.Color.Black);//кисть черного цвета                                                                          //PictureBox picture = new PictureBox();
            picture.Image = bmm;
            WindowsFormsHost3.Child = picture;
            //pict = bmm;//компонент picture, именуемый по умолчанию как pictureBox, позволяет рисовать на bmm
            byte max = 100;
            float l = 120f;//длина линии на Map
            float bypher = 0f;//буферная переменная, для переброски данных массива
            string bypher2 = "";
            float[] map_x1 = new float[max], map_x2 = new float[max], map_y1 = new float[max], map_y2 = new float[max], alfa = new float[max], alfa_t = new float[max];
            byte[] vetvi = new byte[max];
            map_x1[1] = 135; map_x2[1] = 235;//начальные положения
            map_y1[1] = 550; map_y2[1] = 550;
            for (byte i = 0; i < HelpClass.a; i++)
            {
                for (int j = 0; j < HelpClass.a - i; j++)
                {
                    if (HelpClass.n[j] > HelpClass.n[j + 1])
                    {
                        bypher = HelpClass.n[j];
                        HelpClass.n[j] = HelpClass.n[j + 1];
                        HelpClass.n[j + 1] = bypher;
                        bypher = HelpClass.k[j];
                        HelpClass.k[j] = HelpClass.k[j + 1];
                        HelpClass.k[j + 1] = bypher;
                        bypher = HelpClass.Snomj[j];
                        HelpClass.Snomj[j] = HelpClass.Snomj[j + 1];
                        HelpClass.Snomj[j + 1] = bypher;
                        bypher2 = HelpClass.marka[j];
                        HelpClass.marka[j] = HelpClass.marka[j + 1];
                        HelpClass.marka[j + 1] = bypher2;
                    }
                }
            }
            byte[] mass_0_1 = new byte[max];
            for (byte i = 0; i < HelpClass.a; i++)
            {
                for (byte j = 0; j <= HelpClass.a; j++)
                {
                    if (HelpClass.k[i] == HelpClass.n[j])
                    {
                        mass_0_1[i] = 1;
                    }
                }
            }
            byte ch = 0;// просто счётчик               
            for (byte i = 1; i <= HelpClass.a; i++)
            {
                for (byte j = 1; j <= HelpClass.a; j++)
                {
                    if (HelpClass.n[i] == HelpClass.n[j]) ch++;
                }
                vetvi[i] = ch;
                ch = 0;
            }
            alfa[0] = 0f;
            for (int i = 1; i <= HelpClass.a; i++)
            { //глобальный цикл                       
                for (int j = 0; j < vetvi[i]; j++)
                {
                    alfa_t[i + j] = 0;
                    alfa[i] = 180 / (vetvi[i] + 1);//элементарный угол              
                    alfa_t[i + j] = 90 - alfa[i] * (j + 1);
                }
                i = i + vetvi[i] - 1;
            }
            float value = 100f;
            float R = 30f;//радиус кругов(трансформаторов) на карте
            g.DrawLine(pen1, 0, map_y1[1] - 25, 0, map_y1[1] + 25);
            g.DrawLine(pen0, 0, map_y1[1], l, map_y1[1]);
            g.DrawLine(pen0, l, map_y1[1], 2 * l, map_y1[1]);
            g.FillEllipse(brush, l - 5, map_y1[1] - 5, 10, 10);
            g.FillEllipse(brush, 2 * l - 5, map_y1[1] - 5, 10, 10);
            g.DrawString("1", font1, brush, l - 10, map_y1[1] - 35);
            g.DrawString("2", font1, brush, 2 * l - 10, map_y1[1] - 35);
            g.DrawString("A-50", font1, System.Drawing.Brushes.Green, new PointF(155, map_y1[1] - 25));
            for (byte i = 0; i <= HelpClass.a; i++)
            {
                for (byte j = 0; j <= HelpClass.a; j++)
                {
                    float cos_k = (float)Math.Cos((alfa_t[j] * Math.PI) / 180);
                    float sin_k = (float)Math.Sin((alfa_t[j] * Math.PI) / 180);
                    if (HelpClass.k[i] == HelpClass.n[j] & mass_0_1[j] == 1 & vetvi[j] > 0)
                    {
                        map_x1[j] = map_x2[i];
                        map_x2[j] = map_x1[j] + cos_k * l;
                        map_y1[j] = map_y2[i];
                        map_y2[j] = map_y1[j] + sin_k * l;
                        g.DrawLine(pen0, map_x1[j], map_y1[j], map_x2[j], map_y2[j]);
                        g.FillEllipse(brush, map_x2[j] - 5, map_y2[j] - 5, 10, 10);
                        g.DrawString(HelpClass.k[j].ToString(), font1, brush, map_x2[j] - 10, map_y2[j] - 35);
                        g.TranslateTransform(map_x1[j] + 30 * cos_k, map_y1[j] + 30 * sin_k);
                        g.RotateTransform(alfa_t[j]);
                        g.DrawString("A-" + HelpClass.marka[j], font1, System.Drawing.Brushes.Green, 0, 0);
                        g.RotateTransform(-alfa_t[j]);
                        g.TranslateTransform(-(map_x1[j] + 30 * cos_k), -(map_y1[j] + 30 * sin_k));
                    }
                    else if (HelpClass.k[i] == HelpClass.n[j] & mass_0_1[j] == 0)
                    {//рисуем трансформатор                   
                        map_x1[j] = map_x2[i];
                        map_x2[j] = map_x1[j] + cos_k * (value / 4);
                        map_y1[j] = map_y2[i];
                        map_y2[j] = map_y1[j] + sin_k * (value / 4);
                        g.TranslateTransform(map_x1[j] + cos_k * 0.2f * value, map_y1[j] + sin_k * 0.25f * value - 1.5f * R);
                        g.RotateTransform(alfa_t[j]);
                        g.DrawString("ТМ-" + HelpClass.Snomj[j], font1, System.Drawing.Brushes.Blue, 0, 0);
                        g.RotateTransform(-alfa_t[j]);
                        g.TranslateTransform(-(map_x1[j] + cos_k * 0.2f * value), -(map_y1[j] + sin_k * 0.25f * value - 1.5f * R));
                        g.DrawLine(pen0, map_x1[j], map_y1[j], map_x2[j], map_y2[j]);
                        g.DrawEllipse(pen0, map_x1[j] + cos_k * 0.4f * value - R / 2, map_y1[j] + sin_k * 0.4f * value - R / 2, R, R);
                        g.DrawEllipse(pen0, map_x1[j] + cos_k * 0.6f * value - R / 2, map_y1[j] + sin_k * 0.6f * value - R / 2, R, R);
                        map_x1[j] += cos_k * (value * 0.75f);
                        map_x2[j] = map_x1[j] + cos_k * (value / 4);
                        map_y1[j] += sin_k * (value * 0.75f);
                        map_y2[j] = map_y1[j] + sin_k * (value / 4);
                        g.DrawLine(pen0, map_x1[j], map_y1[j], map_x2[j], map_y2[j]);
                        g.DrawString(HelpClass.k[j].ToString(), font2, System.Drawing.Brushes.Red, map_x2[j] + 10, map_y2[j] - 30);
                        i++;
                    }
                }
            }
           
        }
    }
}
