using PRN231_FinalProject.Interface.Repositories;

namespace PRN231_Assignment2_eBookStoreAPI.Interface;

public interface IUnitOfWork
{
    IContactPhoneRepository ContactPhoneRepository { get; }
    IContactEmailRepository ContactEmailRepository { get; }
    IContactRepository ContactRepository { get; }
    IContactsLabelRepository ContactsLabelRepository { get; }
    IUserRepository UserRepository { get; }
    ILabelRepository LabelRepository { get; }
    int Complete();
}