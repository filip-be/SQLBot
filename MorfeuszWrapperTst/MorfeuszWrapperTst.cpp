// MorfeuszWrapperTst.cpp : main project file.

#include "stdafx.h"
#pragma comment(lib, "libmorfeusz2.dll.a")
#include "morfeusz2_c.h"
#include "morfeusz2.h"

using namespace System;

int main(array<System::String ^> ^args)
{
	char *about = morfeusz_about();
	InterpMorf *analiza = morfeusz_analyse("kasia, czy wiesz która godzina?");

    Console::WriteLine(L"Hello World");
    return 0;
}
