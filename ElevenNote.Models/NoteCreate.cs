using ElevenNote.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElevenNote.Models
{
    public class NoteCreate
    {
        [Required]
        [MinLength(2,ErrorMessage ="Please enter at least 2 characters.")]
        [MaxLength(100, ErrorMessage = "This field can only contain a maximum of 1000 characters.")]
        public string Title { get; set; }
        [MaxLength(8000)]
        public string Content { get; set; }
        public int? CategoryID { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }


    }
}
