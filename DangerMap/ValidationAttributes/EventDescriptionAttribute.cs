﻿using DangerMap.Models.db;
using System.ComponentModel.DataAnnotations;

namespace DangerMap.ValidationAttributes
{
    public class EventDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            uploadtestv1Context context = (uploadtestv1Context)validationContext.GetService(typeof(uploadtestv1Context));

            var eventDescription = (string)value;

            if (Global.ILLEGAL_WORD.IsMatch(eventDescription))
            {
                return new ValidationResult("內容包含不合法文字");
            }
            else if (eventDescription.Length > Global.EVENT_DETAIL_LENGTH)
            {
                return new ValidationResult("內容請勿超過 200 個字元");
            }

            return ValidationResult.Success;
        }
    }
}
