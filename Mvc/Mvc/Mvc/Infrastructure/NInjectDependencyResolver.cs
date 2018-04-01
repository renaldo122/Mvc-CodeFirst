// <copyright file="NInjectDependencyResolver.cs">
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>""</author>
// <date>01/24/2018</date>
// <summary>Create dependency and binding with IEntiryRepository and EntiryRepository</summary>

namespace Mvc.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Ninject;
    using System.Linq;
    using System.Web;
    using Mvc.Domain.Abstract;
    using Mvc.Domain.Concrete;
    using System.Web.Mvc;

    public class NInjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NInjectDependencyResolver()
        {
            kernel = new StandardKernel();
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        public void AddBindings()
        {
            kernel.Bind<IEntitiesRepository>().To<EntitiesRepository>();
        }
    }
}