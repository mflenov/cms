using System;
using System.ComponentModel.DataAnnotations;

namespace FCmsManager.ViewModel
{
    public class FolderItemViewModel
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Index { get; set; }

        public string Type { get; set; }
    }
}
