using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SRole
    {
        static IContainer _container;

        static SRole()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RRole>().As<IRRole>();
            _container = builder.Build();
        }

        public static IRRole Role(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRRole>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
