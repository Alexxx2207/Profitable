namespace Profitable.Common.Automapper.Profiles
{
    using AutoMapper;
    using Profitable.Models.EntityModels;
    using Profitable.Models.RequestModels.Organizations;

    public class OrganizationsMapper : Profile
    {
        public OrganizationsMapper()
        {
            CreateMap<UpdateOrganizationRequestModel, Organization>();
        }
    }
}
