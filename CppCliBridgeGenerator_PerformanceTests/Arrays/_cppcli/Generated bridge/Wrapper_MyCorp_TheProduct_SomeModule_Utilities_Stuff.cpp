#pragma once
#pragma managed

#include "marshaller_ext.h"

#define _LNK __declspec(dllexport)
#using "Arrays.dll"
#include "Wrapper_MyCorp_TheProduct_SomeModule_Utilities_Stuff_IL.h"
#include "Wrapper_MyCorp_TheProduct_SomeModule_Utilities_TestEnum_IL.h"

namespace Wrapper {
namespace MyCorp {
namespace TheProduct {
namespace SomeModule {
namespace Utilities {

Stuff::Stuff() {
	__IL = new Stuff_IL;
	__IL->__Impl = gcnew ::MyCorp::TheProduct::SomeModule::Utilities::Stuff();
}

Stuff::Stuff(Stuff_IL* IL) {
	__IL = IL;
}

Stuff::~Stuff() {
	delete __IL;
}

std::vector<int> Stuff::get_IntsField() {
	array<::System::Int32>^ __ReturnVal = __IL->__Impl->IntsField;
	std::vector<int> __ReturnValMarshaled = _marshal_as<std::vector<int>>(__ReturnVal);
	return __ReturnValMarshaled;
}

void Stuff::set_IntsField(std::vector<int> value) {
	array<::System::Int32>^ __Param_value = _marshal_as<array<::System::Int32>^>(value);
	__IL->__Impl->IntsField = __Param_value;
}

Wrapper::MyCorp::TheProduct::SomeModule::Utilities::TestEnum::TestEnumType Stuff::get_lineending() {
	::MyCorp::TheProduct::SomeModule::Utilities::TestEnum __ReturnVal = __IL->__Impl->lineending;
	Wrapper::MyCorp::TheProduct::SomeModule::Utilities::TestEnum::TestEnumType __ReturnValCast = static_cast<Wrapper::MyCorp::TheProduct::SomeModule::Utilities::TestEnum::TestEnumType>(__ReturnVal);
	return __ReturnValCast;
}

void Stuff::set_lineending(Wrapper::MyCorp::TheProduct::SomeModule::Utilities::TestEnum::TestEnumType value) {
	::MyCorp::TheProduct::SomeModule::Utilities::TestEnum __Param_value = static_cast<::MyCorp::TheProduct::SomeModule::Utilities::TestEnum>(value);
	__IL->__Impl->lineending = __Param_value;
}

std::wstring Stuff::get_Name() {
	::System::String^ __ReturnVal = __IL->__Impl->Name;
	std::wstring __ReturnValMarshaled = _marshal_as<std::wstring>(__ReturnVal);
	return __ReturnValMarshaled;
}

void Stuff::set_Name(std::wstring value) {
	::System::String^ __Param_value = _marshal_as<::System::String^>(value);
	__IL->__Impl->Name = __Param_value;
}

void Stuff::set_IntsProperty(std::vector<int> value) {
	array<::System::Int32>^ __Param_value = _marshal_as<array<::System::Int32>^>(value);
	__IL->__Impl->IntsProperty = __Param_value;
}

}
}
}
}
}
