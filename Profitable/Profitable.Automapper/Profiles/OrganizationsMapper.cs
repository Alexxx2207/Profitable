namespace Profitable.Common.Automapper.Profiles
{
    using AutoMapper;
    using Profitable.Models.EntityModels;
    using Profitable.Models.RequestModels.Organizations;
    using Profitable.Models.ResponseModels.Organizations;

    public class OrganizationsMapper : Profile
    {
        public OrganizationsMapper()
        {
            CreateMap<Organization, OrganizationResponseModel>();
            CreateMap<UpdateOrganizationRequestModel, Organization>();
            CreateMap<AddMessageRequestModel, OrganizationMessage>();
            CreateMap<OrganizationMessage, OrganizationMessageResponseModel>()
                .ForMember(
                dest => dest.SenderId,
                opt => opt.MapFrom(src => src.SenderId))
                .ForMember(
                dest => dest.Sender,
                opt => opt.MapFrom(src => $"{src.Sender.FirstName} {src.Sender.LastName}"))
                .ForMember(
                dest => dest.Content,
                opt => opt.MapFrom(src => src.Message))
                .ForMember(
                dest => dest.SentOn,
                opt => opt.MapFrom(src => src.SentOn.ToString("f")));
        }
    }
}
