using Autofac;
using BabySmash.Core.ViewModels;

namespace BabySmash.Core
{
	public class Bootstrapper
	{
		public static void Run(ContainerBuilder builder)
		{
			builder.RegisterType<MainViewModel>();
			builder.RegisterType<SettingsViewModel>();
			var container = builder.Build();
			App.Container = container;
		}
	}
}
