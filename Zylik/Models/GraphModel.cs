using System;
using System.Drawing;
using System.Windows.Forms;
using Energy;

namespace Zylik.Models
{
    class GraphModel
    {
        public const float LengthLine = 120f; // Длинна участка линии на карте

        public const byte Max = 100; //Задает начальное количество элементов в массивах

        public const float Radius = 30f; //радиус кругов(трансформаторов) на карте

        public const float Value = 100f;

        public PictureBox Picture { get; private set; }

        readonly SolidBrush brush;
        readonly Font font20;
        readonly Font font15;
        readonly Font font12;
        readonly Pen penBlack5;
        readonly Pen penBlack1;
        readonly Bitmap bmm;
        readonly Graphics g;
        readonly FontFamily calibryFont;

        public GraphModel()
        {            
            bmm = new Bitmap(3000, 900);//размерность полотна
            g = Graphics.FromImage(bmm);// g - графический компонент

            penBlack5 = new Pen(Color.Black, 5);// перо черного цвета, размер = 5
            penBlack1 = new Pen(Color.Black, 1);// перо черного цвета, размер = 1

            calibryFont = new FontFamily("Calibri");//выбрали шрифт Calibri
            font20 = new Font(calibryFont, 20);// размер шрифта 20
            font15 = new Font(calibryFont, 15);// размер шрифта 15
            font12 = new Font(calibryFont, 12);// размер шрифта 12

            brush = new SolidBrush(Color.Black);//кисть черного цвета                                                                          //PictureBox picture = new PictureBox();
            Picture = new PictureBox() { Height = 1000, Width = 3000, Image = bmm };          

            var map_x1 = new float[Max];
            var map_x2 = new float[Max];
            var map_y1 = new float[Max];
            var map_y2 = new float[Max];
            
            map_x1[1] = 135; map_x2[1] = 235;//начальные положения
            map_y1[1] = 550; map_y2[1] = 550;

            SortInit();

            bool[] mass_0_1 = GetTransformerOrLines();

            byte[] branches = GetCountOfBranches();

            var alfa_t = GetAngleOfRotation(branches);

            DrawingFirstNode(ref g, map_y1[1]);

            for (byte i = 0; i <= HelpClass.a; i++)
            {
                for (byte j = 0; j <= HelpClass.a; j++)
                {
                    float cos_k = (float)Math.Cos((alfa_t[j] * Math.PI) / 180);
                    float sin_k = (float)Math.Sin((alfa_t[j] * Math.PI) / 180);
                    if (HelpClass.k[i] == HelpClass.n[j] & mass_0_1[j] == true & branches[j] > 0)//рисуем линию
                    {
                        map_x1[j] = map_x2[i];
                        map_x2[j] = map_x1[j] + cos_k * LengthLine;
                        map_y1[j] = map_y2[i];
                        map_y2[j] = map_y1[j] + sin_k * LengthLine;
                        DrawingLine(ref g, map_x1[j], map_x2[j], map_y1[j], map_y2[j], alfa_t[j], j);
                    }
                    else if (HelpClass.k[i] == HelpClass.n[j] & mass_0_1[j] == false) //рисуем трансформатор
                    {
                        map_x1[j] = map_x2[i];
                        map_x2[j] = map_x1[j] + cos_k * (Value / 4);
                        map_y1[j] = map_y2[i];
                        map_y2[j] = map_y1[j] + sin_k * (Value / 4);

                        DrawingTransformer(ref g, map_x1[j], map_x2[j], map_y1[j], map_y2[j], alfa_t[j], j);

                        i++;
                    }
                }
            }
            
        }
        /// <summary>
        /// Рисование линии на полотне.
        /// </summary>
        /// <param name="g">Графический компонент, на котором происходит рисание</param>
        /// <param name="x1">X первой координаты</param>
        /// <param name="x2">X второй координаты</param>
        /// <param name="y1">Y первой координаты</param>
        /// <param name="y2">Y второй координаты</param>
        /// <param name="alfa_t">Угол поворота</param>
        /// <param name="j">Номер в массиве</param>
        private void DrawingLine(ref Graphics g, float x1, float x2, float y1, float y2, float alfa_t, int j)
        {
            float cos_k = (float)Math.Cos((alfa_t * Math.PI) / 180);
            float sin_k = (float)Math.Sin((alfa_t * Math.PI) / 180);
            
            g.DrawLine(penBlack1, x1, y1, x2, y2);
            g.FillEllipse(brush, x2 - 5, y2 - 5, 10, 10);
            g.DrawString(HelpClass.k[j].ToString(), font15, brush, x2 - 10, y2 - 35);
            g.TranslateTransform(x1 + 30 * cos_k, y1 + 30 * sin_k);
            g.RotateTransform(alfa_t);
            g.DrawString("A-" + HelpClass.marka[j], font15, Brushes.Green, 0, 0);
            g.RotateTransform(-alfa_t);
            g.TranslateTransform(-(x1 + 30 * cos_k), -(y1 + 30 * sin_k));
        }
        /// <summary>
        /// Рисование трансформатора на полотне.
        /// </summary>
        /// <param name="g">Графический компонент, на котором происходит рисание</param>
        /// <param name="x1">X первой координаты</param>
        /// <param name="x2">X второй координаты</param>
        /// <param name="y1">Y первой координаты</param>
        /// <param name="y2">Y второй координаты</param>
        /// <param name="alfa_t">Угол поворота</param>
        /// <param name="j">Номер в массиве</param>
        private void DrawingTransformer(ref Graphics g, float x1, float x2, float y1, float y2, float alfa_t, int j)
        {
            float cos_k = (float)Math.Cos((alfa_t * Math.PI) / 180);
            float sin_k = (float)Math.Sin((alfa_t * Math.PI) / 180);

            g.TranslateTransform(x1 + cos_k * 0.2f * Value, y1 + sin_k * 0.25f * Value - 1.5f * Radius);
            g.RotateTransform(alfa_t);
            g.DrawString("ТМ-" + HelpClass.Snomj[j], font15, Brushes.Blue, 0, 0);
            g.RotateTransform(-alfa_t);
            g.TranslateTransform(-(x1 + cos_k * 0.2f * Value), -(y1 + sin_k * 0.25f * Value - 1.5f * Radius));
            g.DrawLine(penBlack1, x1, y1, x2, y2);
            g.DrawEllipse(penBlack1, x1 + cos_k * 0.4f * Value - Radius / 2, y1 + sin_k * 0.4f * Value - Radius / 2, Radius, Radius);
            g.DrawEllipse(penBlack1, x1 + cos_k * 0.6f * Value - Radius / 2, y1 + sin_k * 0.6f * Value - Radius / 2, Radius, Radius);
            x1 += cos_k * (Value * 0.75f);
            x2 = x1 + cos_k * (Value / 4);
            y1 += sin_k * (Value * 0.75f);
            y2 = y1 + sin_k * (Value / 4);
            g.DrawLine(penBlack1, x1, y1, x2, y2);
            g.DrawString(HelpClass.k[j].ToString(), font12, Brushes.Red, x2 + 10, y2 - 30);
        }

