using System.ComponentModel.DataAnnotations;

namespace FCmsManager.ViewModel
{
    public class FolderItemViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public int Index { get; set; }
    }
}
