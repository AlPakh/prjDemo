using System;
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

        
        int[,] masField = new int[21*5, 11*5]; //Массив для всей карты
        int[,] masObjects = new int[21*5, 11*5]; //Массив для объектов на карте
        int[,] masView = new int[21, 11]; // Массив для "камеры"
        string[,] strMaps = 
        { 
            {"ter1.txt", "ob1.txt"},
            {"ter2.txt", "ob1.txt"} 
        };


        Color[] masTerColors = { Color.Blue, Color.Lime, Color.Yellow };
        Image[] masObjImages = { 
            Properties.Resources.emp,
            Properties.Resources.tree,
            Properties.Resources.rock,
            Properties.Resources.tele
        };
        int intEncounterChance = 1; //Шанс вступления в случайный бой (Должно работать при значениях 1-50 и 100)

        int currX = 0, currY = 0; // координата появления
        string strCurrentMap = "";

        Random r = new Random();

        private void Form1_Load(object sender, EventArgs e)
        {
            currX = 32; currY = 41;
            int prevX = currX, prevY = currY;

            LoadMap(strMaps[0, 0], strMaps[0, 1]);             //Загрузка карты из файла

            RefreshView(ref currX, ref currY, prevX, prevY);

            ExploreMap(currX + 1, currY + 1);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnGetData_Click(object sender, EventArgs e)
        {
            MessageBox.Show(currX + " " + currY + " " + intEncounterChance);
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
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

            if ((e.KeyCode == Keys.W || e.KeyCode == Keys.A || e.KeyCode == Keys.S || e.KeyCode == Keys.D) &&       // Кнопки передвижения
                masField[currX + 1, currY + 1] != 0 &&                                                              // Цвет клетки - не синий
                masObjects[currX + 1, currY + 1] != 1 && masObjects[currX + 1, currY + 1] != 2)                     // Клетка не занята объектом
            {
                pnlView.Enabled = false; //Запрет на обработку нажатий, пока не будут выполнены все подпрограммы

                ExploreMap(currX + 1, currY + 1);

                RefreshView(ref currX, ref currY, prevX, prevY);

                TriggerLogic(ref currX, ref currY, strCurrentMap);

                EncounterCheck();

                pnlView.Enabled = true; //Можно дальше нажимать кнопки
            }
            else
            {
                currX = prevX; currY = prevY;
            }

        }

        

        //Обновить ячейки
        void RefreshView(ref int currX, ref int currY, int prevX, int prevY)
        {
            //Координаты "Нулевой клетки" массива камеры относительно всей карты
            int frameX = currX - 10, frameY = currY - 5;
            
            //Если клетка, на которую надо наступить - не синего цвета, заполнить массив для masView для "камеры" и перерисовать PicturBbox
            if (masField[currX + 1, currY + 1] != 0)
            {
                for (int x = 0; x < 21; x++)
                {
                    for (int y = 0; y < 11; y++)
                    {
                        int intBackIndex = 0;
                        PictureBox pic = (PictureBox)pnlView.Controls[$"picCreate{x}o{y}"];
                        pic.BackgroundImage = Properties.Resources.emp;
                        pic.Image = null;

                        masView[x, y] = masField[frameX + x + 1, frameY + y + 1];
                        pic.BackColor = masTerColors[masView[x, y]];

                        //Проверка условий на добавление границ на изображение
                        bool bHorizontalBackground = masView[x, y] != masField[frameX + x + 1, frameY + y + 2];
                        bool bVerticalBackground = masView[x, y] != masField[frameX + x + 2, frameY + y+ 1];
                        bool bDiagonalBackGround = masView[x, y] != masField[frameX + x + 2, frameY + y + 2];
                        bool bHorVertBackground = bVerticalBackground&&bHorizontalBackground;

                        if (masView[x, y] != 0) //Если клетка не синего цвета, нарисовать на ней границы
                        {
                            if (bHorVertBackground) { intBackIndex = 3; }
                            else if (bVerticalBackground) { intBackIndex = 2; }
                            else if (bHorizontalBackground && !bVerticalBackground) { intBackIndex = 1; }
                            else if (bDiagonalBackGround && !bHorizontalBackground && !bVerticalBackground) { intBackIndex = 4; }

                            pic.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("back" + intBackIndex.ToString());


                            //Если не надо рисовать границы, загрузить изображение объекта
                            if (masObjects[frameX + x + 1, frameY + y + 1] != 0 && intBackIndex == 0)
                            {
                                pic.BackgroundImage = masObjImages[masObjects[frameX + x + 1, frameY + y + 1]];
                            }

                        }

                        if (x == 10 && y == 5) pic.Image = Properties.Resources.lul;
                    }
                }
            }
            else
            {
                currX = prevX;
                currY = prevY;
            }

        }

        //Вычисление шанса вступить в случайный бой
        void EncounterCheck()
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

            //Какую строку из массива применить для описания поверхности
            indTerrain = masField[currX + 1, currY + 1];

            if (randomEncounter != 0) //Encounter failed
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
            for(int i = y-5; i < y+4; i++)
            {
                for(int j = x - 10; j < x + 9; j++)
                {
                    string picName = $"picCreate{j}o{i}";
                    
                    int iu = pnlField.Controls.Find(picName, true).Length; // Сколько элементов на панели имеют такое имя
                    if (iu == 0)
                    {
                        PictureBox picCreate = new PictureBox();
                        picCreate.BackColor = masTerColors[masField[j, i]];
                        picCreate.Size = new Size(3, 3);
                        picCreate.Name = picName;
                        picCreate.Location = new Point(j * 3, i * 3);

                        pnlField.Controls.Add(picCreate);
                    }
                }
            }


        }

        //Загрузка локации
        void LoadMap(string strTerrainFileName, string strObjectsFileName)
        {
            strCurrentMap = strTerrainFileName;
            string strT = File.ReadAllText(strTerrainFileName); //считать файл с цветами ячеек в одномерный массив
            string[] masTerr = strT.Split(' ');

            string strO = File.ReadAllText(strObjectsFileName); //считать файл с объектами ячеек в одномерный массив
            string[] masObj = strO.Split(' ');

            //Заполнение двумерных массивов из одномерных
            for (int x = 0; x < 105; x++)
            {
                for (int y = 0; y < 55; y++)
                {
                    masField[x, y] = Convert.ToInt32(masTerr[x + y * 105]); //Заполнение массива территории
                    masObjects[x, y] = Convert.ToInt32(masObj[x + y * 105]); //Заполнение массива объектов на карте
                }
            }

            //Заполнение панели кнопками
            pnlView.Controls.Clear();
            for (int x = 0; x < 21; x++)
            {
                for (int y = 0; y < 11; y++)
                {
                    PictureBox picCreate = new PictureBox();
                    picCreate.BackColor = Color.White;
                    picCreate.Size = new Size(40, 60);
                    picCreate.Name = $"picCreate{x}o{y}";
                    picCreate.Location = new Point(x * 40, y * 60);
                    picCreate.SizeMode = PictureBoxSizeMode.Zoom;

                    pnlView.Controls.Add(picCreate);
                }
            }

            pnlField.Controls.Clear();
        }

        //Триггеры
        void TriggerLogic(ref int currX, ref int currY, string strCurrentMap)
        {
            if (currX == 75 && currY == 14 && strCurrentMap == strMaps[0, 0]) //Переход на уровень 2
            {
                DialogResult dr = MessageBox.Show("Переход?", "", MessageBoxButtons.YesNo);
                if (dr == DialogResult.Yes)
                {
                    //Загрузка карты из файла
                    LoadMap(strMaps[1, 0], strMaps[1, 1]);
                    currX = 18; currY = 26;

                    RefreshView(ref currX, ref currY, currX, currY);

                    ExploreMap(currX, currY);
                }
            }
        }
    }
}
