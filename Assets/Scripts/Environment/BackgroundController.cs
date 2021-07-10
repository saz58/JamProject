using System.Collections;
using System.Collections.Generic;
using Pool;
using Scene;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] private Vector3 _startPosition;
    [SerializeField] private List<BackItem> _preloadedItems;

    private CameraController _cameraController;

    public List<BackItem> _liveBlocks = new List<BackItem>(10);
    private List<BackItem> _toRemoveBlocks = new List<BackItem>(10);

    private bool _ready;

    public void Setup(CameraController cameraController)
    {
        _cameraController = cameraController;

        // regenerate
        foreach (var block in _preloadedItems) {
            Destroy(block.gameObject);
        }
        _preloadedItems.Clear();
        Generate();
        _ready = true;
    }

    public void GenerateBlock(Vector3 position)
    {
        var block = PoolManager.Get<BackItem>(nameof(BackItem));
        block.transform.SetPositionAndRotation(position, Quaternion.identity);
        _liveBlocks.Add(block);
    }

    public void RemoveBlock(BackItem block)
    {
        PoolManager.Return<BackItem>(nameof(BackItem), block);
        _liveBlocks.Remove(block);
    }

    private void LateUpdate()
    {
        if (!_ready)
            return;

        _toRemoveBlocks.Clear();
        foreach (var block in _liveBlocks)
        {
            if(!_cameraController.IsInCameraView(block))
            {
                _toRemoveBlocks.Add(block);
            }
        }
        foreach (var block in _toRemoveBlocks)
        {
            RemoveBlock(block);
        }

        Generate();
    }
    public int cc;
    private void Generate()
    {
        var bl = Vector2.one * float.MaxValue;
        var tr = Vector2.one * float.MinValue;

        if(_liveBlocks.Count == 0)
        {
            bl = Vector2.zero;
            tr = Vector2.zero;
        }

        foreach (var block in _liveBlocks)
        {
            if (block.BottomLeftCorner.x < bl.x)
            {
                bl.x = block.BottomLeftCorner.x;
            }
            if (block.TopRightCorner.x > tr.x)
            {
                tr.x = block.TopRightCorner.x;
            }
            if (block.BottomLeftCorner.y < bl.y)
            {
                bl.y = block.BottomLeftCorner.y;
            }
            if (block.TopRightCorner.y > tr.y)
            {
                tr.y = block.TopRightCorner.y;
            }
        }

        if (bl.x > _cameraController.CamBL.x)
        {
            var diff = bl.x - _cameraController.CamBL.x;
            var step = 10;
            var count = (int)(diff / step) + 1;
            var countV = (int)((_cameraController.CamTR.y - bl.y) / step) + 1;
            cc = countV;

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < countV; j++)
                {

                    var position = _startPosition + new Vector3(bl.x - step * 0.5f, bl.y + step * 0.5f + step * j, 0);
                    GenerateBlock(position);
                }
                bl.x -= step;
                //tr.y = bl.y + step * 0.5f + step * countV;
            }
        }
        if (tr.x < _cameraController.CamTR.x)
        {
            var diff = _cameraController.CamTR.x - tr.x;
            var step = 10;
            var count = (int)(diff / step) + 1;
            var countV = (int)((_cameraController.CamTR.y - bl.y) / step) + 1;

            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < countV; j++)
                {
                    var position = _startPosition + new Vector3(tr.x + step * 0.5f, bl.y + step * 0.5f + step * j, 0);
                    GenerateBlock(position);
                }
                tr.x += step;
                //tr.y = bl.y + step * 0.5f + step * countV;
            }
        }

        if (bl.y > _cameraController.CamBL.y)
        {
            var diff = bl.y - _cameraController.CamBL.y;
            var step = 10;
            var countV = (int)(diff / step) + 1;
            var countH = (int)((_cameraController.CamTR.x - bl.x) / step) + 1;

            for (int i = 0; i < countV; i++)
            {
                for (int j = 0; j < countH; j++)
                {
                    var position = _startPosition + new Vector3(bl.x + step * 0.5f + step * j, bl.y - step * 0.5f, 0);
                    GenerateBlock(position);
                }
                bl.y -= step;
            }
        }

        if (tr.y < _cameraController.CamTR.y)
        {
            var diff = _cameraController.CamTR.y - tr.y;
            var step = 10;
            var countV = (int)(diff / step) + 1;
            var countH = (int)((_cameraController.CamTR.x - bl.x) / step) + 1;

            for (int i = 0; i < countV; i++)
            {
                for (int j = 0; j < countH; j++)
                {
                    var position = _startPosition + new Vector3(bl.x + step * 0.5f + step * j, tr.y + step * 0.5f, 0);
                    GenerateBlock(position);
                }
                tr.y += step;
            }
        }
    }

}
