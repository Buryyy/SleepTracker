using Autofac;
using Realms;
using SleepTracker.Repositories;
using SleepTracker.Services;
using SleepTracker.ViewModels;
using SleepTracker.Views;
using Xamarin.Forms;

namespace SleepTracker
{
    public partial class App : Application
    {
        private readonly IContainer _container;

        public App()
        {
            InitializeComponent();
            Device.SetFlags(new string[] { "MediaElement_Experimental" });

            _container = BuildContainer();
            MainPage = _container.Resolve<MainView>();
        }

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();

            var config = new RealmConfiguration
            {
                // Generate a random 64 byte encryption key
                EncryptionKey = new byte[64]
            };
            var realm = Realm.GetInstance(config);

            //REPOS
            builder.Register(c => new RecordRepository(realm)).As<IRecordRepository>().SingleInstance();

            //SERVICES
            builder.Register(c => new AudioService(c.Resolve<IRecordRepository>())).As<IAudioService>().SingleInstance();

            //VIEWMODELS
            builder.Register(c => new MainViewModel(c.Resolve<IAudioService>())).SingleInstance();

            //VIEWS
            builder.Register(c => new MainView(c.Resolve<MainViewModel>()));

            return builder.Build();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}