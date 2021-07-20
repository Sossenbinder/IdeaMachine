﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdeaMachine.Common.Core.Utils.Pagination;
using IdeaMachine.Modules.Idea.DataTypes.Entity;

namespace IdeaMachine.Modules.Idea.Repository.Interface
{
	public interface IIdeaRepository
	{
		Task<IdeaEntity> Add(IdeaEntity idea);

		Task<PaginationResult<int?, IdeaEntity>> Get(int? paginationToken = null);

		Task<List<IdeaEntity>> GetForSpecificUser(Guid userId);

		Task<IdeaEntity?> GetSpecificIdea(int id);

		Task MigrateIdeas(Guid oldOwner, Guid newOwner);
	}
}