﻿using Hover.Utils;
using UnityEngine;

namespace Hover.Layouts.Rect {

	/*================================================================================================*/
	public interface IRectLayoutable {

		Transform transform { get; }
		
		ISettingsControllerMap Controllers { get; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		void SetRectLayout(float pSizeX, float pSizeY, ISettingsController pController);

	}

}
