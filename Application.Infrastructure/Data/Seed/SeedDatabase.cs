using Application.Core.Entities;
using Application.Infrastructure.Data.DbContext;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Infrastructure.Data.Seed
{
    public class SeedDatabase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _appDbContext;

        public SeedDatabase(AppDbContext appDbContext, UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public async Task SeedData()
        {
            if (_userManager.Users.Count() == 0)
            {
                string password = "password";

                var userJane = new ApplicationUser()
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    UserName = "jane@lendastack.io",
                    Email = "jane@lendastack.io",
                    CreatedAt = DateTime.UtcNow
                };

                var userJohn = new ApplicationUser()
                {
                    FirstName = "John",
                    LastName = "Doe",
                    UserName = "john@lendastack.io",
                    Email = "john@lendastack.io",
                    CreatedAt = DateTime.UtcNow
                };

                await _userManager.CreateAsync(userJohn, password);
                await _userManager.CreateAsync(userJane, password);

                await _appDbContext.UserRelationship.AddAsync(new UserRelationship
                {
                    FollowedId = userJohn.Id,
                    FollowerId = userJane.Id
                });

                await _appDbContext.Post.AddRangeAsync(new List<Post>
                {
                    new Post
                    {
                        Text = "In all things consider the Lord",
                        UserId = userJohn.Id,
                        Likes = new List<Like>
                        {
                            new Like
                            {
                                 UserId = userJane.Id,
                            }
                        }
                    },
                    new Post
                    {
                        Text = "Loggins in .NET 6",
                        UserId = userJohn.Id,
                        Likes = new List<Like>
                        {
                            new Like
                            {
                                 UserId = userJane.Id,
                            }
                        }
                    },
                    new Post
                    {
                        Text = "Unit testing in .NET 6",
                        UserId = userJane.Id
                    },
                    new Post
                    {
                        Text = "The Lord shall supply all my needs",
                        UserId = userJane.Id
                    }
                });

                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}
