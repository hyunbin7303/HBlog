﻿using AutoMapper;
using KevBlog.Application.DTOs;
using KevBlog.Application.Services;
using KevBlog.Domain.Entities;
using KevBlog.Domain.Repositories;
using KevBlog.Infrastructure.Extensions;
using KevBlog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KevBlog.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : BaseApiController
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public TagsController(ITagRepository tagRepository, IUserService userService, IMapper mapper)
        {
            this._tagRepository = tagRepository;
            this._userService = userService;
            this._mapper = mapper;  
        }
        [HttpPost]
        public async Task<ActionResult> Create(TagCreateDto tagCreateDto)
        {
            if (tagCreateDto is null)
                throw new ArgumentNullException(nameof(tagCreateDto));

            if (string.IsNullOrEmpty(tagCreateDto.Name))
                return BadRequest("Tag Name cannot be empty.");

            var user = await _userService.GetMembersByUsernameAsync(User.GetUsername());
            if (user.Value is null) return NotFound();

            await _tagRepository.Insert(tagCreateDto.Name, tagCreateDto.Desc, tagCreateDto.Slug);
            return Ok();
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tag>>> Get()
        {
            return Ok(await _tagRepository.GetAll());
        }

    }
}
