using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace console
{
    public class BeanUtils
    {

        /// <summary>
        /// 传入类型B的对象b，将b与a相同名称的值进行赋值给创建的a中
        /// </summary>
        /// <typeparam name="A">类型A</typeparam>
        /// <typeparam name="B">类型B</typeparam>
        /// <param name="b">类型为B的参数b</param>
        /// <returns>拷贝b中相同属性的值的a</returns>
        public static A Mapper<A, B>(B b)
        {
            A a = Activator.CreateInstance<A>();
            try
            {
                Type Typeb = b.GetType();//获得类型  
                Type Typea = typeof(A);
                foreach (PropertyInfo sp in Typeb.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo ap in Typea.GetProperties())
                    {
                        if (ap.Name == sp.Name)//判断属性名是否相同  
                        {
                            ap.SetValue(a, sp.GetValue(b, null), null);//获得b对象属性的值复制给a对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return a;
        }


        /// <summary>
        /// 传入类型A的对象a,类型B的对象b，将b和a相同名称的值进行赋值给a
        /// </summary>
        /// <typeparam name="A"></typeparam>
        /// <typeparam name="B"></typeparam>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void Mapper<A, B>(B b, ref A a)
        {
            try
            {
                Type Typeb = b.GetType();//获得类型  
                Type Typea = typeof(A);
                foreach (PropertyInfo sp in Typeb.GetProperties())//获得类型的属性字段  
                {
                    foreach (PropertyInfo ap in Typea.GetProperties())
                    {
                        if (ap.Name == sp.Name)//判断属性名是否相同  
                        {
                            ap.SetValue(a, sp.GetValue(b, null), null);//获得b对象属性的值复制给a对象的属性  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
