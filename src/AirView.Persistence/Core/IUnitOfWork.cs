using System;

namespace AirView.Persistence.Core
{
    public interface IUnitOfWork :
        IDisposable
    {
        void Commit();
    }
}