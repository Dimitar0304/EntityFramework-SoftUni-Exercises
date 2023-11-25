namespace FastFood.Core.MappingConfiguration
{
    using AutoMapper;
    using FastFood.Core.ViewModels.Categories;
    using FastFood.Core.ViewModels.Employees;
    using FastFood.Core.ViewModels.Items;
    using FastFood.Core.ViewModels.Orders;
    using FastFood.Models;
    using ViewModels.Positions;

    public class FastFoodProfile : Profile
    {
        public FastFoodProfile()
        {
            //Positions
            CreateMap<CreatePositionInputModel, Position>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.PositionName));

            CreateMap<Position, RegisterEmployeeViewModel>()
                .ForMember(x => x.PositionId, y => y.MapFrom(s => s.Id));

            CreateMap<Position, PositionsAllViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.Name));
            //categories
            CreateMap<CreateCategoryInputModel, Category>()
                .ForMember(x => x.Name, y => y.MapFrom(s => s.CategoryName));

            CreateMap<Category, CreateItemViewModel>()
                .ForMember(cm => cm.CategoryId, y => y.MapFrom(c => c.Id));
            CreateMap<Category, CategoryAllViewModel>()
                .ForMember(cm => cm.Name,y=>y.MapFrom(c=>c.Name));

            //Employees
            CreateMap<RegisterEmployeeInputModel, Employee>();
            CreateMap<Employee, EmployeesAllViewModel>()
                .ForMember(evm => evm.Position, e => e.MapFrom(x => x.Position.Name));

            //Item
            CreateMap<CreateItemInputModel, Item>()
                .ForMember(i=>i.Name,y=>y.MapFrom(cm=>cm.Name));


            CreateMap<Item, ItemsAllViewModels>()
                .ForMember(im => im.Category, y => y.MapFrom(i => i.Name));


            //orders
            CreateMap<Order, OrderAllViewModel>()
                .ForMember(om => om.Employee, y => y.MapFrom(o => o.Employee.Id));
            CreateMap<Order, OrderAllViewModel>()
                .ForMember(om => om.OrderId, y => y.MapFrom(o => o.Id));





        }
    }
}
