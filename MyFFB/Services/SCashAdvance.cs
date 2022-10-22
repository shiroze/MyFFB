using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SCashAdvance
    {
        static IContainer _container;

        static SCashAdvance()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RCashAdvance>().As<IRCashAdvance>();
            _container = builder.Build();
        }

        public static IRCashAdvance CashAdvance(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRCashAdvance>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
