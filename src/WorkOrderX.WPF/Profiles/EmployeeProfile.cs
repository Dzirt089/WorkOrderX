using AutoMapper;

using WorkOrderX.Http.Models;
using WorkOrderX.WPF.Models.Model;

namespace WorkOrderX.WPF.Profiles
{
	public class EmployeeProfile : Profile
	{
		public EmployeeProfile()
		{
			CreateMap<LoginResponseDataModel, LoginResponse>().ReverseMap();
			CreateMap<EmployeeDataModel, Employee>().ReverseMap();
		}
	}
}
