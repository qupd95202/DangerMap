using DangerMap.Models.db;
using System.ComponentModel.DataAnnotations;

namespace DangerMap.ValidationAttributes
{
    /// <summary>
    /// 帳號暱稱之驗證
    /// </summary>
    public class AccountNameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            uploadtestv1Context context = (uploadtestv1Context)validationContext.GetService(typeof(uploadtestv1Context));

            var name = (string)value;

            if (name.Length > 16)
            {
                return new ValidationResult("輸入之名稱超過 16 個字");
            }
            return ValidationResult.Success;
        }
    }
}
