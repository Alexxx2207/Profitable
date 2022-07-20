﻿using AutoMapper;
using Profitable.Automapper.TypeConverters;
using Profitable.Models.EntityModels;
using Profitable.Models.InputModels.Posts;
using Profitable.Models.ViewModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profitable.Automapper
{
    public class PostsMapper : Profile
    {
        public PostsMapper()
        {
            CreateMap<AddPostInputModel, Post>();
            CreateMap<UpdatePostInputModel, Post>();
            CreateMap<Post, PostViewModel>()
                .ForMember(dest => dest.Author, opt =>
                    opt.MapFrom(source => $"{source.Author.FirstName} {source.Author.LastName}"))
                .ForMember(dest => dest.PostedOn, opt =>
                opt.MapFrom(source => source.PostedOn.ToString("f")))
                .ForMember(dest => dest.ImageType, opt =>
                    opt.MapFrom(source => (int) source.ImageType))
            .ForMember(dest => dest.Image, opt =>
                    opt.ConvertUsing(new ImageByteArrayConverter(), src => src.ImageURL));
        }
    }
}
