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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private int longitud = 25;
        private int longitudPixel = 20;
        int contsimulacion = 0;
        int[,] celulas;
        public Form1()
        {
            InitializeComponent();

            celulas = new int[longitud, longitud];
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            PintarMatriz();
        }

        private void PintarMatriz()
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            for (int x = 0; x < longitud; x++)
            {
                for (int y = 0; y < longitud; y++)
                {
                    if (celulas[x, y] == 0)
                        PintarPixel(bmp, x, y, Color.White);
                    else
                        PintarPixel(bmp, x, y, Color.Black);
                }
            }

            pictureBox1.Image = bmp;
        }

        private void PintarPixel(Bitmap bmp, int x, int y, Color color)
        {
            for (int i = 0; i < longitudPixel; i++)
                for (int j = 0; j < longitudPixel; j++)
                    bmp.SetPixel( j + (y * longitudPixel), i + (x * longitudPixel), color);
        }

        private void JuegoDeLaVida()
        {
            int[,] celulasTemp = new int[longitud, longitud];
            for (int x = 0; x < longitud; x++)
            {
                for (int y = 0; y < longitud; y++)
                {
                    if (celulas[x, y] == 0)
                        celulasTemp[x, y] = ReglaJuegoVida(x, y, false);
                    else
                        celulasTemp[x, y] = ReglaJuegoVida(x, y, true);
                }
            }

            celulas = celulasTemp;

        }

        private int ReglaJuegoVida(int x, int y, bool EsViva)
        {
            int VecinasVivas = 0;

         //vecina 1
            if (x > 0 && y > 0)
                if (celulas[x - 1, y - 1] == 1)
                    VecinasVivas++;

         //vecina 2
            if (y > 0)
                if (celulas[x, y - 1] == 1)
                    VecinasVivas++;

         //vecina 3   
            if (x < longitud - 1 && y > 0)
                if (celulas[x + 1, y - 1] == 1)
                    VecinasVivas++;

         //vecina 4
            if (x > 0)
                if (celulas[x - 1, y] == 1)
                    VecinasVivas++;

         //vecina 5
            if (x < longitud - 1)
                if (celulas[x + 1, y] == 1)
                    VecinasVivas++;

         //vecina 6
            if (x > 0 && y < longitud - 1)
                if (celulas[x - 1, y + 1] == 1)
                    VecinasVivas++;

         //vecina 7
            if (y < longitud - 1)
                if (celulas[x, y + 1] == 1)
                    VecinasVivas++;


         //vecina 8   
            if (x < longitud - 1 && y < longitud - 1)
                if (celulas[x + 1, y + 1] == 1)
                    VecinasVivas++;

            if (EsViva)
                return (VecinasVivas == 2 || VecinasVivas == 3) ? 1 : 0;
            else
                return VecinasVivas == 3 ? 1 : 0;

        }
        private void button3_Click(object sender, EventArgs e)
        {
            //reiniciamos
            for (int i = 0; i < longitud; i++)
                for (int j = 0; j < longitud; j++)
                    celulas[i, j] = 0;

            Random r = new Random();
            //llenamos random
            for (int i = 0; i < longitud; i++)
                for (int j = 0; j < longitud; j++)
                    celulas[i, j] = r.Next(0, 2);

            PintarMatriz();
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            List<string> lineas;
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Archivos de Texto (*.txt)|*.txt";


            dialog.Title = "Seleccione el archivo de Entrada";

            dialog.FileName = string.Empty;

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                LlenarMatriz(dialog.FileName);


            }
        }

        private void LlenarMatriz(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("No hay hoja para leer");
            }
            else
            {
                try
                {
                    var lines = GetLines(fileName);
                    if (lines.Count == 0)
                    {
                        MessageBox.Show("Archivo Vacio");
                    }
                    else
                    {
                        int[] lineaArchivo;
                        celulas = new int[lines.Count, lines.Count];
                        for (int y = 0; y < lines.Count; y++)
                        {

                            var line = lines[y];
                            lineaArchivo = line.Split(',').Select(x => x.Trim()).Select(x => int.Parse(x)).ToArray();

                            for (int x = 0; x < lineaArchivo.Length; x++)
                            {
                                celulas[y, x] = lineaArchivo[x];
                            }
                        }

                        PintarMatriz();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error, verificar el archivo", ex.Message);
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            timer1.Enabled = true;
           
        }

       

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            JuegoDeLaVida();
            PintarMatriz();

            contsimulacioneslbl.Text = contsimulacion + "";
            contsimulacion++;
        }

        static List<string> GetLines(string inputFile)
        {
            List<string> lines = new List<string>();
            using (var reader = new StreamReader(inputFile))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        lines.Add(line);
                    }
                }
            }
            return lines;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            Pen p = new Pen(Color.Black);

            //linea horizontal
            for (int y = 0; y <= longitud; ++y)
            {
                g.DrawLine(p, 0, y * longitudPixel, longitud * longitudPixel, y * longitudPixel);
            }

            //linea vertical
            for (int x = 0; x <= longitud;++x)
            {
                g.DrawLine(p, x * longitudPixel, 0, x * longitudPixel, longitud * longitudPixel);
            }
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
