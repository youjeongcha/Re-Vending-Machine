using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.SocialPlatforms;

namespace Net
{
    static class Lb
    {
        public const string COL = "Rankings";
        public const string F_SCORE = "score";
        public const string F_NAME = "nickname";
        public const string F_TIME = "timestamp";
    }


    // 랭킹 문서 모델
    public class RankingData
    {
        public string uid;
        public string nickname;
        public int score;
        public Timestamp updatedAt; // 동점 정렬 보조
    }

    /// Firestore 랭킹 쿼리 전담 서비스
    public sealed class LeaderboardService
    {
        private readonly FirebaseFirestore _db;
        private readonly CollectionReference _col;

        public LeaderboardService(FirebaseFirestore db, string collection = Lb.COL)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _col = _db.Collection(collection);
        }

        /// 상위 N 페이지 (startAfter로 페이징)
        public async Task<(List<RankingData> list, DocumentSnapshot last)> GetTopPageAsync(
            int pageSize, DocumentSnapshot startAfter = null)
        {
            Query q = _col.OrderByDescending(Lb.F_SCORE).Limit(pageSize);
            if (startAfter != null) q = q.StartAfter(startAfter);

            var snap = await q.GetSnapshotAsync();

            var list = new List<RankingData>(snap.Count);
            foreach (var d in snap.Documents)
            {
                list.Add(new RankingData
                {
                    uid = d.Id,
                    nickname = d.TryGetValue(Lb.F_NAME, out string nn) ? nn : "(unknown)",
                    score = d.TryGetValue(Lb.F_SCORE, out int sc) ? sc : 0,
                    updatedAt = d.TryGetValue(Lb.F_TIME, out Timestamp ts)
                        ? ts : Timestamp.FromDateTime(DateTime.UtcNow)
                });
            }

            // C# '^' 제거 + IEnumerable 인덱싱 이슈 방지
            var docs = snap.Documents.ToList();
            int docCount = snap.Count; // QuerySnapshot.Count (int)
            DocumentSnapshot last = (docCount > 0) ? docs[docCount - 1] : null;
            return (list, last);
        }

        /// 총 유저 수: Firebase Unity SDK에서 Count 미지원일 수 있음 → 알 수 없으면 -1
        public Task<int> GetTotalCountAsync()
        {
            // Aggregate Count 미지원 환경 호환용. 필요 시 Cloud Function으로 대체 권장.
            return Task.FromResult(-1);
        }

        /// 내 문서 하나
        public async Task<RankingData> GetMeAsync(string uid)
        {
            if (string.IsNullOrEmpty(uid)) return null;
            var d = await _col.Document(uid).GetSnapshotAsync();
            if (!d.Exists) return null;

            return new RankingData
            {
                uid = d.Id,
                nickname = d.TryGetValue(Lb.F_NAME, out string nn) ? nn : "(unknown)",
                score = d.TryGetValue(Lb.F_SCORE, out int sc) ? sc : 0,
                updatedAt = d.TryGetValue(Lb.F_TIME, out Timestamp ts)
                    ? ts : Timestamp.FromDateTime(DateTime.UtcNow)
            };
        }

        /// 내 랭크: Count() 없이 '페이지 스캔' 방식(정확, SDK 호환)
        public async Task<int> GetMyRankAsync(string uid)
        {
            if (string.IsNullOrEmpty(uid)) return -1;
            var meDoc = await _col.Document(uid).GetSnapshotAsync();
            if (!meDoc.Exists) return -1;

            // 예외 대신 안전하게 읽기
            int myScore = meDoc.TryGetValue("score", out int sc) ? sc : 0;
            // 점수가 아직 없으면 랭크 계산하지 않고 -1 반환(혹은 0으로 처리하고 싶으면 바꿔도 됨)
            if (!meDoc.ContainsField("score")) return -1;

            int rank = 1;
            DocumentSnapshot cursor = null;
            const int STEP = 200; // 페이지 크기

            while (true)
            {
                var (page, last) = await GetTopPageAsync(STEP, cursor);
                if (page.Count == 0) break;

                foreach (var r in page)
                {
                    if (r.score > myScore) rank++;
                    else break; // 내 점수 이하로 내려왔으니 중단
                }

                // 페이지 마지막이 내 점수 이하이면 더 볼 필요 없음
                if (page[page.Count - 1].score <= myScore) break;

                cursor = last;
                if (cursor == null) break;
            }

            return rank;
        }

        /// 내 주변(before/after)
        public async Task<List<RankingData>> GetAroundMeAsync(string uid, int before = 10, int after = 10)
        {
            var me = await GetMeAsync(uid);
            if (me == null) return new List<RankingData>();

            var aboveSnap = await _col
                .WhereGreaterThan(Lb.F_SCORE, me.score)
                .OrderByDescending(Lb.F_SCORE).Limit(before).GetSnapshotAsync();

            var belowSnap = await _col
                .WhereLessThanOrEqualTo(Lb.F_SCORE, me.score)
                .OrderByDescending(Lb.F_SCORE).Limit(after + 1).GetSnapshotAsync();

            var list = new List<RankingData>();

            foreach (var d in aboveSnap.Documents)
                list.Add(new RankingData
                {
                    uid = d.Id,
                    nickname = d.TryGetValue(Lb.F_NAME, out string nn) ? nn : "(unknown)",
                    score = d.TryGetValue(Lb.F_SCORE, out int sc) ? sc : 0,
                    updatedAt = d.TryGetValue(Lb.F_TIME, out Timestamp ts)
                        ? ts : Timestamp.FromDateTime(DateTime.UtcNow)
                });

            list.Add(me);

            foreach (var d in belowSnap.Documents)
            {
                if (d.Id == uid) continue;
                list.Add(new RankingData
                {
                    uid = d.Id,
                    nickname = d.TryGetValue(Lb.F_NAME, out string nn) ? nn : "(unknown)",
                    score = d.TryGetValue(Lb.F_SCORE, out int sc) ? sc : 0,
                    updatedAt = d.TryGetValue(Lb.F_TIME, out Timestamp ts)
                        ? ts : Timestamp.FromDateTime(DateTime.UtcNow)
                });
            }

            // 동점 안정 정렬
            list.Sort((a, b) =>
            {
                int cmp = b.score.CompareTo(a.score);
                if (cmp != 0) return cmp;
                cmp = a.updatedAt.ToDateTime().CompareTo(b.updatedAt.ToDateTime());
                if (cmp != 0) return cmp;
                return string.Compare(a.uid, b.uid, StringComparison.Ordinal);
            });

            return list;
        }
    }
}
