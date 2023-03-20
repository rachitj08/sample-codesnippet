using AutoMapper;
using Common.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using Utilities;
using Sample.Customer.Model;
using Sample.Customer.Service.Infrastructure.Repository;

namespace Sample.Customer.Service.ServiceWorker
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IGroupRepository groupsRepository;
        private readonly IMapper mapper;

        /// <summary>
        /// Groups Service constructor to Inject dependency
        /// </summary>
        /// <param name="unitOfWork">unit of work repository</param>
        /// <param name="groupsRepository">Group repository</param>
        public GroupService(IUnitOfWork unitOfWork, IGroupRepository groupsRepository, IMapper mapper)
        {
            Check.Argument.IsNotNull(nameof(unitOfWork), unitOfWork);
            Check.Argument.IsNotNull(nameof(groupsRepository), groupsRepository);
            this.unitOfWork = unitOfWork;
            this.groupsRepository = groupsRepository;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<GroupsVM>> GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all,long accountId) => await this.groupsRepository.GetAllGroups(ordering, offset, pageSize, pageNumber, all, accountId);

        /// <summary>
        /// Get Group By GroupID
        /// </summary>
        /// <returns></returns>
        public async Task<GroupsVM> GetGroupById(long groupId) => await this.groupsRepository.GetGroupById(groupId);
        /// <summary>
        /// GetGroupByUserId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<GroupsModel>> GetGroupByUserId(long userId)
        {
           var result = await this.groupsRepository.GetGroupByUserId(userId);
            return mapper.Map<List<GroupsModel>>(result);
        }

        /// <summary>
        /// To Create New Group
        /// </summary>
        /// <param name="group">new Group object/param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> CreateGroup(GroupsModel group, long userId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(group.Name))
            {
                errorDetails.Add("name", new string[] { "This field may not be blank." });
            }
            else if (group.Name.Length > 50)
            {
                errorDetails.Add("name", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<GroupsVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            var result = await this.groupsRepository.CreateGroup(group, userId);
            if (result.GroupId > 0)
            {
                return new ResponseResult<GroupsVM>()
                {
                    Message = ResponseMessage.RecordSaved,
                    ResponseCode = ResponseCode.RecordSaved,
                    Data = result
                };
            }
            else
            {
                return new ResponseResult<GroupsVM>()
                {
                    Message = ResponseMessage.InternalServerError,
                    ResponseCode = ResponseCode.InternalServerError,
                    Error = new ErrorResponseResult()
                    {
                        Message = ResponseMessage.InternalServerError
                    }
                };
            }
        }

        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<GroupsModel>> GetAllGroups()
        {
            var result = await this.groupsRepository.GetAllGroups();
            return mapper.Map<IEnumerable<GroupsModel>>(result);
        }

        /// <summary>
        /// Groups Rights By GroupId
        /// </summary>
        /// <param name="groupId"> group identifier</param>
        /// <returns></returns>
        public async Task<IEnumerable<GroupRightsModel>> GetGroupsRightsByGroupId(int groupId)
        {
            var result = await this.groupsRepository.GetGroupsRightsByGroupId(groupId);
            return mapper.Map<IEnumerable<GroupRightsModel>>(result);
        }

        /// <summary>
        /// Get Group Modules by user id
        /// </summary>
        /// <param name="userId">user identifier</param>
        /// <returns></returns>
        public async Task<IEnumerable<GroupRightsModel>> GetGroupModules(int userId)
        {
            var result = await this.groupsRepository.GetGroupModules(userId);
            return mapper.Map<IEnumerable<GroupRightsModel>>(result);
        }

        /// <summary>
        /// To Update existing Group
        /// </summary>
        /// <param name="group">group object</param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> UpdateGroup(long groupId, GroupsModel group, long userId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(group.Name))
            {
                errorDetails.Add("name", new string[] { "This field may not be blank." });
            }
            else if (group.Name.Length > 50)
            {
                errorDetails.Add("name", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<GroupsVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            return new ResponseResult<GroupsVM>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = await this.groupsRepository.UpdateGroup(groupId, group, userId)
            };
        }

        /// <summary>
        /// To Update Group Partially
        /// </summary>
        /// /// <param name="groupId">Group ID</param>
        /// <param name="group">New group object</param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> UpdatePartialGroup(long groupId, GroupsModel group, long userId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();
            if (string.IsNullOrWhiteSpace(group.Name))
            {
                errorDetails.Add("name", new string[] { "This field may not be blank." });
            }
            else if (group.Name.Length > 50)
            {
                errorDetails.Add("name", new string[] { "Ensure this field has no more than 50 characters." });
            }

            if (errorDetails.Count > 0)
            {
                return new ResponseResult<GroupsVM>()
                {
                    Message = ResponseMessage.ValidationFailed,
                    ResponseCode = ResponseCode.ValidationFailed,
                    Error = new ErrorResponseResult()
                    {
                        Detail = errorDetails,
                        Message = ResponseMessage.ValidationFailed,
                    }
                };
            }

            return new ResponseResult<GroupsVM>()
            {
                Message = ResponseMessage.RecordSaved,
                ResponseCode = ResponseCode.RecordSaved,
                Data = await this.groupsRepository.UpdatePartialGroup(groupId, group, userId)
            };
        }

        /// <summary>
        /// To delete existing Group
        /// </summary>
        /// <param name="groupId">group identifier</param>
        /// <returns></returns>
        public async Task<long> DeleteGroup(long groupId) => await this.groupsRepository.DeleteGroup(groupId);

    }
}
