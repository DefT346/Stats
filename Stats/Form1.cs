using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stats
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            button1_Click(null, null);
        }

        public static string filePath = @"stats.csv";

        private void button1_Click(object sender, EventArgs e)
        {
            Stage1();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Stage2();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Stage3();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var from = int.Parse(textBox2.Text);
            var via = int.Parse(textBox3.Text);

            if (from >= 1 && via >= 0)
                Custom(from, via);
            else
            {
                Clear(Color.DarkRed);
            }
        }

        #region Stages

        void Stage1()
        {
            Clear(Color.White);
            var data = LoadData(filePath); Print("Генеральная совокупность: "); ShowList(data);
            Console.WriteLine(data.Count());
            var selection = GetSortedSelection(data); Print("Вариационный ряд: "); ShowList(selection);
            var genAverage = GetAverage(data); Print($"Среднее значение генеральной совокупности: {genAverage}");
            var genDispersion = GetDispersion(data, genAverage); Print($"Дисперсия генеральной совокупности: {genDispersion.ToString("F8")}");
            var genDeviation = GetDeviation(genDispersion); Print($"Стандартное отклонение генеральной совокупности: {genDeviation.ToString("F8")}");

            Graph(pictureBox1, GetFrequencies(selection, graphMode.polygon, out float size1), size1, graphMode.polygon);
            Graph(pictureBox2, GetFrequencies(selection, graphMode.gistogram, out float size2), size2, graphMode.gistogram);
            //TestCalculte1(selection);
        }

        void Stage2()
        {
            Clear(Color.White);
            var data = LoadData(filePath); Print("Генеральная совокупность: "); ShowList(data);
            var selection = GetConfiguratedSelection(data, 1, 2); Print("Выборка со 2-го элемента через 1: "); ShowList(selection);

            var varSelection = GetSortedSelection(selection); Print("Вариационный ряд: "); ShowList(varSelection);
            var varAverage = GetAverage(selection); Print($"Среднее значение выборочной совокупности: {varAverage.ToString("F8")}");
            var varDispersion = GetDispersion(selection, varAverage); Print($"Дисперсия выборочной совокупности: {varDispersion.ToString("F8")}");
            var varCorrectedDispersion = GetDispersionCorrected(selection, varDispersion); Print($"Исправленная дисперсия выборочной совокупности: {varCorrectedDispersion.ToString("F8")}");
            var varDeviation = GetDeviation(varDispersion); Print($"Стандартное отклонение выборочной совокупности: {varDeviation.ToString("F8")}");
            var varCorrectedDeviation = GetDeviation(varCorrectedDispersion); Print($"Исправленное стандартное отклонение выборочной совокупности: {varCorrectedDeviation.ToString("F8")}");

            Graph(pictureBox1, GetFrequencies(varSelection, graphMode.polygon, out float size1), size1, graphMode.polygon);
            Graph(pictureBox2, GetFrequencies(varSelection, graphMode.gistogram, out float size2), size2, graphMode.gistogram);
        }

        void Stage3()
        {
            Clear(Color.White);
            var data = LoadData(filePath); Print("Генеральная совокупность: "); ShowList(data);
            var selection = GetConfiguratedSelection(data, 2, 5); Print("Выборка с 3-го элемента через 4: "); ShowList(selection);
            var varSelection = GetSortedSelection(selection); Print("Вариационный ряд: "); ShowList(varSelection);
            var varAverage = GetAverage(selection); Print($"Среднее значение выборочной совокупности: {varAverage.ToString("F8")}");
            var varDispersion = GetDispersion(selection, varAverage); Print($"Дисперсия выборочной совокупности: {varDispersion.ToString("F8")}");
            var varCorrectedDispersion = GetDispersionCorrected(selection, varDispersion); Print($"Исправленная дисперсия выборочной совокупности: {varCorrectedDispersion.ToString("F8")}");
            var varDeviation = GetDeviation(varDispersion); Print($"Стандартное отклонение выборочной совокупности: {varDeviation.ToString("F8")}");
            var varCorrectedDeviation = GetDeviation(varCorrectedDispersion); Print($"Исправленное стандартное отклонение выборочной совокупности: {varCorrectedDeviation.ToString("F8")}");

            Graph(pictureBox1, GetFrequencies(varSelection, graphMode.polygon, out float size1), size1, graphMode.polygon);
            Graph(pictureBox2, GetFrequencies(varSelection, graphMode.gistogram, out float size2), size2, graphMode.gistogram);
        }

        void Custom(int from, int via)
        {
            Clear(Color.White);
            var data = LoadData(filePath); Print("Генеральная совокупность: "); ShowList(data);
            var selection = GetConfiguratedSelection(data, from - 1, via + 1); Print($"Выборка с {from}-го элемента через {via}: "); ShowList(selection);
            var varSelection = GetSortedSelection(selection); Print("Вариационный ряд: "); ShowList(varSelection);
            var varAverage = GetAverage(selection); Print($"Среднее значение выборочной совокупности: {varAverage.ToString("F8")}");
            var varDispersion = GetDispersion(selection, varAverage); Print($"Дисперсия выборочной совокупности: {varDispersion.ToString("F8")}");
            var varCorrectedDispersion = GetDispersionCorrected(selection, varDispersion); Print($"Исправленная дисперсия выборочной совокупности: {varCorrectedDispersion.ToString("F8")}");
            var varDeviation = GetDeviation(varDispersion); Print($"Стандартное отклонение выборочной совокупности: {varDeviation.ToString("F8")}");
            var varCorrectedDeviation = GetDeviation(varCorrectedDispersion); Print($"Исправленное стандартное отклонение выборочной совокупности: {varCorrectedDeviation.ToString("F8")}");

            Graph(pictureBox1, GetFrequencies(varSelection, graphMode.polygon, out float size1), size1, graphMode.polygon);
            Graph(pictureBox2, GetFrequencies(varSelection, graphMode.gistogram, out float size2), size2, graphMode.gistogram);

            Console.WriteLine(selection.Count());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var data = LoadData(filePath); Print("Генеральная совокупность: "); ShowList(data);
            var selection = GetSortedSelection(data); Print("Вариационный ряд: "); ShowList(selection);
            TestCalculte1(selection);
        }

        #endregion

        #region Math

        List<int> GetSortedSelection(List<int> list) => list.OrderBy(element => element).ToList<int>();
        
        List<int> GetConfiguratedSelection(List<int> list, int startIndex, int step)
        {
            var result = new List<int>();
            for(int i = startIndex; i < list.Count(); i += step)
            {
                result.Add(list[i]);
            }
            return result;
        }

        float GetAverage(List<int> list)
        {
            float sum = 0;
            foreach (var element in list) sum += element;
            return sum / (float)list.Count();
        }

        float GetDispersion(List<int> list, float average)
        {
            float sum = 0;
            foreach (var element in list) sum += (float)Math.Pow(element - average, 2);
            return sum / (float)list.Count();
        }

        float GetDispersionCorrected(List<int> list, float dispersionV)
        {
            return ((float)list.Count() / ((float)list.Count() - 1)) * dispersionV;
        }

        float GetDeviation(float dispersion) => (float)Math.Sqrt(dispersion);

        void TestCalculte1(List<int> list)
        {
            var max = list.Max();
            var min = list.Min();
            var step = (max - min) / 25f;
            MultiLog($"min:{min}");
            MultiLog($"max:{max}");
            MultiLog($"count:{list.Count}");


            Console.WriteLine("Middle:");
            List<float> middles = new List<float>();
            List<float> borders = new List<float>();
            borders.Add(min);


            for (float i = min; i + step <= max; i += step)
            {
                borders.Add(i + step);
                var m = i + step / 2f;
                //Console.WriteLine($"i={i}; next={i+step}; m={m}");
                //Console.WriteLine($"{Math.Round(i,2)} - {Math.Round(i +step,2)}");
                Console.WriteLine($"{Math.Round(i,2)}");
                //Console.WriteLine($"{Math.Round(m,2)}");


                middles.Add(m);
            }
            Console.WriteLine("n:");
            InsertColown(" ", new List<int> { 1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25});

            InsertColown("x~i", borders);
            List<int> ni_list = new List<int>();

            
            for (float i = min; i <= max; i += step)
            {
                int n = 0;
                for (int j = 0; j < list.Count; j++)
                {
                    if (list[j] > i && list[j] < i + step)
                        n++;

                }
                Console.WriteLine(n);
                ni_list.Add(n);
            }
            InsertColown("ni", ni_list);

            List<float> wi_list = new List<float>();
            Console.WriteLine("wi:");
            for (int i = 0; i < 25; i++)
            {
                var wi = ni_list[i] / (float)list.Count();
                wi_list.Add(wi);
                Console.WriteLine(Math.Round(wi,4));
            }
            InsertColown("wi", wi_list);

            float xvib = 0;
            for (int i = 0; i < 25; i++)
            {
                xvib += middles[i] * wi_list[i];
                Console.WriteLine($"{borders[i]} - {borders[i+1]}; m:{middles[i]}; wi:{wi_list[i]}|");
                //Console.WriteLine($"{Math.Round(borders[i],2)}-{Math.Round(borders[i+1],2)}");
                //Console.WriteLine($"{middles[i]}");

            }
            MultiLog($"Xвыб: {xvib}");

            double Dvib = 0;
            for (int i = 0; i < 25; i++)
            {
                Dvib += Math.Pow(middles[i] - xvib, 2) * wi_list[i];
                //Console.WriteLine($"m:{middles[i]}; wi:{wi_list[i]}");
            }
            MultiLog($"Dвыб: {Dvib}");
            var sigmavib = Math.Sqrt(Dvib);
            MultiLog($"бвыб: { sigmavib }");
            var Disp = 25f/26f * Dvib;
            MultiLog($"Dиспр: { Disp }");
            var sigmaisp = Math.Sqrt(Disp);
            MultiLog($"биспр: { sigmaisp }");


            //Console.WriteLine($"ksi_list");
            List<double> ksi_list = new List<double>();
            for(int i =0; i< 26; i++)
            {
                var ksi = (borders[i] - xvib) / sigmavib;
                //Console.WriteLine($"{ksi}");
                ksi_list.Add(ksi);
            }
            InsertColown("ksi", ksi_list);

            //// https://kvm.gubkin.ru/pub/fan/laplasetable2.pdf
            //List<double> Fs = new List<double>
            //{
            //    -0.4861,
            //    -0.4772,
            //    -0.4649,
            //    -0.4463,
            //    -0.4207,
            //    -0.3869,
            //    -0.3461,
            //    -0.2939,
            //    -0.2324,
            //    -0.1628,
            //    -0.0910,
            //    -0.0120,
            //    0.0675,
            //    0.1406,
            //    0.2123,
            //    0.2764,
            //    0.3315,
            //    0.3749,
            //    0.4115,
            //    0.4394,
            //    0.4599,
            //    0.4738,
            //    0.4838,
            //    0.4904,
            //    0.4945,
            //    0.4969
            //};

            List<double> Fs = new List<double>();
            for (int i = 0; i < 26; i++)
            {
                Fs.Add(Laplace.F(ksi_list[i]));
            }

            InsertColown("Ф", Fs);
            //Console.WriteLine($"Ф");
            //for (int i = 0; i < 25; i++)
            //{
            //    double f(double s)
            //    {
            //        return -Math.Log(s) / s;
            //    }

            //    double LinvCalc = Laplace.InverseTransform(f, ksi_list[i]);
            //    Console.WriteLine(LinvCalc);
            //}

            //Console.WriteLine($"pi_list");
            List<double> pi_list = new List<double>();
            for (int i = 0; i < 25; i++)
            {
                var pi = Fs[i+1] - Fs[i];
                //Console.WriteLine($"{pi}");
                pi_list.Add(pi);
            }

            InsertColown("Pi", pi_list);

            //Console.WriteLine($"ni'_list");
            List<double> nih_list = new List<double>();
            for (int i = 0; i < 25; i++)
            {
                var nih = pi_list[i]* list.Count();
                //Console.WriteLine($"{nih}");
                nih_list.Add(nih);
            }

            InsertColown("n'i", nih_list);

            //Console.WriteLine("x^2");
            List<double> x2_list = new List<double>();
            for (int i = 0; i < 25; i++)
            {
                var x2 = Math.Pow(ni_list[i] - nih_list[i],2)/ nih_list[i];
                //Console.WriteLine($"{x2}");
                x2_list.Add(x2);
            }

            InsertColown("X^2", x2_list);
            var x2Sum = x2_list.Sum();
            MultiLog($"X^2:{x2Sum}");
            MultiLog($"------------");
            var r = 2;
            var a = 0.01;
            //https://www.matematicus.ru/wp-content/uploads/2019/02/kriticheskie-tochki-raspredeleniya.png
            MultiLog($"a = {a}");
            MultiLog($"r = {r}");
            MultiLog($"k = s - r - 1 = {25 - r - 1}");
            var kk = 40.3f;
            MultiLog($"k_кр = {kk}");
            if (kk < x2Sum) MultiLog($"k_кр({kk}) < X^2({x2Sum})");
            else
            {
                MultiLog($"k_кр({kk}) > X^2({x2Sum})");
            }


        }

        public void SetTextInTable(int x, int y, string text)
        {
            Label label = new Label();
            label.Text = text;
            tableLayoutPanel1.Controls.Add(label, x, y);
        }


        int currientCol = 0;
        public void InsertColown<T>(string colName, List<T> list)
        {
            SetTextInTable(currientCol, 0, colName);
            Console.WriteLine(colName);
            for (int i = 0; i< list.Count; i++)
            {
                SetTextInTable(currientCol, i + 1, list[i].ToString());
                Console.WriteLine(Math.Round(double.Parse(list[i].ToString()), 7));

            }
            currientCol++;
        }
        public void MultiLog(string text)
        {
            multiLog1.Text += text + ";" + Environment.NewLine;
            Console.WriteLine(text);
        }

        List<(float point, float freq)> GetFrequencies(List<int> selection, graphMode mode, out float size)
        {
            var result = new List<(float point, float freq)>();
            var freqs = new Dictionary<int, float>();

            foreach (var el in selection)
            {
                if (freqs.ContainsKey(el))
                    freqs[el]++;
                else
                    freqs.Add(el, 1);
            }

            if (mode == graphMode.polygon)
            {
                foreach (var el in freqs)
                {
                    result.Add((el.Key, (float)el.Value /*/ (float)selection.Count()*/));
                }
                size = 0;
                return result;
            }
            else if (mode == graphMode.gistogram)
            {
                var maxElement = freqs.Max(p => p.Key);
                float step = maxElement / 25f;
                size = step;

                for (float border = 0; border < maxElement; border += step)
                {
                    float sum = 0;
                    foreach (var element in freqs)
                    {
                        if (border > element.Key && element.Key >= border - step)
                        {
                            sum += element.Value;
                        }
                    }
                    result.Add((border + step / 2f, sum / (step)));
                }
                return result;
            }
            else
            {
                size = 0;
                return null;
            }

        }

        #endregion

        #region InputOutputMethods

        enum graphMode {gistogram, polygon};
        void Graph(PictureBox field, List<(float point, float freq)> data, float size = 0, graphMode mode = graphMode.polygon)
        {
            Bitmap image = new Bitmap(field.Width, field.Height);
            var maxX = data.Max(p => p.point);
            var maxY = data.Max(p => p.freq);

            float scaleX = (image.Width) / maxX/1.2f;
            float scaleY = (image.Height) / maxY/1.2f;
            float stepX = maxX / 7f;
            float stepY = maxY / 7f;

            if (mode == graphMode.polygon)
            {
                stepX = maxX / 2;
                stepY = maxY / 2;
            }
            
            Graphics g = Graphics.FromImage(image);

            g.Clear(Color.White);
            Pen blackPen = new Pen(Color.Black, 1);

            var axisLenght = 30;

            g.DrawLine(blackPen, tr(new Point(0, -10)), tr(new Point((int)(axisLenght * scaleX * stepX), -10)));
            g.DrawLine(blackPen, tr(new Point(0, 0)), tr(new Point(0, (int)(axisLenght * scaleY * stepY))));

            var font = new Font("Khmer UI", 6);

            for (float x = 0; x <= axisLenght * scaleX * stepX; x += scaleX * stepX)
            {
                g.DrawLine(blackPen, tr(new Point((int)x, -3 - 10)), tr(new Point((int)x, -10)));
                var textPointX = tr(new Point((int)x -3, -16)); g.DrawString((x/scaleX).ToString("0.000"), font, Brushes.Black, textPointX.X, textPointX.Y);

            }

            for (float y = 0; y <= axisLenght * scaleY * stepY; y += scaleY * stepY)
            {
                g.DrawLine(blackPen, tr(new Point(-3, (int)y)), tr(new Point(0, (int)y)));
                var textPointY = tr(new Point(-13 - (y == 0 ? 9 : y.ToString().Length) * 2, (int)y + 3)); g.DrawString((y / scaleY).ToString("0.000"), font, Brushes.Black, textPointY.X, textPointY.Y);
            }

            (float x, float y) temp = (-1,-1);
            foreach (var el in data)
            {
                var x = el.point;
                var y = el.freq;

                if (mode == graphMode.polygon)
                {
                    if (temp.x == -1)
                    {
                        temp = (x, y);
                        continue;
                    }
                    g.DrawLine(blackPen, tr2(temp), tr2((x, y)));
                    temp = (x, y);
                }
                else if (mode == graphMode.gistogram)
                {
                    var p1 = tr2((x - size / 2f, y));
                    var p2 = new Point((int)((size * scaleX)), (int)(y * scaleY));
                    g.FillRectangle(new SolidBrush(Color.Gray), new Rectangle(p1.X, p1.Y, p2.X, p2.Y));
                    g.DrawRectangle(blackPen, new Rectangle(p1.X, p1.Y, p2.X, p2.Y));
                }
            }

            field.Image = image;

            Point tr(Point point)
            {
                var d = 30;
                var offset = new Point(d, field.Height - d);
                return new Point(point.X + offset.X + 5, -point.Y + offset.Y);
            }

            Point tr2((float x, float y) point)
            {
                return tr(new Point((int)(point.x * scaleX), (int)(point.y * scaleY)));
            }

        }

        List<int> LoadData(string path)
        {
            using (var reader = new StreamReader(path))
            {
                List<int> values = new List<int>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var elements = line.Split(' ');

                    var value = int.Parse(elements[0]);

                    if (value != 1000000) values.Add(value/10000);
                    //foreach (var x in elements)
                    //    values.Add(int.Parse(x.Replace($"\"", "")));
                }
                return values;
            }
        }

        void Clear(Color color)
        {
            textBox1.Text = "";

            var image1 = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics.FromImage(image1).Clear(color);
            pictureBox1.Image = image1;

            var image2 = new Bitmap(pictureBox2.Width, pictureBox2.Height);
            Graphics.FromImage(image2).Clear(color);
            pictureBox2.Image = image2;
        }

        void Print(object message)
        {

            textBox1.Text += $"{(textBox1.Text == "" ? "": "\r\n\r\n")}{message}";
        }

        void ShowList<T>(List<T> list)
        {
            string result = "\r\n[";
            foreach (var element in list)
            {
                result += $"{element.ToString()}, ";
            }
            textBox1.Text += (result.Length > 1 ? result.Remove(result.Length - 2) : result) + "]";
        }

        #endregion

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }


    }
}
