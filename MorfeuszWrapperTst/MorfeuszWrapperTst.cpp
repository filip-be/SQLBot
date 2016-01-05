// MorfeuszWrapperTst.cpp : main project file.

#include "stdafx.h"
#pragma comment(lib, "libmorfeusz2.dll.a")
#include "morfeusz2_c.h"
#include <vector>

using namespace System;

int main(array<System::String ^> ^args)
{
	char *about = morfeusz_about();
	InterpMorf *analiza2 = morfeusz_analyse("kasia, czy wiesz która godzina?");
	int count2 = 0;
	while (analiza2[count2].p != -1)
	{
		count2++;
	};

	auto items = new std::vector<InterpMorf>();
	InterpMorf *analiza = morfeusz_analyse("");
	int count = 0;
	while (analiza[count].p != -1)
	{
		InterpMorf item;
		item.p = analiza[count].p;
		item.k = analiza[count].k;
		item.forma = new char[sizeof analiza[count].forma];
		std::strcpy(item.forma, analiza[count].forma);

		item.haslo = new char[sizeof analiza[count].haslo];
		std::strcpy(item.haslo, analiza[count].haslo);

		item.interp = new char[sizeof analiza[count].interp];
		std::strcpy(item.interp, analiza[count].interp);

		items->push_back(item);
		count++;
	};

    Console::WriteLine(L"Hello World");
    return 0;
}
