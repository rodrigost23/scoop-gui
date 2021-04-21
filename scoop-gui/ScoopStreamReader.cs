using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScoopGui
{
    class ScoopStreamReader
    {
        private static readonly CancellationTokenSource tokenSource = new();

        public static void Read(Action<string> callback)
        {
            CancellationToken cancellationToken = tokenSource.Token;

            _ = Task.Run(() =>
              {
                  StreamReader reader = new(Scoop.stream);

                  while (true)
                  {
                      if (cancellationToken.IsCancellationRequested)
                      {
                          break;
                      }

                      string line = reader.ReadLine();

                      if (line != null)
                      {
                          callback(line);
                      }
                  }

                  reader.Close();
              });
        }

        public static void Stop()
        {
            tokenSource.Cancel();
        }

    }
}
