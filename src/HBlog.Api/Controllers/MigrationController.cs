
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
        public async Task<Results<Ok<MigrationDto>,InternalServerError,BadRequest>> Migration(
            [FromServices] IdentityContext identityContext,
            [FromServices] BlogContext blogContext,
            CancellationToken cancellationToken)
        {
            var identityApplied = identityContext.Database.GetAppliedMigrations();
            var identityPending = identityContext.Database.GetPendingMigrations();
            var blogApplied = blogContext.Database.GetAppliedMigrations();
            var blogPending = blogContext.Database.GetPendingMigrations();
            
            try
            {
                await identityContext.Database.MigrateAsync(cancellationToken);
                await blogContext.Database.MigrateAsync(cancellationToken);
                
                var allApplied = identityApplied.Concat(blogApplied);
                var allPending = identityPending.Concat(blogPending);
                
                return TypedResults.Ok(new MigrationDto
                {
                    AppliedMigration = new MigrationDetail
                    {
                        Count = allApplied.Count(),
                        MigrationNames = allApplied
                    },
                    PendingMigration = new MigrationDetail
                    {
                        Count = allPending.Count(),
                        MigrationNames = allPending
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