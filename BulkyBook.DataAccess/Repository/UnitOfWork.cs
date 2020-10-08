using BulkyBook.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            ISP_Call = new SP_Call(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public ISP_Call ISP_Call { get; private set; }

        public ISP_Call SP_Call => throw new NotImplementedException();

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
