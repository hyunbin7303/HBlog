
using System.Reflection.Metadata;
using HBlog.Contract.DTOs;
using HBlog.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HBlog.Api.Controllers
{
    public class MigrationController() : BaseApiController
    {
        [HttpPost("/migration")]
        public async Task<Results<Ok<MigrationDto>,InternalServerError,BadRequest>> Migration([FromServices] DataContext context, CancellationToken cancellationToken)
        {
            IEnumerable<string> applied = context.Database.GetAppliedMigrations();
            IEnumerable<string> pending = context.Database.GetPendingMigrations();
            try
            {
                await context.Database.MigrateAsync(cancellationToken); 
                return TypedResults.Ok(new MigrationDto
                {
                    AppliedMigration = new MigrationDetail
                    {
                        Count = applied.Count(),
                        MigrationNames = applied
                    },
                    PendingMigration = new MigrationDetail
                    {
                        Count = pending.Count(),
                        MigrationNames = pending
                    }
                }); 
            }
            catch(Exception ex)
            {
                //TODO: Handle exception;
            }
            return TypedResults.InternalServerError();
        }
    }

}