namespace BLL.Sub
{
    public class UserBll : Base.BaseServer<Entity.entity.User>
    {
        public UserBll()
        {
            baseDal = new Dll.Sub.UserDal();
        }
    }
}
