using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersStore.Dal.Abstract;
using UsersStore.Dal.EF;
using UsersStore.Dal.Entities;

namespace UsersStore.Dal.Concrete
{
    public class UsersRepository : IUsersRepository
    {
        private UsersStoreContext _context;

        public UsersRepository(UsersStoreContext context)
        {
            this._context = context;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            Save();
        }

        public void Delete(int id)
        {
            User u = _context.Users.Find(id);
            if (u != null)
                _context.Users.Remove(u);
        }

        public IEnumerable<User> Get() => _context.Users;

        public IEnumerable<User> Find(Func<User, bool> predicate) => _context.Users.Where(predicate).ToList();

        public User GetById(int id) => _context.Users.Find(id);

        public void Update(User user) {
            _context.Entry(user).State = EntityState.Modified;
            Save();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
