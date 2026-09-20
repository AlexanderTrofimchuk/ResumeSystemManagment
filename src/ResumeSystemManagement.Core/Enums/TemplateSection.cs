using System.ComponentModel.DataAnnotations;

namespace ResumeSystemManagement.Core.Enums;

public enum TemplateSection
{
    [Display(Name = "Личные данные", Description = "bi-person")]
    Personal,

    [Display(Name = "Контакты", Description = "bi-telephone")]
    Contacts,

    [Display(Name = "Навыки", Description = "bi-stars")]
    Skills,

    [Display(Name = "Языки", Description = "bi-translate")]
    Languages,

    [Display(Name = "Образование", Description = "bi-mortarboard")]
    Education,

    [Display(Name = "Прочее", Description = "bi-three-dots")]
    Other
}

public static class EnumExtensions
{
    public static DisplayAttribute? GetDisplay(this Enum value)
    {
        return value.GetType()
            .GetField(value.ToString())
            ?.GetCustomAttributes(typeof(DisplayAttribute), false)
            .OfType<DisplayAttribute>()
            .FirstOrDefault();
    }

    public static string GetDisplayName(this Enum value) =>
        value.GetDisplay()?.Name ?? value.ToString();

    public static string GetDisplayIcon(this Enum value) =>
        value.GetDisplay()?.Description ?? "bi-question-circle";
}