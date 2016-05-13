﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Hover.Common.Input;
using Hover.Common.Items;

namespace Hover.Common.State {

	/*================================================================================================*/
	[RequireComponent(typeof(HoverItemsManager))]
	public class HoverItemsHighlightManager : MonoBehaviour {

		public HovercursorDataProvider CursorDataProvider;

		private List<HoverItemHighlightState> vHighStates;
		

		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public void Awake() {
			if ( CursorDataProvider == null ) {
				FindObjectOfType<HovercursorDataProvider>();
			}

			if ( CursorDataProvider == null ) {
				throw new ArgumentNullException("CursorDataProvider");
			}

			vHighStates = new List<HoverItemHighlightState>();
		}

		/*--------------------------------------------------------------------------------------------*/
		public void Update() {
			HoverItemsManager itemsMan = GetComponent<HoverItemsManager>();
			
			itemsMan.FillListWithExistingItemComponents(vHighStates);
			ResetItems();
			UpdateItems();
		}
		
		
		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		private void ResetItems() {
			for ( int i = 0 ; i < vHighStates.Count ; i++ ) {
				HoverItemHighlightState highState = vHighStates[i];
				
				if ( highState == null ) {
					vHighStates.RemoveAt(i);
					i--;
					Debug.LogWarning("Found and removed a null item; use RemoveItem() instead.");
					continue;
				}
				
				highState.ResetAllNearestStates();
			}
		}

		/*--------------------------------------------------------------------------------------------*/
		private void UpdateItems() {
			List<HovercursorData> cursors = CursorDataProvider.Cursors;
			int cursorCount = cursors.Count;
			
			for ( int i = 0 ; i < cursorCount ; i++ ) {
				HovercursorData cursor = cursors[i];
				HoverItemHighlightState item = FindNearestItemToCursor(cursor.Type);

				item?.SetNearestAcrossAllItemsForCursor(cursor.Type);
			}
		}
		
		/*--------------------------------------------------------------------------------------------*/
		private HoverItemHighlightState FindNearestItemToCursor(CursorType pCursorType) {
			float maxProg = 0;
			HoverItemHighlightState nearestItem = null;
			
			for ( int i = 0 ; i < vHighStates.Count ; i++ ) {
				HoverItemHighlightState item = vHighStates[i];
				
				if ( !item.gameObject.activeInHierarchy || item.IsHighlightPrevented ) {
					continue;
				}
				
				HoverItemHighlightState.Highlight? high = item.GetHighlight(pCursorType);
				
				if ( high == null || high.Value.Progress <= maxProg ) {
					continue;
				}
				
				maxProg = high.Value.Progress;
				nearestItem = item;
			}
			
			return nearestItem;
		}

	}

}
