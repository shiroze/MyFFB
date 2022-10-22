using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SRoleDet
    {
        static IContainer _container;

        static SRoleDet()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RRoleDet>().As<IRRoleDet>();
            _container = builder.Build();
        }

        public static IRRoleDet RoleDet(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRRoleDet>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
