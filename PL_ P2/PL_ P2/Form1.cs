using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;

namespace PL__P2
{
    public partial class Form1 : Form
    {
        string[] basicMas;
        List<string> rehashList;

        private const string indifFile = "Input_indificators.txt";
        private enum C_state {Start, Indif, IEnd};

        public Form1()
        {
            InitializeComponent();
        }

        private void ReadIndificatorsFile_Click(object sender, EventArgs e)
        {
            AnalyzeIndifFile();
        }


        private void ClearLists() {
            basicMas = null;
            basicMas = new string[0];
            rehashList = new List<string>();
        }

        private int Hash (string inputText) {
            int sign = 1;
            int f = 0;
            for(int i = 0; i < inputText.Length; i++) {
                f = sign * (byte)inputText[i]; 
                sign = sign * -1;
            } //Вычисление функции по значению ключа

            f = f + 255 * 2; //Cовмещение начала области значений функции с начальным адресом хеш - таблицы (a = 1)
            f = (f * 10000) / (255 * 4); //Совмещение конца области значений функции с конечным адресомхеш - таблицы(a = 10 000)
            return f;
        }

        private void SetListLength(int len) {
            len = len - rehashList.Count + 1;
            for (int i = 0; i < len; i++)
                rehashList.Add("");
        }

        private bool AddToReHash(string Word) {
            int i = 0;
            bool inserted = false;
            bool isError = false;
            do {
                int a = (Hash(Word) + i) % 10000;
                if(rehashList.Count < a || rehashList[a] == ""){
                    if (rehashList.Count < a)
                        SetListLength(a);
                    rehashList[a] = Word;
                    inserted = true;
                }
                if(rehashList[a] == Word){
                    isError = true;
                }
                i++;
            } while (!inserted && !isError);
            return inserted;
        }

        public int FindInReHash(string Word)
        {
            int i = 0;
            int index = -1;
            bool found = false;
            bool isError = false;
            do
            {
                int a = (Hash(Word) + i) % 10000;
                if (rehashList[a] == Word)
                {
                    index = a;
                    found = true;
                }
                if (rehashList[a] == "")
                {
                    isError = true;
                }
                i++;
            } while (!found && !isError);
            return index;
        }

        private void AddToBasicMas(string Word) {
            Array.Resize(ref basicMas, basicMas.Length + 1);
            basicMas[basicMas.Length - 1] = Word;
        }

        public int FindInBasicMas(string Word)
        {
            int i = -1;
            do
            {
                i++;
            } while ((i < basicMas.Length - 1) && (basicMas[i] != Word));
            if (basicMas[i] != Word)
                i = -1;
            return i;
        }

        private void AnalyzeIndifFile() {
            string res_str = "";
            ClearLists();
            if (indifFile == "")
                res_str += "Задано пустое имя файла.";
            else{
                try
                {
                    using (StreamReader sr = new StreamReader(indifFile))
                    {
                        string line = sr.ReadToEnd();
                        C_state CS = C_state.Start;
                        int i = 0;
                        string identificator = "";
                        do
                        {
                            char simb = line[i];
                            
                            switch (CS)
                            {
                                case C_state.Start: {
                                    if ((line[i] >= 'A' && line[i] <= 'Z') || (line[i] >= 'a' && line[i] <= 'z') || (line[i] >= '0' && line[i] <= '9'))
                                    {
                                        identificator = "";
                                        identificator += line[i];
                                        CS = C_state.Indif;
                                    }
                                    break;                                  
                                }

                                case C_state.Indif:{
                                    if ((line[i] >= 'A' && line[i] <= 'Z') || (line[i] >= 'a' && line[i] <= 'z') || (line[i] >= '0' && line[i] <= '9'))
                                        identificator += line[i];
                                    if (line[i] == ' ') {
                                        AddToBasicMas(identificator);
                                        AddToReHash(identificator);
                                        CS = C_state.IEnd;
                                    }
                                    break;
                                }

                                case C_state.IEnd: {
                                    if ((line[i] >= 'A' && line[i] <= 'Z') || (line[i] >= 'a' && line[i] <= 'z') || (line[i] >= '0' && line[i] <= '9'))
                                    {
                                        identificator = "";
                                        identificator += line[i];
                                        CS = C_state.Indif;
                                    }
                                    break;                                  
                                }

                            }
                            i++;
                        } while (i < line.Length);
                        if(identificator != "") {
                            AddToBasicMas(identificator);
                            AddToReHash(identificator);
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    res_str += "\r\n" + ex.Message;
                }
            }
        }

        public void FindWord (string word) {
            textBox1.Text += word + " = BM: " + FindInBasicMas(word).ToString() + "; ReHash: " + FindInReHash(word).ToString() + "\r\n";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            FindWord(textBox2.Text);
        }
    }
}
