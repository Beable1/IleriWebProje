using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IleriWeb.Core.DTOs
{
    public class UserSignUpDto
    {
        [Display(Name = "Ad Soyad")]
        [Required(ErrorMessage = "Lütfen ad soyad giriniz ")]
        public string NameSurname { get; set; }

        [Display(Name = "Şifre")]
        [Required(ErrorMessage = "Lütfen şifre giriniz ")]
        public string Password { get; set; }

        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler Uyuyşmuyor! ")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Mail")]
        [Required(ErrorMessage = "Lütfen Mail giriniz")]
        public string Mail { get; set; }


        [Display(Name = "Kullanıcı Adı")]
        [Required(ErrorMessage = "Lütfen Kullanıcı Adı giriniz ")]
        public string UserName { get; set; }

    }
}
