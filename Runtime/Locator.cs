using System;
using System.Collections.Generic;

namespace DCTools
{
    public static class Locator
    {
        private static Dictionary<Type, object> _services;

        public static void InitCoreServices()
        {
            _services = new Dictionary<Type, object>
            {
                { typeof(ITinyMessengerHub), new TinyMessengerHub() }
            };
        }

        public static void CleanUp() => _services.Clear();

        public static T Find<T>()
        {
            try
            {
                return (T)_services[typeof(T)];
            }
            catch
            {
                throw new ApplicationException("The requested service could not be found!");
            }
        }

        // Defined explicit helper methods for simplicity.
        public static ITinyMessengerHub EventHub => Find<ITinyMessengerHub>();
    }
}
