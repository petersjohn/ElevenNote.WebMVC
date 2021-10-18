using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElevenNote.Models
{
    public class NoteEdit
    {
        public int NoteId { get; set; }
        public string Title { get; set; }
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public string Content { get; set; }
    }
}
