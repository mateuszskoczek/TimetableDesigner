using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimetableDesigner.Services
{
    public class ServiceProvider
    {
        #region FIELDS

        private Dictionary<Type, IService> _services;

        #endregion



        #region CONSTRUCTORS

        public static readonly ServiceProvider Instance = new ServiceProvider();

        private ServiceProvider() 
        {
            _services = new Dictionary<Type, IService>();
        }

        #endregion



        #region PUBLIC METHODS

        public void AddService<T>(T service) where T : IService
        {
            _services[typeof(T)] = service;
        }

        public T GetService<T>() where T : IService
        {
            return (T)_services[typeof(T)];
        }

        #endregion
    }
}
