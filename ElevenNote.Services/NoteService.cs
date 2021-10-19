using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public NoteCreate GetCreateView()
        {
            var ctx = new ApplicationDbContext();
            var viewModel = new NoteCreate();
            viewModel.Categories = ctx.Categories.Select(category => new SelectListItem
            {
                Text = category.Name,
                Value = category.CategoryId.ToString()
            }) ;
            return viewModel;
        }
        public bool CreateNote(NoteCreate model)
        {
            var entity =
                new Note()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    CategoryId = model.CategoryID,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Notes.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<NoteListItem> GetNotes()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Notes
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new NoteListItem
                                {
                                    NoteId = e.NoteId,
                                    Title = e.Title,
                                    CategoryId = e.CategoryId,
                                    CreatedUtc = e.CreatedUtc,
                                    Category = e.Category
                                }
                               );
                return query.ToArray();
            }
        }

        public IEnumerable<NoteListItem> GetNotesByCatId(int? id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Notes.Where(e => e.CategoryId == id && e.OwnerId == _userId)
                    .Select(e => new NoteListItem
                    {
                        NoteId = e.NoteId,
                        Title = e.Title,
                        CategoryId = e.CategoryId,
                        CreatedUtc = e.CreatedUtc

                    });
                return query.ToArray();
            }
        }
        public NoteDetail GetNoteById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                            .Single(e => e.NoteId == id && e.OwnerId == _userId);
                return
                    new NoteDetail
                    {
                        NoteId = entity.NoteId,
                        Title = entity.Title,
                        Content = entity.Content,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
            }
        }

        public bool UpdateNote(NoteEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Notes.Single
                    (e => e.NoteId == model.NoteId && e.OwnerId == _userId);

                entity.Title = model.Title;
                entity.CategoryId = model.CategoryId;
                entity.Content = model.Content;
                entity.ModifiedUtc = DateTimeOffset.Now;

                return ctx.SaveChanges() == 1;
            }
        }

        public bool DeleteNote(int noteId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Notes.Single(e => e.NoteId == noteId && e.OwnerId == _userId);
                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }


    }
}
