using AutoMapper;
using Sample.Admin.Model;
using DataModel = Sample.Admin.Service.Infrastructure.DataModels;
using DomainModel = Sample.Admin.Model.Account.Domain;

namespace Sample.Admin.Service.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            #region[Account Create Response]
            CreateMap<DataModel.Accounts, DomainModel.Account>();
            CreateMap<DataModel.Subscriptions, DomainModel.Subscription>();
            CreateMap<DataModel.Modules, ModulesModel>();
            CreateMap<ModulesModel, DataModel.Modules>();
            CreateMap<DataModel.VersionModules, VersionModulesModel>();
            CreateMap<VersionModulesModel, DataModel.VersionModules>();
            CreateMap<DataModel.Accounts, AccountsModel>();
            CreateMap<AccountsModel, DataModel.Accounts>();
            CreateMap<DataModel.Accounts, AccountsCreateVM>();
            CreateMap<DataModel.AccountServices, AccountServicesModel>().ReverseMap();
            CreateMap<SubscriptionsModel, DataModel.Subscriptions>()
                .ForMember(dest => dest.VersionId, opt => opt.MapFrom(src => src.Version))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.Account))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.Accounts));
            CreateMap<DataModel.Subscriptions, SubscriptionsVM>()
                .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.VersionId))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.AccountId));

            CreateMap<DataModel.AuthenticationConfigKeys, AuthenticationConfigKeyModel>();
            CreateMap<DataModel.AdminUsers, AdminUsersModel>().ReverseMap();

            // Code commented after from master removed from instance mangement

            //CreateMap<ScreenControlsViewModel, ScreenControlsVM>();
            //CreateMap<ScreenControlsViewModel, Fields>();
            //CreateMap<ControlsData, FieldProperties>();
            //CreateMap<ControlsData, ControlsDataVM>();
            //CreateMap<ControlsData, RowSet>();
            //CreateMap<FieldProperties, RowSet>();
            //CreateMap<RowSet, ControlsDataVM>();
            //CreateMap<ControlsData, DropDownDataSource>();
            //CreateMap<ControlsData, DropDownPickList>();
            //CreateMap<DropDownPickList, FieldProperties>();
            //CreateMap<FieldProperties, DropDownPickList>();
            #endregion

            #region Version
            CreateMap<DataModel.Versions, VersionsModel>();
            CreateMap<VersionsModel, DataModel.Versions>();
            #endregion

            #region[Input for Account Creation]
            // CreateMap<DomainModel.NewAccount, DataModel.Accounts>()
            //     //.ForMember(dest => dest.Subscriptions, opt => opt.MapFrom(src => src.Subscriptions as IList<DataModel.Subscriptions>))
            //     //.ForMember(dest => dest.Subscriptions, opt => opt.ConvertUsing(new SubscriptionFormatter()))
            //     //.ForMember(dest => dest.Users, opt => opt.ConvertUsing(new UsersFormatter()))
            //     // { new DataModel.Subscriptions { Description=src.Description } }))
            //     //.ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users as IList<DataModel.Users>))
            //     //.ForMember(dest => dest.PasswordPolicy, opt => opt.MapFrom(src => src.PasswordPolicy as IList<DataModel.PasswordPolicy>));
            //     .IgnoreAllPropertiesWithAnInaccessibleSetter().IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            //     .ForMember(dest => dest.GroupsRights, opt => opt.Ignore());
            //// .ForMember(dest => dest.GroupsRights, opt => opt.Ignore());
            // CreateMap<DomainModel.NewPasswordPolicy, DataModel.PasswordPolicy>();
            // CreateMap<DomainModel.NewSubscriptions, DataModel.Subscriptions>();
            // CreateMap<DomainModel.NewUsers, DataModel.Users>();
            //     //.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            //     //.ForMember(dest => dest.PasswordSalt, opt => opt.Ignore());
            // CreateMap< DomainModel.NewGroups, DataModel.Groups>();
            // CreateMap< DomainModel.NewUsersGroupsMapping, DataModel.UsersGroupsMapping>();
            // //CreateMap< DomainModel.NewGroupsRights, DataModel.GroupsRights>();
            #endregion

        }
    }
    #region TODO Need to resolve error
    //public class SubscriptionFormatter : IValueConverter<DomainModel.NewSubscriptions,ICollection<DataModel.Subscriptions>>
    //{
    //    public ICollection<DataModel.Subscriptions> Convert(NewSubscriptions sourceMember, ResolutionContext context)
    //    {
    //        var result = new List<DataModel.Subscriptions>();
    //          result.Add(new DataModel.Subscriptions
    //        {
    //            Description = sourceMember.Description,
    //            StartDate = sourceMember.StartDate,
    //            EndDate = sourceMember.EndDate
    //        });
    //        return result;
    //    }


    //}
    //public class UsersFormatter : IValueConverter<object, ICollection<DataModel.Users>>
    //{
    //    public ICollection<DataModel.Users> Convert(object sourceMember, ResolutionContext context)
    //    {
    //        var result = new List<DataModel.Users>();
    //        result.Add(sourceMember as DataModel.Users);
    //        return result;
    //    }
    //}
    #endregion
}
