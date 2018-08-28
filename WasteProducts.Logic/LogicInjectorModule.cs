using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using WasteProducts.Logic.Common.Services;
using WasteProducts.Logic.Services;
using WasteProducts.DataAccess;

namespace WasteProducts.Logic
{
    public class LogicInjectorModule : NinjectModule
    {
        public override void Load()
        {            
            Bind<ISearchService>().To<LuceneSearchService>();
        }
    }
}
