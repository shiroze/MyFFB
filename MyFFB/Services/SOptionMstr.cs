using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SOptionMstr
    {
        static IContainer _container;

        static SOptionMstr()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<ROptionMstr>().As<IROptionMstr>();
            _container = builder.Build();
        }

        public static IROptionMstr OptionMstr(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IROptionMstr>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
