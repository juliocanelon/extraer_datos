using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ExtraerDatos
{
    public partial class Form1 : Form
    {
        OpenFileDialog openDialog = new OpenFileDialog();
        SaveFileDialog saveDialog = new SaveFileDialog();
        Dictionary<int, List<String>> Diccionario = new Dictionary<int, List<string>>();

        public Form1()
        {

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            openDialog.Filter = "Txt Texto|*.txt";
            openDialog.Title = "Seleccionar archivo de restaurantes a analizar";

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            /// el archivo no debe comenzar en ......
            /// 
            Diccionario.Clear();
            textBox2.Text = "Procesando...";
            try
            {
                int i = 0;
                using (System.IO.StreamReader openFile = new System.IO.StreamReader(openDialog.FileName, System.Text.Encoding.GetEncoding(1252)))
                {
                    bool nuevoRegistro = true;
                    List<String> registro = null;

                    while (!openFile.EndOfStream)
                    {                        
                        String line = openFile.ReadLine();
                        if (line.Length > 0)
                        {
                            String c = line.Substring(0, 1);
                            if (c.Equals("."))
                            {
                                nuevoRegistro = true;
                                if (registro != null)
                                {
                                    Diccionario.Add(i++, registro);
                                }
                            }
                            else
                            {
                                if (nuevoRegistro)
                                {
                                    registro = new List<String>();
                                }
                                registro.Add(line);
                                nuevoRegistro = false;
                            }
                        }
                    }
                }

                

                System.IO.StreamWriter SaveFile = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.GetEncoding(1252));
                foreach (KeyValuePair<int, List<String>> result in Diccionario)
                {
                    List<String> registro = result.Value;

                    if (registro.Count == 3)
                    {
                        SaveFile.WriteLine(registro[1]+ ";" + registro[0] + ";" + registro[2]);
                    }
                }

                SaveFile.WriteLine("Registros que no coinciden con el formato:");
                foreach (KeyValuePair<int, List<String>> result in Diccionario)
                {
                    List<String> registro = result.Value;

                    if (registro.Count > 3)
                    {
                        string salida = "";
                        foreach (String campo in registro)
                        {
                            salida += campo + ";";
                        }

                        SaveFile.WriteLine(salida);
                    }
                }
                SaveFile.Close();
                textBox2.Text = "Proceso finalizado...";

            }
            catch (Exception exc)
            {
                MessageBox.Show("No se pudo leer el archivo - " + exc.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveDialog.Filter = "Csv Texto|*.csv";
            saveDialog.Title = "Seleccionar archivo a guardar la exportación";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = saveDialog.FileName;
            }
        }
    }
}
