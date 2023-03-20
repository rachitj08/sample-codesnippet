using Common.Model;
using Sample.Admin.Service.Infrastructure.Repository;
using Sample.Admin.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sample.Admin.Service.ServiceWorker
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork unitOfWork;

        private readonly IGroupRepository groupsRepository;

        /// <summary>
        /// configuration variable
        /// </summary>
        public IConfiguration configuration { get; }

        /// <summary>
        /// Modules Service constructor to Inject dependency
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="groupRepository">group repository</param>
        public GroupService(IUnitOfWork unitOfWork, IGroupRepository groupsRepository, IConfiguration configuration)
        {
            this.unitOfWork = unitOfWork;
            this.groupsRepository = groupsRepository;
            this.configuration = configuration;
        }

        /// <summary>
        /// Get All Groups
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseResultList<GroupsVM>> GetAllGroups(string ordering, int offset, int pageSize, int pageNumber, bool all) => await this.groupsRepository.GetAllGroups(ordering, offset, pageSize, pageNumber, all);

        /// <summary>
        /// Get Group By GroupID
        /// </summary>
        /// <returns></returns>
        public async Task<GroupsVM> GetGroupById(long groupId) => await this.groupsRepository.GetGroupById(groupId);

        /// <summary>
        /// To Create New Group
        /// </summary>
        /// <param name="group">new Group object/param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> CreateGroup(GroupsModel group, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(group.Name))
            {
                errorDetails.Add("name", new string[] { "This field may not be blank." });
            }
            else if (group.Name.Length > 50)
            {
                errorDetails.Add("name", new string[] { "Ensure this field has no more than 200 characters." });
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

            var result = await this.groupsRepository.CreateGroup(group, loggedInUserId);

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
        /// To Update existing Group
        /// </summary>
        /// <param name="group">group object</param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> UpdateGroup(long groupId, GroupsModel group, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(group.Name))
            {
                errorDetails.Add("name", new string[] { "This field may not be blank." });
            }
            else if (group.Name.Length > 50)
            {
                errorDetails.Add("name", new string[] { "Ensure this field has no more than 200 characters." });
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
                Data = await this.groupsRepository.UpdateGroup(groupId, group, loggedInUserId)
            };

        }

        /// <summary>
        /// To Update Group Partially
        /// </summary>
        /// /// <param name="groupId">Group ID</param>
        /// <param name="group">New group object</param>
        /// <returns></returns>
        public async Task<ResponseResult<GroupsVM>> UpdatePartialGroup(long groupId, GroupsModel group, int loggedInUserId)
        {
            // Validate Model
            var errorDetails = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(group.Name))
            {
                errorDetails.Add("name", new string[] { "This field may not be blank." });
            }
            else if (group.Name.Length > 50)
            {
                errorDetails.Add("name", new string[] { "Ensure this field has no more than 200 characters." });
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
                Data = await this.groupsRepository.UpdatePartialGroup(groupId, group, loggedInUserId)
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
