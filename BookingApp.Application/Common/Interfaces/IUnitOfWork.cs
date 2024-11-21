using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IPropertyRepository Property { get; }
        void Save();
    }
}
