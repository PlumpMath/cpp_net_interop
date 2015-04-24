using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Collections.Generic;

namespace ManagedToNativeWrapperGenerator
{
    public class WrapperHeaderGenerator : TypeGenerator
    {
        public WrapperHeaderGenerator(String outputFolder)
            : base(outputFolder)
        {

        }


        StringBuilder outHeader;
        StringBuilder outClass;

        HashSet<Type> declaredClasses;

        public override void EnumLoad(Type type, FieldInfo[] fields)
        {
            this.outHeader = new StringBuilder();
            this.outClass = new StringBuilder();

            this.declaredClasses = new HashSet<Type>();
            this.declaredClasses.Add(type);

            this.outHeader.AppendLine("#pragma once");
            this.outHeader.AppendLine();

            this.GenerateEnum(type, fields);

            // append class text to header
            this.outHeader.Append(this.outClass.ToString());

            this.WriteToFile(this.outHeader.ToString(), GetFileName(type));
        }

        #region EnumValues generator

        private void GenerateEnum(Type type, FieldInfo[] fields)
        {
            WrapperSourceGenerator.GenerateNamespaces(type, this.outClass);

            this.outClass.AppendLine("enum " + Utils.GetWrapperTypeNameFor(type) + " {"); // Wrapper enum

            var strFields = fields.Select(f => f.Name + " = " + Convert.ChangeType(f.GetValue(null), typeof(ulong))).ToArray();
            this.outClass.AppendLine("\t" + String.Join("," + Environment.NewLine + "\t", strFields));
 
            this.outClass.AppendLine("};");


            WrapperSourceGenerator.GenerateEndNamespaces(type, this.outClass);
        }

        #endregion



        public override void ClassLoad(Type type, FieldInfo[] fields, ConstructorInfo[] ctors, MethodInfo[] methods)
        {
            this.outHeader = new StringBuilder();
            this.outClass = new StringBuilder();

            this.declaredClasses = new HashSet<Type>();
            this.declaredClasses.Add(type);

            this.outHeader.AppendLine("#pragma once");
            this.outHeader.AppendLine("#include <string>");
            this.outHeader.AppendLine();

            this.outHeader.AppendLine("#ifndef _LNK");
            this.outHeader.AppendLine("#define _LNK __declspec(dllimport)");
            this.outHeader.AppendLine("#endif");
            this.outHeader.AppendLine();


            this.GenerateClass(type);

            this.GenerateCtors(type, ctors);

            this.GenerateFields(type, fields);

            this.GenerateMethods(type, methods);

            this.GenerateEndClass(type);


            // append class text to header
            this.outHeader.Append(this.outClass.ToString());

            this.WriteToFile(this.outHeader.ToString(), GetFileName(type));
        }


        #region Class generators

        private void GenerateClass(Type type)
        {
            WrapperSourceGenerator.GenerateNamespaces(type, this.outClass);

            this.outClass.AppendLine("class " + Utils.GetWrapperILBridgeTypeNameFor(type) + ";"); // IL Bridge
            this.outClass.AppendLine();

            this.outClass.AppendLine("class _LNK " + Utils.GetWrapperTypeNameFor(type) + " {"); // Wrapper class
            this.outClass.AppendLine();
            this.outClass.AppendLine("\tpublic:");
            this.outClass.AppendLine("\t\t" + Utils.GetWrapperILBridgeTypeNameFor(type) + "* __IL;"); // IL Bridge instance
            this.outClass.AppendLine();
        }

        private void GenerateEndClass(Type type)
        {
            this.outClass.AppendLine("};");
            WrapperSourceGenerator.GenerateEndNamespaces(type, this.outClass);
        }

        #endregion


        #region Ctors/dctors generators

        private void GenerateCtors(Type type, ConstructorInfo[] ctors)
        {
            string className = Utils.GetWrapperTypeNameFor(type);

            foreach (ConstructorInfo ctor in ctors)
            {
                if (ctor.DeclaringType == type)
                {
                    GenerateCtor(ctor, this.outClass);
                    this.outClass.AppendLine();
                }
            }

            this.outClass.AppendLine("\t\t" + className + "(" + Utils.GetWrapperILBridgeTypeNameFor(type) + "* IL);"); // IL constructor
            this.outClass.AppendLine();
            this.outClass.AppendLine("\t\t~" + className + "();"); // Destructor
        }

        private void GenerateCtor(ConstructorInfo ctor, StringBuilder builder)
        {
            // generate method parameters
            List<string> parList = new List<string>();

            foreach (ParameterInfo parameter in ctor.GetParameters())
            {
                // translate parameter
                TypeConverter.TypeTranslation parTypeTransl = TypeConverter.TranslateParameterType(parameter.ParameterType);
                if (parTypeTransl.isWrapperRequired)
                {
                    this.GenerateWrapperDeclaration(parTypeTransl.ManagedType, this.outHeader);
                }

                parList.Add(parTypeTransl.NativeType + " " + parameter.Name);
                WrapperSourceGenerator.GenerateArrayLengthParameters(parameter.Name, parameter.ParameterType, parList, WrapperSourceGenerator.GenParametersType.Parameter); // generate parameters for parameter-array lengths (if array used)
            }

            builder.Append("\t\t");
            builder.Append(Utils.GetWrapperTypeNameFor(ctor.DeclaringType) + "("); // method name
            builder.Append(String.Join(", ", parList));
            builder.AppendLine(");");
        }

