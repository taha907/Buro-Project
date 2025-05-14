using BuroManagementProject.Data;
using System.Security.Permissions;

namespace BuroManagementProject.Models
{
    public class KisilerPageModel
    {
        public List<Kisiler> Kisiler { get; set; }

        public KisilerPageModel(KisilerData kisilerData)
        {
            Kisiler = kisilerData.GetKisiler();
        }
    }

}
