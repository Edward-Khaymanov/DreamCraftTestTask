using System.Threading;

namespace Project.Utils
{
    public static class CancellationTokenUtils
    {
        public static void Destroy(ref CancellationTokenSource source)
        {
            if (source == null)
                return;

            source.Cancel();
            source.Dispose();
            source = null;
        }
    }
}