using System.Linq.Expressions;
using FluentValidation;

namespace FaceShuffle.Models.Validation;

internal static class SinglePropertyValidator
{
    public static void ValidatePropertyAndThrow<TEntity, TProperty>(this TEntity @this,
        Expression<Func<TEntity, TProperty>> propertyExpression,
        Func<IRuleBuilder<TEntity, TProperty>, IRuleBuilder<TEntity, TProperty>> rule) =>
        Create(propertyExpression, rule).ValidateDomainAndThrow(@this);

    public static IValidator<TEntity> Create<TEntity, TProperty>(Expression<Func<TEntity, TProperty>> propertyExpression,
        Func<IRuleBuilder<TEntity, TProperty>, IRuleBuilder<TEntity, TProperty>> rule)
    {
        return new Validator<TEntity, TProperty>(propertyExpression, rule);
    }

    class Validator<TEntity, TProperty> : AbstractValidator<TEntity>
    {
        public Validator(Expression<Func<TEntity, TProperty>> propertyExpression, Func<IRuleBuilder<TEntity, TProperty>, IRuleBuilder<TEntity, TProperty>> rule)
        {
            rule(RuleFor(propertyExpression));
        }
    }
}
