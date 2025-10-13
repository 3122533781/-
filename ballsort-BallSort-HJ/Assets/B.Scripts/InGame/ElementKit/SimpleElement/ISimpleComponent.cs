
namespace Lei31.SElement
{
    public interface ISimpleComponent 
    {
        bool IsInited { get; }
        void Init(SimpleElement context);
    }
} 
