using AutoMapper;
using BookStore.API.Data;
using BookStore.API.Models;

namespace BookStore.API.Helpers
{
    public class ApplicationMapper : Profile
    {
        public ApplicationMapper()
        {
            //maps the data of both classes to each other
            CreateMap<Book, BookModel>().ReverseMap();
        }
    }
}
