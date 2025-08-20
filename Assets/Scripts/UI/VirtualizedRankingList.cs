using Net; // RankingData
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// 스크롤 가상화 리스트 (보이는 셀만 재사용).
    /// - Content에 자동 레이아웃(VerticalLayout/ContentSizeFitter) 제거!
    /// - rowHeight 고정행 기반 배치
    /// </summary>
    public class VirtualizedRankingList : MonoBehaviour
    {
        [Header("Refs")]
        public ScrollRect scrollRect;          // Scroll View 오브젝트(ScrollRect 컴포넌트)
        public RectTransform content;          // ScrollRect.content (RectTransform)
        public GameObject itemPrefab;          // 루트에 RankingItemUI 포함

        [Header("Layout")]
        public float rowHeight = 68f;
        public int poolSize = 16;         // 화면 표시 수 + 여유

        // 내부
        private readonly List<RankingItemUI> _pool = new();
        private List<RankingData> _data = new();
        private string _myUid = "";
        private int _totalCount = 0;
        private int _firstIndex = -1;

        /// <summary> 하단 근처에서 더 로드가 필요할 때 호출됩니다. (현재 보유 개수 전달) </summary>
        public Func<int, bool> OnNeedMore;

        public void Init()
        {
            if (scrollRect == null || content == null || itemPrefab == null)
            {
                Debug.LogError("[VirtualizedRankingList] Inspector refs missing.");
                enabled = false;
                return;
            }

            // 방어: content 연결 안 했으면 보정
            if (scrollRect.content != content) scrollRect.content = content;

            // 자동 레이아웃은 제거되어 있어야 함(수동 배치)
            // 풀 생성
            for (int i = 0; i < poolSize; i++)
            {
                var go = Instantiate(itemPrefab, content);
                var ui = go.GetComponentInChildren<RankingItemUI>(true);
                if (!ui)
                {
                    Debug.LogError("[VirtualizedRankingList] itemPrefab에 RankingItemUI가 없습니다.");
                    go.SetActive(false);
                    continue;
                }
                _pool.Add(ui);
                go.SetActive(false);
            }

            scrollRect.onValueChanged.AddListener(_ => Refresh());
        }

        public void SetTotalCount(int total)
        {
            _totalCount = Mathf.Max(0, total);
            content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Max(1, _totalCount) * rowHeight);
        }

        public void SetData(List<RankingData> list, string myUid)
        {
            _data = list ?? new List<RankingData>();
            _myUid = myUid ?? "";
            if (_totalCount < _data.Count) _totalCount = _data.Count;
            content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Max(1, _totalCount) * rowHeight);
            Refresh(force: true);
        }

        public void AppendPage(List<RankingData> page)
        {
            if (page == null || page.Count == 0) return;
            _data.AddRange(page);
            if (_totalCount < _data.Count) _totalCount = _data.Count;
            content.sizeDelta = new Vector2(content.sizeDelta.x, Mathf.Max(1, _totalCount) * rowHeight);
            Refresh(force: false);
        }

        public void ScrollToRank(int rank, float duration = 0.8f)
        {
            rank = Mathf.Clamp(rank, 1, Mathf.Max(1, _totalCount));
            float target = 1f;
            if (_totalCount > 1) target = 1f - (rank - 1) / (float)(_totalCount - 1);

            StopAllCoroutines();
            StartCoroutine(SmoothScroll(target, duration));
        }

        private System.Collections.IEnumerator SmoothScroll(float target, float duration)
        {
            float start = scrollRect.verticalNormalizedPosition;
            float t = 0f;
            while (t < duration)
            {
                t += Time.unscaledDeltaTime;
                scrollRect.verticalNormalizedPosition = Mathf.Lerp(start, target, t / duration);
                yield return null;
            }
            scrollRect.verticalNormalizedPosition = target;
            Refresh();
        }

        private void Refresh(bool force = false)
        {
            float contentH = Mathf.Max(1f, content.sizeDelta.y);
            float viewportH = ((RectTransform)scrollRect.viewport).rect.height;
            float topY = (1f - scrollRect.verticalNormalizedPosition) * Mathf.Max(0, contentH - viewportH);
            int first = Mathf.Clamp(Mathf.FloorToInt(topY / rowHeight), 0, Mathf.Max(0, _totalCount - poolSize));

            if (!force && first == _firstIndex) return;
            _firstIndex = first;

            // 하단 근처 프리페치
            if (_data.Count < _totalCount)
            {
                if (first + poolSize + 3 >= _data.Count)
                    OnNeedMore?.Invoke(_data.Count);
            }

            // 풀 업데이트
            for (int i = 0; i < _pool.Count; i++)
            {
                int dataIndex = first + i;
                var cell = _pool[i];
                var rt = (RectTransform)cell.transform;

                if (dataIndex < _data.Count)
                {
                    cell.gameObject.SetActive(true);

                    rt.anchorMin = new Vector2(0, 1);
                    rt.anchorMax = new Vector2(1, 1);
                    rt.pivot = new Vector2(0.5f, 1);
                    rt.sizeDelta = new Vector2(0, rowHeight);
                    rt.anchoredPosition = new Vector2(0, -dataIndex * rowHeight);

                    // RankingItemUI.Set(RankingData, rank, myUid)는 호출자가 채워 넣습니다.
                    // 외부에서 myUid를 모르면 여기서 직접 못 채우므로,
                    // RankingUIManager에서 vlist를 소유하고 있을 때, cell.Set(...)을 대신 호출합니다.
                    // ->간단히 위해 여기선 셀만 배치하고, 데이터 바인딩은 콜백으로 위임해도 됩니다.

                    // 여기서 바로 텍스트까지 세팅
                    cell.Set(_data[dataIndex], dataIndex + 1, _myUid);
                }
                else
                {
                    cell.gameObject.SetActive(false);
                }
            }
        }

        // 현재 화면에 보이는 구간(첫 인덱스) 얻기 — 데이터 바인딩에 사용
        public int GetFirstVisibleIndex()
        {
            float contentH = Mathf.Max(1f, content.sizeDelta.y);
            float viewportH = ((RectTransform)scrollRect.viewport).rect.height;
            float topY = (1f - scrollRect.verticalNormalizedPosition) * Mathf.Max(0, contentH - viewportH);
            return Mathf.Clamp(Mathf.FloorToInt(topY / rowHeight), 0, Mathf.Max(0, _totalCount - poolSize));
        }

        public IReadOnlyList<RankingItemUI> GetPool() => _pool;
        public int TotalCount => _totalCount;
        public int DataCount => _data.Count;

        // 외부에서 호출: 현재 가시 구간의 셀에 데이터 바인딩
        public void BindVisibleCells(Action<RankingItemUI, int/*dataIndex*/, int/*rank*/> binder)
        {
            int first = GetFirstVisibleIndex();
            for (int i = 0; i < _pool.Count; i++)
            {
                int dataIndex = first + i;
                if (dataIndex >= 0 && dataIndex < _data.Count)
                {
                    binder?.Invoke(_pool[i], dataIndex, dataIndex + 1);
                }
            }
        }
    }
}