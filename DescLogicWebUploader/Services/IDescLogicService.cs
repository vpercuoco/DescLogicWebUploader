using FirstASPNETCOREProject.ViewModels;
using DescLogicFramework;
using System.Threading.Tasks;

namespace FirstASPNETCOREProject
{
    public interface IDescLogicService
    {
        Task Convert(TestModel model);
    }
}