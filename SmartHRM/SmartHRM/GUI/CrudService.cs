using System;
using System.Collections.Generic;
using System.Linq;
using SmartHRM.BUS;
using SmartHRM.DTO;

namespace SmartHRM.GUI
{
    /// <summary>Cầu nối giữa CrudForm (không generic, thiết kế được trong Designer) và GenericBUS&lt;T&gt;.</summary>
    public interface ICrudService
    {
        string Module { get; }
        string Title { get; }
        Type EntityType { get; }
        List<BaseDTO> GetAll();
        BaseDTO CreateNew();
        void Save(BaseDTO entity);
        void Delete(BaseDTO entity);
    }

    public class CrudService<T> : ICrudService where T : BaseDTO, new()
    {
        readonly GenericBUS<T> bus;
        public CrudService(GenericBUS<T> bus, string title) { this.bus = bus; Title = title; }
        public string Module { get { return bus.Module; } }
        public string Title { get; private set; }
        public Type EntityType { get { return typeof(T); } }
        public List<BaseDTO> GetAll() { return bus.GetAll().Cast<BaseDTO>().ToList(); }
        public BaseDTO CreateNew() { return bus.CreateNew(); }
        public void Save(BaseDTO entity) { bus.Save((T)entity); }
        public void Delete(BaseDTO entity) { bus.Delete((T)entity); }
    }
}
