using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zylik.Models
{
    class FlowsPower
    {
        Transformers transformers;

        public float[] p { get; private set; }
        public float[] q { get; private set; }
        public float[] Wpi { get; private set; }
        public float[] Wqi { get; private set; }

        public FlowsPower(Transformers transformers, byte[] ao)
        {
            p = new float[MainModel.max];//массив потоков активных мощностей на всех участках
            q = new float[MainModel.max];//массив потоков реактивных мощностей на всех участках
            Wpi = new float[MainModel.max];//массив потоков энергий активных мощностей на всех участках
            Wqi = new float[MainModel.max];//массив потоков энергий реактивных мощностей на всех участках

            this.transformers = transformers;

            Init(transformers);

            FlowDistribution(ao);
        }
        private void Init(Transformers t)
        {
            for (byte i = Lines.Count; i < Lines.Count+Transformers.Count; i++)
            {//для начала заполним массивы со значениями трансформаторов          
                p[i] = t.Pj[i];
                q[i] = t.Qj[i];
                Wpi[i] = t.Wpj[i];
                Wqi[i] = t.Wqj[i];
            }
        }
        //=======================Потокораспределение
        private void FlowDistribution(byte[] ao)
        {
            int b = Lines.Count + Transformers.Count + 1;
            int a = Lines.Count + Transformers.Count;

            for (int i = a; i >= 0; i--)
            {
                for (byte j = 0; j < a; j++)
                {//цикл "переберет" все значения             
                    if (ao[j] == b)
                    {//суммируем величины при выполнении условия                 
                        p[i] += p[j];
                        q[i] += q[j];
                        Wpi[i] += Wpi[j];
                        Wqi[i] += Wqi[j];
                    }
                }
                b--;//декрементируем, чтобы приближаться к головному участку
            }
        }
    }
}
