using Autofac;
using MyAttd.DataAccess;

namespace MyAttd.Services
{
    public class SSupplier
    {
        static IContainer _container;

        static SSupplier()
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterType<RSupplier>().As<IRSupplier>();
            _container = builder.Build();
        }

        public static IRSupplier Supplier(string ConStr, string PhaseKey)
        {
            return _container.Resolve<IRSupplier>(new[] { new NamedParameter("constr", ConStr), new NamedParameter("phasekey", PhaseKey) });
        }
    }
}
