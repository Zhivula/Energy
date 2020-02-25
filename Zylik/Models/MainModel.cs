using Energy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zylik.Models
{
    class MainModel
    {
        public const byte max = 100;// для быстрого измения размерности основных массивов, используемых при расчетах

        public const float tgf = 0.75f;

        public const float cosf = 0.8f;//коэффициент мощности трансформатора;

        public const float Unom = 10.5f;//номинальное напряжение, кВ 

        public const float kz = 1;//коэффициент загрузки трансформатора

        public float[] dU { get; private set; }//Потери напряжения в узлах
        public float[] n { get; private set; }//массив с номерами начал участков
        public float[] k { get; private set; }//массив с номерами концов участков

        public byte[] ao { get; private set; }

        public string[] marka { get; private set; }
        public float[] dl { get; private set; }
        public float[] R0 { get; private set; }
        public float[] X0 { get; private set; }
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

        public float[] Wpj { get; private set; }
        public float[] Wqj { get; private set; }
        public float[] dWj { get; private set; }
        public float[] p { get; private set; }
        public float[] q { get; private set; }
        public float[] Wpi { get; private set; }
        public float[] Wqi { get; private set; }
        public float[] rl { get; private set; }
        public float[] xl { get; private set; }
        public float[] Tmai { get; private set; }
        public float[] kzi { get; private set; }
        public float[] kf2i { get; private set; }
        public float[] tgfl { get; private set; }
        public float[] dWi { get; private set; }
        public float[] dQli { get; private set; }
        public float[] dPli { get; private set; }
        public float[] U;

        public float dPt { get; private set; }//Суммарные потери активной мощности в трансформаторах
        public float dQt { get; private set; }//Суммарные потери реактивной мощности в трансформаторах
        public float dPxx { get; private set; }//суммарные потери холостого хода трансформатора
        public float dQxx { get; private set; }//суммарные потери реактивной мощности в стали трансформаторов
        public float dWt { get; private set; }//Суммарные нагрузочные потери электроэнергии в трансформаторах
        public float dWxx { get; private set; }//Суммарные нагрузочные потери электроэнергии в стали трансформаторов

        public float dQl { get; private set; }//суммарные потери реактивной мощности на линейных участках
        public float dPl { get; private set; }//суммарные потери активной мощности на линейных участках
        public float dWl { get; private set; }//суммарные нагрузочные потери электроэнергии на линейных участках схемы

        public float olPj { get; private set; }
        public float olQj { get; private set; }
        public float olWpj { get; private set; }

        public float dW { get; private set; }
        public float dQ { get; private set; }
        public float dP { get; private set; }
        public float dQlt { get; private set; }
        public float dPlt { get; private set; }
        public float Wpgy { get; private set; }
        public float Pgy { get; private set; }
        public float Qgy { get; private set; }
        public float dWproc { get; private set; }
        public float dWtproc { get; private set; }
        public float dWlproc { get; private set; }
        public float dWxxproc { get; private set; }
        public float dPproc { get; private set; }
        public float dPlproc { get; private set; }
        public float dPtproc { get; private set; }
        public float dPxxproc { get; private set; }
        public float dPltproc { get; private set; }
        public float dQproc { get; private set; }
        public float dQtproc { get; private set; }
        public float dQlproc { get; private set; }
        public float dQxxproc { get; private set; }
        public float dQltproc { get; private set; }
        public int a { get; private set; }

        public MainModel()
        {
            dU = new float[max];
            n = new float[max];
            k = new float[max];

            Lines lines = new Lines(n,k,dU); 
            dl = lines.dl;
            R0 = lines.R0;
            X0 = lines.X0;
            k = lines.k;
            n = lines.n;
            marka = lines.marka;
            dU = lines.dU;

            Transformers transformers = new Transformers(n, k, dU);

            n = transformers.n;
            k = transformers.k;
            dU = transformers.dU;

            a = Transformers.Count + Lines.Count;//общее количество участков

            Snomj = transformers.Snomj;
            Tmaj = transformers.Tmaj;
            Ixxj = transformers.Ixxj;
            Pkzj = transformers.Pkzj;
            Ukz = transformers.Ukz;
            dPxxj = transformers.dPxxj;
            Qxxj = transformers.Qxxj;

            Sj = transformers.Sj;
            Pj = transformers.Pj;
            Qj = transformers.Qj;
            rj = transformers.rj;
            dPj = transformers.dPj;
            zj = transformers.zj;
            xj = transformers.xj;
            dQj = transformers.dQj;
            kzj = transformers.kzj;
            kf2j = transformers.kf2j;
            Ij = transformers.Ij;
            dQxxj = transformers.dQxxj;
            dWxxj = transformers.dWxxj;

            Wpj = transformers.Wpj;
            Wqj = transformers.Wqj;
            dWj = transformers.dWj;

            ao = GetAO(n,k);//массив адресных отображений

            FlowsPower flowsPower = new FlowsPower(transformers, ao);

            p = flowsPower.p;
            q = flowsPower.q;
            Wpi = flowsPower.Wpi;
            Wqi = flowsPower.Wqi;

            lines.Calculation(Wqi, Wpi, p, q);  
            
            // расчёт данных линий
            rl = lines.rl;
            xl = lines.xl;
            Tmai = lines.Tmai;
            kzi = lines.kzi;
            kf2i = lines.kf2i;
            tgfl = lines.tgfl;
            dWi = lines.dWi;
            dQli = lines.dQli;
            dPli = lines.dPli;

            U = new float[max];

            U[0] = 10.5f - dU[0];//чтобы не нарушать границы массива, рассчитываем напряжение на первом участке вне цикла

            lines.VoltageCalculations(ref U);
            transformers.VoltageCalculation(ref U);

            transformers.LeadToTheLowerSide(ref U);//Приведение к нижней стороне напряжения

            FullPowerTransformers();

            FullPowerLines();

            SumPowerTransformers();

            SumPowerLinesTransformers();
        }

        private byte[] GetAO(float[] n, float[] k)
        {
            var ao = new byte[max];
            for (byte i = 0; i < Lines.Count+Transformers.Count; i++)
            {
                for (byte j = 0; j < Lines.Count + Transformers.Count; j++)
                {
                    if (k[i] == n[j])
                    {//сравнение конца одного участка с началами всех участков                   
                        ao[j] = (byte)(i+1);//если нашли совпадение, то в массив ao попадает значение b
                    }
                }
            }
            return ao;
        }
        /// <summary>
        /// расчет активных и реактивных потерь мощностей и электроэнергии на трансформатарах
        /// </summary>
        private void FullPowerTransformers()
        {
            for (byte i = Lines.Count; i < Lines.Count+Transformers.Count; i++)
            {
                dPt += dPj[i];
                dQt += dQj[i];
                dPxx += dPxxj[i];
                dQxx += dQxxj[i];
                dWt += dWj[i];
                dWxx += dWxxj[i];
            }
        }
        /// <summary>
        /// расчет активных и реактивных потерь мощностей и электроэнергии на линиях.
        /// </summary>
        private void FullPowerLines()
        {
            for (byte i = 0; i < Lines.Count; i++)
            {
                dQl += dQli[i];
                dPl += dPli[i];
                dWl += dWi[i];
            }
        }
        private void SumPowerTransformers()
        {
            for (byte i = Lines.Count; i < Lines.Count+Transformers.Count; i++)
            {
                olPj += Pj[i];
                olQj += Qj[i];
                olWpj += Wpj[i];
            }
        }
        private void SumPowerLinesTransformers()
        {
            dW = dWl + dWt + dWxx;
            dQ = dQl + dQt + dQxx;
            dP = dPl + dPt + dPxx;
            dQlt = dQt + dQl;
            dPlt = dPl + dPt;

            Wpgy = dW + olWpj;//Поток электроэнергии на головной участке линии
            Pgy = dP + olPj;//Поток активной мощности на головном участке линии
            Qgy = dQ + olQj;//Поток реактивной мощности на головном участке линии
            dWproc = dW / Wpgy * 100;//Суммарные потери электроэнергии в процентах
            dWtproc = dWt / Wpgy * 100;//Суммарные потери электроэнергии на участках с трансформаторами в процентах
            dWlproc = dWl / Wpgy * 100;//Суммарные потери электроэнергии на участках линий в процентах
            dWxxproc = dWxx / Wpgy * 100;//Суммарные потери электроэнергии в стали трансформаторов в процентах
            dPproc = dP / Pgy * 100;//Потери активной мощности в процентах
            dPtproc = dPt / Pgy * 100;//Потери активной мощности в трансформаторах в процентах
            dPlproc = dPl / Pgy * 100;//Потери активной мощности на линейных участках в процентах
            dPxxproc = dPxx / Pgy * 100;//Потери реактивной мощности в стали трансформаторов в процентах
            dPltproc = dPlt / Pgy * 100;//Потери активной мощности в трансформаторах и на линейных участках схемы в процентах
            dQproc = dQ / Qgy * 100;//Потери реактивной мощности в процентах
            dQtproc = dQt / Qgy * 100; //Потери реактивной мощности в трансформаторах в процентах
            dQlproc = dQl / Qgy * 100;//Потери реактивной мощности на линейных участках в процентах
            dQxxproc = dQxx / Qgy * 100;//Потери реактивной мощности в стали трансформаторов в процентах          
            dQltproc = dQlt / Qgy * 100;//Потери реактивной мощности в трансформаторах и на линейных участках схемы в процентах
        }
    }
}