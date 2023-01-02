using System.Windows.Controls;

namespace Ciel.WPF.Contracts.Services;

public interface IPageService
{
    Type GetPageType(string key);

    Page GetPage(string key);
}
