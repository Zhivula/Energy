using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zylik.Models
{
    class Lines
    {
        public const byte MAX = 100;

        public static byte Count { get; private set; }

        public float[] dl { get; private set; }
        public  float[] R0 { get; private set; }
        public float[] X0 { get; private set; }
        public float[] k { get; private set; }
        public string[] marka { get; private set; }
        public float[] n { get; private set; }

        public float[] rl { get; private set; }
        public float[] xl { get; private set; }
        public float[] Tmai { get; private set; }
        public float[] kzi { get; private set; }
        public float[] kf2i { get; private set; }
        public float[] tgfl { get; private set; }
        public float[] dWi { get; private set; }
        public float[] dQli { get; private set; }
        public float[] dPli { get; private set; }

        public float[] dU { get; private set; }

        public Lines(float[] n, float[] k, float[] dU)
        {
            this.k = k;
            this.n = n;
            this.dU = dU;

            dl = new float[MAX];//длина участка
            R0 = new float[MAX]; //удельное активное сопротивление участка, Ом/ км
            X0 = new float[MAX];//удельное реактивное сопротивление, Ом/км          
            marka = new string[MAX];

            rl = new float[MAX];//активное сопротивление i-о участка линии, Ом.
            xl = new float[MAX];//реактивное сопротивление i-о линейного участка схемы, Ом;
            Tmai = new float[MAX];//Число часов использования максимальной активной нагрузки на участках линий
            kzi = new float[MAX];//коэффициент заполнения графика, равный относительному числу часов использования максимальной активной нагрузки Тма i:
            kf2i = new float[MAX];//коэффициент формы графика нагрузки, о.е.;
            tgfl = new float[MAX];//коэффициент реактивной мощности, о.е.;          
            dWi = new float[MAX]; //поток активной энергии на i - м линейном участке схемы, кВт*ч;
            dQli = new float[MAX];//потери реактивной мощности на участках линий
            dPli = new float[MAX];//потери активной мощности на участках линий

            var mainArray = GetMainArray();

            InitArray(mainArray);
        }

        private float[,] GetMainArray()
        {
            var size = File.ReadAllLines(@"linii.txt").Where(x => x != "").Count();//количество считываемых строк с файла данных о линиях
            string[] lines = File.ReadAllLines(@"linii.txt").Take(size).ToArray();//в массив lines передаем все строки с файла linii.txt
            float[,] array = new float[size, 6];
            byte kolvo_lines = 0;//будет хранить количество участков линий
            for (byte i = 4; i < size; i++)//цикл начинается с 4, так как в первых 4-ёх строках содержится описание данных о линиях    
            {     
                kolvo_lines++;//счетчик, отражающий количество участков линий
                float[] row = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Single.Parse).ToArray();//заполняем массив row1 строками из массива lines, разделяя строки на отдельные значения, пропуская знак пробела, с дальнейшим преобразованием из типа string в тип float
                for (byte j = 0; j < 6; j++)//каждая строка с характеристиками линии имеет 5 величин 
                {               
                    array[i, j] = row[j];
                }
            }
            Count = kolvo_lines;
            return array;
        }

        private void InitArray(float[,] array)
        {
            for (byte i = 0; i < Count; i++)
            {           
                for (byte j = 0; j < 6; j++)
                {//в каждый массив данных о линиях по очереди будет попадать величина находящаяся в двумерном массиве array1               
                    switch (j)
                    {
                        case 0:
                            n[i] = array[i + 4, 0];//заполнение массива начал учатков линий только если это первая величина по второму индексу в массиве array1
                            break;//если число попало в массив n, то выходим из цикла switch                       
                        case 1:
                            k[i] = array[i + 4, 1];//заполнение массива концов учатков линий только если это вторая величина по второму индексу в массиве array1
                            break;//если число попало в массив k, то выходим из цикла switch                      
                        case 2:
                            R0[i] = array[i + 4, 2];//заполнение массива удельных активных сопротивлений учатков линий только если это третья величина по второму индексу в массиве array1
                            break;//если число попало в массив R0, то выходим из цикла switch                   
                        case 3:
                            X0[i] = array[i + 4, 3];//заполнение массива удельных реактивных сопротивлений учатков линий только если это четвертая величина по второму индексу в массиве array1
                            break;//если число попало в массив X0, то выходим из цикла switch                     
                        case 4:
                            dl[i] = array[i + 4, 4];//заполнение массива длин учатков линий только если это пятая величина по второму индексу в массиве array1
                            break;//если число попало в массив dl, то выходим из цикла switch                       
                        case 5: marka[i] = array[i + 4, 5].ToString(); break;
                    }
                }
            }
        }
        /// <summary>
        /// Расчет основных характеристик для линий электропередач.
        /// </summary>
        public void Calculation(float[] Wqi, float[] Wpi, float[] p, float[] q)
        {
            for (int i = 0; i < Count; i++)
            {//расчет величин только для участков линий          
                rl[i] = R0[i] * dl[i];
                xl[i] = X0[i] * dl[i];
                Tmai[i] = Wpi[i] / p[i];
                kzi[i] = Tmai[i] / 8760;
                kf2i[i] = (float)(0.16 / kzi[i] + 0.82) * (float)(0.16 / kzi[i] + 0.82);
                tgfl[i] = Wqi[i] / Wpi[i];
                dWi[i] = (Wpi[i] * Wpi[i] * (1 + tgfl[i] * tgfl[i]) * kf2i[i] * rl[i]) / (MainModel.Unom * MainModel.Unom * 8760 * 1000);
                dU[i] = (rl[i] * p[i] + xl[i] * q[i]) / (MainModel.Unom * 1000);
                dQli[i] = (p[i] * p[i] + q[i] * q[i]) * xl[i] / (MainModel.Unom * MainModel.Unom * 1000);
                dPli[i] = (p[i] * p[i] + q[i] * q[i]) * rl[i] / (MainModel.Unom * MainModel.Unom * 1000);
            }
        }
        /// <summary>
        /// Вычисление напряжения на каждом узле в сети с учетом потерь.
        /// </summary>
        /// <param name="U">Массив напряжений всей сети</param>
        public void VoltageCalculations(ref float[] U)
        {
            for (byte i = 1; i < Lines.Count; i++)
            {
                U[i] = U[i - 1] - dU[i];//расчет наприжений на участках линий
            }
        }
    }
}