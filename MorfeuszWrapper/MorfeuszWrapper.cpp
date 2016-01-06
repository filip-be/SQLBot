// This is the main DLL file.

#include "stdafx.h"

#include "MorfeuszWrapper.h"
namespace MorfeuszWrapper {

	bool ParseQuery(ItemListHandle* hItems, InterpMorf** itemsFound, int* itemCount, char *query)
	{
		auto items = new std::vector<InterpMorf>();
		InterpMorf *analiza = morfeusz_analyse(query);
		int count = 0;
		while (analiza[count].p != -1)
		{
			InterpMorf item;
			item.p = analiza[count].p;
			item.k = analiza[count].k;

			size_t sLength = strlen(analiza[count].forma);
			item.forma = new char[sLength + 1];
			strcpy_s(item.forma, sLength+1, analiza[count].forma);

			sLength = strlen(analiza[count].haslo);
			item.haslo = new char[sLength + 1];
			strcpy_s(item.haslo, sLength + 1, analiza[count].haslo);

			sLength = strlen(analiza[count].interp);
			item.interp = new char[sLength + 1];
			strcpy_s(item.interp, sLength + 1, analiza[count].interp);

			items->push_back(item);
			count++;
		};

		*hItems = reinterpret_cast<ItemListHandle>(items);
		*itemsFound = items->data();
		*itemCount = items->size();

		return true;
	}

	bool ReleaseItems(ItemListHandle hItems)
	{
		auto items = reinterpret_cast<std::vector<InterpMorf>*>(hItems);
		delete items;

		return true;
	}
}