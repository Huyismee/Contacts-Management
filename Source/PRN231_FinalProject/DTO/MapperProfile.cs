using AutoMapper;
using PRN231_FinalProject.Models;

namespace PRN231_FinalProject.DTO;

public class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<Contact, ContactDTO>()
            .ForMember(e => e.ContactsLabels, opt => opt.MapFrom(src => src.ContactsLabels))
            .ReverseMap();
        CreateMap<Label, LabelDTO>().ReverseMap();
        CreateMap<ContactsLabel, ContactsLabelDTO>().ReverseMap();
    }
}