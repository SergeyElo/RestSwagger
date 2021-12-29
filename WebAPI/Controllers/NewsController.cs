using AutoMapper;
using Common.Identity;
using Domain.Core.Filters.Base;
using Domain.Core.Filters.News;
using Domain.Core.Models.News;
using Infrastructure.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using WebAPI.Models.News;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Контроллер новостей
    /// </summary>
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsServices _news;
        private readonly IMapper _mapper;
        private readonly ILogger<NewsController> _logger;

        public NewsController(INewsServices news, IMapper mapper, ILogger<NewsController> logger)
        {
            _news = news;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Получение новости по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([Required] Guid id)
        {
            try
            {
                var entity = await _news.GetById(id);

                if (entity is null)
                    return NoContent();

                return Ok(_mapper.Map<NewsFullDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при получении новости");
                return StatusCode(500);
            }
        }
        
        /// <summary>
        /// Получение списка новостей по фильтру
        /// </summary>
        /// <param name="entityFilterDto"></param>
        /// <param name="filterPaging"></param>
        /// <returns></returns>        
        [HttpGet("list")]
        public async Task<IActionResult> GetByFilter([FromQuery] NewsEntityFilterDto entityFilterDto, [FromQuery] FilterPagingDto filterPaging)
        {
            try
            {
                var entityList = await _news.GetByFilter(entityFilterDto, filterPaging);

                return Ok(_mapper.Map<IEnumerable<NewsDto>>(entityList));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при получении списка новостей");
                return StatusCode(500);
            }
        }
        
        // new 25.12.2021 --------------------
        /// <summary>
        /// Получение всех новостей
        /// </summary>
        [HttpGet("listall")]
        public async Task<IActionResult> GetListNews()
        {
            try
            {
                var entityList = await _news.GetByFilter();
                return Ok(entityList);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при получении всех новостей");
                return StatusCode(500);
            }
        }
        // end new 25.12.2021 --------------------

        /// <summary>
        /// Создание новости
        /// </summary>
        /// <param name="createNewsItem"></param>
        /// <returns></returns>
        //[Authorize(Roles = IdentityRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNewsItemDto createNewsItem)
        {
            try
            {
                var entity = _mapper.Map<NewsEntity>(createNewsItem);

                return Ok(await _news.Create(entity));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при создании новости");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Обновление новости
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newsItem"></param>
        /// <returns></returns>
        /// [Authorize(Roles = IdentityRoles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateNewsItemDto newsItem)
        {
            try
            {
                var entity = await _news.GetById(id);
                if (entity is null)
                    return BadRequest();

                _mapper.Map(newsItem, entity);
                await _news.Update(entity);

                return Ok(_mapper.Map<NewsFullDto>(entity));
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при обновлении новости");
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Удаление новости
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// [Authorize(Roles = IdentityRoles.Admin)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var entity = await _news.GetById(id);
                if (entity is null)
                    return BadRequest();
                await _news.Delete(entity);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Ошибка при удалении новости");
                return StatusCode(500);
            }
        }
    }
}
