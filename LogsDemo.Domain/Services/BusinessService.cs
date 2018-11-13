using LogsDemo.Domain.Entities;
using LogsDemo.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Domain.Services
{
    public abstract class BusinessService<T> : IBusinessService<T> where T : class
    {
        protected readonly ILogSystemUnitOfWork logSystemUnitOfWork;

        public BusinessService(ILogSystemUnitOfWork logSystemUnitOfWork)
        {
            this.logSystemUnitOfWork = logSystemUnitOfWork;
        }

        public void Dispose()
        {

        }
    }
}
