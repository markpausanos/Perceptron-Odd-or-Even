using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PerceptronNumbers
{
    public partial class odd_even_label : Form
    {
        PictureBox[] inputs;
        List<int[]> numbers;
        List<double> weights;
        Dictionary<int, int> numbersDict;
        int[] numberKeys;
        Label[] weightsLabel;
        static Random random;

        double weightBound = 1;
        const double learningRate = 1;
        double bias = 0;

        public odd_even_label()
        {
            InitializeComponent();
            inputs = new PictureBox[] 
            { 
                p1, p2, p3, p4, p5, 
                p6,p7, p8, p9, p10, 
                p11, p12, p13, p14, p15,
                p16, p17, p18, p19, p20,
                p21, p22, p23, p24, p25,
                p26, p27, p28, p29, p30,
                p31, p32, p33, p34, p35
            }
            ;
            numbers = new List<int[]>()
            {
                Numbers.Get0(),
                Numbers.Get1(),
                Numbers.Get2(),
                Numbers.Get3(),
                Numbers.Get4(),
                Numbers.Get5(),
                Numbers.Get6(),
                Numbers.Get7(),
                Numbers.Get8(),
                Numbers.Get9(),
            };
            numbersDict = new Dictionary<int, int>
            {
                { 0, -1 },
                { 1, 1 },
                { 2, -1 },
                { 3, 1 },
                { 4, -1 },
                { 5, 1 },
                { 6, -1 },
                { 7, 1 },
                { 8, -1 },
                { 9, 1 }
            };
            numberKeys = new int[] { -1, 1, -1, 1, -1, 1, -1, 1, -1, 1};
            weightsLabel = new Label[]
            {
                w1, w2, w3, w4, w5, 
                w6, w7, w8, w9, w10, 
                w11, w12, w13, w14, w15,
                w16, w17, w18, w19, w20,
                w21, w22, w23, w24, w25, 
                w26, w27, w28, w29, w30,
                w31, w32, w33, w34, w35
            };
        }
      
        private int HardLimiting(double x)
        {
            return x > 0 ? 1 : -1;
        }
        private void ResetPictureBoxes()
        {
            foreach (PictureBox pictureBox in inputs)
            {
                pictureBox.BackColor = Color.White;
            }
        }
     
        private void colorPictureBoxes(int[] values)
        {
            ResetPictureBoxes();
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == 1)
                {
                    inputs[i].BackColor = Color.Red;
                }
            }
        }
        private void num_Click(object sender, EventArgs e)
        {
            int number = int.Parse(((Button)sender).Text);
            colorPictureBoxes(numbers[number]);
        }

        private double[] GiveRandomWeights()
        {
            double[] weightsInitial = new double[weightsLabel.Length];
            int count = 0;
            
            while (count < weightsLabel.Length)
            {
                random = new Random(count + DateTime.Now.Millisecond);
                var weight = random.NextDouble();
                weight = Math.Round(0 + (weight * (weightBound - (0))), 4);
                if (weight != 0)
                {
                    weightsInitial[count++] = weight;
                }
            }
            weights = weightsInitial.ToList();
            return weightsInitial;
        }
        private void randomize_weights_click(object sender, EventArgs e)
        {
            var init = GiveRandomWeights();

            for (int i = 0; i < init.Length; i++)
            {
                weightsLabel[i].Text = init[i].ToString("0.0000");
            }
            learn.Enabled = true;
        }
        private bool CheckCorrectAlready(int[] hardLimitedOutputs)
        {
            return Enumerable.SequenceEqual(hardLimitedOutputs, numberKeys);
        }
        private void UpdateWeights(int[] numberDrawing, int currentNumber)
        {
            for (int i = 0; i < weightsLabel.Length; i++)
            {
                weights[i] = weights[i] + (learningRate * numbersDict[currentNumber] * numberDrawing[i]);
                bias = bias + (learningRate * numbersDict[currentNumber]);
                weightsLabel[i].Text = weights[i].ToString("0.0000");
            }
        }
        private void train_Click(object sender, EventArgs e)
        {
            int[] hardLimitedOutputs = new int[35];
            int epochs = textBox1.Text.Length == 0 ? 100 : int.Parse(textBox1.Text);
            textBox1.Text = epochs.ToString();
            int count = 0;
            while (!CheckCorrectAlready(hardLimitedOutputs) && count <= epochs)
            {
                for (int i = 0; i < numbers.Count; i++)
                {
                    double output = Enumerable.Range(0, numbers[i].Length).Sum(j => numbers[i][j] * weights[j]) + bias;
                    hardLimitedOutputs[i] = HardLimiting(output);
                    UpdateWeights(numbers[i], i);
                }
                count++;
                Console.WriteLine(count); 
            }
            determine_button.Enabled = true;
        }
        private void determine_button_Click(object sender, EventArgs e)
        {
            /*
            Console.WriteLine();
            for (int i = 0; i < inputs.Length; i++)
            {
                if (i != 0 && i % 5 == 0)
                {
                    Console.WriteLine();
                }
                if (inputs[i].BackColor == Color.Red)
                {
                    Console.Write(1 + ", ");
                }
                else
                {
                    Console.Write(-1 + ", ");
                }
               
            }
            Console.WriteLine();
            ResetPictureBoxes();
            */
            int[] inputsX = new int[inputs.Length];

            for (int i = 0; i < inputs.Length; i++)
            {
                if (inputs[i].BackColor == Color.Red)
                {
                    inputsX[i] = 1;
                }
                else
                {
                    inputsX[i] = -1;
                }
            }

            int output = HardLimiting(Enumerable.Range(0, inputsX.Length).Sum(j => inputsX[j] * weights[j]) + bias);

            label1.Text = output == 1 ? "Odd" : "Even";
        }
        // Helpers
        private void InvertColor(PictureBox pictureBox)
        {
            if (pictureBox.BackColor == Color.White)
            {
                pictureBox.BackColor = Color.Red;
            }
            else
            {
                pictureBox.BackColor = Color.White;
            }
        }
        private void pictureBox_Click(object sender, EventArgs e)
        {
            InvertColor(sender as PictureBox);
        }
    }
}
