using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            player = new Player(pbPlayer, 8);
        }

        private Player player;
        private int spawnCount = 20;
        private List<Enemy> enemies = new List<Enemy>();
        private Random rnd = new Random();

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                player.left = true;
            if (e.KeyCode == Keys.D)
                player.right = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
                player.left = false;
            if (e.KeyCode == Keys.D)
                player.right = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            player.Move();
            SpawnEnemy();

            MoveEnemies();

            Collisions();
        }

        private void MoveEnemies()
        {
            List<Enemy> toRemove = new List<Enemy>();

            foreach (var enemy in enemies)
            {
                if (enemy.Move() > Height)
                {
                    toRemove.Add(enemy);
                }
            }
            


            foreach (var enemy in toRemove)
            {
                Controls.Remove(enemy.Pb);
                enemies.Remove(enemy);
            }
        }

        private void SpawnEnemy()
        {
            --spawnCount;

            if (spawnCount > 0)
                return;

            spawnCount = 20;

            const int offset = 30;
            const int enemySize = 10;

            int x = rnd.Next(offset, Width - offset - enemySize);

            enemies.Add(new Enemy(rnd.Next(4, 10), x, enemySize));

            Controls.Add(enemies[enemies.Count - 1].Pb);

            
        }

        private void Collisions()
        {
            foreach (var enemy in enemies)
            {
                if (enemy.Pb.Bounds.IntersectsWith(player.Pb.Bounds))
                {
                    Gameover();
                }
            }
        }

        private void Gameover()
        {
            timer1.Enabled = false;
            MessageBox.Show("Gameover");
        }
    }

    public class Player
    {
        public bool left = false;
        public bool right = false;

        private PictureBox pb;
        private readonly int speed;

        public Player(PictureBox pb, int speed)
        {
            this.pb = pb;
            this.speed = speed;
        }

        public PictureBox Pb { get => pb; }

        public void Move()
        {
            Point location = Pb.Location;

            if (left)
                location.X -= speed;
            if (right)
                location.X += speed;

            Pb.Location = location;
        }
    }

    public class Enemy
    {
        private PictureBox pb;
        private int speed;

        public Enemy(int speed, int x, int size)
        {
            pb = new PictureBox();
            pb.Location = new Point(x, 0);
            pb.Size = new Size(size, size);
            pb.BackColor = Color.FromArgb(255, 0, 0);

            this.speed = speed;
        }

        public PictureBox Pb
        {
            get => pb;
        }

        public int Move()
        {
            Point location = pb.Location;

            location.Y += speed;
            

            pb.Location = location;

            return location.Y;
        }

    }

}
