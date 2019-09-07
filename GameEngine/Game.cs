using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GameEngine.Input;
namespace GameEngine
{
    public abstract partial class Game : Form
    {
        public abstract void Render(Graphics graphics);
        public abstract void UpdateLogic(double elapsedTime);
        public abstract void UpdateTime(double elapsedTime);
        public Game()
        {
            InitializeComponent();

            FrameTimer = new Timer() { Interval = 1 };
            FrameTimer.Tick += OnFrameUpdate;
            Watch = new System.Diagnostics.Stopwatch();
            GameTime = new TimeSpan();

            MouseInput = new Mouse();
            KeyboardInput = new Keyboard();
        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Watch.Stop();
            FrameTimer.Dispose();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FrameTimer.Start();
            Watch.Start();
        }
        protected override void OnPaintBackground(PaintEventArgs e) { }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Render(e.Graphics);
        }

        private void OnFrameUpdate(object sender, EventArgs e)
        {
            double beginUpdateTime = Watch.Elapsed.TotalSeconds;

            double elapsedTime;
            do
            {
                // Process Input
                MouseInput.ReadInput();
                KeyboardInput.ReadInput();
                // Update Logic
                UpdateLogic(m_lastElapsed);
                // Update GameTime
                UpdateTime(m_lastElapsed);

                elapsedTime = Watch.Elapsed.TotalSeconds - beginUpdateTime;

            } while (1.0 / elapsedTime > m_MaxFPS);

            // Update frame
            Refresh();

            m_lastElapsed = elapsedTime;
            //this.Text = string.Format($"{(int)(1.0 / elapsedTime)}");
        }


        public TimeSpan GameTime { get; set; }
        public TimeSpan Elapsed { get => Watch.Elapsed; }
        public long ElapsedTicks { get => Watch.ElapsedTicks; }
        public double FPS
        {
            get => 1.0 / (m_lastElapsed + 0.001);
            set => m_MaxFPS = value;
        }

        private Timer FrameTimer { get; set; }
        private System.Diagnostics.Stopwatch Watch { get; set; }
        private double m_lastElapsed;
        private double m_MaxFPS = int.MaxValue;
    }
}
