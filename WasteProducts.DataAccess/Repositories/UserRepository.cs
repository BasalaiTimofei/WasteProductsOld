using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WasteProducts.DataAccess.Common.Models.Users;
using WasteProducts.DataAccess.Common.Repositories;
using WasteProducts.DataAccess.Contexts;

namespace WasteProducts.DataAccess.Repositories
{
    class UserRepository : IUserRepository
    {
        public void Add(UserDB user)
        {
            if (user.Id != default(int))
                throw new ArgumentException("Cannot Add User with id which is not 0."); 

            using (var db = new WasteContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public void Delete(UserDB user)
        {
            if (user.Id != default(int))
            {
                using (var db = new WasteContext())
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                }
            }
            else
            {
                throw new ArgumentException("Cannot delete User without ID.");
            }
        }

        public UserDB Select(string email, string password)
        {
            using (var db = new WasteContext())
            {
                var result = db.Users.Where(user => user.Email == email && user.Password == password);
                if (result.Count() > 0)
                {
                    return result.First();
                }
                else
                {
                    return null;
                }
            }
        }

        public IEnumerable<UserDB> SelectAll()
        {
            using (var db = new WasteContext())
            {
                return db.Users.ToList();
            }
        }

        public IEnumerable<UserDB> SelectWhere(Predicate<UserDB> predicate)
        {
            Func<UserDB, bool> condition = new Func<UserDB, bool>(predicate);

            using (var db = new WasteContext())
            {
                return db.Users.Where(condition);
            }
        }

        public void Update(UserDB user)
        {

            using (var db = new WasteContext())
            {
                db.Database.Log = (s => System.Diagnostics.Debug.WriteLine(s));

                db.Entry<UserDB>(user).State = EntityState.Modified;

                db.Users.Where(u => u.Id == user.Id).First().Modified = DateTime.UtcNow;
                db.SaveChanges();
        }
    }
}
