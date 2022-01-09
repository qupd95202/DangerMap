using DangerMap.Dtos;
using DangerMap.Models.db;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DangerMap.ValidationAttributes
{
    public class RegisterIDAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            uploadtestv1Context context = (uploadtestv1Context)validationContext.GetService(typeof(uploadtestv1Context));

            var AccountID = (string)value;

            if (!Global.LEGAL_ID_PASSWORD.IsMatch(AccountID))
            {
                return new ValidationResult("輸入之 ID 不符合規範");
            }

            var findId = from a in context.AccountProfiles
                         where a.AccountId == AccountID
                         select a;

            if (findId.FirstOrDefault() != null)
            {
                return new ValidationResult("已存在相同 ID");
            }

            return ValidationResult.Success;
        }
    }
}
