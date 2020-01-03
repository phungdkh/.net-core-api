using System;
using System.Linq;
using HVS.Api.Core.Business.Models.Posts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using HVS.Api.Core.Business.Models.Base;
using System.Collections.Generic;
using HVS.Api.Core.DataAccess.Repositories.Base;
using HVS.Api.Core.Entities;
using HVS.Api.Core.Common.Utilities;
using HVS.Api.Core.Common.Constants;
using HVS.Api.Core.Business.Models;
using System.Threading.Tasks;
using HVS.Api.Core.Common.Helpers;
using HVS.Api.Core.Common.Reflections;
using AutoMapper;

namespace HVS.Api.Core.Business.Services
{
    public interface IPostService
    {
        Task<PagedList<PostViewModel>> ListPostAsync(PostRequestListViewModel postRequestListViewModel);
        
    }

    public class PostService : IPostService
    {
        #region Fields

        private readonly IRepository<Post> _postRepository;

        #endregion

        public PostService(IRepository<Post> postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<PagedList<PostViewModel>> ListPostAsync(PostRequestListViewModel postRequestListViewModel)
        {
            var list = await GetAll().Where(
                x => (string.IsNullOrEmpty(postRequestListViewModel.Query)) 
                || (x.Title.Contains(postRequestListViewModel.Query)
                )).Select(x => new PostViewModel(x)).ToListAsync();

            var postViewModelProperties = GetAllPropertyNameOfViewModel();
            var requestPropertyName = !string.IsNullOrEmpty(postRequestListViewModel.SortName) ? postRequestListViewModel.SortName.ToLower() : string.Empty;
            string matchedPropertyName = string.Empty;

            foreach (var postViewModelProperty in postViewModelProperties)
            {
                var lowerTypeViewModelProperty = postViewModelProperty.ToLower();
                if (lowerTypeViewModelProperty.Equals(requestPropertyName))
                {
                    matchedPropertyName = postViewModelProperty;
                    break;
                }
            }

            if (string.IsNullOrEmpty(matchedPropertyName))
            {
                matchedPropertyName = "Title";
            }

            var type = typeof(PostViewModel);
            var sortProperty = type.GetProperty(matchedPropertyName);

            list = postRequestListViewModel.IsDesc ? list.OrderByDescending(x => sortProperty.GetValue(x, null)).ToList() : list.OrderBy(x => sortProperty.GetValue(x, null)).ToList();

            return new PagedList<PostViewModel>(list, postRequestListViewModel.Offset ?? CommonConstants.Config.DEFAULT_SKIP, postRequestListViewModel.Limit ?? CommonConstants.Config.DEFAULT_TAKE);
        }

        #region Private Method 

        public IQueryable<Post> GetAll()
        {
            return _postRepository.GetAll().Where(x => !x.RecordDeleted);
        }

        private List<string> GetAllPropertyNameOfViewModel()
        {
            var postViewModel = new PostViewModel();
            var type = postViewModel.GetType();

            return ReflectionUtilities.GetAllPropertyNamesOfType(type);
        }

        #endregion
    }
}
