using Shoot_Out_Game_MOO_ICT.Properties;
using System.IO.Packaging;
using System.Numerics;

namespace Shoot_Out_Game_MOO_ICT
{
    public partial class Form1 : Form
    {

        bool goLeft, goRight, goUp, goDown, gameOver, punchR;
        string facing = "up";
        int playerHealth = 100;
        int speed = 10;
        int ammo = 10;
        int zombieSpeed = 3;
        Random randNum = new Random();
        int score;
        int currentBackground = 0;
        List<PictureBox> zombiesList = new List<PictureBox>();

        public Form1()
        {
            InitializeComponent();
            RestartGame();
        }

        private void MainTimerEvent(object sender, EventArgs e)
        {
            if (playerHealth > 1)
            {
                healthBar.Value = playerHealth;
            }
            else
            {
                gameOver = true;
                player.Image = Properties.Resources.dead;
                GameTimer.Stop();
            }

            txtAmmo.Text = "Ammo: " + ammo;
            txtScore.Text = "Kills: " + score;

            if (goLeft == true && player.Left > 0)
            {
                player.Left -= speed;
            }
            if (goRight == true && player.Left + player.Width < this.ClientSize.Width)
            {
                player.Left += speed;
            }
            if (goUp == true && player.Top > 45)
            {
                player.Top -= speed;
            }
            if (goDown == true && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += speed;
            }

            ChangeBackgroundBasedOnScore();

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "ammo")
                {
                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        this.Controls.Remove(x);
                        ((PictureBox)x).Dispose();
                        ammo += 5;

                    }
                }


                if (x is PictureBox && (string)x.Tag == "zombie")
                {

                    if (player.Bounds.IntersectsWith(x.Bounds))
                    {
                        if (punchR == true)
                        {
                            score++;

                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            zombiesList.Remove(((PictureBox)x));
                            MakeZombies();
                        }
                        else
                        {
                            playerHealth -= 2;
                            x.Left -= zombieSpeed;
                            x.Top -= zombieSpeed;
                        }

                    }
                  
                    if (x.Left > player.Left)
                    {
                        x.Left -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zleft;
                    }
                    if (x.Left < player.Left)
                    {
                        x.Left += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zright;
                    }
                    if (x.Top > player.Top)
                    {
                        x.Top -= zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zup;
                    }
                    if (x.Top < player.Top)
                    {
                        x.Top += zombieSpeed;
                        ((PictureBox)x).Image = Properties.Resources.zdown;
                    }

                    
                }



                foreach (Control j in this.Controls)
                {
                    if (j is PictureBox && (string)j.Tag == "bullet" && x is PictureBox && (string)x.Tag == "zombie")
                    {
                        if (x.Bounds.IntersectsWith(j.Bounds))
                        {
                            score++;

                            this.Controls.Remove(j);
                            ((PictureBox)j).Dispose();
                            this.Controls.Remove(x);
                            ((PictureBox)x).Dispose();
                            zombiesList.Remove(((PictureBox)x));
                            MakeZombies();
                        }
                    }
                }

               
            }


        }
        private void ChangeBackgroundBasedOnScore()
        {
            if (score >= 20 && score < 40 && currentBackground != 1)
            {
                this.BackgroundImage = Properties.Resources.background1; // Cenário 1
                currentBackground = 1;
            }
            else if (score >= 40 && score < 60 && currentBackground != 2)
            {
                this.BackgroundImage = Properties.Resources.background2; // Cenário 2
                currentBackground = 2;
            }
            else if (score >= 60 && score < 80 && currentBackground != 3)
            {
                this.BackgroundImage = Properties.Resources.background3; // Cenário 3
                currentBackground = 3;
            }
            else if (score >= 80 && currentBackground != 4)
            {
                this.BackgroundImage = Properties.Resources.background4; // Cenário 4
                currentBackground = 4;
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {

            if (gameOver == true)
            {
                return;
            }

            if (e.KeyCode == Keys.Left)
            {
                if (score >= 10)
                {
                    player.Image = Properties.Resources.peashooterL;
                }
                else
                {
                    player.Image = Properties.Resources.left;
                }
                goLeft = true;
                facing = "left";
            }

            if (e.KeyCode == Keys.Right)
            {
                if (score > 10)
                {
                    player.Image = Properties.Resources.peashooterR;
                }
                else
                {
                    player.Image = Properties.Resources.right;
                }
                goRight = true;
                facing = "right";
            }

            if (e.KeyCode == Keys.Up)
            {
                if (score > 10)
                {
                    player.Image = Properties.Resources.peashooterT;
                }
                else
                {
                    player.Image = Properties.Resources.up;
                }
                goUp = true;
                facing = "up";
            }

            if (e.KeyCode == Keys.Down)
            {
                if (score > 10)
                {
                    player.Image = Properties.Resources.peashooterD;
                }
                else
                {
                    player.Image = Properties.Resources.down;
                }
                goDown = true;
                facing = "down";
            }

            if(e.KeyCode == Keys.R)
            {
                punchR = true;
            }

        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }

            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }

            if (e.KeyCode == Keys.Up)
            {
                goUp = false;
            }

            if (e.KeyCode == Keys.Down)
            {
                goDown = false;
            }
            
            if(e.KeyCode == Keys.R)
            {
                punchR = false;
            }

            if (e.KeyCode == Keys.Space && ammo > 0 && gameOver == false)
            {
                ShootBullet(facing);
                ammo--;


                if (ammo < 1)
                {
                    DropAmmo();
                }
            }

            if (e.KeyCode == Keys.Enter && gameOver == true)
            {
                RestartGame();
            }


        }

        private void ShootBullet(string direction)
        {
            Bullet shootBullet = new Bullet();
            shootBullet.direction = direction;
            shootBullet.bulletLeft = player.Left + (player.Width / 2);
            shootBullet.bulletTop = player.Top + (player.Height / 2);
            shootBullet.MakeBullet(this);
        }

        private void MakeZombies()
        {
            PictureBox zombie = new PictureBox();
            zombie.Tag = "zombie";
            zombie.Image = Properties.Resources.zdown;
            zombie.Left = randNum.Next(0, 900);
            zombie.Top = randNum.Next(0, 800);
            zombie.SizeMode = PictureBoxSizeMode.AutoSize;
            zombiesList.Add(zombie);
            this.Controls.Add(zombie);
            player.BringToFront();

        }

        private void DropAmmo()
        {

            PictureBox ammo = new PictureBox();
            ammo.Image = Properties.Resources.ammo_Image;
            ammo.SizeMode = PictureBoxSizeMode.AutoSize;
            ammo.Left = randNum.Next(10, this.ClientSize.Width - ammo.Width);
            ammo.Top = randNum.Next(60, this.ClientSize.Height - ammo.Height);
            ammo.Tag = "ammo";
            this.Controls.Add(ammo);

            ammo.BringToFront();
            player.BringToFront();



        }

        private void RestartGame()
        {
            player.Image = Properties.Resources.up;

            foreach (PictureBox i in zombiesList)
            {
                this.Controls.Remove(i);
            }

            zombiesList.Clear();

            for (int i = 0; i < 3; i++)
            {
                MakeZombies();
            }

            goUp = false;
            goDown = false;
            goLeft = false;
            goRight = false;
            gameOver = false;

            playerHealth = 100;
            score = 0;
            ammo = 10;

            GameTimer.Start();
        }

        private void player_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
