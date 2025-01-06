using Microsoft.EntityFrameworkCore.Storage;
using PRN231_Assignment2_eBookStoreAPI.Interface;
using PRN231_Assignment2_eBookStoreAPI.Repositories;
using PRN231_FinalProject.Interface.Repositories;
using PRN231_FinalProject.Models;
using PRN231_FinalProject.Repositories;

namespace PRN231_Assignment2_eBookStoreAPI.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly PRN231_FinalProjectContext _dbContext;

    public UnitOfWork(PRN231_FinalProjectContext dbContext)
    {
        _dbContext = dbContext;
        ContactRepository = new ContactRepository(_dbContext);
        UserRepository  = new UserRepository(_dbContext);
        ContactEmailRepository = new ContactEmailRepository(_dbContext);
        ContactPhoneRepository = new ContactPhoneRepository(_dbContext);
        ContactsLabelRepository = new ContactsLabelRepository(_dbContext);
        LabelRepository = new LabelRepository(_dbContext);
    }
    public ILabelRepository LabelRepository { get; }
    public IContactEmailRepository ContactEmailRepository { get; }
    public IContactPhoneRepository ContactPhoneRepository { get; }
    public IContactsLabelRepository ContactsLabelRepository { get; }
    public IContactRepository ContactRepository { get; }
    public IUserRepository UserRepository { get; }

    public int Complete()
    {
        return _dbContext.SaveChanges();
    }
    public IDbContextTransaction BeginTransaction()
    {
        return _dbContext.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}