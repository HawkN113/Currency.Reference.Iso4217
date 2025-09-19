using Currency.Reference.Iso4217.Builders.Abstractions;
using Currency.Reference.Iso4217.Common.Models;
namespace Currency.Reference.Iso4217.Abstractions;

/// <summary>
/// 
/// </summary>
public interface ICurrencyService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="criteria"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    bool IsValid(string value, Criteria[] criteria, CurrencyType? type = null);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="value"></param>
    /// <param name="criteria"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    CurrencyInfo? Get(string value, Criteria criteria, CurrencyType? type = null);
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    ICurrencyQueryStart Query();
}