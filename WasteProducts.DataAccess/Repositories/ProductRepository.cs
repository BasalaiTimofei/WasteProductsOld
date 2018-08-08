using System;
using System.Collections.Generic;
using System.Linq;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Contexts;
using WasteProducts.Logic.Common.Models.Product;

namespace WasteProducts.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly WasteContext _context;

        public ProductRepository(WasteContext context)
        {
            this._context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return this._context.Products.ToList();
        }

        public Product GetById(string id)
        {
            return this._context.Products.Find(id);
        }

        public void DeleteById(string id)
        {
            var product = this._context.Products.Find(id);
            if (product != null) this._context.Products.Remove(product);
        }

        public void Save()
        {
            this._context.SaveChanges();
        }

        public void Update(Product product)
        {
            throw new NotImplementedException();
        }

        public void Add(Product product)
        {
            this._context.Products.Add(product);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    this._context.Dispose();
                }

                this._disposed = true;
            }
        }
    }
}
