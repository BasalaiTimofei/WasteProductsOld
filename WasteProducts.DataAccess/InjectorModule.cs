using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using WasteProducts.DataAccess.Common.Repositories.Search;
using WasteProducts.DataAccess.Repositories;

namespace WasteProducts.DataAccess
{
    public class InjectorModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<ISearchRepository>().To<LuceneSearchRepository>();
            //throw new NotImplementedException();
        }
    }
}
