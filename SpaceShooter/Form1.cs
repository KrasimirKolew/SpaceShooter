using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WMPLib;

namespace SpaceShooter
{
    public partial class SpaceShooter : Form
    {
        //Въвеждаме нашата музика

        WindowsMediaPlayer gameMedia;
        WindowsMediaPlayer shootgMedia;
        WindowsMediaPlayer explosion;

        //създаваме масиви от картинки за движештите се обекти

        PictureBox[] enemiesMun;
        int enemyMunSpeed;

        PictureBox[] stars;
        int backgraundspeed;
        int playerSpeed;

        PictureBox[] munitions;
        int munitionSpeed;

        PictureBox[] enemies;
        int enemiesSpeed;

        Random rnd;

        //дефинираме променливи

        int score;
        int level;
        int dificult;
        bool pause;
        bool gameOver;

        public SpaceShooter()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pause = false;
            gameOver = false;
            score = 0;
            level = 1;
            dificult = 9;

            backgraundspeed = 4;
            playerSpeed = 4;
            enemiesSpeed = 4;
            enemyMunSpeed = 4;

            munitionSpeed = 20;
            munitions = new PictureBox[3];
            
            //load imgs / Зареждане на снимките
            Image munition = Image.FromFile(@"asserts\munition.png");

            Image enemi1 = Image.FromFile(@"asserts\\E1.png");
            Image enemi2 = Image.FromFile(@"asserts\\E2.png");
            Image enemi3 = Image.FromFile(@"asserts\\E3.png");
            Image boss1 = Image.FromFile(@"asserts\\Boss1.png");
            Image boss2 = Image.FromFile(@"asserts\\Boss2.png");

            enemies = new PictureBox[10];

