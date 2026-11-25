using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SmartHRM.DAL;
using SmartHRM.DTO;

namespace SmartHRM.BUS
{
    public interface IGenericBUS<T> where T : BaseDTO, new()
    {
        string Module { get; }
        List<T> GetAll();
        T CreateNew();
        void Save(T entity);
        void Delete(T entity);
        void DeleteById(int id);
    }

    /// <summary>BUS tổng quát: kiểm tra quyền RBAC, kiểm tra dữ liệu bắt buộc, ghi nhật ký hệ thống, gọi DAL.</summary>
    public class GenericBUS<T> : IGenericBUS<T> where T : BaseDTO, new()
    {
        protected readonly IGenericDAL<T> Dal;
        public string Module { get; private set; }

        public GenericBUS(string module, IGenericDAL<T> dal = null)
        {
            Module = module;
            Dal = dal ?? new GenericDAL<T>();
        }

        public virtual List<T> GetAll()
        {
            Session.Require(Module, "view");
            return Dal.GetAll();
        }

        public virtual T CreateNew() { return new T(); }

        protected virtual void Validate(T e)
        {
            foreach (var p in typeof(T).GetProperties())
            {
                var f = p.GetCustomAttribute<FieldAttribute>();
                if (f == null || !f.Required) continue;
                var v = p.GetValue(e);
                if (v == null || (v is string && string.IsNullOrWhiteSpace((string)v)))
                    throw new ValidationException("\"" + f.Label + "\" không được để trống.");
            }
        }

        protected virtual void BeforeSave(T e, bool isNew) { }

        public virtual void Save(T e)
        {
            bool isNew = e.Id == 0;
            Session.Require(Module, isNew ? "add" : "edit");
            Validate(e);
            BeforeSave(e, isNew);
            if (isNew) Dal.Insert(e); else Dal.Update(e);
            AfterSave(e, isNew);
            AuditBUS.Log(isNew ? "Thêm" : "Sửa", Module, Describe(e));
        }

        protected virtual void AfterSave(T e, bool isNew) { }

        public virtual void Delete(T e)
        {
            Session.Require(Module, "delete");
            BeforeDelete(e);
            Dal.Delete(e.Id);
            AuditBUS.Log("Xóa", Module, Describe(e));
        }

        public virtual void DeleteById(int id)
        {
            Session.Require(Module, "delete");
            var entity = Dal.GetById(id);
            if (entity == null)
                throw new ValidationException("Không tìm thấy bản ghi cần xóa.");
            Delete(entity);
        }

        protected virtual void BeforeDelete(T e) { }

        /// <summary>Mô tả ngắn bản ghi để ghi vào nhật ký.</summary>
        protected virtual string Describe(T e)
        {
            var parts = new List<string> { "#" + e.Id };
            foreach (var p in typeof(T).GetProperties())
            {
                var f = p.GetCustomAttribute<FieldAttribute>();
                if (f == null || f.Hidden || f.Kind == FieldKind.Password || f.Kind == FieldKind.MultiLine) continue;
                var v = p.GetValue(e);
                if (v == null || p.Name == "Id") continue;
                parts.Add(f.Label + "=" + v);
                if (parts.Count >= 5) break;
            }
            return string.Join("; ", parts);
        }
    }
}
