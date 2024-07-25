using Application.DTOs;
using Application.DTOs.QuotationDto;
using Domain.Entities;
using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application
{
    public static class ApplicationConfiguration
    {
        public static void AddMapster(this IServiceCollection services)
        {
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
            Assembly applicationAssembly = typeof(BaseDto<,>).Assembly;
            typeAdapterConfig.Scan(applicationAssembly);
            MappingConfig.Configure();
        }

        public static void AddApplicationRegitrations(this IServiceCollection services)
        {
            //services.AddScoped<ApiResponse<bool>>();
            services.AddScoped(typeof(ApiResponse<>));
        }
    }





    public static class MappingConfig
    {
        public static void Configure()
        {
            //src.HireDate.ToString("yyyy-MM-dd")

            TypeAdapterConfig<Quotation, QuotationResponse>.NewConfig()
                .Map(dest => dest.BookAuther, src => src.Book!=null? $"{src.Book.Auther}  ":"")
                .Map(dest => dest.BookTitle, src => src.Book != null ? $"{src.Book.Title}  " : "")
                //.Map(dest => dest.BookTitle, src =>" src.Book != null ? '")
                .Map(dest => dest.CommentsNumber, src => src.Comments != null ?  src.Comments.Count:0)
                .Map(dest => dest.SharesNumber, src => src.QuotationShares != null ?  src.QuotationShares.Count:0)
                .Map(dest => dest.LikesNumber, src => src.QuotationLikes != null ?  src.QuotationLikes.Count:0)
                .Map(dest => dest.ReQueteNumber, src => src.ReQuotes != null ?  src.ReQuotes.Count:0)
                ;
        }
    }

}
