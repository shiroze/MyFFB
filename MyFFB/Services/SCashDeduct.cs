using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SCashDeduct
    {
        static IContainer _container;

        static SCashDeduct()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RCashDeduct>().As<IRCashDeduct>();
            _container = builder.Build();
        }

        public static IRCashDeduct CashDeduct(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRCashDeduct>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
