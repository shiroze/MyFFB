using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SMenu
    {
        static IContainer _container;

        static SMenu()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RMenu>().As<IRMenu>();
            _container = builder.Build();
        }

        public static IRMenu Menu(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRMenu>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
