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

        
        string[,] masField = new string[21*5, 11*5]; //Массив для всей карты
        int[,] masObjects = new int[21*5, 11*5]; //Массив для объектов на карте
        string[,] masView = new string[21, 11]; // Массив для "камеры"


        Color[] masTerColors = { Color.Blue, Color.Lime, Color.Yellow };
        Image[] masObjImages = { 
            Properties.Resources.emp,
            Properties.Resources.tree,
            Properties.Resources.rocks};
        int intEncounterChance = 1; //Шанс вступления в случайный бой

        int currX = 0, currY = 0; // координата появления
        Random r = new Random();

        private void Form1_Load(object sender, EventArgs e)
        {
            //Загрузка карты из файла
            LoadMap("tr1.txt", "ob1.txt");
            
            currX = 32; currY = 41;
            int prevX = currX, prevY = currY;

            RefreshView(ref currX, ref currY, prevX, prevY);

            ExploreMap(currX + 1, currY + 1);
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

                if (masField[currX, currY] != "Blue")
                {
                    pnlView.Enabled = false; //Запрет на обработку нажатий, пока не будут выполнены все подпрограммы

                    EncounterCheck();
                    Application.DoEvents();

                    ExploreMap(currX + 1, currY + 1);

                    Thread.Sleep(100);

                    RefreshView(ref currX, ref currY, prevX, prevY);

                    pnlView.Enabled = true; //Можно дальше нажимать кнопки
                }
                else
                {
                    currX = prevX; currY = prevY;
                }
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
                        int intBackIndex = 0;
                        PictureBox pic = (PictureBox)pnlView.Controls[$"picCreate{x}o{y}"];
                        pic.BackgroundImage = (Bitmap)Properties.Resources.emp;

                        masView[x, y] = masField[frameX + x, frameY + y];
                        pic.BackColor = Color.FromName(masView[x, y]);

                        if (!(x == 11 && y == 6))
                        {
                            pic.Image = masObjImages[masObjects[frameX + x, frameY + y]];
                        }

                        bool bHorizontalBackground = masView[x, y] != masField[frameX + x, frameY + y + 1];
                        bool bVerticalBackground = masView[x, y] != masField[frameX + x + 1, frameY + y];
                        bool bDiagonalBackGround = masView[x, y] != masField[frameX + x + 1, frameY + y + 1];
                        bool bHorVertBackground = bVerticalBackground&&bHorizontalBackground;

                        if (masView[x, y] != "Blue")
                        {
                            if (bHorVertBackground) { intBackIndex = 3; }
                            else if (bVerticalBackground) { intBackIndex = 2; }
                            else if (bHorizontalBackground && !bVerticalBackground) { intBackIndex = 1; }
                            else if (bDiagonalBackGround && !bHorizontalBackground && !bVerticalBackground) { intBackIndex = 4; }

                            pic.BackgroundImage = (Bitmap)Properties.Resources.ResourceManager.GetObject("back" + intBackIndex.ToString());
                        }
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Загрузка локации
        void LoadMap(string strTerrainFileName, string strObjectsFileName)
        {
            string strT = File.ReadAllText(strTerrainFileName);
            string[] masTerr = strT.Split(' ');

            //Заполнение двумерного массива территории из одномерного
            for (int x = 0; x < 105; x++)
            {
                for (int y = 0; y < 55; y++)
                {
                    masField[x, y] = masTerr[x + y * 105];
                }
            }

            string strO = File.ReadAllText(strObjectsFileName);
            string[] masObj = strO.Split(' ');

            //Заполнение двумерного массива объектов из одномерного
            for (int x = 0; x < 105; x++)
            {
                for (int y = 0; y < 55; y++)
                {
                    masObjects[x, y] = Convert.ToInt32(masObj[x + y * 105]);
                }
            }

            masObjects[34, 44] = 1;
            masObjects[35, 45] = 2;
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

                    if (x == 11 && y == 6)
                    {
                        picCreate.Image = Properties.Resources.lul;
                    }

                    picCreate.SizeMode = PictureBoxSizeMode.Zoom;

                    pnlView.Controls.Add(picCreate);
                }
            }

            pnlField.Controls.Clear();
        }
    }
}
