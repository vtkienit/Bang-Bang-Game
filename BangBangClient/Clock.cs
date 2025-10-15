using SplashKitSDK;

namespace BangBang
{
    public class Clock
    {
        private const int FPS = 60;
        private Counter _minutes;
        private Counter _seconds;
        private int count = 0;

        public Clock()
        {
            _minutes = new Counter("minutes");
            _seconds = new Counter("seconds");
        }

        public void Draw(int fontsize, int x, int y)
        {
            string time;
            time = string.Format("{0:00}:{1:00}", Minutes, Seconds);
            SplashKit.DrawText(time, Color.White, "RussoOne", fontsize, x - TextWidth(time, "RussoOne", fontsize) / 2, y);
        }

        public void Update()
        {
            count++;
            if (count % FPS == 0)
            {
                _seconds.Increment();
                count = 0;
            }

            if (_seconds.Ticks >= 60)
            {
                _seconds.Reset();
                _minutes.Increment();
            }
        }

        public void Reset()
        {
            _minutes.Reset();
            _seconds.Reset();
        }

        public float TextWidth(string letters, string font, int size_font)
        {
            return SplashKit.TextWidth(letters, font, size_font);
        }

        public long Minutes
        {
            get { return _minutes.Ticks; }
        }

        public long Seconds
        {
            get { return _seconds.Ticks; }
        }
    }
}
