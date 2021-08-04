using ObservableComputations;
using ScoopGui.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace ScoopGui
{
    public sealed class State
    {
        public AppsListClass AppsList { get; } = new AppsListClass();

        public ObservableCollection<ScoopBucket> BucketsList { get; } = new();

        public ObservableCollection<ScoopBucket> BucketsKnown { get; } = new();

        private State()
        {
        }

        public static State Instance { get; } = new State();

        public class AppsListClass
        {

            private ObservableCollection<ScoopApp> appsList = new();

            public ObservableCollection<ScoopApp> All => appsList;

            private readonly OcConsumer consumer = new();

            public Filtering<ScoopApp> Installed => appsList.Filtering(app => app.IsInstalled ?? false).For(consumer);

            public Filtering<ScoopApp> InstalledNotUpdatable => appsList.Filtering(app => (app.IsInstalled ?? false) && !(app.IsUpdatable ?? false)).For(consumer);

            public Filtering<ScoopApp> Updatable => appsList.Filtering(app => app.IsUpdatable ?? false).For(consumer);

            public void AddOrUpdate(ScoopApp app)
            {
                int index = appsList.ToList().FindIndex(a => a.Name == app.Name);

                if (index > -1)
                {
                    appsList[index] = app;
                }
                else
                {
                    index = appsList.ToList().FindIndex(a => string.Compare(a.Name, app.Name, StringComparison.CurrentCultureIgnoreCase) > 0);

                    if (index > 0)
                    {
                        appsList.Insert(index, app);
                    }
                    else
                    {
                        appsList.Add(app);
                    }
                }
            }
        }
    }

}
