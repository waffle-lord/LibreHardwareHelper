using System.Text;

namespace Sandbox.Helpers
{
    public class MemoryHelper
    {
        private float _total;
        private float _used;
        private int _percentageUsed;

        public string GetMemoryInfo() => $"({_used.ToString("0.0")} Gb/{_total.ToString("0.0")} Gb)";

        public string GetMemoryProgressBar()
        {
            // only printing 50 columns, so halving this for the progress bar.
            var fixedSizeUsedPerc = (int)Math.Floor((double)_percentageUsed / 2);
            var bar = new string(' ', 50).Remove(0, fixedSizeUsedPerc)
                                          .Insert(0, new string('=', fixedSizeUsedPerc));

            return new StringBuilder().Append('[')
                                      .Append(bar)
                                      .Append(']')
                                      .ToString();
        }

        public MemoryHelper(float totalMemMb, float usedMemMb)
        {
            _total = totalMemMb;
            _used = usedMemMb;

            _percentageUsed = (int)Math.Floor(_used / _total * 100);
        }
    }
}
