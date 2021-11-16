using Schaeffler.Persistence.Implementation;
using Schaeffler.Persistence.Interface;
using Schaeffler.Service.Implementations;
using Schaeffler.Service.Interfaces;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace Schaeffler.Web
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IFeedbackDao, FeedbackDao>();
            container.RegisterType<IUtilityService, UtilityService>();
            container.RegisterType<IUserAccountDao, UserAccountDao>();
            container.RegisterType<IUserDao, UserDao>();
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}