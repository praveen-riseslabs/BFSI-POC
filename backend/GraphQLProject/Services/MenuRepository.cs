using GraphQLProject.Interfaces;
using GraphQLProject.Models;

namespace GraphQLProject.Services
{
    public class MenuRepository : IMenuRepository
    {
        private static List<Menu> MenuList = new List<Menu>()
        {
            new Menu() {Id=1, Name="test1", Email="test1@gmail.ocm" , Password="test1"},
            new Menu() {Id=2, Name="test2", Email="test2@gmail.ocm" , Password="test2"},
        };
        public Menu AddMenu(Menu menu)
        {
             MenuList.Add(menu);
            return menu;
        }

        public void DeleteMenu(int id)
        {
            MenuList.RemoveAt(id);
            
        }

        public List<Menu> GetAllMenu()
        {
            return MenuList;
        }

        public Menu GetMenuById(int id)

        {
            return MenuList.Find(m => m.Id == id);
        }

        public Menu UpdateMenu(int id, Menu menu)
        {
            MenuList[id] = menu;
            return menu;
        }
    }
}
