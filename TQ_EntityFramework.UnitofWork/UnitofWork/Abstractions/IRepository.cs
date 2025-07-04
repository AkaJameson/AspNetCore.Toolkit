﻿using System.Linq.Expressions;

namespace TQ_EntityFramework.UnitofWork.UnitofWork.Abstractions
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 根据ID异步获取实体
        /// </summary>
        /// <param name="id">实体的ID</param>
        /// <returns>实体对象</returns>
        Task<T> GetByIdAsync(object id);
        /// <summary>
        /// 获取所有实体的异步方法
        /// </summary>
        /// <returns>所有实体的集合</returns>
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// 根据条件获取单个实体
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 分页获取实体集合
        /// </summary>
        /// <param name="pageIndex">页索引（从0开始）</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="predicate">过滤条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="ascending">是否升序排序</param>
        /// <returns>分页后的实体集合</returns>
        Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(
            int pageIndex,
            int pageSize,
            Expression<Func<T, bool>> predicate = null,
            Expression<Func<T, object>> orderBy = null,
            bool ascending = true);
        Task<(IEnumerable<TResult> Items, int TotalCount)> GetPagedAsync<TResult>(
        int pageIndex,
        int pageSize,
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
        Expression<Func<T, TResult>> selector = null);
        /// <summary>
        /// 异步添加实体
        /// </summary>
        /// <param name="entity">要添加的实体</param>
        Task AddAsync(T entity);

        /// <summary>
        /// 异步添加多个实体
        /// </summary>
        /// <param name="entities">要添加的实体集合</param>
        Task AddRangeAsync(IEnumerable<T> entities);
        /// <summary>
        /// 异步更新实体
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task UpdateAsync(T entity);
        /// <summary>
        /// 异步更新多个实体
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task UpdateRangeAsync(IEnumerable<T> entities);
        /// <summary>
        /// 异步删除实体
        /// </summary>
        /// <param name="id">要删除的实体ID</param>
        Task DeleteAsync(object id);

        /// <summary>
        /// 根据条件异步删除实体集合
        /// </summary>
        /// <param name="predicate">删除条件</param>
        Task DeleteAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 异步删除多个实体
        /// </summary>
        /// <param name="entities">要删除的实体集合</param>
        Task DeleteRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// 检查是否存在符合条件的实体
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>是否存在符合条件的实体</returns>
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 获取满足条件的实体数量
        /// </summary>
        /// <param name="predicate">条件表达式</param>
        /// <returns>符合条件的实体数量</returns>
        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

        /// <summary>
        /// 获取不跟踪的查询对象，支持链式调用
        /// </summary>
        IQueryable<T> AsNoTracking();

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
    }
}
