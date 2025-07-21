using AutoMapper;

using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Profiles
{
	public class ReferenseDataProfile : Profile
	{
		public ReferenseDataProfile()
		{
			CreateMap<ApplicationStatusDataModel, ApplicationStatus>().ReverseMap();
			CreateMap<ApplicationTypeDataModel, ApplicationType>().ReverseMap();
			CreateMap<EquipmentKindDataModel, EquipmentKind>().ReverseMap();
			CreateMap<EquipmentModelDataModel, EquipmentModel>().ReverseMap();
			CreateMap<EquipmentTypeDataModel, EquipmentType>().ReverseMap();
			CreateMap<TypeBreakdownDataModel, TypeBreakdown>().ReverseMap();
			CreateMap<ImportancesDataModel, Importance>().ReverseMap();
		}
	}
}
