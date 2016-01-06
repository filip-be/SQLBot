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
	InterpMorf *analiza = morfeusz_analyse("What's going on?");
	int count = 0;
	while (analiza[count].p != -1)
	{
		InterpMorf item;
		item.p = analiza[count].p;
		item.k = analiza[count].k;

		Console::Write("Forma" + count + " - " + strlen(analiza[count].forma) + " ");
		std::printf("%s\n", analiza[count].forma);
		size_t sLength = strlen(analiza[count].forma);
		item.forma = new char[sLength + 1];
		strcpy_s(item.forma, sLength + 1, analiza[count].forma);

		Console::Write("Haslo" + count + " - " + strlen(analiza[count].haslo) + " ");
		std::printf("%s\n", analiza[count].haslo);
		sLength = strlen(analiza[count].haslo);
		item.haslo = new char[sLength + 1];
		strcpy_s(item.haslo, sLength + 1, analiza[count].haslo);

		Console::Write("Interp" + count + " - " + strlen(analiza[count].interp) + " ");
		std::printf("%s\n", analiza[count].interp);
		sLength = strlen(analiza[count].interp);
		item.interp = new char[sLength + 1];
		strcpy_s(item.interp, sLength + 1, analiza[count].interp);

		items->push_back(item);
		count++;
	};

    Console::WriteLine(L"Hello World");
    return 0;
}