        private void DrawingFirstNode(ref Graphics g, float y1)
        {
            g.DrawLine(penBlack5, 0, y1 - 25, 0, y1 + 25);
            g.DrawLine(penBlack1, 0, y1, LengthLine, y1);
            g.DrawLine(penBlack1, LengthLine, y1, 2 * LengthLine, y1);
            g.FillEllipse(brush, LengthLine - 5, y1 - 5, 10, 10);
            g.FillEllipse(brush, 2 * LengthLine - 5, y1 - 5, 10, 10);
            g.DrawString("1", font15, brush, LengthLine - 10, y1 - 35);
            g.DrawString("2", font15, brush, 2 * LengthLine - 10, y1 - 35);
            g.DrawString("A-50", font15, Brushes.Green, new PointF(155, y1 - 25));
        }
        /// <summary>
        /// Возвращает массив, который заполнен значениями углов, 
        /// на которые нужно здвинуть линию в сторону, 
        /// Этот угол зависит от количества отходящих ветвей от данного узла
        /// </summary>
        /// <param name="branches"></param>
        /// <param name="max"></param>
        /// <returns>Массив углов</returns>
        private float[] GetAngleOfRotation(byte[] branches)
        {
            float alfa;
            var alfa_t = new float[Max];

            for (int i = 1; i <= HelpClass.a; i++) //глобальный цикл
            {
                alfa = 180 / (branches[i] + 1);//элементарный угол   

                for (int j = 0; j < branches[i]; j++)
                {
                    alfa_t[i + j] = 90 - alfa * (j + 1);
                }
                i = i + branches[i] - 1;
            }
            return alfa_t;
        }

        /// <summary>
        /// Заполниет массив таким образом, что
        /// true - это линия
        /// false - это трансформатор
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        private bool[] GetTransformerOrLines()
        {
            bool[] mass_0_1 = new bool[Max];

            for (byte i = 0; i < HelpClass.a; i++)
            {
                for (byte j = 0; j <= HelpClass.a; j++)
                {
                    if (HelpClass.k[i] == HelpClass.n[j])
                    {
                        mass_0_1[i] = true; //линия найдена, т.к. только у линии в данном случае есть дальше элементы
                    }
                }
            }
            return mass_0_1;
        }

        /// <summary>
        /// Заполниет массив таким образом, что каждый его элемент отображает количество 
        /// отводимых ветвей от данной узла (поторые идут по порядку)
        /// </summary>
        /// <param name="max"></param>
        /// <returns></returns>
        private byte[] GetCountOfBranches()
        {
            var branches = new byte[Max];
            byte ch = 0;// просто счётчик               
            for (var i = 1; i <= HelpClass.a; i++)
            {
                for (var j = 1; j <= HelpClass.a; j++)
                {
                    if (HelpClass.n[i] == HelpClass.n[j]) ch++;
                }
                branches[i] = ch;
                ch = 0;
            }
            return branches;
        }
        private void SortInit()
        {
            float bypher = 0f;//буферная переменная, для переброски данных массива
            string bypher2 = "";//буферная переменная, для переброски данных массива

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
        }
    }
}
