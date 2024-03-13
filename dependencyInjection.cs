using System.Data;
using System.Data.SqlClient;
using Autofac;
using code.business.impl;
using code.business.service;
using code.common;
using code.interfaces;
using code.Interfaces;
using code.providers.sqlServer;
using code.queries;
using code.queries.sqlServer;
using code.repositories.impl;
using code.repositories.services;
using Microsoft.Extensions.Options;

namespace code;

public sealed class DependencyInjection<TC, TI> : Module
where TI : struct, IEquatable<TI>
        where TC : struct
{
    protected override void Load(ContainerBuilder builder)
    {
        base.Load(builder);
        builder.Register(c => new SqlConnection(c.Resolve<IOptions<ConnectionString>>().Value.RelationalDBConn))
            .InstancePerLifetimeScope()
            .As<IDbConnection>();

        #region "Low level DAL Infrastructure"
        builder.Register(c => new clsConcurrency<TC>())
            .SingleInstance()
            .As<IDBConcurrencyHandler<TC>>();
        builder.Register(c => new RelationalContext<TC>(c.Resolve<IDbConnection>(),
                                                        c.Resolve<ILogger<RelationalContext<TC>>>(),
                                                        c.Resolve<IDBConcurrencyHandler<TC>>()))
                .InstancePerLifetimeScope()
                .As<IRelationalContext<TC>>();
        #endregion

        #region "Queries"
        builder.Register(c => new qDoctor())
          .SingleInstance()
          .As<IQDoctor>();
          builder.Register(c => new qPaciente())
          .SingleInstance()
          .As<IQPaciente>();
        #endregion

        #region "Repositories"
        builder.Register(c => new DoctorRepository<TI, TC>(c.Resolve<IRelationalContext<TC>>(),
                                                           c.Resolve<IQDoctor>(),
                                                           c.Resolve<ILogger<DoctorRepository<TI, TC>>>()))
               .InstancePerDependency()
               .As<IDoctorRepository<TI, TC>>();
        builder.Register(c => new PacienteRepository<TI, TC>(c.Resolve<IRelationalContext<TC>>(),
                                                           c.Resolve<IQPaciente>(),
                                                           c.Resolve<ILogger<PacienteRepository<TI, TC>>>()))
               .InstancePerDependency()
               .As<IPacienteRepository<TI, TC>>();
        #endregion

        #region "Entity Factories"
        builder.Register<Func<IDoctorRepository<TI, TC>>>(delegate (IComponentContext context)
        {
            IComponentContext cc = context.Resolve<IComponentContext>();
            return cc.Resolve<IDoctorRepository<TI, TC>>;
        });
        builder.Register<Func<IPacienteRepository<TI, TC>>>(delegate (IComponentContext context)
        {
            IComponentContext cc = context.Resolve<IComponentContext>();
            return cc.Resolve<IPacienteRepository<TI, TC>>;
        });
        #endregion

        #region "Business classes"
        builder.Register(c => new DoctorBusiness<TI, TC>(c.Resolve<IDoctorRepository<TI, TC>>()))
               .InstancePerDependency()
               .As<IDoctorBusiness<TI>>();
        builder.Register(c => new PacienteBusiness<TI, TC>(c.Resolve<IPacienteRepository<TI, TC>>()))
               .InstancePerDependency()
               .As<IPacienteBusiness<TI>>();
        #endregion
    }
}