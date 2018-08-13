using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using WasteProducts.DataAccess.Common.Models.Product;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories
{/// <summary>
///This class is a context class. A binder for the 'ProductDB' class with a data access.
/// </summary>
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// A method that provides a list of all products.
        /// </summary>
        /// <returns>List of products.</returns>
        public IQueryable<ProductDB> GetAll()
        {
            using (var db = new WasteContext())
            {
                return db.Products;
            }
        }

        /// <summary>
        /// A method that selectively provides a product by product's ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Product by ID.</returns>
        public ProductDB GetById(string id)
        {
            using (var db = new WasteContext())
            {
                return db.Products.Find(id);
            }
        }

        /// <summary>
        /// The method that delets the product by ID.
        /// </summary>
        /// <param name="id"></param>
        public void DeleteById(string id)
        {
            using (var db = new WasteContext())
            {
                var product = db.Products.Find(id);
                if (product != null) db.Products.Remove(product);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// This method allows you to modify some or all of the product values.
        /// </summary>
        /// <param name="product"></param>
        public void Update(ProductDB product)
        {
            using (var db = new WasteContext())
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        /// <summary>
        /// Allows you to add new product to the products colletion.
        /// </summary>
        /// <param name="product"></param>
        public void Add(ProductDB product)
        {
            using (var db = new WasteContext())
            {
                db.Products.Add(product);
                db.SaveChanges();
            }
        }
    }
}