        #endregion


        #region Fields generator

        private void GenerateFields(Type type, FieldInfo[] fields)
        {
            foreach (FieldInfo field in fields)
            {
                if (field.DeclaringType == type)
                {
                    this.outClass.AppendLine();
                    GenerateField(field, this.outClass);
                }
            }
        }

        private void GenerateField(FieldInfo field, StringBuilder builder)
        {
            // translate return type
            TypeConverter.TypeTranslation typeTransl = TypeConverter.TranslateParameterType(field.FieldType);
            if (typeTransl.isWrapperRequired)
            {
                this.GenerateWrapperDeclaration(typeTransl.ManagedType, this.outHeader);
            }

            // GETTER
            {
                List<string> parList = new List<string>();
                WrapperSourceGenerator.GenerateArrayLengthParameters(Utils.GetLocalTempNameForReturn(), field.FieldType, parList, WrapperSourceGenerator.GenParametersType.OutParameter);

                builder.Append("\t\t");

                if (field.IsStatic) builder.Append("static "); // static keyword
                builder.Append(typeTransl.NativeType + " get_" + field.Name + "(");
                builder.Append(String.Join(", ", parList));
                builder.AppendLine(");");
            }

            // SETTER
            if (!field.IsInitOnly)
            {
                List<string> parList = new List<string>();
                parList.Add(typeTransl.NativeType + " value");
                WrapperSourceGenerator.GenerateArrayLengthParameters("value", field.FieldType, parList, WrapperSourceGenerator.GenParametersType.OutParameter);


                builder.AppendLine();
                builder.Append("\t\t");

                if (field.IsStatic) builder.Append("static "); // static keyword
                builder.Append("void set_" + field.Name + "(");
                builder.Append(String.Join(", ", parList));
                builder.AppendLine(");");
            }
        }

        #endregion


        #region Methods generator

        private void GenerateMethods(Type type, MethodInfo[] methods)
        {
            foreach (MethodInfo method in methods)
            {
                if (method.DeclaringType == type)
                {
                    this.outClass.AppendLine();
                    GenerateMethod(method, this.outClass);
                }
            }
        }



        private void GenerateMethod(MethodInfo method, StringBuilder builder)
        {
            // translate return type
            TypeConverter.TypeTranslation returnTypeTransl = TypeConverter.TranslateParameterType(method.ReturnType);
            if (returnTypeTransl.isWrapperRequired)
            {
                this.GenerateWrapperDeclaration(returnTypeTransl.ManagedType, this.outHeader);
            }


            // generate method parameters
            List<string> parList = new List<string>();

            foreach (ParameterInfo parameter in method.GetParameters())
            {
                // translate parameter
                TypeConverter.TypeTranslation parTypeTransl = TypeConverter.TranslateParameterType(parameter.ParameterType);
                if (parTypeTransl.isWrapperRequired)
                {
                    this.GenerateWrapperDeclaration(parTypeTransl.ManagedType, this.outHeader);
                }

                parList.Add(parTypeTransl.NativeType + " " + parameter.Name);
                WrapperSourceGenerator.GenerateArrayLengthParameters(parameter.Name, parameter.ParameterType, parList, WrapperSourceGenerator.GenParametersType.Parameter); // generate parameters for parameter-array lengths (if array used)
            }
            // generate parameters for returnval-array lengths (if array used)
            WrapperSourceGenerator.GenerateArrayLengthParameters(Utils.GetLocalTempNameForReturn(), method.ReturnType, parList, WrapperSourceGenerator.GenParametersType.OutParameter);


            builder.Append("\t\t");

            if (method.IsStatic) builder.Append("static "); // static keyword
            builder.Append(returnTypeTransl.NativeType + " " + method.Name + "("); // method name
            builder.Append(String.Join(", ", parList));
            builder.AppendLine(");");
        }


        private void GenerateWrapperDeclaration(Type type, StringBuilder builder)
        {
            Type t = type;
            if (t.IsArray && t.HasElementType)
            {
                t = t.GetElementType();
            }

            if (this.declaredClasses.Contains(t)) return; // already declared class

            WrapperSourceGenerator.GenerateNamespaces(t, builder);

            if(t.IsClass) builder.AppendLine("class " + Utils.GetWrapperTypeNameFor(t) + ";");
            else if (t.IsEnum) builder.AppendLine("enum " + Utils.GetWrapperTypeNameFor(t) + ";"); 

            WrapperSourceGenerator.GenerateEndNamespaces(t, builder);
            builder.AppendLine();

            this.declaredClasses.Add(t);
        }

        #endregion




        public static string GetFileName(Type type)
        {
            return Utils.GetWrapperTypeNameFor(type) + ".h";
        }

        public override string GetFileNameFor(Type type)
        {
            return GetFileName(type);
        }


    }

}
