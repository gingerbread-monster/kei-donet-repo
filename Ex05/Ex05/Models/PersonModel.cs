using System.ComponentModel.DataAnnotations;
using Ex05.CustomValidators;

namespace Ex05.Models
{
    public class PersonModel
    {
        [Required(ErrorMessage = "Поле \"Имя\" является обязательным.")]
        [MaxLength(length: 50, ErrorMessage = "Превышена максимально допустимая длина имени.")]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Display(Name = "Возраст")]
        [Range(minimum: 18, maximum: 60, ErrorMessage = "Возвраст не входит в ОДЗ.")]
        public int Age { get; set; }

        [Display(Name = "Веган")]
        [ValidateTrue(ErrorMessage = "Данный человек не веган 🥦")]
        public bool IsVegan { get; set; }
    }
}
