﻿using System;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
namespace prjDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string[,] masField = new string[21*5, 11*5]; //Массив для всей карты
        string[,] masView = new string[21, 11]; // Массив для "камеры"
        int intEncounterChance = 1; //Шанс вступления в случайный бой

        int currX = 32, currY = 41; // координата появления
        Random r = new Random();

        private void Form1_Load(object sender, EventArgs e)
        {
            //Загрузка карты из файла
            string s = File.ReadAllText("tr1.txt");
            string[] masTerr = s.Split(' ');

            //Заполнение карты
            for(int x = 0; x < 105; x++)
            {
                for(int y = 0; y < 55; y++)
                {
                    masField[x, y] = masTerr[y + x*55];

                    //PictureBox picCreate = new PictureBox();
                    //picCreate.BackColor = Color.FromName(masField[x, y]);
                    //picCreate.Size = new Size(3, 3);
                    //picCreate.Name = $"picCreate{x}o{y}";
                    //picCreate.Location = new Point(x * 3, y * 3);

                    //pnlField.Controls.Add(picCreate);
                }
            }

            //Заполнение панели кнопками
            for (int x = 0; x < 21; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    PictureBox picCreate = new PictureBox();
                    picCreate.BackColor = Color.White;
                    picCreate.Size = new Size(40, 60);
                    picCreate.Name = $"picCreate{x}o{y}";
                    picCreate.Location = new Point(x * 40, y * 60);

                    if (x == 11 && y == 6)
                    {
                        picCreate.Image = Properties.Resources.lul;
                    }
                    //else
                    //{
                    //    picCreate.Image = Properties.Resources.emp;
                    //}


                    pnlView.Controls.Add(picCreate);
                }
            }

            int prevX = currX, prevY = currY;
            RefreshView(ref currX, ref currY, prevX, prevY);

            ExploreMap(currX + 1, currY + 1);
        }

        private void PicCreate_MouseHover(object sender, EventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            textBox1.Text = pic.Name;
            if (checkBox1.Checked)
            {
                if (pic.BackColor == Color.Blue)
                {
                    pic.BackColor = Color.Lime;
                }
                else if (pic.BackColor == Color.Lime)
                {
                    pic.BackColor = Color.Yellow;
                }
                else
                {
                    pic.BackColor = Color.Blue;
                }
            }
        }

        private void PicCreate_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            if(pic.BackColor == Color.Blue)
            {
                pic.BackColor = Color.Lime;
            }
            else if (pic.BackColor == Color.Lime)
            {
                pic.BackColor = Color.Yellow;
            }
            else
            {
                pic.BackColor = Color.Blue;
            }
        }

        private void btnSaveMap_Click(object sender, EventArgs e)
        {
            string masColors = ""; 
            foreach(PictureBox pic in pnlMakeMap.Controls)
            {
                masColors += pic.BackColor.Name + " ";
            }
            File.WriteAllText("terrain1.txt", masColors);
        }


        bool processingRefresh = false;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!processingRefresh)
            {

                processingRefresh = true;
                int prevX = currX, prevY = currY;

                switch (e.KeyCode)
                {
                    case Keys.W:
                        currY--;
                        break;
                    case Keys.A:
                        currX--;
                        break;
                    case Keys.D:
                        currX++;
                        break;
                    case Keys.S:
                        currY++;
                        break;
                }

                this.Enabled = false; //Запрет на обработку нажатий, пока не будут выполнены все подпрограммы

                EncounterCheck();

                ExploreMap(currX + 1, currY + 1);
                //Application.DoEvents();

                //Thread.Sleep(100);

                RefreshView(ref currX, ref currY, prevX, prevY);

                this.Enabled = true; //Можно дальше нажимать кнопки

            }
            processingRefresh = false;
        }

        void RefreshView(ref int currX, ref int currY, int prevX, int prevY)
        {
            //Координаты "Нулевой клетки" массива камеры относительно всей карты
            int frameX = currX - 10, frameY = currY - 5;
            
            //Если клетка, на которую надо наступить - не синего цвета, заполнить массив для masView для "камеры" и перерисовать PicturBbox
            if (masField[currX + 1, currY + 1] != "Blue")
            {
                for (int x = 0; x < 21; x++)
                {
                    for (int y = 0; y < 11; y++)
                    {
                        PictureBox pic = (PictureBox)pnlView.Controls[$"picCreate{x}o{y}"];

                        masView[x, y] = masField[frameX + x, frameY + y];
                        pic.BackColor = Color.FromName(masView[x, y]);
                    }
                }
            }
            else
            {
                currX = prevX;
                currY = prevY;
            }

        }

        public void EncounterCheck()
        {
            int calcChance = 100/intEncounterChance;
            int randomEncounter = r.Next( calcChance );
            int indTerrain, indEncounter;
            string[] masMessages = {
                "Перед вашими глазами волны, вы остаётесь на месте. ", //0
                "Под вашими ногами тёплый песок. ", //1
                "Под вашими ногами мягкая трава. ", //2

                "Вы легко продвигаетесь вперёд. ", //3
                "Вы смело делаете шаг вперёд. ", //4
                "На вашем пути нет никаких препятствий. ", //5
                "Кругом всё спокойно. ", // 6
                "Тишина сопровождает вас. ", //7
                "Ничто не прерывает ваше путешествие. ", // 8
                "Вы не ощущаете угрозы. ", //9
                "Ваше приключение ощущается как прогулка. ", //10
                "Вокруг нет ни души. ", // 11

                "Вы слышите гулкий рёв. ",
                "Что-то надвигается. ",
                "Краем глаза вы замечаете мелькнувшую тень. ",
                "Враг явил себя. "
            };

            //Какую строку из массива применить для поверхности
            switch (masField[currX + 1, currY + 1])
            {
                case "Blue":
                    indTerrain = 0;
                    break;

                case "Yellow":
                    indTerrain = 1;
                    break;

                default:
                    indTerrain = 2;
                    break;
            }

            if (randomEncounter != 0)
            {
                indEncounter = r.Next(3, 12);
            }
            else
            {
                indEncounter = r.Next(7, 16);
            }

            this.Invoke(new MethodInvoker(() =>
            {
                txtMessage.Text = masMessages[indTerrain] + masMessages[indEncounter];
            }));
        }

        //Добавление на карту пройденных локаций
        void ExploreMap(int x, int y)
        {
            for(int i = y-6; i < y+5; i++)
            {
                for(int j = x - 11; j < x + 10; j++)
                {
                    string picName = $"picCreate{j}o{i}";
                    
                    int iu = pnlField.Controls.Find(picName, true).Length; // Сколько элементов на панели имеют такое имя
                    if (iu == 0)
                    {
                        PictureBox picCreate = new PictureBox();
                        picCreate.BackColor = Color.FromName(masField[j, i]);
                        picCreate.Size = new Size(3, 3);
                        picCreate.Name = picName;
                        picCreate.Location = new Point(j * 3, i * 3);

                        pnlField.Controls.Add(picCreate);
                    }
                }
            }


        }
    }
}