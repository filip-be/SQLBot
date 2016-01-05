// MorfeuszWrapper.h

#pragma once
#include <vector>
#include <Windows.h>
#include "morfeusz2_c.h"

namespace MorfeuszWrapper {

	#define EXPORT_C extern "C" __declspec(dllexport)
	typedef intptr_t ItemListHandle;

	EXPORT_C bool ParseQuery(ItemListHandle* hItems, InterpMorf** itemsFound, int* itemCount, char *query)
	{
		auto items = new std::vector<InterpMorf>();
		InterpMorf *analiza = morfeusz_analyse(query);
		int count = 0;
		while (analiza[count].p != -1)
		{
			InterpMorf item;
			item.p = analiza[count].p;
			item.k = analiza[count].k;
			item.forma = new char[sizeof analiza[count].forma + 1];
			std::strcpy(item.forma, analiza[count].forma);
			
			item.haslo = new char[sizeof analiza[count].haslo + 1];
			std::strcpy(item.haslo, analiza[count].haslo);

			item.interp = new char[sizeof analiza[count].interp + 1];
			std::strcpy(item.interp, analiza[count].interp);

			items->push_back(item);
			count++;
		};

		*hItems = reinterpret_cast<ItemListHandle>(items);
		*itemsFound = items->data();
		*itemCount = items->size();

		return true;
	}

	EXPORT_C bool ReleaseItems(ItemListHandle hItems)
	{
		auto items = reinterpret_cast<std::vector<InterpMorf>*>(hItems);
		delete items;

		return true;
	}
}
