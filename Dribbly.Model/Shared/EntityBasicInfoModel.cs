using Dribbly.Service.Enums;
using Dribbly.Model.Courts;
using System.ComponentModel.DataAnnotations.Schema;
using Dribbly.Model.Account;

namespace Dribbly.Model.Shared
{
    [NotMapped]
    public class EntityBasicInfoModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public EntityStatusEnum EntityStatus { get; set; }
        public virtual PhotoModel Photo { get; set; }
        public EntityTypeEnum Type { get; set; }

        public EntityBasicInfoModel()
        {

        }

        public EntityBasicInfoModel(AccountModel account)
        {
            Id = account.Id;
            Name = account.User != null ? account.User.UserName : account.Username;
            Photo = account.ProfilePhoto;
            Type = EntityTypeEnum.Account;
            EntityStatus = account.EntityStatus;
        }
    }
}