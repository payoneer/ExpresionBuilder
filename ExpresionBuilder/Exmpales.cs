using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpresionBuilder
{
    public class UserModel
    {
        public bool isTemp { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Last { get; set; }
        public int Age { get; set; }
        public int Rank { get; set; }
    }

    class Exmpales
    {
        private IQueryable<UserModel> Getusers ()
        {
            var list = new List<UserModel>();
            return list.AsQueryable();
        }
        public void Query()
        {
            var allUsers = Getusers();
            List<Expression<Func<UserModel,bool>>> andList = new List<Expression<Func<UserModel,bool>>>();

            andList.Add((o) => o.Last != "berezin" && o.Age > 5);
            andList.Add((o) => o.Age > 5);
            andList.Add((o) => o.isTemp);
            andList.Add((o) => o.Rank == 1);


            var test = allUsers.Where(ExpresionTreeBuilder.CreateANDQuery<UserModel>(andList,true).Compile());


            List<Expression<Func<UserModel,dynamic>>> list = new List<Expression<Func<UserModel,dynamic>>>();
            List<Expression<Func<UserModel,dynamic>>> list2 = new List<Expression<Func<UserModel,dynamic>>>();
            list.Add((o) => o.Age);
            list2.Add((o) => o.Rank);

            allUsers = ExpresionTreeBuilder.CreateOrderQuery<UserModel>(allUsers,list,list2,false);
            
        }
    }
}
