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
                throw new ArgumentException("Cannot Add User with Id different from 0.");

            user.Created = DateTime.UtcNow;
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
                    var result = db.Users
                        .Where(f => f.Id == user.Id)
                        .FirstOrDefault();

                    if (result != null)
                    {
                        db.Users.Remove(result);
                        db.SaveChanges();
                    }
                    else
                    {
                        throw new ArgumentException($"The User cannot be deleted because User with Id = {user.Id} doesn't exist.");
                    }
                }
            }
            else
            {
                throw new ArgumentException("Cannot delete User with Id = 0.");
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
                    throw new ArgumentException("User with specified Email and Password isn't found.");
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
                var userInDB = db.Users.Find(user.Id);
                db.Entry(userInDB).CurrentValues.SetValues(user);
                userInDB.Modified = DateTime.UtcNow;
                db.SaveChanges();
        }
    }
}
