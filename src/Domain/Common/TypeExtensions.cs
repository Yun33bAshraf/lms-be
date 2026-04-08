using System.Net.Mime;
using System.Reflection;

namespace LMS.Domain.Common;
public static class TypeExtensions
{
    public static string GetDescription(this Enum genericEnum) //Hint: Change the method signature and input paramter to use the type parameter T
    {
        Type genericEnumType = genericEnum.GetType();
        MemberInfo[] memberInfo = genericEnumType.GetMember(genericEnum.ToString());

        if (memberInfo != null && memberInfo.Length > 0)
        {
            var attributes = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (attributes != null && attributes.Count() > 0)
            {
                return ((System.ComponentModel.DescriptionAttribute)attributes.ElementAt(0)).Description;
            }
        }

        return genericEnum.ToString();
    }

    public static string Fallback(string? primary, string? secondary)
    {
        return string.IsNullOrWhiteSpace(primary) ? secondary ?? string.Empty : primary!;
    }

    //public static ContentType GetContentTypeEnum(string contentType)
    //{
    //    switch (contentType.ToLower())
    //    {

    //        case "image/png":
    //            return ContentType.PNG;

    //        case "image/jpeg":
    //            return ContentType.JPG;

    //        case "image/gif":
    //            return ContentType.GIF;

    //        case "video/mp4":
    //            return ContentType.MP4;

    //        case "video/x-msvideo":
    //            return ContentType.AVI;

    //        case "video/x-matroska":
    //            return ContentType.MKV;

    //        case "video/quicktime":
    //            return ContentType.MOV;

    //        case "video/x-ms-wmv":
    //            return ContentType.WMV;
    //        // Add other cases as necessary
    //        default:
    //            throw new ArgumentException("Unsupported content type");
    //    }
    //}
}
