using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ScoopGui
{
    class ScoopStreamReader
    {
        private static readonly CancellationTokenSource tokenSource = new CancellationTokenSource();

        public static void Read(Action<string> callback)
        {
            var cancellationToken = tokenSource.Token;

            Task.Run(() =>
            {
                var reader = new StreamReader(Scoop.stream);

                while (true)
                {
                    if (cancellationToken.IsCancellationRequested)
                        break;

                    var line = reader.ReadLine();

                    if (line != null)
                    {
                        callback(line);
                    }
                }

                reader.Close();
            });
        }

        public static void Stop() {
            tokenSource.Cancel();
        }

    }
}
