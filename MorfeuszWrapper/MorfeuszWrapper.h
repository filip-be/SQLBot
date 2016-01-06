// MorfeuszWrapper.h

#pragma once
#include <vector>
#include <Windows.h>
#include "morfeusz2_c.h"

namespace MorfeuszWrapper {

	#define EXPORT_C extern "C" __declspec(dllexport)
	typedef intptr_t ItemListHandle;

	EXPORT_C bool ParseQuery(ItemListHandle* hItems, InterpMorf** itemsFound, int* itemCount, char *query);

	EXPORT_C bool ReleaseItems(ItemListHandle hItems);
}
