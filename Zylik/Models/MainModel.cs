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

        public float[] dU { get; private set; }
        public float[] n { get; private set; }//массив с номерами начал участков
        public float[] k { get; private set; }//массив с номерами концов участков

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

            var a = Transformers.Count + Lines.Count;//общее количество участков

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

            byte[] ao = GetAO(n,k);//массив адресных отображений

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

            //Вычисление суммарных потерь активной, реактивной мощности и электроэнергии                     
            float olPj = 0;
            float olQj = 0;
            float olWpj = 0;
            float dW = dWl + dWt + dWxx;
            float dQ = dQl + dQt + dQxx;
            float dP = dPl + dPt + dPxx;
            float dQlt = dQt + dQl;
            float dPlt = dPl + dPt;
            for (byte i = Lines.Count; i < a; i++)
            {
                olPj += Pj[i];
                olQj += Qj[i];
                olWpj += Wpj[i];
            }
            float Wpgy = dW + olWpj;//Поток электроэнергии на головной участке линии
            float Pgy = dP + olPj;//Поток активной мощности на головном участке линии
            float Qgy = dQ + olQj;//Поток реактивной мощности на головном участке линии
            float dWproc = dW / Wpgy * 100;//Суммарные потери электроэнергии в процентах
            float dWtproc = dWt / Wpgy * 100;//Суммарные потери электроэнергии на участках с трансформаторами в процентах
            float dWlproc = dWl / Wpgy * 100;//Суммарные потери электроэнергии на участках линий в процентах
            float dWxxproc = dWxx / Wpgy * 100;//Суммарные потери электроэнергии в стали трансформаторов в процентах
            float dPproc = dP / Pgy * 100;//Потери активной мощности в процентах
            float dPtproc = dPt / Pgy * 100;//Потери активной мощности в трансформаторах в процентах
            float dPlproc = dPl / Pgy * 100;//Потери активной мощности на линейных участках в процентах
            float dPxxproc = dPxx / Pgy * 100;//Потери реактивной мощности в стали трансформаторов в процентах
            float dPltproc = dPlt / Pgy * 100;//Потери активной мощности в трансформаторах и на линейных участках схемы в процентах
            float dQproc = dQ / Qgy * 100;//Потери реактивной мощности в процентах
            float dQtproc = dQt / Qgy * 100; //Потери реактивной мощности в трансформаторах в процентах
            float dQlproc = dQl / Qgy * 100;//Потери реактивной мощности на линейных участках в процентах
            float dQxxproc = dQxx / Qgy * 100;//Потери реактивной мощности в стали трансформаторов в процентах          
            float dQltproc = dQlt / Qgy * 100;//Потери реактивной мощности в трансформаторах и на линейных участках схемы в процентах
            HelpClass.kolvo_lines = Lines.Count;
            HelpClass.kolvo_trans = Transformers.Count;
            HelpClass.Wpgy = Wpgy;
            HelpClass.Pgy = Pgy;
            HelpClass.Qgy = Qgy;
            HelpClass.dPt = dPt;
            HelpClass.dQt = dQt;
            HelpClass.dPxx = dPxx;
            HelpClass.dQxx = dQxx;
            HelpClass.dWt = dWt;
            HelpClass.dWxx = dWxx;
            HelpClass.dQl = dQl;
            HelpClass.dPl = dPl;
            HelpClass.dWl = dWl;
            HelpClass.dW = dW;
            HelpClass.dQ = dQ;
            HelpClass.dP = dP;
            HelpClass.dQlt = dQlt;
            HelpClass.dPlt = dPlt;
            HelpClass.dPtproc = dPtproc;
            HelpClass.dQtproc = dQtproc;
            HelpClass.dPxxproc = dPxxproc;
            HelpClass.dQxxproc = dQxxproc;
            HelpClass.dWtproc = dWtproc;
            HelpClass.dWxxproc = dWxxproc;
            HelpClass.dQlproc = dQlproc;
            HelpClass.dPlproc = dPlproc;
            HelpClass.dWlproc = dWlproc;
            HelpClass.dWproc = dWproc;
            HelpClass.dQproc = dQproc;
            HelpClass.dPproc = dPproc;
            HelpClass.dQltproc = dQltproc;
            HelpClass.dPltproc = dPltproc;
            HelpClass.a = a;
            Array.Copy(Snomj, HelpClass.Snomj = new float[Snomj.Length], Snomj.Length);
            Array.Copy(marka, HelpClass.marka = new string[marka.Length], marka.Length);
            Array.Copy(n, HelpClass.n = new float[n.Length], n.Length);
            Array.Copy(k, HelpClass.k = new float[k.Length], k.Length);
            Array.Copy(ao, HelpClass.ao = new float[ao.Length], ao.Length);
            Array.Copy(p, HelpClass.p = new float[p.Length], p.Length);
            Array.Copy(q, HelpClass.q = new float[q.Length], q.Length);
            Array.Copy(Wpi, HelpClass.Wpi = new float[Wpi.Length], Wpi.Length);
            Array.Copy(Wqi, HelpClass.Wqi = new float[Wqi.Length], Wqi.Length);
            Array.Copy(U, HelpClass.U = new float[U.Length], U.Length);
            Array.Copy(dQli, HelpClass.dQli = new float[dQli.Length], dQli.Length);
            Array.Copy(dPli, HelpClass.dPli = new float[dPli.Length], dPli.Length);
            Array.Copy(Tmai, HelpClass.Tmai = new float[Tmai.Length], Tmai.Length);
            Array.Copy(dWi, HelpClass.dWi = new float[dWi.Length], dWi.Length);
            Array.Copy(dU, HelpClass.dU = new float[dU.Length], dU.Length);
            Array.Copy(dQj, HelpClass.dQj = new float[dQj.Length], dQj.Length);
            Array.Copy(dPj, HelpClass.dPj = new float[dPj.Length], dPj.Length);
            Array.Copy(Tmaj, HelpClass.Tmaj = new float[Tmaj.Length], Tmaj.Length);
            Array.Copy(dWj, HelpClass.dWj = new float[dWj.Length], dWj.Length);
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
    }
}