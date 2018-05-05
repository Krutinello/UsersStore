using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersStore.Dal.Entities;

namespace UsersStore.Dal.Abstract
{
    public interface IUsersRepository
    {
        IEnumerable<User> Get();

        User GetById(int id);

        void Add(User user);

        IEnumerable<User> Find(Func<User, bool> predicate);

        void Update(User person);

        void Delete(int id);

        void Save();

        Task SaveAsync();
    }
}
