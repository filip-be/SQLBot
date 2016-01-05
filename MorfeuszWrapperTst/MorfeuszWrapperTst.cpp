// MorfeuszWrapperTst.cpp : main project file.

#include "stdafx.h"
#pragma comment(lib, "libmorfeusz2.dll.a")
#include "morfeusz2_c.h"

using namespace System;

int main(array<System::String ^> ^args)
{
	char *about = morfeusz_about();
	//std::string ver  = morfeusz::Morfeusz::getVersion();

    Console::WriteLine(L"Hello World");
    return 0;
}
