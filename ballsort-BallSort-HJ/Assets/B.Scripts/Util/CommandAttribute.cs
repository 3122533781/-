using System;

namespace WhiteEggTart
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RegisterCommandAttribute : Attribute
    {
        public string Name { get; set; }   //命令名称
        public string Help { get; set; }  //解释命令的作用
        public string Hint { get; set; }  //命令行提示，用错的时候提示一哈

        public RegisterCommandAttribute(string commandName = null)
        {
            Name = commandName;
        }
    }
}