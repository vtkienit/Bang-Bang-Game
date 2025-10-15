namespace BangBang
{
    public class Counter
    {
        private long _count;
        private string _name;

        public Counter(string name)
        {
            _name = name;
        }

        public void Increment()
        {
            _count++;
        }

        public void Reset()
        {
            _count = 0;
        }

        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public long Ticks
        {
            get { return _count; }
        }
    }
}
