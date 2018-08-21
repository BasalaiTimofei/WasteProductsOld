using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models;
using WasteProducts.DataAccess.Common.Models.Security.Infrastructure;

namespace WasteProducts.DataAccess.Common.Repositories.Security
{
    public interface IRepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
    {
       
    }
}