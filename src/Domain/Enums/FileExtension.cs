using System.ComponentModel;

namespace LMS.Domain.Enums;

public enum FileExtension
{
    [Description(".pdf")]
    PDF = 1,
    [Description(".docx")]
    DOCX = 2,
    [Description(".png")]
    PNG = 3,
    [Description(".jpg")]
    JPG = 4,
    [Description(".jpeg")]
    JPEG = 5,
}
