using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonBehaviour<UIManager>
{
    public Transform CanvasTransform;
    public Transform ClosedUiTransform;

    private BaseUI _frontUI;
    private Dictionary<Type, BaseUI> _openUIPool = new();
    private Dictionary<Type, BaseUI> _closeUIPool = new();

    public Camera uiCamera;

    private BaseUI GetUI<T>(out bool isAlreadyOpen) where T : BaseUI
    {
        Type uiType = typeof(T);

        if (_openUIPool.TryGetValue(uiType, out var result))
        {
            isAlreadyOpen = true;
            return result;
        }
        else if (_closeUIPool.TryGetValue(uiType, out result))
        {
            isAlreadyOpen = false;
            return result;
        }
        else
        {
            var prefab = Resources.Load<BaseUI>($"UI/{uiType}");
            result = Instantiate(prefab);
            isAlreadyOpen = false;
            return result;
        }
    }

    public void OpenUI<T>() where T : BaseUI
    {
        Type uiType = typeof(T);

        bool isAlreadyOpen = false;
        var ui = GetUI<T>(out isAlreadyOpen);

        if (ui == null)
        {
            Debug.Log($"{uiType} does not exist.");
            return;
        }
        
        if (isAlreadyOpen)
        {
            Debug.Log($"{uiType} is already open.");
            return;
        }

        ui.Init(CanvasTransform);
        ui.transform.SetSiblingIndex(CanvasTransform.childCount - 1);
        ui.gameObject.SetActive(true);
        ui.Show();

        _frontUI = ui;
        _openUIPool[uiType] = ui;
    }

    public void CloseUI(BaseUI ui)
    {
        Type uiType = ui.GetType();

        Debug.Log($"{GetType()}::{nameof(CloseUI)}({uiType})");

        ui.gameObject.SetActive(false);
        _openUIPool.Remove(uiType);
        _closeUIPool[uiType] = ui;
        ui.transform.SetParent(ClosedUiTransform);

        _frontUI = null;
        var lastChild = CanvasTransform.GetChild(CanvasTransform.childCount - 1);
        if (lastChild != null)
        {
            _frontUI = lastChild.GetComponent<BaseUI>();
        }
    }

    public BaseUI GetActiveUI<T>() where T : BaseUI
    {
        Type uiType = typeof(T);

        if (_openUIPool.TryGetValue(uiType, out var result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }

    public bool ExistAnyOpenUI() => _frontUI != null;
    public BaseUI GetFrontUI() => _frontUI;
    public void CloseFrontUI() => _frontUI.Close();
    public void CloseAllUI()
    {
        while (_frontUI != null)
        {
            _frontUI.Close();
        }
    }
}
