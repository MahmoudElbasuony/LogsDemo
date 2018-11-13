using System;
using System.Collections.Generic;
using System.Text;

namespace LogsDemo.Domain.Interfaces
{
    public interface ILogSystemUnitOfWork : IDisposable
    {
        ILogRepository LogRepository { get;  }

        IUserRepository UserRepository { get;  }

    }
}
