using JVMLibrary.ConstantPoolInfos;
using JVMLibrary.JVMExceptions;
using JVMLibrary.Information;
using JVMLibrary.Attributes;
using System.Text;
using JVMLibrary;

namespace Emulator
{
	public class Identify
	{
		private static bool CheckMethodForMain(ClassFile classFile, MethodInfo method)
		{
			if (!method.AccessFlags.HasFlag(AccessFlags.ACC_PUBLIC))
				return false;

			if (!method.AccessFlags.HasFlag(AccessFlags.ACC_STATIC))
				return false;

			if (classFile.ConstantPool[method.NameIndex] is not ConstUtf8Info nameIndexUtf8)
				throw new InvalidMethodException($"Invalid Method NameIndex, expected {ConstantPoolTag.Utf8}, got '{method.NameIndex}' instead.");

			if (Encoding.UTF8.GetString(nameIndexUtf8.Bytes) != "main")
				return false;

			if (classFile.ConstantPool[method.DescriptorIndex] is not ConstUtf8Info descriptorIndexUtf8)
				throw new InvalidOperationException($"Invalid Method DescriptorIndex, expected {ConstantPoolTag.Utf8}, got '{method.DescriptorIndex}' instead.");

			if (Encoding.UTF8.GetString(descriptorIndexUtf8.Bytes) != "([Ljava/lang/String;)V")
				return false;

			return true;
		}

		public static MethodInfo IdentifyMain(ClassFile classFile)
		{
			MethodInfo mainMethod = new MethodInfo();
			bool mainMethodFound = false;

			foreach (MethodInfo method in classFile.Methods)
			{
				if (CheckMethodForMain(classFile, method))
				{
					if (mainMethodFound) throw new MultipleMainMethodsException($"Multiple main methods found, '{method}' and '{mainMethod}'.");
					mainMethodFound = true;
					mainMethod = method;
				}
			}

			if (!mainMethodFound)
				throw new MissingMainMethodException();

			return mainMethod;
		}

		public static CodeAttributeInfo IdentifyCodeInfo(ClassFile classFile, MethodInfo mainMethod)
		{
			CodeAttributeInfo codeInfo = new CodeAttributeInfo();
			bool codeInfoFound = false;

			foreach (AttributeInfo info in mainMethod.Attributes)
			{
				if (info is not CodeAttributeInfo tempCodeInfo) continue;

				codeInfo = tempCodeInfo;
				codeInfoFound = true;
			}

			if (!codeInfoFound)
				throw new InvalidMethodException($"Code attribute info not found in the main method, '{mainMethod}'.");	

			return codeInfo;
		}
	}
}