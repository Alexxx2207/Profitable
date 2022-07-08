using AutoMapper;
using Profitable.Models;
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
            CreateMap<Post, PostViewModel>();
        }
    }
}
