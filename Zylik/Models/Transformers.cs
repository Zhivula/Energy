using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zylik.Models
{
    class Transformers
    {
        public const byte MAX = 100;

        public static byte Count { get; private set; }

        
        public float[] Snomj { get; private set; }
        public float[] Tmaj { get; private set; }
        public float[] Ixxj { get; private set; }
        public float[] Pkzj { get; private set; }
        public float[] Ukz { get; private set; }
        public float[] dPxxj { get; private set; }
        public float[] Qxxj { get; private set; }

        public float[] Sj { get; private set; }
        public float[] Pj { get; private set; }
        public float[] Qj { get; private set; }
        public float[] rj { get; private set; }
        public float[] dPj { get; private set; }
        public float[] zj { get; private set; }
        public float[] xj { get; private set; }
        public float[] dQj { get; private set; }
        public float[] kzj { get; private set; }
        public float[] kf2j { get; private set; }
        public float[] Ij { get; private set; }
        public float[] dQxxj { get; private set; }
        public float[] dWxxj { get; private set; }
        public float[] dU { get; private set; }
        public float[] Wpj { get; private set; }
        public float[] Wqj { get; private set; }
        public float[] dWj { get; private set; }

        public float[] n { get; private set; }
        public float[] k { get; private set; }

        public Transformers(float[] n, float[] k, float[] dU)
        {
            this.n = n;
            this.k = k;
            this.dU = dU;

            Snomj = new float[MAX];//номинальная мощность трансформатора j, кВА
            Tmaj = new float[MAX]; //Число часов использования максимальной активной нагрузки трансформатора
            Ixxj = new float[MAX];//ток холостого хода трансформатора j, % 
            Pkzj = new float[MAX];//потери мощности короткого замыкания трансформатора j, кВт 
            Ukz = new float[MAX];//напряжение короткого замыкания трансформатора j, % 
            dPxxj = new float[MAX];//потери холостого хода трансформатора j, кВт 
            Qxxj = new float[MAX];//потери холостого хода трансформатора j, кВт 

            Sj = new float[MAX];//полная нагрузка трансформатора
            Pj = new float[MAX];//поток активной мощности
            Qj = new float[MAX];//поток реактивной мощности
            rj = new float[MAX];//активное сопротивление трансформатора j, Ом:                    
            dPj = new float[MAX];//потери активной мощности на участках с трансформаторами
            zj = new float[MAX];//модуль полного сопротивления трансформатора j, Ом:         
            xj = new float[MAX];//реактивное сопротивление трансформатора j, Ом;
            dQj = new float[MAX];//потери реактивной мощности на участках с трансформаторами
            kzj = new float[MAX];//коэффициент заполнения графика
            kf2j = new float[MAX];//Коэффициент формы графика нагрузки
            Ij = new float[MAX];//Ток в трансформаторе
            dQxxj = new float[MAX];//потери реактивной мощности холостого хода трансформатора 
            dWxxj = new float[MAX];//потери энергии холостого ъода трансформатора
            dU = new float[MAX];//потери напряжения для каждого участка схемы
            Wpj = new float[MAX];//энергия потока активной мощности
            Wqj = new float[MAX];//энергия потока реактивной мощности
            dWj = new float[MAX];//Нагрузочные потери электроэнергии на трансформаторных участках            

            var mainArray = GetMainArray();

            InitArray(mainArray);

            Calculation();
        }

        private float[,] GetMainArray()
        {
            var size = File.ReadAllLines(@"trans.txt").Where(x => x != "").Count();//количество считываемых строк с файла данных о трансформаторах
            string[] trans = File.ReadAllLines(@"trans.txt").Take(size).ToArray();//в массив trans передаем все строки с файла trans.txt
            float[,] array = new float[size, 9];
            byte kolvo_trans = 0;//будет хранить количество участков с трансформаторами
            for (byte i = 2; i < size; i++)
            {//цикл начинается с 2, так как в первых 2-ух строках содержится описание данных о трансформаторах           
                kolvo_trans++;//счетчик, отражающий количество участков с трансформаторами
                float[] row = trans[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Single.Parse).ToArray();//заполняем массив row2 строками из массива trans, разделяя строки на отдельные значения, пропуская знак пробела, с дальнейшим преобразованием из типа string в тип float
                for (byte j = 0; j < 9; j++)
                {//каждая строка с характеристиками трансформаторами имеет 15 величин               
                    array[i, j] = row[j];//заполнение двумерного массива элементами row2
                }
            }
            Count = kolvo_trans;
            return array;
        }

        private void InitArray(float[,] array)
        {
            for (byte i = 0; i < Count; i++)
            {
                for (byte j = 0; j < 9; j++)
                {//в цикле каждый массив относящий к определенному трансформатору получит значение                
                    switch (j)
                    {
                        case 0:
                            n[i + Lines.Count] = array[i + 2, 0];//заполнение массива начал учатков линий только если это 1-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив n, то выходим из цикла switch                       
                        case 1:
                            k[i + Lines.Count] = array[i + 2, 1];//заполнение массива начал учатков линий только если это 2-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив k, то выходим из цикла switch                       
                        case 2:
                            Snomj[i + Lines.Count] = array[i + 2, 2];//заполнение массива начал учатков линий только если это 3-я величина по второму индексу в массиве array2
                            break;//если число попало в массив Snomj, то выходим из цикла switch                                            
                        case 3:
                            Tmaj[i + Lines.Count] = array[i + 2, 3];//заполнение массива начал учатков линий только если это 5-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Tmaj, то выходим из цикла switch                       
                        case 4:
                            dPxxj[i + Lines.Count] = array[i + 2, 4];//заполнение массива начал учатков линий только если это 7-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Pxxj, то выходим из цикла switch                      
                        case 5:
                            Qxxj[i + Lines.Count] = array[i + 2, 5];//заполнение массива начал учатков линий только если это 8-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Qxxj, то выходим из цикла switch                                                                      
                        case 6:
                            Ukz[i + Lines.Count] = array[i + 2, 6];//заполнение массива начал учатков линий только если это 11-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Ukz, то выходим из цикла switch                        
                        case 7:
                            Pkzj[i + Lines.Count] = array[i + 2, 7];//заполнение массива начал учатков линий только если это 12-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Pkzj, то выходим из цикла switch                        
                        case 8:
                            Ixxj[i + Lines.Count] = array[i + 2, 8];//заполнение массива начал учатков линий только если это 13-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Unomj, то выходим из цикла switch                       
                    }
                }
            }
        }
        private void Calculation()
        {
            for (int i = Lines.Count; i < Lines.Count+Transformers.Count; i++)
            {//в цикле ведется расчет значений для трансформаторов            
                Sj[i] = MainModel.kz * Snomj[i];
                Pj[i] = Sj[i] * MainModel.cosf;
                Qj[i] = Sj[i] * (float)Math.Sqrt(1 - MainModel.cosf * MainModel.cosf);
                rj[i] = (Pkzj[i] * MainModel.Unom * MainModel.Unom) / (Snomj[i] * Snomj[i]);
                dPj[i] = (Pj[i] * Pj[i] + Qj[i] * Qj[i]) * rj[i] / (MainModel.Unom * MainModel.Unom);
                zj[i] = (Ukz[i] * MainModel.Unom * MainModel.Unom) / (100 * Snomj[i]);
                xj[i] = (float)Math.Sqrt(zj[i] * zj[i] - rj[i] * rj[i]);
                dQj[i] = (Pj[i] * Pj[i] + Qj[i] * Qj[i]) * xj[i] / (MainModel.Unom * MainModel.Unom);
                kzj[i] = Tmaj[i] / 8760;
                kf2j[i] = (float)(0.16 / kzj[i] + 0.82) * (float)(0.16 / kzj[i] + 0.82);
                Ij[i] = Pj[i] / (float)(Math.Sqrt(3) * 0.38 * MainModel.cosf);
                dQxxj[i] = Ixxj[i] * Snomj[i] / 100;
                dWxxj[i] = dPxxj[i] * 8760;
                dU[i] = (Qj[i] * xj[i] + rj[i] * Pj[i]) / (MainModel.Unom * 1000);
                Wpj[i] = Pj[i] * Tmaj[i];
                Wqj[i] = Wpj[i] * MainModel.tgf;
                dWj[i] = (Wpj[i] * Wpj[i] * (1 + MainModel.tgf * MainModel.tgf) * rj[i] * kf2j[i]) / (8760 * MainModel.Unom * MainModel.Unom);
            }
        }
        public void LeadToTheLowerSide(ref float[] U)
        {
            for (int i = Lines.Count; i < Lines.Count + Count; i++)
            {//рассматриваем участки с трансформаторами           
                U[i] = U[i] / 25;//приводим к стороне низшего напряжения 
            }
        }
        public void VoltageCalculation(ref float[] U)
        {
            var a = Lines.Count + Transformers.Count;

            for (byte i = 0; i < Lines.Count; i++)
            {
                for (byte j = Lines.Count; j < a; j++)
                {
                    if (k[i] == n[j])
                    {  //если конце участка линии совпадает с началом участка с трансформатором                
                        U[j] = U[i] - dU[j];//расчет напряжений на участках с трансформаторами
                    }
                }
            }
        }
    }
}