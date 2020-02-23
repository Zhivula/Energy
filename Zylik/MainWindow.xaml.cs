using System;
using System.IO;
using System.Linq;
using System.Windows;
using Zylik.ViewModel;

namespace Zylik
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //<summary>
        //ЗАПУСКАЕТСЯ ПРИ ЗАПУСКЕ ПРИЛОЖЕНИЯ -> ВНОСИТ НАЧАЛЬНЫЕ ДАННЫЕ, СОХРАНЕННЫЕ В ПАМЯТИ ПОД ПРИЛОЖЕНИЕ
        //</summary>
        public void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainWindow window = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            window.mainGrid.Children.Add(new View.TableOneView());
        }
        public MainWindow()
        {
            byte max = 100; //переменная для быстрого измения размерности основных массивов, используемых при расчетах
            var size2 = File.ReadAllLines(@"trans.txt").Where(x => x != "").Count();//количество считываемых строк с файла данных о трансформаторах
            var size1 = File.ReadAllLines(@"linii.txt").Where(x => x != "").Count();//количество считываемых строк с файла данных о линиях
            string[] lines = File.ReadAllLines(@"linii.txt").Take(size1).ToArray();//в массив lines передаем все строки с файла linii.txt
            string[] trans = File.ReadAllLines(@"trans.txt").Take(size2).ToArray();//в массив trans передаем все строки с файла trans.txt
            float[,] array1 = new float[size1, 6];
            float[,] array2 = new float[size2, 9];
            byte kolvo_lines = 0;//будет хранить количество участков линий
            for (byte i = 4; i < size1; i++)
            {//цикл начинается с 4, так как в первых 4-ёх строках содержится описание данных о линиях         
                kolvo_lines++;//счетчик, отражающий количество участков линий
                float[] row1 = lines[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Single.Parse).ToArray();//заполняем массив row1 строками из массива lines, разделяя строки на отдельные значения, пропуская знак пробела, с дальнейшим преобразованием из типа string в тип float
                for (byte j = 0; j < 6; j++)
                {//каждая строка с характеристиками линии имеет 5 величин                
                    array1[i, j] = row1[j];
                }
            }
            byte kolvo_trans = 0;//будет хранить количество участков с трансформаторами
            for (byte i = 2; i < size2; i++)
            {//цикл начинается с 2, так как в первых 2-ух строках содержится описание данных о трансформаторах           
                kolvo_trans++;//счетчик, отражающий количество участков с трансформаторами
                float[] row2 = trans[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(Single.Parse).ToArray();//заполняем массив row2 строками из массива trans, разделяя строки на отдельные значения, пропуская знак пробела, с дальнейшим преобразованием из типа string в тип float
                for (byte j = 0; j < 9; j++)
                {//каждая строка с характеристиками трансформаторами имеет 15 величин               
                    array2[i, j] = row2[j];//заполнение двумерного массива элементами row2
                }
            }
            short a = (short)(kolvo_trans + kolvo_lines);//общее количество участков
            float[] dl = new float[max];//длина участка
            float[] R0 = new float[max]; //удельное активное сопротивление участка, Ом/ км
            float[] X0 = new float[max];//удельное реактивное сопротивление, Ом/км
            float[] k = new float[max];//массив с номерами концов участков
            float[] n = new float[max];//массив с номерами начал участков
            string[] marka = new string[max];
            for (byte i = 0; i < kolvo_lines; i++)
            {//индексация в массивах будет начинаться с 0 и до значения количества участков линий            
                for (byte j = 0; j < 6; j++)
                {//в каждый массив данных о линиях по очереди будет попадать величина находящаяся в двумерном массиве array1               
                    switch (j)
                    {
                        case 0:
                            n[i] = array1[i + 4, 0];//заполнение массива начал учатков линий только если это первая величина по второму индексу в массиве array1
                            break;//если число попало в массив n, то выходим из цикла switch                       
                        case 1:
                            k[i] = array1[i + 4, 1];//заполнение массива концов учатков линий только если это вторая величина по второму индексу в массиве array1
                            break;//если число попало в массив k, то выходим из цикла switch                      
                        case 2:
                            R0[i] = array1[i + 4, 2];//заполнение массива удельных активных сопротивлений учатков линий только если это третья величина по второму индексу в массиве array1
                            break;//если число попало в массив R0, то выходим из цикла switch                   
                        case 3:
                            X0[i] = array1[i + 4, 3];//заполнение массива удельных реактивных сопротивлений учатков линий только если это четвертая величина по второму индексу в массиве array1
                            break;//если число попало в массив X0, то выходим из цикла switch                     
                        case 4:
                            dl[i] = array1[i + 4, 4];//заполнение массива длин учатков линий только если это пятая величина по второму индексу в массиве array1
                            break;//если число попало в массив dl, то выходим из цикла switch                       
                        case 5: marka[i] = array1[i + 4, 5].ToString(); break;
                    }
                }
            }
            float kz = 1;//коэффициент загрузки трансформатора
            float[] Snomj = new float[max];//номинальная мощность трансформатора j, кВА
            float Unom = 10.5f;//номинальное напряжение, кВ 
            float[] Tmaj = new float[max]; //Число часов использования максимальной активной нагрузки трансформатора
            float[] Ixxj = new float[max];//ток холостого хода трансформатора j, % 
            float[] Pkzj = new float[max];//потери мощности короткого замыкания трансформатора j, кВт 
            float[] Ukz = new float[max];//напряжение короткого замыкания трансформатора j, % 
            float tgf = 0.75f;
            float cosf = 0.8f;//коэффициент мощности трансформатора;
            float[] dPxxj = new float[max];//потери холостого хода трансформатора j, кВт 
            float[] Qxxj = new float[max];//потери холостого хода трансформатора j, кВт 
            for (byte i = 0; i < kolvo_trans; i++)
            {
                for (byte j = 0; j < 9; j++)
                {//в цикле каждый массив относящий к определенному трансформатору получит значение                
                    switch (j)
                    {
                        case 0:
                            n[i + kolvo_lines] = array2[i + 2, 0];//заполнение массива начал учатков линий только если это 1-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив n, то выходим из цикла switch                       
                        case 1:
                            k[i + kolvo_lines] = array2[i + 2, 1];//заполнение массива начал учатков линий только если это 2-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив k, то выходим из цикла switch                       
                        case 2:
                            Snomj[i + kolvo_lines] = array2[i + 2, 2];//заполнение массива начал учатков линий только если это 3-я величина по второму индексу в массиве array2
                            break;//если число попало в массив Snomj, то выходим из цикла switch                                            
                        case 3:
                            Tmaj[i + kolvo_lines] = array2[i + 2, 3];//заполнение массива начал учатков линий только если это 5-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Tmaj, то выходим из цикла switch                       
                        case 4:
                            dPxxj[i + kolvo_lines] = array2[i + 2, 4];//заполнение массива начал учатков линий только если это 7-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Pxxj, то выходим из цикла switch                      
                        case 5:
                            Qxxj[i + kolvo_lines] = array2[i + 2, 5];//заполнение массива начал учатков линий только если это 8-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Qxxj, то выходим из цикла switch                                                                      
                        case 6:
                            Ukz[i + kolvo_lines] = array2[i + 2, 6];//заполнение массива начал учатков линий только если это 11-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Ukz, то выходим из цикла switch                        
                        case 7:
                            Pkzj[i + kolvo_lines] = array2[i + 2, 7];//заполнение массива начал учатков линий только если это 12-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Pkzj, то выходим из цикла switch                        
                        case 8:
                            Ixxj[i + kolvo_lines] = array2[i + 2, 8];//заполнение массива начал учатков линий только если это 13-ая величина по второму индексу в массиве array2
                            break;//если число попало в массив Unomj, то выходим из цикла switch                       
                    }
                }
            }
            float[] Sj = new float[max];//полная нагрузка трансформатора
            float[] Pj = new float[max];//поток активной мощности
            float[] Qj = new float[max];//поток реактивной мощности
            float[] rj = new float[max];//активное сопротивление трансформатора j, Ом:                    
            float[] dPj = new float[max];//потери активной мощности на участках с трансформаторами
            float[] zj = new float[max];//модуль полного сопротивления трансформатора j, Ом:         
            float[] xj = new float[max];//реактивное сопротивление трансформатора j, Ом;
            float[] dQj = new float[max];//потери реактивной мощности на участках с трансформаторами
            float[] kzj = new float[max];//коэффициент заполнения графика
            float[] kf2j = new float[max];//Коэффициент формы графика нагрузки
            float[] Ij = new float[max];//Ток в трансформаторе
            float[] dQxxj = new float[max];//потери реактивной мощности холостого хода трансформатора 
            float[] dWxxj = new float[max];//потери энергии холостого ъода трансформатора
            float[] dU = new float[max];//потери напряжения для каждого участка схемы
            float[] Wpj = new float[max];//энергия потока активной мощности
            float[] Wqj = new float[max];//энергия потока реактивной мощности
            float[] dWj = new float[max];//Нагрузочные потери электроэнергии на трансформаторных участках
            for (byte i = kolvo_lines; i < a; i++)
            {//в цикле ведется расчет значений для трансформаторов            
                Sj[i] = kz * Snomj[i];
                Pj[i] = Sj[i] * cosf;
                Qj[i] = Sj[i] * (float)Math.Sqrt(1 - cosf * cosf);
                rj[i] = (Pkzj[i] * Unom * Unom) / (Snomj[i] * Snomj[i]);
                dPj[i] = (Pj[i] * Pj[i] + Qj[i] * Qj[i]) * rj[i] / (Unom * Unom);
                zj[i] = (Ukz[i] * Unom * Unom) / (100 * Snomj[i]);
                xj[i] = (float)Math.Sqrt(zj[i] * zj[i] - rj[i] * rj[i]);
                dQj[i] = (Pj[i] * Pj[i] + Qj[i] * Qj[i]) * xj[i] / (Unom * Unom);
                kzj[i] = Tmaj[i] / 8760;
                kf2j[i] = (float)(0.16 / kzj[i] + 0.82) * (float)(0.16 / kzj[i] + 0.82);
                Ij[i] = Pj[i] / (float)(Math.Sqrt(3) * 0.38 * cosf);
                dQxxj[i] = Ixxj[i] * Snomj[i] / 100;
                dWxxj[i] = dPxxj[i] * 8760;
                dU[i] = (Qj[i] * xj[i] + rj[i] * Pj[i]) / (Unom * 1000);
                Wpj[i] = Pj[i] * Tmaj[i];
                Wqj[i] = Wpj[i] * tgf;
                dWj[i] = (Wpj[i] * Wpj[i] * (1 + tgf * tgf) * rj[i] * kf2j[i]) / (8760 * Unom * Unom);
            }
            float[] p = new float[max];//массив потоков активных мощностей на всех участках
            float[] q = new float[max];//массив потоков реактивных мощностей на всех участках
            float[] Wpi = new float[max];//массив потоков энергий активных мощностей на всех участках
            float[] Wqi = new float[max];//массив потоков энергий реактивных мощностей на всех участках
            for (byte i = kolvo_lines; i < a; i++)
            {//для начала заполним массивы со значениями трансформаторов          
                p[i] = Pj[i];
                q[i] = Qj[i];
                Wpi[i] = Wpj[i];
                Wqi[i] = Wqj[i];
            }
            byte b = 1;//начальное значение 1, так как 0 значение принадлежит головному участку
            byte[] ao = new byte[max];//массив адресных отображений
            for (byte i = 0; i < a; i++)
            {
                for (byte j = 0; j < a; j++)
                {
                    if (k[i] == n[j])
                    {//сравнение конца одного участка с началами всех участков                   
                        ao[j] = b;//если нашли совпадение, то в массив ao попадает значение b
                    }
                }
                b++;//инкрементируем после завершения сравнения
            }
            //=======================Потокораспределение
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
            // расчёт данных линий
            float[] rl = new float[max];//активное сопротивление i-о участка линии, Ом.
            float[] xl = new float[max];//реактивное сопротивление i-о линейного участка схемы, Ом;
            float[] Tmai = new float[max];//Число часов использования максимальной активной нагрузки на участках линий
            float[] kzi = new float[max];//коэффициент заполнения графика, равный относительному числу часов использования максимальной активной нагрузки Тма i:
            float[] kf2i = new float[max];//коэффициент формы графика нагрузки, о.е.;
            float[] tgfl = new float[max];//коэффициент реактивной мощности, о.е.;          
            float[] dWi = new float[max]; //поток активной энергии на i - м линейном участке схемы, кВт*ч;
            float[] dQli = new float[max];//потери реактивной мощности на участках линий
            float[] dPli = new float[max];//потери активной мощности на участках линий
            for (byte i = 0; i < kolvo_lines; i++)
            {//расчет величин только для участков линий          
                rl[i] = R0[i] * dl[i];
                xl[i] = X0[i] * dl[i];
                Tmai[i] = Wpi[i] / p[i];
                kzi[i] = Tmai[i] / 8760;
                kf2i[i] = (float)(0.16 / kzi[i] + 0.82) * (float)(0.16 / kzi[i] + 0.82);
                tgfl[i] = Wqi[i] / Wpi[i];
                dWi[i] = (Wpi[i] * Wpi[i] * (1 + tgfl[i] * tgfl[i]) * kf2i[i] * rl[i]) / (Unom * Unom * 8760 * 1000);
                dU[i] = (rl[i] * p[i] + xl[i] * q[i]) / (Unom * 1000);
                dQli[i] = (p[i] * p[i] + q[i] * q[i]) * xl[i] / (Unom * Unom * 1000);
                dPli[i] = (p[i] * p[i] + q[i] * q[i]) * rl[i] / (Unom * Unom * 1000);
            }
            //======================= Расчёт напряжений в узлах сети
            float[] U = new float[max];//инициализация массива напряжений на каждом участке
            U[0] = 10.5f - dU[0];//чтобы не нарушать границы массива, рассчитываем напряжение на первом участке вне цикла
            for (byte i = 1; i < kolvo_lines; i++)
            {
                U[i] = U[i - 1] - dU[i];//расчет наприжений на участках линий
            }
            for (byte i = 0; i < kolvo_lines; i++)
            {
                for (byte j = kolvo_lines; j < a; j++)
                {
                    if (k[i] == n[j])
                    {  //если конце участка линии совпадает с началом участка с трансформатором                
                        U[j] = U[i] - dU[j];//расчет напряжений на участках с трансформаторами
                    }
                }
            }
            for (byte i = kolvo_lines; i < a; i++)
            {//рассматриваем участки с трансформаторами           
                U[i] = U[i] / 25;//приводим к стороне низшего напряжения 
            }
            // расчет активных и реактивных потерь мощностей и электроэнергии на трансформатарах
            float dPt = 0;//Суммарные потери активной мощности в трансформаторах
            float dQt = 0;//Суммарные потери реактивной мощности в трансформаторах
            float dPxx = 0;//суммарные потери холостого хода трансформатора
            float dQxx = 0;//суммарные потери реактивной мощности в стали трансформаторов
            float dWt = 0;//Суммарные нагрузочные потери электроэнергии в трансформаторах
            float dWxx = 0;//Суммарные нагрузочные потери электроэнергии в стали трансформаторов
            for (byte i = kolvo_lines; i < a; i++)
            {
                dPt += dPj[i];
                dQt += dQj[i];
                dPxx += dPxxj[i];
                dQxx += dQxxj[i];
                dWt += dWj[i];
                dWxx += dWxxj[i];
            }
            //================ расчет активных и реактивных потерь мощностей и электроэнергии на линиях
            float dQl = 0;//суммарные потери реактивной мощности на линейных участках
            float dPl = 0; //суммарные потери активной мощности на линейных участках
            float dWl = 0;//суммарные нагрузочные потери электроэнергии на линейных участках схемы
            for (byte i = 0; i < kolvo_lines; i++)
            {
                dQl += dQli[i];
                dPl += dPli[i];
                dWl += dWi[i];
            }
            //Вычисление суммарных потерь активной, реактивной мощности и электроэнергии                     
            float olPj = 0;
            float olQj = 0;
            float olWpj = 0;
            float dW = dWl + dWt + dWxx;
            float dQ = dQl + dQt + dQxx;
            float dP = dPl + dPt + dPxx;
            float dQlt = dQt + dQl;
            float dPlt = dPl + dPt;
            for (byte i = kolvo_lines; i < a; i++)
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
            HelpClass.kolvo_lines = kolvo_lines;
            HelpClass.kolvo_trans = kolvo_trans;
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
            InitializeComponent();//инициализация компонентов    
            DataContext = new MainWindowViewModel();
        }
        //<summary>
        //ЗАПУСКАЕТСЯ ПРИ ЗАПУСКЕ ПРИЛОЖЕНИЯ -> ВЫХОДЕ ИЗ ПРИЛОЖЕНИЯ
        //</summary>
        private void WindowClosing(object sender, RoutedEventArgs e)
        {
            var response = MessageBox.Show("Вы действительно хотите выйти?", "ВЫХОД", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.Yes) Application.Current.Shutdown();
        }
    }
}
