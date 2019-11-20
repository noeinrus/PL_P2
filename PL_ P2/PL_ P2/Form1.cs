using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace PL__P2
{
    public partial class Form1 : Form
    {
        private const string indifFile = "Input_indificators.txt";
        private enum c_state {Start, Indif, IEnd};

        public Form1()
        {
            InitializeComponent();
        }

        private void readIndificatorsFile_Click(object sender, EventArgs e)
        {
            AnalyzeIndifFile();
        }

        private string[] AnalyzeIndifFile() {
            string res_str = "";
            string[] indentificators = new string[0];
            if (indifFile == "")
                res_str += "Задано пустое имя файла.";
            else{
                try
                {
                    using (StreamReader sr = new StreamReader(indifFile))
                    {
                        string line = sr.ReadToEnd();
                        c_state CS = c_state.Start;
                        int i = 0;
                        string indentificator = "";
                        do
                        {
                            char simb = line[i];
                            
                            switch (CS)
                            {
                                case c_state.Start: {
                                    if ((line[i] >= 'A' && line[i] <= 'Z') || (line[i] >= 'a' && line[i] <= 'z') || (line[i] >= '0' && line[i] <= '9'))
                                    {
                                        indentificator = "";
                                        indentificator += line[i];
                                        CS = c_state.Indif;
                                    }
                                    break;                                  
                                }

                                case c_state.Indif:{
                                    if ((line[i] >= 'A' && line[i] <= 'Z') || (line[i] >= 'a' && line[i] <= 'z') || (line[i] >= '0' && line[i] <= '9'))
                                        indentificator += line[i];
                                    if (line[i] == ' ') {
                                        Array.Resize(ref indentificators, indentificators.Length + 1);
                                        indentificators[indentificators.Length - 1] = indentificator;
                                        CS = c_state.IEnd;
                                    }
                                    break;
                                }

                                case c_state.IEnd: {
                                    if ((line[i] >= 'A' && line[i] <= 'Z') || (line[i] >= 'a' && line[i] <= 'z') || (line[i] >= '0' && line[i] <= '9'))
                                    {
                                        indentificator = "";
                                        indentificator += line[i];
                                        CS = c_state.Indif;
                                    }
                                    break;                                  
                                }

                            }
                            i++;
                        } while (i < line.Length);
                        if(indentificator != "") {
                            Array.Resize(ref indentificators, indentificators.Length + 1);
                            indentificators[indentificators.Length - 1] = indentificator;
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    res_str += "\r\n" + ex.Message;
                }
            }
            return indentificators;
        }
    }
}
