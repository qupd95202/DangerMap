using DangerMap.Dtos;
using DangerMap.Models.db;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DangerMap.ValidationAttributes
{
    /// <summary>
    /// AccountID 驗證: 驗證是否存在該 ID, 如取得特定 ID 實體可進行操作修改
    /// </summary>
    public class AccountIDAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            uploadtestv1Context context = (uploadtestv1Context)validationContext.GetService(typeof(uploadtestv1Context));

            var AccountID = (string)value;

            var findId = from a in context.AccountProfiles
                         where a.AccountId == AccountID
                         select a;

            var dto = validationContext.ObjectInstance;

            if (dto.GetType() == typeof(AccountProfileInputDto))
            {
                var updateDto = (AccountProfileInputDto)dto;
                findId = findId.Where(a => a.AccountId != updateDto.AccountId);
            }

            if (findId.FirstOrDefault() == null)
            {
                return new ValidationResult("查無此 ID");
            }

            return ValidationResult.Success;
        }
    }
}