            //създаване на противници
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i] = new PictureBox();
                enemies[i].Size = new Size(40, 40);
                enemies[i].SizeMode = PictureBoxSizeMode.Zoom;
                enemies[i].BorderStyle=BorderStyle.None;
                enemies[i].Visible = false;
                this.Controls.Add(enemies[i]);
                enemies[i].Location = new Point((i + 1) * 50,-50);
            }

            enemies[0].Image = boss1;
            enemies[1].Image = enemi2;
            enemies[2].Image = enemi3;
            enemies[3].Image = enemi3;
            enemies[4].Image = enemi1;
            enemies[5].Image = enemi3;
            enemies[6].Image = enemi2;
            enemies[7].Image = enemi3;
            enemies[8].Image = enemi2;
            enemies[9].Image = boss2;

            //създаване на изтрелите
            for (int i = 0; i < munitions.Length; i++)
            {
                munitions[i]=new PictureBox();
                munitions[i].Size = new Size(8, 8);
                munitions[i].Image = munition;
                munitions[i].SizeMode=PictureBoxSizeMode.Zoom;
                munitions[i].BorderStyle = BorderStyle.None;
                this.Controls.Add(munitions[i]);
            }

            //creat WMP / Създаване на музика
            gameMedia = new WindowsMediaPlayer();
            shootgMedia = new WindowsMediaPlayer();
            explosion = new WindowsMediaPlayer();

            //load all songs / Зареждане на всички песни
            gameMedia.URL = "songs\\GameSong.mp3";
            shootgMedia.URL = "songs\\shoot.mp3";
            explosion.URL = "songs\\boom.mp3";

            //setup songs settings / задаваме желани от нас настройки
            gameMedia.settings.setMode("loop", true);
            gameMedia.settings.volume = 4;
            shootgMedia.settings.volume = 1;
            explosion.settings.volume = 5;

            stars = new PictureBox[10];
            rnd = new Random();

            //движение на звездите на задния фон
            for(int i=0; i < stars.Length; i++)
            {
                stars[i] = new PictureBox();
                stars[i].BorderStyle = BorderStyle.None;
                stars[i].Location = new Point(rnd.Next(20, 580), rnd.Next(-10, 400));
                if (i%2==1)
                {
                    stars[i].Size = new Size(2, 2);
                    stars[i].BackColor = Color.Wheat;
                }
                else
                {
                    stars[i].Size = new Size(3, 3);
                    stars[i].BackColor = Color.DarkGray;
                }

                this.Controls.Add(stars[i]);
            }

            //Enemis  Mulition / противникови изтрели    
            enemiesMun = new PictureBox[10];

            for (int i = 0; i < enemiesMun.Length; i++)
            {
                enemiesMun[i] = new PictureBox();
                enemiesMun[i].Size = new Size(2, 25);
                enemiesMun[i].Visible = false;
                enemiesMun[i].BackColor = Color.Yellow;
                int x = rnd.Next(0, 10);
                enemiesMun[i].Location = new Point(enemies[x].Location.X, enemies[x].Location.Y - 20);
                this.Controls.Add(enemiesMun[i]);
            }
            gameMedia.controls.play();
        }

        //background move / движение на задния фон
        private void MoveBgTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < stars.Length/2; i++)
            {
                stars[i].Top += backgraundspeed;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }

            for (int i = stars.Length / 2; i < stars.Length; i++)
            {
                stars[i].Top += backgraundspeed-2;

                if (stars[i].Top >= this.Height)
                {
                    stars[i].Top = -stars[i].Height;
                }
            }
        }

        //player moves left / движение на ляво
        private void LeftMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left>10)
            {
                Player.Left -= playerSpeed;
            }
        }

        //player moves Right / движение на дясно
        private void RightMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Left < 580)
            {
                Player.Left += playerSpeed;
            }
        }

        //player moves Down / движение на долу
        private void DownMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top < 400)
            {
                Player.Top += playerSpeed;
            }
        }

        //player moves Up / движение на горе
        private void UpMoveTimer_Tick(object sender, EventArgs e)
        {
            if (Player.Top >10)
            {
                Player.Top -= playerSpeed;
            }
        }
        
        // движение при натиснати копчета на клавиатурата
        private void SpaceShooter_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pause)
            {
                if (e.KeyCode == Keys.Right)
                {
                    RightMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Left)
                {
                    LeftMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Down)
                {
                    DownMoveTimer.Start();
                }
                if (e.KeyCode == Keys.Up)
                {
                    UpMoveTimer.Start();
                }
            }          
        }
        // спиране на движение при НЕ натиснати копчета на клавиатурата
        private void SpaceShooter_KeyUp(object sender, KeyEventArgs e)
        {
            RightMoveTimer.Stop();
            LeftMoveTimer.Stop();
            DownMoveTimer.Stop();
            UpMoveTimer.Stop();

            if (e.KeyCode == Keys.Space)
            {
                if (!gameOver)
                {
                    if (pause)
                    {
                        StartTimers();
                        label1.Visible = false;
                        gameMedia.controls.play();
                        pause = false;
                    }
                    else
                    {
                        label1.Location = new Point(this.Width / 2 -50 , 150);
                        label1.Text = "PAUSED";
                        label1.Visible = true;
                        gameMedia.controls.pause();
                        StopTimers();
                        pause = true;
                    }
                }              
            }
        }

        //shooting / стрелба
        private void MoveMunitionTimer_Tick(object sender, EventArgs e)
        {
            shootgMedia.controls.play();

            for (int i = 0; i < munitions.Length; i++)
            {
                if (munitions[i].Top>0)
                {
                    munitions[i].Visible = true;
                    munitions[i].Top -= munitionSpeed;

                    Collision();
                }
                else
                {
                    munitions[i].Visible = false;
                    munitions[i].Location=new Point(Player.Location.X + 20, Player.Location.Y - i * 30);
                }
            }
        }

        //enemy movement / движение на противници
        private void MoveEnemiesTimer_Tick(object sender, EventArgs e)
        {
            MoveEnemies(enemies, enemiesSpeed);
        }

        private void MoveEnemies(PictureBox[] array,int speed)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i].Visible = true;
                array[i].Top += speed;
                if (array[i].Top >this.Height)
                {
                    array[i].Location = new Point((i + 1) * 50, -200);
                }
            }
        }

        // collision / сблъсък
        private void Collision()
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                if (munitions[0].Bounds.IntersectsWith(enemies[i].Bounds) 
                    || munitions[1].Bounds.IntersectsWith(enemies[i].Bounds) 
                        || munitions[2].Bounds.IntersectsWith(enemies[i].Bounds)) 
                {
                    explosion.controls.play();

                    score += 1;
                    skurelb.Text = (level < 10) ? "0" + score.ToString() : score.ToString();

                    if (score % 30 ==0)
                    {
                        level += 1;
                        lvllb.Text=(level<10) ? "0" + level.ToString() : level.ToString();

                        if (enemiesSpeed <= 10 && enemyMunSpeed <= 10 && dificult >= 0)
                        {
                            dificult--;
                            enemiesSpeed++;
                            enemyMunSpeed++;
                        }

                        if (level == 10)
                        {
                            GameOver("NICE DONE");
                        }
                    }
                   
                    enemies[i].Location = new Point((i + 1) * 50, -100);
                }

                if (Player.Bounds.IntersectsWith(enemies[i].Bounds))
                {
                    explosion.settings.volume = 30;
                    explosion.controls.play();
                    Player.Visible=false;
                    GameOver("Game Over");
                }
            }
        }
         
        // край на играта
        private void GameOver(String str)
        {
            label1.Text = str;
            label1.Location = new Point(225, 120);
            label1.Visible = true;
            ReplayBtn.Visible = true;
            ExitBtn.Visible = true;

            gameMedia.controls.stop();
            StopTimers();
        }
        // stop timers / спиране на таймери
        private void StopTimers()
        {
            MoveBgTimer.Stop();
            MoveEnemiesTimer.Stop();
            MoveMunitionTimer.Stop();
            EnemiesMunitionTimer.Stop();
        }
        // start Timers / пускане на таймери
        private void StartTimers()
        {
            MoveBgTimer.Start();
            MoveEnemiesTimer.Start();
            MoveMunitionTimer.Start();
            EnemiesMunitionTimer.Start();
        }

        // движение на изтелите на протижниците
        private void EnemiesMunitionTimer_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < enemiesMun.Length- dificult; i++)
            {
                if (enemiesMun[i].Top < this.Height)
                {
                    enemiesMun[i].Visible = true;
                    enemiesMun[i].Top += enemyMunSpeed;

                    CollisionWithEnemyMun();
                }
                else
                {
                    enemiesMun[i].Visible=false;
                    int x = rnd.Next(0 ,10);
                    enemiesMun[i].Location=new Point(enemies[x].Location.X + 20, enemies[x].Location.Y + 30);
                }
            }
        }

        // сблъсък с изтрели на противници
        private void CollisionWithEnemyMun()
        {
            for (int i = 0; i < enemiesMun.Length; i++)
            {
                if (enemiesMun[i].Bounds.IntersectsWith(Player.Bounds))
                {
                    enemiesMun[i].Visible = false;
                    explosion.settings.volume = 15;
                    explosion.controls.play();
                    Player.Visible=false;
                    GameOver("Game Over");
                }
            }
        }

        // бутон за повторение на играта
        private void ReplayBtn_Click(object sender, EventArgs e)
        {
            this.Controls.Clear();
            InitializeComponent();
            Form1_Load(e, e);
        }

        // бутон за изход от играта
        private void ExitBtn_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
